<?php
/**
 * Microsoft Dynamics 365 Fraud Protection module for Magento
 *
 * NOTICE OF LICENSE
 *
 * This source file is subject to the Open Software License (OSL 3.0)
 * that is bundled with this package in the file LICENSE.txt.
 * It is also available through the world-wide-web at this URL:
 * http://opensource.org/licenses/osl-3.0.php
 *
 * @category   Microsoft
 * @package    Dynamics 365 Fraud Protection
 * @copyright  Copyright (c) Microsoft Corporation. (https://www.microsoft.com)
 * @license    http://opensource.org/licenses/osl-3.0.php  Open Software License (OSL 3.0)
 */

require_once "Mage/Checkout/controllers/OnepageController.php";
class Microsoft_Dfp_Checkout_OnepageController extends Mage_Checkout_OnepageController
{
	private $dfp;

	/**
	 * Constructor
	 *
	 * @param Zend_Controller_Request_Abstract $request
	 * @param Zend_Controller_Response_Abstract $response
	 * @param array $invokeArgs
	 */
	public function __construct(Zend_Controller_Request_Abstract $request, Zend_Controller_Response_Abstract $response, array $invokeArgs = array())
	{
		parent::__construct($request, $response, $invokeArgs);
		$this->dfp = Mage::helper('dfp');
	}

	public function postDispatch()
	{
		parent::postDispatch();
		Mage::dispatchEvent('controller_action_postdispatch_adminhtml', array('controller_action' => $this));

		// *********************************************** DFP Changes START *************************************// 
		$session = Mage::getSingleton('core/session');
		$customerLocalDate = $this->getRequest()->getPost('dfp_customerLocalDate', false);
		$customerTimeZone = $this->getRequest()->getPost('dfp_customerTimeZone', false);
		if (!empty($customerLocalDate)) {
			$session->setCustomerLocalDate($customerLocalDate);
		}

		if (!empty($customerTimeZone)) {
			$session->setCustomerTimeZone($customerTimeZone);
		}
		// *********************************************** DFP Changes END *************************************//
	}

