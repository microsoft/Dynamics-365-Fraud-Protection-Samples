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

class Microsoft_Dfp_Model_Observer
{
	private $dfp;

	function __construct() {
        $this->dfp = Mage::helper('dfp');
	}
	
	public function customerEventAfterAddressSave(Varien_Event_Observer $observer)
	{
		try {
			if (Mage::app()->getRequest()->getRequestedRouteName() == 'customer') {

				$this->dfp->log('************** customerEventAfterAddressSave ************* ');

				if (empty(Mage::getSingleton('core/session')->getFptDfpSessionId())) {
					return;
				}
				$customer = Mage::getModel('customer/customer')->load($observer->getCustomerAddress()->getCustomerId());

				$this->dfp->log('************** Invoking UpdateAccount Address Change API - START ************* ');
				$this->invokeUpdateAccountDFP($customer);
				$this->dfp->log('************** Invoking UpdateAccount Address Change API - END ************* ');
			}
		} catch (exception $e) {
			Mage::logException($e);
		}
	}

	public function salesEventOrderAfterSave(Varien_Event_Observer $observer)
	{
		try {
			$this->UpdateStatusToDFP($observer);
		} catch (exception $e) {
			Mage::logException($e);
		}
	}

	private function UpdateStatusToDFP($observer)
	{
		$this->dfp->log("  -----------  UpdateStatusToDFP Observer Event START --------------  ");
		$order = $observer->getEvent()->getOrder();
		if ($order) {
			if (!empty($order->getData()['status'])) {
				$status = $order->getData()['status'];
				if (!empty($status)) {
					$this->dfp->log(" UpdateStatusToDFP Observer Event Status : " . $status);
					switch (strtolower($status)) {
						case 'order_approved':
						case 'canceled':
							$this->dfp->log("  -----------   Invoking PurchaseStatus API - START --------------  ");
							$this->invokePurchaseStatusDFP($order);
							$this->dfp->log("  -----------   Invoking PurchaseStatus API - END --------------  ");
							break;
						case 'refunded':
							$this->dfp->log("  -----------   Invoking Refund API - START --------------  ");
							$this->invokeRefundDFP($order);
							$this->dfp->log("  -----------   Invoking Refund API - END --------------  ");
							break;
						case 'chargeback':
							$this->dfp->log("  -----------   Invoking Chargeback API - START --------------  ");
							$this->invokeChargebackDFP($order);
							$this->dfp->log("  -----------   Invoking Chargeback API - END --------------  ");
							break;
					}
				}
			}
		}

		$this->dfp->log("  -----------  UpdateStatusToDFP Observer Event END --------------  ");
	}

	private function invokeRefundDFP($refund)
	{
		$payload = $this->generateRefundPayload($refund);
		return $this->dfp->sendRefund($payload);
	}

	private function invokeChargebackDFP($order)
	{
		$payload = $this->generateChargebackPayload($order);
		return $this->dfp->sendChargeback($payload);
	}

	private function invokePurchaseStatusDFP($order)
	{
		$payload = $this->generatePurchaseStatusPayload($order);
		return $this->dfp->sendPurchaseStatus($payload);
	}

	private function invokeUpdateAccountDFP($customer)
	{
		$payload = $this->generateUpdateAccountPayload($customer);
		return $this->dfp->sendUpdateAccount($payload);
	}

	private function generatePurchaseStatusPayload($order)
	{
		$_metadata = array(
			"trackingId"		=> $this->dfp->GUID(),
			"merchantTimeStamp"	=> date('c')
		);

		$status = $order->getData()['status'];
		if ($status == 'order_approved') {
			$status = 'Approved';
		} else {
			$status = 'Canceled';
		}

		$purchaseStatusPayload = array(
			'purchaseId'	=> $order->getData()['increment_id'],
			'statusType'	=> $status,
			'statusDate'	=> date('c'),
			'reason'	    => '',
			'_metadata'		=> $_metadata

		);
		return $purchaseStatusPayload;
	}

