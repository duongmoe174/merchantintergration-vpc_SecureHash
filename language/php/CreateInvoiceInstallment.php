<?php
class CreateInvoiceInstallment
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
    public function cInvoiceInstallment()
    {
        $vpcMerchantTxnRef = "TEST_" . time();
        $merchantParam = [
            "vpc_Version" => "2",
            "vpc_Currency" => "VND",
            "vpc_Command" => "pay",
            "vpc_AccessCode" => $this->merchantAccessCode,
            "vpc_MerchTxnRef" => $vpcMerchantTxnRef,
            "vpc_Merchant" => $this->merchantId,
            "vpc_Locale" => "vn",
            "vpc_ReturnURL" => "https=>//mtf.onepay.vn/client/qt/dr/?id=1&mode=TEST_PAYGATE",
            "vpc_OrderInfo" => "Ma Don Hang",
            "vpc_Amount" => "1000000000",
            "vpc_TicketNo" => "192.168.166.149",
            "AgainLink" => "https=>//mtf.onepay.vn/client/qt/",
            "Title" => "PHP VPC 3-Party",
            "vpc_Customer_Phone" => "84987654321",
            "vpc_Customer_Email" => "test@onepay.vn",
            "vpc_Customer_Id" => "test"
        ];
        $util = new Util();
        ksort($merchantParam);
        $stringToHash = $util->generateStringToHash($merchantParam);
        $secureHash = $util->generateSecureHash($stringToHash, $this->merchantHashCode);
        // Thêm tham số mới
        $merchantParam['vpc_SecureHash'] = $secureHash;
        $requestUrl = Config::BASE_URL . Config::URL_PREFIX . http_build_query($merchantParam);
        $util->sendHttpRequest($requestUrl);
    }

    public function cInvoiceInstallmentThemeITA($amount, $cardList, $itaTime, $itaFeeAmount, $itaBankSwiftcode)
    {
        $vpcMerchantTxnRef = "TEST_" . time();
        $merchantParam = [
            "vpc_Version" => "2",
            "vpc_Currency" => "VND",
            "vpc_Command" => "pay",
            "vpc_AccessCode" => $this->merchantAccessCode,
            "vpc_MerchTxnRef" => $vpcMerchantTxnRef,
            "vpc_Merchant" => $this->merchantId,
            "vpc_Locale" => "vn",
            "vpc_ReturnURL" => "https=>//mtf.onepay.vn/client/qt/dr/?id=1&mode=TEST_PAYGATE",
            "vpc_OrderInfo" => "Ma Don Hang",
            "vpc_Amount" => $amount,
            "vpc_TicketNo" => "192.168.166.149",
            "AgainLink" => "https=>//mtf.onepay.vn/client/qt/",
            "Title" => "PHP VPC 3-Party",
            "vpc_Customer_Phone" => "84987654321",
            "vpc_Customer_Email" => "test@onepay.vn",
            "vpc_Customer_Id" => "test",
            "vpc_CardList" => $cardList,
            "vpc_ItaTime" => $itaTime,
            "vpc_ItaFeeAmount" => $itaFeeAmount,
            "vpc_ItaBank" => $itaBankSwiftcode,
            "vpc_Theme" => "ita",

        ];
        $util = new Util();
        ksort($merchantParam);
        $stringToHash = $util->generateStringToHash($merchantParam);
        $secureHash = $util->generateSecureHash($stringToHash, $this->merchantHashCode);
        // Thêm tham số mới
        $merchantParam['vpc_SecureHash'] = $secureHash;
        $requestUrl = Config::BASE_URL . Config::URL_PREFIX . http_build_query($merchantParam);
        $util->sendHttpRequest($requestUrl);
    }

    public function getInstallment($amount)
    {
        $signedHeaderNames = ["(request-target)", "(created)", "host", "accept"];
        $accept = "application/json";
        $header = array(
            'Host' => Config::HOST,
            'Accept' => $accept
        );
        $uri = "/msp/api/v1/merchants/" . $this->merchantId . "/installments?amount=" . $amount;

        $util = new Util();

        $signature = $util->createRequestSignatureITA(
            "GET",
            $uri,
            $header,
            $signedHeaderNames,
            $this->merchantId,
            $this->merchantHashCode
        );

        $headerRequest = array(
            "Host:" . Config::HOST,
            "accept:" . $accept,
            "signature:" . $signature,
        );
        $urlRequest = Config::BASE_URL . $uri;
        $util->sendHttpGetWithHeader($urlRequest, $headerRequest);
    }
}