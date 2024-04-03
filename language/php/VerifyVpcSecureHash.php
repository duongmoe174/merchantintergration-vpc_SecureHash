<?php
include_once "Util.php";
class VerifyVpcSecureHash
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
    function onePayVerifySecureHash($url)
    {
        $parts = parse_url($url);
        $queriesString = $parts['query'];
        $queriesParamMap = [];
        parse_str($queriesString, $queriesParamMap);
        $merchantHash = $queriesParamMap['vpc_SecureHash'];
        ksort($queriesParamMap);
        $util = new Util();
        $stringToHash = $util->generateStringToHash($queriesParamMap);
        $onePayHash = $util->generateSecureHash($stringToHash, $this->merchantHashCode);
        print_r(nl2br("Merchant's hash: $merchantHash"));
        print_r(nl2br("OnePay's hash: $onePayHash"));
        if ($merchantHash != $onePayHash) {
            print_r("Invalid vpc_SecureHash");
        } else {
            print_r("vpc_SecureHash is valid");
        }
    }
}