	public function saveOrderAction()
	{
		if (!$this->_validateFormKey()) {
			$this->_redirect('*/*');
			return;
		}

		if ($this->_expireAjax()) {
			return;
		}

		$result = array();
		try {
			$requiredAgreements = Mage::helper('checkout')->getRequiredAgreementIds();
			if ($requiredAgreements) {
				$postedAgreements = array_keys($this->getRequest()->getPost('agreement', array()));
				$diff = array_diff($requiredAgreements, $postedAgreements);
				if ($diff) {
					$result['success'] = false;
					$result['error'] = true;
					$result['error_messages'] = $this->__('Please agree to all the terms and conditions before placing the order.');
					$this->getResponse()->setBody(Mage::helper('core')->jsonEncode($result));
					return;
				}
			}

			$data = $this->getRequest()->getPost('payment', array());
			if ($data) {
				$data['checks'] = Mage_Payment_Model_Method_Abstract::CHECK_USE_CHECKOUT
					| Mage_Payment_Model_Method_Abstract::CHECK_USE_FOR_COUNTRY
					| Mage_Payment_Model_Method_Abstract::CHECK_USE_FOR_CURRENCY
					| Mage_Payment_Model_Method_Abstract::CHECK_ORDER_TOTAL_MIN_MAX
					| Mage_Payment_Model_Method_Abstract::CHECK_ZERO_TOTAL;
				$this->getOnepage()->getQuote()->getPayment()->importData($data);
			}

			$this->getOnepage()->saveOrder();

			// *********************************************** DFP Changes START *************************************//
			try {
				$correlationId = $this->dfp->GUID();
				$order = Mage::getModel('sales/order')->load($this->getOnepage()->getQuote()->getId(), 'quote_id');
				$optionArray = Mage::getModel('dfp/config_source_assessmenttypes')->toOptionArray();
				$assessmentType = $optionArray[Mage::getStoreConfig('dfp/general/assessmenttype')]['label'];
				$this->dfp->log('************** Invoking Purchase API - START ************* ');
				$merchantRuleDecision = $this->invokePurchaseDFP($order, $correlationId, $assessmentType);
				$this->dfp->log('************** Invoking Purchase API - END ************* ');

				if (
					strtolower($merchantRuleDecision) == strtolower(Microsoft_Dfp_Helper_Data::MERCHANT_DECISION_REJECT) &&
					strtolower($assessmentType) == strtolower(Microsoft_Dfp_Helper_Data::ASSESSMENT_TYPE_PROTECT)
				) {
					$this->cancelOrder('Order is rejected by DFP.');
					$result['redirect'] = Mage::getUrl('checkout/onepage/failure');
					$this->getResponse()->setBody(Mage::helper('core')->jsonEncode($result));

					return;
				}

				try {
					$this->dfp->log('************** Invoking Bank Event API - START ************* ');
					$this->invokeBankEventDFP("Auth", $correlationId);
					$this->invokeBankEventDFP("Charge", $correlationId);
					$this->dfp->log('************** Invoking Bank Event API - END ************* ');
				} catch (exception $e) {
					$this->dfp->log($e->getMessage());
				}
			} catch (exception $e) {
				$this->dfp->log($e->getMessage());
			}

			//DFP failing should not break checkout.
			$redirectUrl = $this->getOnepage()->getCheckout()->getRedirectUrl();
			$result['success'] = true;
			$result['error']   = false;

			// *********************************************** DFP Changes END *************************************// 
		} catch (Mage_Payment_Model_Info_Exception $e) {
			$message = $e->getMessage();
			if (!empty($message)) {
				$result['error_messages'] = $message;
			}
			$result['goto_section'] = 'payment';
			$result['update_section'] = array(
				'name' => 'payment-method',
				'html' => $this->_getPaymentMethodsHtml()
			);
		} catch (Mage_Core_Exception $e) {
			Mage::logException($e);
			Mage::helper('checkout')->sendPaymentFailedEmail($this->getOnepage()->getQuote(), $e->getMessage());
			$result['success'] = false;
			$result['error'] = true;
			$result['error_messages'] = $e->getMessage();

			$gotoSection = $this->getOnepage()->getCheckout()->getGotoSection();
			if ($gotoSection) {
				$result['goto_section'] = $gotoSection;
				$this->getOnepage()->getCheckout()->setGotoSection(null);
			}
			$updateSection = $this->getOnepage()->getCheckout()->getUpdateSection();
			if ($updateSection) {
				if (isset($this->_sectionUpdateFunctions[$updateSection])) {
					$updateSectionFunction = $this->_sectionUpdateFunctions[$updateSection];
					$result['update_section'] = array(
						'name' => $updateSection,
						'html' => $this->$updateSectionFunction()
					);
				}
				$this->getOnepage()->getCheckout()->setUpdateSection(null);
			}
		} catch (Exception $e) {
			Mage::logException($e);
			Mage::helper('checkout')->sendPaymentFailedEmail($this->getOnepage()->getQuote(), $e->getMessage());
			$result['success']  = false;
			$result['error']    = true;
			$result['error_messages'] = $this->__('There was an error processing your order. Please contact us or try again later.');
		}
		$this->getOnepage()->getQuote()->save();
		/**
		 * when there is redirect to third party, we don't want to save order yet.
		 * we will save the order in return action.
		 */
		if (isset($redirectUrl)) {
			$result['redirect'] = $redirectUrl;
		}

		$this->getResponse()->setBody(Mage::helper('core')->jsonEncode($result));
	}

	// *********************************************** DFP Changes START *************************************// 

	public function cancelOrder($cancelMessage = '')
	{
		if (Mage::getSingleton('checkout/session')->getLastRealOrderId()) {
			$order = Mage::getModel('sales/order')->loadByIncrementId(Mage::getSingleton('checkout/session')->getLastRealOrderId());
			if ($order->getId()) {
				$order->cancel()->setState(Mage_Sales_Model_Order::STATE_CANCELED, true, $cancelMessage)->save();
			}
		}
	}

