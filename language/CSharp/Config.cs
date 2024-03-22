namespace AppConfig
{
    public interface Config
    {
        public static readonly string MERCHANT_PAYNOW_ID = "DUONGTT";
        public static readonly string MERCHANT_PAYNOW_ACCESS_CODE = "6BEB2546";
        public static readonly string MERCHANT_PAYNOW_HASH_CODE = "6D0870CDE5F24F34F3915FB0045120DB";
        public static readonly string MERCHANT_INSTALLMENT_ID = "TESTTRAGOP";
        public static readonly string MERCHANT_INSTALLMENT_ACCESS_CODE = "D51C5CD6";
        public static readonly string MERCHANT_INSTALLMENT_HASH_CODE = "EB1B7F75EBB2FAABD6763FC37A3628AF";
        public static readonly string BASE_URL = "https://mtf.onepay.vn";
        public static readonly string URL_PREFIX = "/paygate/vpcpay.op?";

        public static readonly string HOST = "mtf.onepay.vn";
    }
}