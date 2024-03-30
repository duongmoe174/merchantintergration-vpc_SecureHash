package code;

public class Main {
    public static void main(String[] args) {
        verifySign();
    }

    private static void makeInvoice() {
        CreateInvoice createInvoice = new CreateInvoice();
        String merchantId = Config.MERCHANT_PAYNOW_ID;
        String merchantAccessCode = Config.MERCHANT_PAYNOW_ACCESS_CODE;
        String merchantHashCode = Config.MERCHANT_PAYNOW_HASH_CODE;
        createInvoice.createInvoice(merchantId, merchantAccessCode, merchantHashCode);
    }

    private static void makeInvoiceCreateToken() {
        CreateInvoiceToken createInvoiceToken = new CreateInvoiceToken();
        String merchantId = Config.MERCHANT_PAYNOW_ID;
        String merchantAccessCode = Config.MERCHANT_PAYNOW_ACCESS_CODE;
        String merchantHashCode = Config.MERCHANT_PAYNOW_HASH_CODE;
        createInvoiceToken.createInvoiceCreateToken(merchantId, merchantAccessCode, merchantHashCode);

    }

    private static void makeInvoiceAndPaymentWithToken() {
        CreateInvoiceToken createInvoiceToken = new CreateInvoiceToken();
        String merchantId = Config.MERCHANT_PAYNOW_ID;
        String merchantAccessCode = Config.MERCHANT_PAYNOW_ACCESS_CODE;
        String merchantHashCode = Config.MERCHANT_PAYNOW_HASH_CODE;
        String tokenNum = "5123451517076481";
        String tokenExp = "1225";
        createInvoiceToken.createInvoiceAndPaymentWithToken(merchantId, merchantAccessCode, merchantHashCode, tokenNum,
                tokenExp);
    }

    private static void makeInvoiceInstallmentAtOnePaySite() {
        CreateInvoiceInstallment createInvoiceInstallment = new CreateInvoiceInstallment();
        String merchantId = Config.MERCHANT_INSTALLMENT_ID;
        String merchantAccessCode = Config.MERCHANT_INSTALLMENT_ACCESS_CODE;
        String merchantHashCode = Config.MERCHANT_INSTALLMENT_HASH_CODE;
        createInvoiceInstallment.createInvoiceInstallment(merchantId, merchantAccessCode, merchantHashCode);
    }

    private static void makeInvoiceInstallmentAtMerchantSite() {
        CreateInvoiceInstallment createInvoiceInstallment = new CreateInvoiceInstallment();
        String merchantId = Config.MERCHANT_INSTALLMENT_ID;
        String merchantAccessCode = Config.MERCHANT_INSTALLMENT_ACCESS_CODE;
        String merchantHashCode = Config.MERCHANT_INSTALLMENT_HASH_CODE;
        String cardList = "VC";
        String itaTime = "3";
        String itaBankSwiftCode = "BFTVVNVX";
        String amount = "1000000000";
        String itaFeeAmount = "20000200";
        createInvoiceInstallment.createInvoiceInstallmentThemeIta(merchantId, merchantAccessCode, merchantHashCode,
                amount, cardList, itaTime, itaFeeAmount, itaBankSwiftCode);
    }

    private static void getInstallmentByMerchantId() {
        CreateInvoiceInstallment createInvoiceInstallment = new CreateInvoiceInstallment();
        String merchantId = Config.MERCHANT_INSTALLMENT_ID;
        String merchantHashCode = Config.MERCHANT_INSTALLMENT_HASH_CODE;
        String amount = "3000000";
        createInvoiceInstallment.getInstallment(amount, merchantId, merchantHashCode);

    }

    private static void queryTransaction() {
        QueryDR queryDR = new QueryDR();
        String merchantId = Config.MERCHANT_PAYNOW_ID;
        String merchantAccessCode = Config.MERCHANT_PAYNOW_ACCESS_CODE;
        String merchantHashCode = Config.MERCHANT_PAYNOW_HASH_CODE;
        String merchTxnRef = "TEST_638466347228355308";
        queryDR.QueryDRApi(merchantId, merchantAccessCode, merchantHashCode, merchTxnRef);
    }

    private static void verifySign() {
        String url = "https://webhook.site/6ddd97bd-d51c-4c36-b16b-7755ce68a010?vpc_Amount=10000000&vpc_Command=pay&vpc_MerchTxnRef=TEST_1711444940129917200&vpc_Merchant=DUONGTT&vpc_Message=Canceled&vpc_OrderInfo=Ma+Don+Hang&vpc_TxnResponseCode=99&vpc_Version=2&vpc_SecureHash=E083AFF7100E679678A68E6FDCB4751A75C2775EB42A5D24209E42B7665313D0";
        String merchantHashCode = Config.MERCHANT_PAYNOW_HASH_CODE;
        VerifyVpcSecureHash verifyVpcSecureHash = new VerifyVpcSecureHash();
        verifyVpcSecureHash.onePayVerifySecureHash(url, merchantHashCode);
    }
}
