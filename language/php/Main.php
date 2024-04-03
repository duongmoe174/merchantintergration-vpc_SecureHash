<?php
include_once 'CreateInvoice.php';
include_once 'CreateInvoiceToken.php';
include_once 'Config.php';
include_once 'CreateInvoiceInstallment.php';
include_once 'QueryDR.php';
include_once 'VerifyVpcSecureHash.php';

function main()
{
    verifySign();
}

function makeInvocie()
{
    $merchantId = Config::MERCHANT_PAYNOW_ID;
    $merchantAccessCode = Config::MERCHANT_PAYNOW_ACCESS_CODE;
    $merchantHashCode = Config::MERCHANT_PAYNOW_HASH_CODE;
    $createInvoice = new CreateInvoice($merchantId, $merchantAccessCode, $merchantHashCode);
    $createInvoice->cInvoice();
}

function makeInvoiceAndCreateToken()
{
    $merchantId = Config::MERCHANT_PAYNOW_ID;
    $merchantAccessCode = Config::MERCHANT_PAYNOW_ACCESS_CODE;
    $merchantHashCode = Config::MERCHANT_PAYNOW_HASH_CODE;
    $createInvoiceToken = new CreateInvoiceToken($merchantId, $merchantAccessCode, $merchantHashCode);
    $createInvoiceToken->createInvoiceAndCreateToken();
}

function makeInvoiceAndPaymentWithToken()
{
    $merchantId = Config::MERCHANT_PAYNOW_ID;
    $merchantAccessCode = Config::MERCHANT_PAYNOW_ACCESS_CODE;
    $merchantHashCode = Config::MERCHANT_PAYNOW_HASH_CODE;
    $tokenNum = "5123451517076481";
    $tokenExp = "1225";
    $createInvoiceToken = new CreateInvoiceToken($merchantId, $merchantAccessCode, $merchantHashCode);
    $createInvoiceToken->createInvoiceAndPaymentWithToken($tokenNum, $tokenExp);
}

function makeInvoiceInstallmentAtOnePaySite()
{
    $merchantId = Config::MERCHANT_INSTALLMENT_ID;
    $merchantAccessCode = Config::MERCHANT_INSTALLMENT_ACCESS_CODE;
    $merchantHashCode = Config::MERCHANT_INSTALLMENT_HASH_CODE;
    $createInvoiceInstallment = new CreateInvoiceInstallment($merchantId, $merchantAccessCode, $merchantHashCode);
    $createInvoiceInstallment->cInvoiceInstallment();
}

function makeInvoiceInstallmentAtMerchantSite()
{
    $merchantId = Config::MERCHANT_INSTALLMENT_ID;
    $merchantAccessCode = Config::MERCHANT_INSTALLMENT_ACCESS_CODE;
    $merchantHashCode = Config::MERCHANT_INSTALLMENT_HASH_CODE;

    $amount = "1000000000";
    $cardList = "VC";
    $itaTime = "3";
    $itaBankSwiftCode = "BFTVVNVX";
    $itaFeeAmount = "20000200";

    $createInvoiceInstallment = new CreateInvoiceInstallment($merchantId, $merchantAccessCode, $merchantHashCode);
    $createInvoiceInstallment->cInvoiceInstallmentThemeITA($amount, $cardList, $itaTime, $itaFeeAmount, $itaBankSwiftCode);
}

function getInstallmentByMerchantId()
{
    $merchantId = Config::MERCHANT_INSTALLMENT_ID;
    $merchantAccessCode = Config::MERCHANT_INSTALLMENT_ACCESS_CODE;
    $merchantHashCode = Config::MERCHANT_INSTALLMENT_HASH_CODE;
    $amount = "3000000";
    $createInvoiceInstallment = new CreateInvoiceInstallment($merchantId, $merchantAccessCode, $merchantHashCode);
    $createInvoiceInstallment->getInstallment($amount);
}

function queryTransaction()
{
    $merchantId = Config::MERCHANT_PAYNOW_ID;
    $merchantAccessCode = Config::MERCHANT_PAYNOW_ACCESS_CODE;
    $merchantHashCode = Config::MERCHANT_PAYNOW_HASH_CODE;
    $vpcMerchTxnRef = "TEST_638466347228355308";
    $queryApi = new QueryDR($merchantId, $merchantAccessCode, $merchantHashCode);
    $queryApi->queryDRApi($vpcMerchTxnRef);
}

function verifySign()
{
    $url = "https://webhook.site/6ddd97bd-d51c-4c36-b16b-7755ce68a010?vpc_Amount=10000000&vpc_Command=pay&vpc_MerchTxnRef=TEST_1711444940129917200&vpc_Merchant=DUONGTT&vpc_Message=Canceled&vpc_OrderInfo=Ma+Don+Hang&vpc_TxnResponseCode=99&vpc_Version=2&vpc_SecureHash=E083AFF7100E679678A68E6FDCB4751A75C2775EB42A5D24209E42B7665313D0";
    $merchantId = Config::MERCHANT_PAYNOW_ID;
    $merchantAccessCode = Config::MERCHANT_PAYNOW_ACCESS_CODE;
    $merchantHashCode = Config::MERCHANT_PAYNOW_HASH_CODE;
    $veify = new VerifyVpcSecureHash($merchantId, $merchantAccessCode, $merchantHashCode);
    $veify->onePayVerifySecureHash($url);
}

main();