using System;
using System.Web;
using System.Security.Cryptography;
using System.Text;
using System.Net.Http;
using System.Net;
namespace AppUtil
{
    class Util
    {
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

        public static void ExcuteGetMethod(string url)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.AllowAutoRedirect = false;
            using var webResponse = request.GetResponse();
            WebHeaderCollection header = webResponse.Headers;
            var link = header["Location"];
            Console.WriteLine("Link redirect: " + link);
        }

        public static void ExcutePostMethod(string url, Dictionary<string, string> formData, Dictionary<string, string> headerRequest)
        {
            Console.WriteLine("========== start call http post =========");
            Console.WriteLine(url);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded"; // Set content type to x-www-form-urlencoded

            foreach (var entry in headerRequest)
            {
                string key = entry.Key;
                string value = entry.Value;
                request.Headers.Add(key, value);
            }

            // Convert form data to x-www-form-urlencoded format
            string encodedFormData = string.Join("&", formData.Select(kvp => $"{WebUtility.UrlEncode(kvp.Key)}={WebUtility.UrlEncode(kvp.Value)}"));

            // Write the form data to the request stream
            using (StreamWriter writer = new StreamWriter(request.GetRequestStream()))
            {
                writer.Write(encodedFormData);
                writer.Flush();
            }

            try
            {
                // Get the response
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    string result = reader.ReadToEnd();
                    Console.WriteLine(result);
                }
            }
            catch (WebException ex)
            {
                // Handle exceptions
                if (ex.Response is HttpWebResponse errorResponse)
                {
                    Console.WriteLine($"Error: {errorResponse.StatusCode} - {errorResponse.StatusDescription}");
                    // Handle error response
                }
                else
                {
                    Console.WriteLine($"Exception: {ex.Message}");
                }
            }
        }
    }
}