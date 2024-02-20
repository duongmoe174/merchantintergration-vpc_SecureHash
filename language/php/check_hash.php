<?php
$MERCHANT_HASH_CODE = "6D0870CDE5F24F34F3915FB0045120DB";

define('BASE_URL', "https://mtf.onepay.vn/paygate/vpcpay.op");

//runMain($MERCHANT_HASH_CODE);

function runMain($MERCHANT_HASH_CODE)
{
    //$url = merchantSendRequestStatic($MERCHANT_HASH_CODE);
    $url = "https://mtf.onepay.vn/paygate/vpcpay.op?AgainLink=https%3A%2F%2Fmtf.onepay.vn%2Fclient%2Fqt%2F&Title=PHP+VPC+3-Party&vpc_AccessCode=6BEB2546&vpc_Amount=10000000&vpc_CardList=VC&vpc_Command=pay&vpc_Currency=VND&vpc_Customer_Email=test%40onepay.vn&vpc_Customer_Id=test&vpc_Customer_Phone=84987654321&vpc_Locale=vn&vpc_MerchTxnRef=TEST_1706240061&vpc_Merchant=TESTONEPAY&vpc_OrderInfo=Ma+Don+Hang&vpc_ReturnURL=https%3A%2F%2Fportal.lanit.com.vn%3A4083%2FsessYBzPl9UJVn4bm2uE%2Findex.php%3Fact%3Dpayment%26process_payment%3Donepay&vpc_TicketNo=192.168.166.149&vpc_Version=2&vpc_SecureHash=11396C01BC33F204691399900DED2D34AB4F9499FF0BA1651E6F7A1F19DAFE3B";
    onePayVerifySecureHash($url, $MERCHANT_HASH_CODE);
}

runMain2($MERCHANT_HASH_CODE);

function runMain2($MERCHANT_HASH_CODE)
{
    sendHttpRequest($MERCHANT_HASH_CODE);
}

function merchantSendRequestStatic($MERCHANT_HASH_CODE)
{
    $merchantParam = '{
        "vpc_Version": "2",
        "vpc_Currency": "VND",
        "vpc_Command": "pay",
        "vpc_AccessCode": "6BEB2546",
        "vpc_Merchant": "TESTONEPAY",
        "vpc_Locale": "vn",
        "vpc_ReturnURL": "https://mtf.onepay.vn/client/qt/dr/",
        "vpc_MerchTxnRef": "TEST_1698989260880",
        "vpc_OrderInfo": "Ma Don Hang",
        "vpc_Amount": "10000000",
        "vpc_TicketNo": "192.168.166.149",
        "vpc_CardList": "VC",
        "AgainLink": "https://mtf.onepay.vn/client/qt/",
        "Title": "PHP VPC 3-Party",
        "vpc_Customer_Phone": "84987654321",
        "vpc_Customer_Email": "test@onepay.vn",
        "vpc_Customer_Id": "test"
    }';
    $object = json_decode($merchantParam);
    $array = get_object_vars($object);
    ksort($array);
    $stringToHash = generateStringToHash($array);
    $secureHash = generateSecureHash($stringToHash, $MERCHANT_HASH_CODE);
    // Thêm tham số mới
    $array['vpc_SecureHash'] = $secureHash;
    return $array;
}

function merchantSendRequestDynamic($MERCHANT_HASH_CODE)
{
    $vpcMerchantTxnRef = "TEST_" . time();
    $merchantParam = '{
        "vpc_Version": "2",
        "vpc_Currency": "VND",
        "vpc_Command": "pay",
        "vpc_AccessCode": "6BEB2546",
        "vpc_Merchant": "TESTONEPAY",
        "vpc_Locale": "vn",
        "vpc_ReturnURL": "https://mtf.onepay.vn/client/qt/dr/?id=1&mode=TEST_PAYGATE",
        "vpc_OrderInfo": "Ma Don Hang",
        "vpc_Amount": "10000000",
        "vpc_TicketNo": "192.168.166.149",
        "vpc_CardList": "VC",
        "AgainLink": "https://mtf.onepay.vn/client/qt/",
        "Title": "PHP VPC 3-Party",
        "vpc_Customer_Phone": "84987654321",
        "vpc_Customer_Email": "test@onepay.vn",
        "vpc_Customer_Id": "test"
    }';
    $object = json_decode($merchantParam);
    $object->vpc_MerchTxnRef = $vpcMerchantTxnRef;
    $array = get_object_vars($object);
    ksort($array);
    $stringToHash = generateStringToHash($array);
    $secureHash = generateSecureHash($stringToHash, $MERCHANT_HASH_CODE);
    // Thêm tham số mới
    $array['vpc_SecureHash'] = $secureHash;
    return $array;
}

function onePayVerifySecureHash(string $url, string $merchantHashCode)
{
    $parts = parse_url($url);
    $queriesString = $parts['query'];
    $queriesParamMap = [];
    parse_str($queriesString, $queriesParamMap);
    $merchantHash = $queriesParamMap['vpc_SecureHash'];
    ksort($queriesParamMap);
    $stringToHash = generateStringToHash($queriesParamMap);
    $onePayHash = generateSecureHash($stringToHash, $merchantHashCode);
    print_r(nl2br("Merchant's hash: $merchantHash"));
    print_r(nl2br("OnePay's hash: $onePayHash"));
    if ($merchantHash != $onePayHash) {
        print_r("Invalid vpc_SecureHash");
    } else {
        print_r("vpc_SecureHash is valid");
    }
}

function generateStringToHash($array)
{
    $stringToHash = "";
    foreach ($array as $key => $value) {
        $pref4 = substr($key, 0, 4);
        $pref5 = substr($key, 0, 5);
        if ($pref4 == "vpc_" || $pref5 == "user_") {
            if ($key != "vpc_SecureHashType" && $key != "vpc_SecureHash") {
                if (strlen($value) > 0) {
                    if (strlen($stringToHash) > 0) {
                        $stringToHash = $stringToHash . "&";
                    }
                    $stringToHash = $stringToHash . $key . "=" . $value;
                }
            }
        }
    }
    return $stringToHash;
}

function generateSecureHash($stringToHash, $MERCHANT_HASH_CODE)
{
    $merchantHex = cryptoJsHexParse($MERCHANT_HASH_CODE);
    $secureHash = cryptoJsHmacSha256($stringToHash, $merchantHex);
    return $secureHash;
}

function cryptoJsHmacSha256(string $data, string $key): string
{
    $sign = hash_hmac("sha256", $data, $key, false);
    return strtoupper($sign);
}

function cryptoJsHexParse(string $hexString): string
{
    return hex2bin($hexString);
}

function sendHttpRequest($MERCHANT_HASH_CODE)
{
    $param = merchantSendRequestDynamic($MERCHANT_HASH_CODE);

    $queryString = http_build_query($param);
    $apiUrl = BASE_URL . "?" . $queryString;

    print_r('<br>');
    print_r($apiUrl);
    print_r('<br>');
    print_r('<br>');

    // Create a stream
    $curl = curl_init($apiUrl);
    curl_setopt($curl, CURLOPT_HEADER, true);  // we want headers
    curl_exec($curl);
    curl_close($curl);
}