	private function invokePurchaseDFP($order, $correlationId, $assessmentType)
	{
		$payload = $this->generatePurchasePayload($order, $assessmentType);
		$result = $this->dfp->sendPurchase($payload, $correlationId);
		return json_decode($result, true)['resultDetails']['MerchantRuleDecision'];
	}

	private function invokeBankEventDFP($type, $correlationId)
	{
		$payload = $this->generateBankEventPayload($type);
		return $this->dfp->sendBankEvent($payload, $correlationId);
	}

	private function GetCardType($cardCode)
	{
		$ccMap = array(
			'VI' => Microsoft_Dfp_Helper_Data::PI_CARD_TYPE_VISA,
			'MC' => Microsoft_Dfp_Helper_Data::PI_CARD_TYPE_MASTERCARD,
			'DI' => Microsoft_Dfp_Helper_Data::PI_CARD_TYPE_DISCOVER,
			'AE' => Microsoft_Dfp_Helper_Data::PI_CARD_TYPE_AMEX,
			'DN' => Microsoft_Dfp_Helper_Data::PI_CARD_TYPE_DINERS,
			'JCB' => Microsoft_Dfp_Helper_Data::PI_CARD_TYPE_JCB,
		);

		return $ccMap[$cardCode];
	}

	private function GetPaymentInstrumentDetails($order)
	{
		$paymentData = $order->getPayment()->getData();

		$billingAddress = $order->getBillingAddress();
		$billingData = $billingAddress->getData();
		$billingStreet = $billingAddress->getStreet();

		$paymentInstrumentList = array();

		$billingAddress = array(
			"firstName"	     => $billingData['firstname'],
			"lastName"	     => $billingData['lastname'],
			"phoneNumber"    => $billingAddress->telephone,
			"street1"	     => $billingStreet[0],
			"street2"	     => count($billingStreet) > 1 ? $billingStreet[1] : null,
			"city"		     => $billingAddress->city,
			"state"		     => $billingAddress->region,
			"zipCode"	     => $billingAddress->postcode,
			"country"	     => $billingAddress->countryId
		);

		switch ($paymentData['method']) {
			case 'ccsave':
				$expirationDate = new DateTime($paymentData['cc_exp_year'] . '-' . $paymentData['cc_exp_month']);
				$expirationDateISO = $expirationDate->format(DateTime::ATOM);
				$paymentInstrumentList[] = array(
					'type'			  => Microsoft_Dfp_Helper_Data::PI_TYPE_CREDIT,
					'cardType'		  => $this->GetCardType($paymentData['cc_type']),
					'holderName'	  => $paymentData['cc_owner'],
					'expirationDate'  => $expirationDateISO,
					'lastFourDigits'  => $paymentData['cc_last4'],
					'purchaseAmount'  => $order->getGrandTotal(),
					'billingAddress'  => $billingAddress
				);
				break;
			case 'checkmo':
				$paymentInstrumentList[] = array(
					'type'			 => Microsoft_Dfp_Helper_Data::PI_TYPE_INVOICE,
					'purchaseAmount' => $order->getGrandTotal(),
					'billingAddress' => $billingAddress
				);
				break;
		}

		return $paymentInstrumentList;
	}

