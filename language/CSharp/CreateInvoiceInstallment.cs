using System.Web;
using AppConfig;
using AppUtil;

namespace AppCreateInvoiceInstallment
{
    class CreateInvoiceInstallment
    {
        public static void MerchantSendRequet(string merchantId, string merchantAccessCode, string merchantHashCode)
        {
            long ticks = DateTime.Now.Ticks;
            string vpcMerchantTxnRef = "TEST_" + ticks.ToString();
            Dictionary<string, string> merchantParams = new Dictionary<string, string>
            {
                { "vpc_Version", "2" },
                { "vpc_Currency", "VND" },
                { "vpc_Command", "pay" },
                { "vpc_AccessCode", merchantAccessCode },
                { "vpc_Merchant", merchantId },
                { "vpc_Locale", "vn" },
                { "vpc_ReturnURL", "https://mtf.onepay.vn/client/qt/dr/?duongtt=duongtt&mode=TEST_PAYGATE" },
                { "vpc_MerchTxnRef", vpcMerchantTxnRef },
                { "vpc_OrderInfo", "Ma Don Hang" },
                { "vpc_Amount", "1000000000" }, // amount must >= 3.000.000 VND
                { "vpc_TicketNo", "192.168.166.149" },
                { "AgainLink", "https://mtf.onepay.vn/client/qt/" },
                { "Title", "PHP VPC 3-Party" },
                { "vpc_Customer_Phone", "84987654321" },
                { "vpc_Customer_Email", "test@onepay.vn" },
                { "vpc_Customer_Id", "test" }
            };
            Dictionary<string, string> dictSorted = Util.SortParamsMap(merchantParams);
            var stringToHash = Util.GenerateStringToHash(dictSorted);
            Console.WriteLine("merchant's string to hash: " + stringToHash);
            var sign = Util.GenSecurehash(stringToHash, merchantHashCode);
            merchantParams.Add("vpc_SecureHash", sign);
            var queryParam = "";
            foreach (var items in merchantParams)
            {
                var key = items.Key;
                var value = items.Value;
                queryParam += key + "=" + HttpUtility.UrlEncode(value) + "&";
            }
            var requestsUrl = Config.BASE_URL + Config.URL_PREFIX + queryParam;
            Util.ExcuteGetMethod(requestsUrl);
        }

        public static void MerchantSendRequetThemeITA(string merchantId, string merchantAccessCode, string merchantHashCode, string amount, string cardList, string itaTime, string itaFeeAmount, string itaBankSwiftCode)
        {
            long ticks = DateTime.Now.Ticks;
            string vpcMerchantTxnRef = "TEST_" + ticks.ToString();
            Dictionary<string, string> merchantParams = new Dictionary<string, string>
            {
                { "vpc_CardList", cardList },
                { "vpc_ItaTime", itaTime },
                { "vpc_ItaFeeAmount", itaFeeAmount },
                { "vpc_ItaBank", itaBankSwiftCode },
                { "vpc_Theme", "ita" },
                { "vpc_Version", "2" },
                { "vpc_Currency", "VND" },
                { "vpc_Command", "pay" },
                { "vpc_AccessCode", merchantAccessCode },
                { "vpc_Merchant", merchantId },
                { "vpc_Locale", "vn" },
                { "vpc_ReturnURL", "https://mtf.onepay.vn/client/qt/dr/?duongtt=duongtt&mode=TEST_PAYGATE" },
                { "vpc_MerchTxnRef", vpcMerchantTxnRef },
                { "vpc_OrderInfo", "Ma Don Hang" },
                { "vpc_Amount", amount },
                { "vpc_TicketNo", "192.168.166.149" },
                { "AgainLink", "https://mtf.onepay.vn/client/qt/" },
                { "Title", "PHP VPC 3-Party" },
                { "vpc_Customer_Phone", "84987654321" },
                { "vpc_Customer_Email", "test@onepay.vn" },
                { "vpc_Customer_Id", "test" }
            };
            Dictionary<string, string> dictSorted = Util.SortParamsMap(merchantParams);
            var stringToHash = Util.GenerateStringToHash(dictSorted);
            Console.WriteLine("merchant's string to hash: " + stringToHash);
            var sign = Util.GenSecurehash(stringToHash, merchantHashCode);
            merchantParams.Add("vpc_SecureHash", sign);
            var queryParam = "";
            foreach (var items in merchantParams)
            {
                var key = items.Key;
                var value = items.Value;
                queryParam += key + "=" + HttpUtility.UrlEncode(value) + "&";
            }
            var requestsUrl = Config.BASE_URL + Config.URL_PREFIX + queryParam;
            Util.ExcuteGetMethod(requestsUrl);
        }

        public static void GetInstallment(string amount, string merchantId, string merchantHashCode)
        {
            List<string> signedHeaderNames = new List<string>();
            signedHeaderNames.Add("(request-target)");
            signedHeaderNames.Add("(created)");
            signedHeaderNames.Add("host");
            signedHeaderNames.Add("accept");
            string accept = "";
            string uriPath = "/msp/api/v1/merchants/" + merchantId + "/installments?amount=" + amount;
            Dictionary<string, string> headerSign = new Dictionary<string, string>();
            headerSign.Add("Host", Config.HOST);
            headerSign.Add("Accept", accept);
            string method = "GET";
            string signature = Util.CreateRequestSignatureITA(method, uriPath, signedHeaderNames, headerSign, merchantId, merchantHashCode);

            Dictionary<string, string> headerRequest = new Dictionary<string, string>();
            headerRequest.Add("accept", accept);
            headerRequest.Add("Host", Config.HOST);
            headerRequest.Add("signature", signature);

            string url = Config.BASE_URL + uriPath;

            Util.ExcuteGetMethodWithHeaders(url, headerRequest);
        }
    }
}