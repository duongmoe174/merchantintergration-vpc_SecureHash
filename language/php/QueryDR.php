<?php
class QueryDR
{
    private $merchantId;
    private $merchantAccessCode;
    private $merchantHashCode;
    public function __construct($merchantId, $merchantAccessCode, $merchantHashCode)
    {
        $this->merchantId = $merchantId;
        $this->merchantAccessCode = $merchantAccessCode;
        $this->merchantHashCode = $merchantHashCode;
    }
    public function queryDRApi($vpcMerchantTxnRef)
    {
        $merchantParam = [
            "vpc_Version" => "2",
            "vpc_Command" => "queryDR",
            "vpc_AccessCode" => $this->merchantAccessCode,
            "vpc_MerchTxnRef" => $vpcMerchantTxnRef,
            "vpc_Merchant" => $this->merchantId,
            "vpc_User" => "Administrator",
            "vpc_Password" => "admin@123456",
        ];
        $util = new Util();
        ksort($merchantParam);
        $stringToHash = $util->generateStringToHash($merchantParam);
        $secureHash = $util->generateSecureHash($stringToHash, $this->merchantHashCode);
        // Thêm tham số mới
        $merchantParam['vpc_SecureHash'] = $secureHash;
        $requestUrl = Config::BASE_URL . "/msp/api/v1/vpc/invoices/queries";
        $util->sendHttpPost($requestUrl, $merchantParam);
    }
}