	private function generatePurchasePayload($order, $assessmentType)
	{
		$quote = Mage::getModel('sales/quote')->load($order->getQuoteId());
		$userId = $order->customer_email;
		if ($quote->getCheckoutMethod(true) == 'guest') {
			$userId = $this->dfp->GUID();
		}

		// Shipping address details
		$shippingAddress = $order->getShippingAddress();
		$shipStreet = $shippingAddress->getStreet();
		$shippingData = $shippingAddress->getData();

		$orderShippingAddress = array(
			'firstName'		=> $shippingData['firstname'],
			'lastName'		=> $shippingData['lastname'],
			'phoneNumber'	=> $shippingData['telephone'],
			'street1'		=> $shipStreet[0],
			'street2'		=> count($shipStreet) > 1 ? $shipStreet[1] : null,
			'city'			=> $shippingAddress->city,
			'state'			=> $shippingAddress->region,
			'zipCode'		=> $shippingAddress->postcode,
			'country'		=> $shippingAddress->countryId
		);

		// User Details
		$timeZone = (Mage::getSingleton('core/session')->getCustomerTimeZone()) / 60;
		if ($timeZone > 0)
		{
			$timeZone = '-' . $timeZone;
		}
		else if ($timeZone < 0)
		{
			$timeZone = '+' . $timeZone;
		}
		$user = array(
			'userId'					=> $userId,
			'firstName'					=> $order->customer_firstname,
			'lastName' 					=> $order->customer_lastname,
			'country'					=> $shippingAddress->countryId,
			'zipCode'					=> $shippingAddress->postcode,
			'timeZone'					=> $timeZone,
			'language'					=> strtoupper(substr(Mage::app()->getLocale()->getLocaleCode(), 0, 2)),
			'phoneNumber'				=> $shippingData['telephone'],
			'email'						=> $order->customer_email,
			'isEmailValidated'			=> false,
			'isPhoneNumberValidated'	=> false
		);

		// Device FingerPrinting Details
		$deviceContext = array(
			'deviceContextId'	=> Mage::getSingleton('core/session')->getFptDfpSessionId() ?? $this->dfp->GUID(),
			'ipAddress'			=> getenv('HTTP_CLIENT_IP') ?? getenv('HTTP_X_FORWARDED_FOR') ?? getenv('HTTP_X_FORWARDED') ?? getenv('HTTP_FORWARDED_FOR') ?? getenv('HTTP_FORWARDED') ?? getenv('REMOTE_ADDR'),
			'provider'			=> Microsoft_Dfp_Helper_Data::DEVICE_CONTEXT_PROVIDER_DFP
		);

		// Billing Address Details
		$paymentInstrumentList = $this->GetPaymentInstrumentDetails($order);

		// Cart Details
		$productList = array();
		foreach ($order->getAllItems() as $item) {
			$productList[] = array(
				"productId"		=> 	$item->getProduct()->getId(),
				"purchasePrice"	=>	$item->getPrice(),
				"quantity"		=>	intval($item->getQtyOrdered()),
				"productName"	=>	$item->getName(),
				"sku"			=>	$item->getSku(),
				"currency" 		=>	$order->getOrderCurrencyCode(),
				"isRecurring"	=> 	false
			);
		}

		$_metadata = array(
			"trackingId"		=> $this->dfp->GUID(),
			"merchantTimeStamp"	=> date('c')
		);

		return array(
			'purchaseId'       		=> Mage::getSingleton('checkout/session')->getLastRealOrderId(),
			'assessmentType'        => $assessmentType,
			'customerLocalDate'     => Mage::getSingleton('core/session')->getcustomerLocalDate(),
			'merchantLocalDate'     => date('c'),
			'totalAmount'           => $order->getGrandTotal(),
			'salesTax'            	=> $order->getTaxAmount(),
			'currency'            	=> $order->getOrderCurrencyCode(),
			'shippingMethod'        => Microsoft_Dfp_Helper_Data::PRODUCT_SHIPPING_STANDARD,
			'user'                	=> $user,
			'deviceContext'         => $deviceContext,
			'shippingAddress' 		=> $orderShippingAddress,
			'paymentInstrumentList'	=> $paymentInstrumentList,
			'productList'           => $productList,
			'_metadata'             => $_metadata
		);
	}

	private function generateBankEventPayload($type)
	{
		$_metadata = array(
			"trackingId"		=> $this->dfp->GUID(),
			"merchantTimeStamp"	=> date('c')
		);

		//Some details below normally come from your bank / payment processor's response to the auth/charge.
		//For sample purposes, we use mock values.
		return array(
			'bankEventId'       	=> $this->dfp->GUID(),
			'type'        			=> $type,
			'bankEventTimestamp'	=> date('c'),
			'status'     			=> Microsoft_Dfp_Helper_Data::BANK_STATUS_APPROVED,
			'bankResponseCode'     	=> 'A000',
			'paymentProcessor'     	=> 'Payment Processor Co',
			'purchaseId'			=> Mage::getSingleton('checkout/session')->getLastRealOrderId(),
			'_metadata'             => $_metadata
		);
	}
	// *********************************************** DFP Changes END *************************************// 
}