	private function generateRefundPayload($refund)
	{
		$_metadata = array(
			"trackingId"		=> $this->dfp->GUID(),
			"merchantTimeStamp"	=> date('c')
		);

		$refundPayload = array(
			'refundId'       	    => $this->dfp->GUID(),
			'status'        		=> "Approved",
			'bankEventTimestamp'	=> date('c'),
			'amount'     			=> $refund->getGrandTotal(),
			'currency'     	        => $refund->getOrderCurrencyCode(),
			'userId'				=> $refund->getCustomerEmail(),
			'purchaseId'			=> $refund->getIncrementId(),
			'_metadata'     		=> $_metadata
		);
		return $refundPayload;
	}

	private function generateUpdateAccountPayload($customer)
	{
		$_metadata = array(
			"trackingId"		=> $this->dfp->GUID(),
			"merchantTimeStamp"	=> date('c')
		);

		// Device FingerPrinting Details
		$deviceContext = array(
			"deviceContextId"	=> Mage::getSingleton('core/session')->getFptDfpSessionId(),
			"ipAddress"			=> getenv('HTTP_CLIENT_IP') ?: getenv('HTTP_X_FORWARDED_FOR') ?: getenv('HTTP_X_FORWARDED') ?: getenv('HTTP_FORWARDED_FOR') ?: getenv('HTTP_FORWARDED') ?: getenv('REMOTE_ADDR'),
			"provider"			=> "DFPFingerPrinting",
		);

		//Address details
		$AddressList = array();
		$billingAddress = $customer->getDefaultBillingAddress();
		$billingAddressStreet = $billingAddress->getStreet();
		$AddressList[] = array(
			"type"		=> 	"BILLING",
			"firstName"	=>	$billingAddress["firstname"],
			"lastName"	=>	$billingAddress["lastname"],
			"street1"	=>	$billingAddressStreet[0],
			"street2"	=>	count($billingAddressStreet) > 1 ? $billingAddressStreet[1] : null,
			"city"		=>	$billingAddress["city"],
			"state"		=>	$billingAddress["region"],
			"zipCode"	=>	$billingAddress["postcode"],
			"country"	=>	$billingAddress["country_id"]
		);

		$shippingAddress = $customer->getDefaultShippingAddress();
		$shippingAddressStreet = $shippingAddress->getStreet();
		$AddressList[] = array(
			"type"		=> 	"SHIPPING",
			"firstName"	=>	$shippingAddress["firstname"],
			"lastName"	=>	$shippingAddress["lastname"],
			"street1"	=>	$shippingAddressStreet[0],
			"street2"	=>	count($shippingAddressStreet) > 1 ? $shippingAddressStreet[1] : null,
			"city"		=>	$shippingAddress["city"],
			"state"		=>	$shippingAddress["region"],
			"zipCode"	=>	$shippingAddress["postcode"],
			"country"	=>	$shippingAddress["country_id"]
		);

		foreach ($customer->getAdditionalAddresses() as $item) {
			$AddressList[] = array(
				"type"		=> 	"SHIPPING",
				"firstName"	=>	$item["firstname"],
				"lastName"	=>	$item["lastname"],
				"street1"	=>	$item["street"],
				"city"		=>	$item["city"],
				"state"		=>	$item["region"],
				"zipCode"	=>	$item["postcode"],
				"country"	=>	$item["country_id"],
			);
		}

		$updateAccountPayload = array(
			'userId'       	         => $customer->getEmail(),
			'firstName'       	     => $customer->getFirstname(),
			'lastName'       	     => $customer->getLastname(),
			'email'       	         => $customer->getEmail(),
			'profileType'       	 => "Consumer",
			'isEmailValidated'       => false,
			'isPhoneNumberValidated' => false,
			'addressList'            => $AddressList,
			'deviceContext'          => $deviceContext,
			'_metadata'     		 => $_metadata
		);
		return $updateAccountPayload;
	}

	private function generateChargebackPayload($order)
	{
		$_metadata = array(
			"trackingId"		=> $this->dfp->GUID(),
			"merchantTimeStamp"	=> date('c')
		);

		$chargeBackPayload = array(
			'chargebackId'       	=> $this->dfp->GUID(),
			'status'       			=> 'Inquiry',
			'bankEventTimestamp'    => date('c'),
			'amount'       			=> $order->getGrandTotal(),
			'currency'       		=> $order->getOrderCurrencyCode(),
			'userId'            	=> $order->getCustomerEmail(),
			'purchaseId' 			=> $order->getData()['increment_id'],
			'_metadata'     		=> $_metadata
		);
		return $chargeBackPayload;
	}
}
