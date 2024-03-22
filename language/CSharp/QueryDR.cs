using System.ComponentModel.DataAnnotations;
using System.Web;
using AppConfig;
using AppUtil;

namespace AppQueryDR
{
    class QueryDR
    {
        public static void QueryDRApi(string merchantId, string merchantAccessCode, string merchantHashCode, string merchTxnRef)
        {
            Dictionary<string, string> merchantParams = new Dictionary<string, string>
            {
                { "vpc_Version", "2" },
                { "vpc_Command", "queryDR" },
                { "vpc_AccessCode", merchantAccessCode },
                { "vpc_Merchant", merchantId },
                { "vpc_Password", "admin@123456" },
                { "vpc_User", "Administrator" },
                { "vpc_MerchTxnRef", merchTxnRef },
            };
            Dictionary<string, string> dictSorted = Util.SortParamsMap(merchantParams);
            var stringToHash = Util.GenerateStringToHash(dictSorted);
            Console.WriteLine("merchant's string to hash: " + stringToHash);
            var sign = Util.GenSecurehash(stringToHash, merchantHashCode);
            merchantParams.Add("vpc_SecureHash", sign);

            Dictionary<string, string> headerRequest = new Dictionary<string, string>();

            string url = Config.BASE_URL + "/msp/api/v1/vpc/invoices/queries";

            Util.ExcutePostMethod(url, merchantParams, headerRequest);
        }
    }
}