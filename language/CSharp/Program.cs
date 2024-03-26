using AppCreateInvoice;
using AppCreateInvoiceToken;
using AppConfig;
using AppCreateInvoiceInstallment;
using AppQueryDR;
using AppVerify;
namespace CheckHash
{
    class Program
    {
        static void Main(string[] args)
        {
            VerifySign();
        }

        public static void MakeInvoice()
        {
            string merchantId = Config.MERCHANT_PAYNOW_ID;
            string merchantAccessCode = Config.MERCHANT_PAYNOW_ACCESS_CODE;
            string merchantHashCode = Config.MERCHANT_PAYNOW_HASH_CODE;
            CreateInvoice.MerchantSendRequet(merchantId, merchantAccessCode, merchantHashCode);
        }

        public static void MakeInvoiceAndCreateToken()
        {
            string merchantId = Config.MERCHANT_PAYNOW_ID;
            string merchantAccessCode = Config.MERCHANT_PAYNOW_ACCESS_CODE;
            string merchantHashCode = Config.MERCHANT_PAYNOW_HASH_CODE;
            CreateInvoiceToken.MerchantSendRequetToCreateToken(merchantId, merchantAccessCode, merchantHashCode);
        }

        public static void MakeInvoiceAndPaymentWithToken()
        {
            string merchantId = Config.MERCHANT_PAYNOW_ID;
            string merchantAccessCode = Config.MERCHANT_PAYNOW_ACCESS_CODE;
            string merchantHashCode = Config.MERCHANT_PAYNOW_HASH_CODE;
            CreateInvoiceToken.MerchantSendRequetToPaymentWithToken(merchantId, merchantAccessCode, merchantHashCode);
        }

        public static void MakeInvoiceInstallmentAtOnePaySite()
        {
            string merchantId = Config.MERCHANT_INSTALLMENT_ID;
            string merchantAccessCode = Config.MERCHANT_INSTALLMENT_ACCESS_CODE;
            string merchantHashCode = Config.MERCHANT_INSTALLMENT_HASH_CODE;
            CreateInvoiceInstallment.MerchantSendRequet(merchantId, merchantAccessCode, merchantHashCode);
        }

        public static void MakeInvoiceInstallmentAtMerchantSite()
        {
            string merchantId = Config.MERCHANT_INSTALLMENT_ID;
            string merchantAccessCode = Config.MERCHANT_INSTALLMENT_ACCESS_CODE;
            string merchantHashCode = Config.MERCHANT_INSTALLMENT_HASH_CODE;

            string cardList = "VC";
            string itaTime = "3";
            string itaBankSwiftCode = "BFTVVNVX";
            string amount = "1000000000";
            string itaFeeAmount = "20000200";

            CreateInvoiceInstallment.MerchantSendRequetThemeITA(merchantId, merchantAccessCode, merchantHashCode, amount, cardList, itaTime, itaFeeAmount, itaBankSwiftCode);
        }

        public static void GetInstallmentByMerchantId()
        {
            string merchantId = Config.MERCHANT_INSTALLMENT_ID;
            string merchantHashCode = Config.MERCHANT_INSTALLMENT_HASH_CODE;
            string amount = "3000000";
            CreateInvoiceInstallment.GetInstallment(amount, merchantId, merchantHashCode);
        }

        public static void QueryTransaction()
        {
            string merchantId = Config.MERCHANT_PAYNOW_ID;
            string merchantAccessCode = Config.MERCHANT_PAYNOW_ACCESS_CODE;
            string merchantHashCode = Config.MERCHANT_PAYNOW_HASH_CODE;
            string merchTxnRef = "TEST_638466347228355308";
            QueryDR.QueryDRApi(merchantId, merchantAccessCode, merchantHashCode, merchTxnRef);
        }

        public static void VerifySign()
        {
            string url = "https://webhook.site/6ddd97bd-d51c-4c36-b16b-7755ce68a010?vpc_Amount=10000000&vpc_Command=pay&vpc_MerchTxnRef=TEST_1711444940129917200&vpc_Merchant=DUONGTT&vpc_Message=Canceled&vpc_OrderInfo=Ma+Don+Hang&vpc_TxnResponseCode=99&vpc_Version=2&vpc_SecureHash=E083AFF7100E679678A68E6FDCB4751A75C2775EB42A5D24209E42B7665313D0";
            string merchantId = Config.MERCHANT_PAYNOW_HASH_CODE;
            VerifyVpcSecureHash.OnePayVerifySecureHash(url, merchantId);
        }
    }
}