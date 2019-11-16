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

class Microsoft_Dfp_Helper_Data extends Mage_Core_Helper_Abstract
{
    public function GUID()
	{
		if (function_exists('com_create_guid')) {
			return trim(com_create_guid(), '{}');
		}
		return sprintf('%04X%04X-%04X-%04X-%04X-%04X%04X%04X', mt_rand(0, 65535), mt_rand(0, 65535), mt_rand(0, 65535), mt_rand(16384, 20479), mt_rand(32768, 49151), mt_rand(0, 65535), mt_rand(0, 65535), mt_rand(0, 65535));
	}

	public function log($msg)
	{
		Mage::log($msg, null, 'MicrosoftDFP.log');
	}

	public function getFingerprintingSessionId()
	{
		$sessionId = Mage::getSingleton('core/session')->getFptDfpSessionId();
		if (empty($sessionId)) {
			$sessionId = $this->GUID();
			Mage::getSingleton('core/session')->setFptDfpSessionId($sessionId);
		}
		return $sessionId;
	}

	public function sendUpdateAccount($payload, $correlationId = null)
	{
		return $this->invokeDFP('dfp/endpoints/updateaccount', $payload, $correlationId);
	}

	public function sendPurchase($payload, $correlationId = null)
	{
		return $this->invokeDFP('dfp/endpoints/purchase', $payload, $correlationId);
	}

	public function sendBankEvent($payload, $correlationId = null)
	{
		return $this->invokeDFP('dfp/endpoints/bankevent', $payload, $correlationId);
	}

	public function sendPurchaseStatus($payload, $correlationId = null)
	{
		return $this->invokeDFP('dfp/endpoints/purchasestatus', $payload, $correlationId);
	}

	public function sendRefund($payload, $correlationId = null)
	{
		return $this->invokeDFP('dfp/endpoints/refund', $payload, $correlationId);
	}

	public function sendChargeback($payload, $correlationId = null)
	{
		return $this->invokeDFP('dfp/endpoints/chargeback', $payload, $correlationId);
	}

	private function invokeDFP($configName, $payload, $correlationId)
	{
		$apiMethod = Mage::getStoreConfig($configName);
		$correlationId = $correlationId ?: $this->GUID();
		$jsonPayload = json_encode($payload);

		$this->log("Payload " . $apiMethod . ": " . $jsonPayload);
		$this->log("Correlation Id: " . $correlationId);

		$accessToken = $this->getAccessToken();
		if (empty($accessToken)) {
			$this->log("Failed to get access token!");
			throw new Exception("Failed to get access token!");
		}

		$this->log("Access Token: " . $accessToken);

		$headers = array();
		$headers[] = 'x-ms-correlation-id: ' . $correlationId;
		$headers[] = 'Authorization: bearer ' . $accessToken;
		$headers[] = 'Content-Type: application/json; charset=utf-8';

		$baseurl = Mage::getStoreConfig('dfp/endpoints/baseurl');
		$url = $this->buildUrl($baseurl, $apiMethod);

		$curl = curl_init();
		curl_setopt($curl, CURLOPT_POST, true);
		curl_setopt($curl, CURLOPT_POSTFIELDS, $jsonPayload);
		curl_setopt($curl, CURLOPT_URL, $url);
		curl_setopt($curl, CURLOPT_RETURNTRANSFER, 1);
		curl_setopt($curl, CURLOPT_HTTPHEADER, $headers);
		$result = curl_exec($curl);
		$httpCode = curl_getinfo($curl, CURLINFO_HTTP_CODE);
		curl_close($curl);

		$this->log("Response ($httpCode) : " . json_encode($result));
		return $result;
	}

	private function getAccessToken()
	{
		$cacheId = 'dfp_accessToken';
		if (false !== ($accessToken = Mage::app()->getCache()->load($cacheId)))
		{
			return $accessToken;
		}

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

		$tokenUrl = 'https://login.microsoftonline.com/' . $directoryId . "/oauth2/token";

		$curl = curl_init();
		curl_setopt($curl, CURLOPT_POST, 1);
		curl_setopt($curl, CURLOPT_POSTFIELDS, $auth_data);
		curl_setopt($curl, CURLOPT_URL, $tokenUrl);
		curl_setopt($curl, CURLOPT_RETURNTRANSFER, 1);
		$result = curl_exec($curl);
		curl_close($curl);

		$response = json_decode($result, true);

		$accessToken = $response['access_token'];
		$expiresInSeconds = intval($response['expires_in']) - 60; //Expire one minute early to give some buffer
		$cacheTags = array();

		Mage::app()->getCache()->save($accessToken, $cacheId, $cacheTags, $expiresInSeconds);

		return $accessToken;
	}

	private function buildUrl($base, $endpoint)
	{
		$base = rtrim($base, '/');
		$endpoint = ltrim($endpoint, '/');
		return $base . '/' . $endpoint;
	}
}
