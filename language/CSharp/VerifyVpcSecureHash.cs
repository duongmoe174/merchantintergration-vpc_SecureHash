using System.Web;
using AppUtil;

namespace AppVerify
{
    class VerifyVpcSecureHash
    {
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

            var dicSorted = Util.SortParamsMap(queryParamMap);
            var stringToHash = Util.GenerateStringToHash(dicSorted);
            var OnePaySign = Util.GenSecurehash(stringToHash, merchantHashCode);
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
    }
}