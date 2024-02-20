using System;
using System.Web;
using System.Security.Cryptography;
using System.Text;
using System.Net.Http;
using System.Net;

namespace CheckHash
{
    class Program
    {
        public static string merchantHashCode = "6D0870CDE5F24F34F3915FB0045120DB";
        public static string BASE_URL = "https://mtf.onepay.vn/paygate/vpcpay.op";
        static void Main(string[] args)
        {
            // var url = MerchantSendRequestStatic();
            // Console.WriteLine("merchant request: " + url);
            // OnePayVerifySecureHash(url, merchantHashCode);

            ExcuteGetMethod();
        }

        public static string MerchantSendRequestStatic()
        {
            Dictionary<string, string> merchantParams = new Dictionary<string, string>
            {
                { "vpc_Version", "2" },
                { "vpc_Currency", "VND" },
                { "vpc_Command", "pay" },
                { "vpc_AccessCode", "6BEB2546" },
                { "vpc_Merchant", "TESTONEPAY" },
                { "vpc_Locale", "vn" },
                { "vpc_ReturnURL", "https://mtf.onepay.vn/client/qt/dr/" },
                { "vpc_MerchTxnRef", "TEST_1698291680035" },
                { "vpc_OrderInfo", "Ma Don Hang" },
                { "vpc_Amount", "10000000" },
                { "vpc_TicketNo", "192.168.166.149" },
                { "vpc_CardList", "VC" },
                { "AgainLink", "https://mtf.onepay.vn/client/qt/" },
                { "Title", "PHP VPC 3-Party" },
                { "vpc_Customer_Phone", "84987654321" },
                { "vpc_Customer_Email", "test@onepay.vn" },
                { "vpc_Customer_Id", "test" }
            };
            Dictionary<string, string> dictSorted = SortParamsMap(merchantParams);
            var stringToHash = GenerateStringToHash(dictSorted);
            Console.WriteLine("merchant's string to hash: " + stringToHash);
            var sign = GenSecurehash(stringToHash, merchantHashCode);
            merchantParams.Add("vpc_SecureHash", sign);
            var queryParam = "";
            foreach (var items in merchantParams)
            {
                var key = items.Key;
                var value = items.Value;
                queryParam += key + "=" + HttpUtility.UrlEncode(value) + "&";
            }
            var requetsUrl = BASE_URL + "?" + queryParam;
            return requetsUrl;
        }

        public static string MerchantSendRequestDynamic()
        {
            long ticks = DateTime.Now.Ticks;
            string vpcMerchantTxnRef = "TEST_" + ticks.ToString();
            Dictionary<string, string> merchantParams = new Dictionary<string, string>
            {
                { "vpc_Version", "2" },
                { "vpc_Currency", "VND" },
                { "vpc_Command", "pay" },
                { "vpc_AccessCode", "6BEB2546" },
                { "vpc_Merchant", "TESTONEPAY" },
                { "vpc_Locale", "vn" },
                { "vpc_ReturnURL", "https://mtf.onepay.vn/client/qt/dr/?duongtt=duongtt&mode=TEST_PAYGATE" },
                { "vpc_MerchTxnRef", vpcMerchantTxnRef },
                { "vpc_OrderInfo", "Ma Don Hang" },
                { "vpc_Amount", "10000000" },
                { "vpc_TicketNo", "192.168.166.149" },
                { "vpc_CardList", "VC" },
                { "AgainLink", "https://mtf.onepay.vn/client/qt/" },
                { "Title", "PHP VPC 3-Party" },
                { "vpc_Customer_Phone", "84987654321" },
                { "vpc_Customer_Email", "test@onepay.vn" },
                { "vpc_Customer_Id", "test" }
            };
            Dictionary<string, string> dictSorted = SortParamsMap(merchantParams);
            var stringToHash = GenerateStringToHash(dictSorted);
            Console.WriteLine("merchant's string to hash: " + stringToHash);
            var sign = GenSecurehash(stringToHash, merchantHashCode);
            merchantParams.Add("vpc_SecureHash", sign);
            var queryParam = "";
            foreach (var items in merchantParams)
            {
                var key = items.Key;
                var value = items.Value;
                queryParam += key + "=" + HttpUtility.UrlEncode(value) + "&";
            }
            var requetsUrl = BASE_URL + "?" + queryParam;
            return requetsUrl;
        }

        public static void OnePayVerifySecureHash(string url, string merchantHashCode)
        {
            Dictionary<string, string> queryParamMap = new Dictionary<string, string>();
            var uri = new Uri(url);
            var param = HttpUtility.ParseQueryString(uri.Query);
            for (int i = 0; i < param.Count; i++)
            {
                var key = param.GetKey(i);
                var value = param.Get(i);
                if (key != null && value != null)
                {
                    queryParamMap.Add(key, value);
                }
            }

            var dicSorted = SortParamsMap(queryParamMap);
            var stringToHash = GenerateStringToHash(dicSorted);
            var OnePaySign = GenSecurehash(stringToHash, merchantHashCode);
            var merchantSign = queryParamMap["vpc_SecureHash"];
            Console.WriteLine("OnePay's Sign: " + OnePaySign);
            Console.WriteLine("Merchant's Sign: " + merchantSign);
            if (OnePaySign == merchantSign)
            {
                Console.WriteLine("vpc_SecureHash is Valid");
            }
            else
            {
                Console.WriteLine("vpc_SecureHash invalid");
            }
        }
        public static byte[] HexToBytes(string hex)
        {
            var bytes = new byte[hex.Length / 2];
            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
            }
            return bytes;
        }

        public static string GenSecurehash(string notEncodeData, string merchantHashCode)
        {
            var keyHash = HexToBytes(merchantHashCode);
            var hmac = new HMACSHA256(keyHash);
            var inputBytes = Encoding.UTF8.GetBytes(notEncodeData);
            var hashBytes = hmac.ComputeHash(inputBytes);
            var sign = BitConverter.ToString(hashBytes).Replace("-", "").ToUpper();
            return sign;
        }

        public static string GenerateStringToHash(Dictionary<string, string> dictSorted)
        {
            var notEncodeData = "";
            foreach (var items in dictSorted)
            {
                var key = items.Key;
                var value = items.Value;
                var pref4 = key[..4];
                var pref5 = key[..5];
                if (pref4 == "vpc_" || pref5 == "user_")
                {
                    if (!key.Equals("vpc_SecureHashType") && !key.Equals("vpc_SecureHash"))
                    {
                        if (value.Length > 0)
                        {
                            if (notEncodeData.Length > 0)
                            {
                                notEncodeData += "&";
                            }
                            notEncodeData += key + "=" + value;
                        }
                    }
                }
            }
            return notEncodeData;
        }

        public static Dictionary<string, string> SortParamsMap(Dictionary<string, string> paramsMap)
        {
            var l = paramsMap.OrderBy(key => key.Key, StringComparer.Ordinal);
            Dictionary<string, string> dictSorted = l.ToDictionary((keyItem) => keyItem.Key, (valueItem) => valueItem.Value);
            return dictSorted;
        }

        public static void ExcuteGetMethod()
        {
            string uri = MerchantSendRequestDynamic();
            var request = (HttpWebRequest)WebRequest.Create(uri);
            request.Method = "GET";
            request.AllowAutoRedirect = false;
            using var webResponse = request.GetResponse();
            WebHeaderCollection header = webResponse.Headers;
            var link = header["Location"];
            Console.WriteLine("Link redirect: " + link);
        }
    }
}