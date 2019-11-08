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
	public function customerEventAfterAddressSave(Varien_Event_Observer $observer)
	{
		try {
			if (Mage::app()->getRequest()->getRequestedRouteName() == 'customer') {

				Mage::log('************** customerEventAfterAddressSave ************* ', null, 'MicrosoftDFP.log');

				if (empty(Mage::getSingleton('core/session')->getFptDfpSessionId())) {
					return;
				}
				$customer = Mage::getModel('customer/customer')->load($observer->getCustomerAddress()->getCustomerId());

				Mage::log('************** Invoking DFP UpdateAccount Address Change API - START ************* ', null, 'MicrosoftDFP.log');
				$this->invokeUpdateAccountDFP($customer);
				Mage::log('************** Invoking DFP UpdateAccount Address Change API - END ************* ', null, 'MicrosoftDFP.log');
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

	public function UpdateStatusToDFP($observer)
	{
		Mage::log("  -----------  UpdateStatusToDFP Observer Event  START --------------  ", null, 'MicrosoftDFP.log');
		$order = $observer->getEvent()->getOrder();
		if ($order) {
			if (!empty($order->getData()['status'])) {
				$status = $order->getData()['status'];
				if (!empty($status)) {
					Mage::log(" UpdateStatusToDFP Observer Event Status : " . $status, null, 'MicrosoftDFP.log');
					switch (strtolower($status)) {
						case 'order_approved':
						case 'canceled':
							Mage::log("  -----------   Invoking DFP PurchaseStatus API -  START --------------  ", null, 'MicrosoftDFP.log');
							$this->invokePurchaseStatusDFP($order);
							Mage::log("  -----------   Invoking DFP PurchaseStatus API -  END --------------  ", null, 'MicrosoftDFP.log');
							break;
						case 'refunded':
							Mage::log("  -----------   Invoking DFP Refund API -  START --------------  ", null, 'MicrosoftDFP.log');
							$this->invokeRefundDFP($order);
							Mage::log("  -----------   Invoking DFP Refund API -  END --------------  ", null, 'MicrosoftDFP.log');
							break;
						case 'chargeback':
							Mage::log("  -----------   Invoking Charge back API -  START --------------  ", null, 'MicrosoftDFP.log');
							$this->invokeChargeBackDFP($order);
							Mage::log("  -----------   Invoking Charge back API -  END --------------  ", null, 'MicrosoftDFP.log');
							break;
					}
				}
			}
		}

		Mage::log("  -----------  UpdateStatusToDFP Observer Event  END --------------  ", null, 'MicrosoftDFP.log');
	}

	public function GUID()
	{
		if (function_exists('com_create_guid')) {
			return trim(com_create_guid(), '{}');
		}
		return sprintf('%04X%04X-%04X-%04X-%04X-%04X%04X%04X', mt_rand(0, 65535), mt_rand(0, 65535), mt_rand(0, 65535), mt_rand(16384, 20479), mt_rand(32768, 49151), mt_rand(0, 65535), mt_rand(0, 65535), mt_rand(0, 65535));
	}

	public function getAccessToken()
	{
		$clientSecret = Mage::getStoreConfig('dfp/tokenprovider/clientsecret');
		$clientId = Mage::getStoreConfig('dfp/tokenprovider/clientid');
		$apiResourceUri = Mage::getStoreConfig('dfp/tokenprovider/apiresourceuri');
		$directoryId = Mage::getStoreConfig('dfp/tokenprovider/directoryid');

		$auth_data = array(
			'client_id' => $clientId,
			'client_secret' => $clientSecret,
			'resource' => $apiResourceUri,
			'grant_type' => 'client_credentials'
		);

		$tokenUrl = 'https://login.microsoftonline.com/' . $directoryId ."/oauth2/token";

		$curl = curl_init();
		curl_setopt($curl, CURLOPT_POST, 1);
		curl_setopt($curl, CURLOPT_POSTFIELDS, $auth_data);
		curl_setopt($curl, CURLOPT_URL, $tokenUrl);
		curl_setopt($curl, CURLOPT_RETURNTRANSFER, 1);
		$result = curl_exec($curl);
		curl_close($curl);

		return json_decode($result, true)['access_token'];
	}

	public function invokeDFP($url, $access_token, $payload, $trackingid)
	{
		$correlationid = $this->GUID();
		Mage::log("Correlation Id : " . $correlationid, null, 'MicrosoftDFP.log');
		Mage::log("Tracking Id : " . $trackingid, null, 'MicrosoftDFP.log');

		$baseurl = Mage::getStoreConfig('dfp/endpoints/baseurl');

		$headers = array();
		$headers[] = 'x-ms-correlation-id: ' . $correlationid;
		$headers[] = 'x-ms-tracking-id: ' . $trackingid;
		$headers[] = 'Authorization: bearer ' . $access_token;
		$headers[] = 'Content-Type: application/json; charset=utf-8';
		$url = $baseurl . $url;

		$curl = curl_init();
		curl_setopt($curl, CURLOPT_POST, true);
		curl_setopt($curl, CURLOPT_POSTFIELDS, json_encode($payload));
		curl_setopt($curl, CURLOPT_URL, $url);
		curl_setopt($curl, CURLOPT_RETURNTRANSFER, 1);
		curl_setopt($curl, CURLOPT_HTTPHEADER, $headers);
		$result = curl_exec($curl);
		$httpcode = curl_getinfo($curl, CURLINFO_HTTP_CODE);
		curl_close($curl);

		Mage::log("Response Code : " . $httpcode, null, 'MicrosoftDFP.log');
		Mage::log("Response : " . json_encode($result, true), null, 'MicrosoftDFP.log');
		return $result;
	}

	public function invokeRefundDFP($refundDetails)
	{
		$trackingid = $this->GUID();
		$payload = $this->generateRefundPayload($refundDetails, $trackingid);
		$access_token = $this->getAccessToken();

		Mage::log("Payload : " . json_encode($payload, true), null, 'MicrosoftDFP.log');
		Mage::log("Access Token : " .  $access_token, null, 'MicrosoftDFP.log');

		if (!empty($access_token)) {
			$bankEvent = Mage::getStoreConfig('dfp/endpoints/refund');
			return $this->invokeDFP($bankEvent, $access_token, $payload, $trackingid);
		}
	}

	public function invokePurchaseStatusDFP($order)
	{
		$trackingid = $this->GUID();
		$payload = $this->generatePurchaseStatusPayload($order, $trackingid);
		$access_token = $this->getAccessToken();

		Mage::log("Payload : " . json_encode($payload, true), null, 'MicrosoftDFP.log');
		Mage::log("Access Token : " .  $access_token, null, 'MicrosoftDFP.log');

		if (!empty($access_token)) {
			$purchaseStatus = Mage::getStoreConfig('dfp/endpoints/purchasestatus');
			return $this->invokeDFP($purchaseStatus, $access_token, $payload, $trackingid);
		}
	}

	public function invokeUpdateAccountDFP($customer)
	{
		$trackingid = $this->GUID();
		$payload = $this->generateUpdateAccountPayload($customer, $trackingid);
		$access_token = $this->getAccessToken();

		Mage::log("UpdateAccount Payload : " . json_encode($payload, true), null, 'MicrosoftDFP.log');
		Mage::log("Access Token : " .  $access_token, null, 'MicrosoftDFP.log');

		if (!empty($access_token)) {
			$updateaccountEvent = Mage::getStoreConfig('dfp/endpoints/updateaccount');
			return $this->invokeDFP($updateaccountEvent, $access_token, $payload, $trackingid);
		}
	}

	public function generatePurchaseStatusPayload($order, $trackingid)
	{
		$_metadata = array(
			"trackingId"		=> $trackingid,
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

	public function generateRefundPayload($refund, $trackingid)
	{
		$_metadata = array(
			"trackingId"		=> $trackingid,
			"merchantTimeStamp"	=> date('c')
		);

		$refundPayload = array(
			'refundId'       	    => $this->GUID(),
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

	public function generateUpdateAccountPayload($customer, $trackingid)
	{
		$_metadata = array(
			"trackingId"		=> $trackingid,
			"merchantTimeStamp"	=> date('c')
		);

		// Device FingerPrinting Details
		$deviceContext = array(
			"deviceContextId"	=> Mage::getSingleton('core/session')->getFptDfpSessionId(),
			"ipAddress"			=> getenv('HTTP_CLIENT_IP') ?: getenv('HTTP_X_FORWARDED_FOR') ?: getenv('HTTP_X_FORWARDED') ?: getenv('HTTP_FORWARDED_FOR') ?: getenv('HTTP_FORWARDED') ?: getenv('REMOTE_ADDR'),
			"provider"			=> "DFPFINGERPRINTING",
			"deviceContextDC"	=> Mage::getSingleton('core/session')->getFptDfpDc(),
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

	public function invokeChargeBackDFP($order)
	{
		$trackingid = $this->GUID();
		$payload = $this->generateChargeBackPayload($order, $trackingid);
		$access_token = $this->getAccessToken();

		Mage::log("ChargeBack Payload : " . json_encode($payload, true), null, 'MicrosoftDFP.log');
		Mage::log("Access Token : " .  $access_token, null, 'MicrosoftDFP.log');

		if (!empty($access_token)) {
			$chargebackEvent = Mage::getStoreConfig('dfp/endpoints/chargeback');
			return $this->invokeDFP($chargebackEvent, $access_token, $payload, $trackingid);
		}
	}

	public function generateChargeBackPayload($order, $trackingid)
	{
		$_metadata = array(
			"trackingId"		=> $trackingid,
			"merchantTimeStamp"	=> date('c')
		);

		$chargeBackPayload = array(
			'chargebackId'       	=> $this->GUID(),
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
