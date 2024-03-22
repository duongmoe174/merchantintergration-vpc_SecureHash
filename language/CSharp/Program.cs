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
            GetInstallmentByMerchantId();
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
            string url = "https://webhook.site/6ddd97bd-d51c-4c36-b16b-7755ce68a010?vpc_Amount=10000000&vpc_AuthorizeId=831000&vpc_Card=MC&vpc_CardExp=1225&vpc_CardNum=512345***0008&vpc_CardUid=INS-siwpapmFOCjgUAB_AQAz_w&vpc_Command=pay&vpc_MerchTxnRef=TEST_638467148212861199&vpc_Merchant=DUONGTT&vpc_Message=Approved&vpc_OrderInfo=Ma+Don+Hang&vpc_PayChannel=WEB&vpc_TransactionNo=PAY-KerO5VHiTIyL7gt3V3MVCA&vpc_TxnResponseCode=0&vpc_Version=2&vpc_BinCountry=EC&vpc_SecureHash=B28E14D379C91A3375970F2D132E416EC835DA45407B9C139CC0E450891EFE97";
            string merchantId = Config.MERCHANT_PAYNOW_HASH_CODE;
            VerifyVpcSecureHash.OnePayVerifySecureHash(url, merchantId);
        }
    }
}