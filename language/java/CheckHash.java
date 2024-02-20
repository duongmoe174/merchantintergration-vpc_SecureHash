
import java.util.Date;
import java.util.HashMap;
import java.util.Map;
import java.util.TreeMap;
import javax.crypto.Mac;
import javax.crypto.spec.SecretKeySpec;
import static java.nio.charset.StandardCharsets.UTF_8;
import java.io.DataOutputStream;
import java.net.*;
import java.text.SimpleDateFormat;

public class CheckHash {
    private static final String BASE_URL = "https://mtf.onepay.vn/paygate/vpcpay.op";

    private static final char[] hexCode = "0123456789ABCDEF".toCharArray();
    private static final String MERCHANT_HASH_CODE = "6D0870CDE5F24F34F3915FB0045120DB"; // TESTONEPAY

    public static void main(String[] args) throws Exception {
        System.out.println(createHttpsRequest());
    }

    private static String merchantSendRequestStatic() throws Exception {
        Map<String, Object> param = new HashMap<>();
        param.put("vpc_Version", "2");
        param.put("vpc_Currency", "VND");
        param.put("vpc_Command", "pay");
        param.put("vpc_AccessCode", "6BEB2556");
        param.put("vpc_Merchant", "TESTONEPAY35");
        param.put("vpc_Locale", "vn");
        param.put("vpc_ReturnURL", "https://mtf.onepay.vn/client/qt/dr/");
        param.put("vpc_MerchTxnRef", "DH-40911279711700196620");
        param.put("vpc_OrderInfo", "Ma Don Hang");
        param.put("vpc_Amount", "10000000");
        param.put("vpc_TicketNo", "192.168.166.149");
        param.put("vpc_CardList", "VC");
        param.put("AgainLink", "https://mtf.onepay.vn/client/qt/");
        param.put("Title", "PHP VPC 3-Party");
        param.put("vpc_Customer_Phone", "84987654321");
        param.put("vpc_Customer_Email", "test@onepay.vn");
        param.put("vpc_Customer_Id", "test");

        TreeMap<String, Object> queryParamSorted = sortMap(param);
        String stringToHash = generateStringToHash(queryParamSorted);
        System.out.println("merchant's string to hash: " + stringToHash);
        String merchantSecureHash = generateSecureHash(stringToHash, MERCHANT_HASH_CODE);

        param.put("vpc_SecureHash", merchantSecureHash);
        String requestParam = "";
        for (Map.Entry<String, Object> items : param.entrySet()) {
            String key = items.getKey();
            String value = items.getValue().toString();
            requestParam += key + "=" + URLEncoder.encode(value, "UTF-8") + "&";
        }
        return requestParam;
    }

    private static String merchantSendRequestDynamic() throws Exception {

        SimpleDateFormat sdfDate = new SimpleDateFormat("yyyyMMddHHmmss");// dd/MM/yyyy
        Date now = new Date();
        String strDate = sdfDate.format(now);
        String vpcMerchTxnRef = "TEST_" + strDate;
        Map<String, Object> param = new HashMap<>();
        param.put("vpc_Version", "2");
        param.put("vpc_Currency", "VND");
        param.put("vpc_Command", "pay");
        param.put("vpc_AccessCode", "6BEB2546");
        param.put("vpc_Merchant", "TESTONEPAY");
        param.put("vpc_Locale", "vn");
        param.put("vpc_ReturnURL", "https://mtf.onepay.vn/client/qt/dr/?duongtt=duongtt&mode=TEST_PAYGATE");
        param.put("vpc_MerchTxnRef", vpcMerchTxnRef);
        param.put("vpc_OrderInfo", "Ma Don Hang");
        param.put("vpc_Amount", "10000000");
        param.put("vpc_TicketNo", "192.168.166.149");
        param.put("vpc_CardList", "VC");
        param.put("AgainLink", "https://mtf.onepay.vn/client/qt/");
        param.put("Title", "PHP VPC 3-Party");
        param.put("vpc_Customer_Phone", "84987654321");
        param.put("vpc_Customer_Email", "test@onepay.vn");
        param.put("vpc_Customer_Id", "test");

        TreeMap<String, Object> queryParamSorted = sortMap(param);
        String stringToHash = generateStringToHash(queryParamSorted);
        System.out.println("merchant's string to hash: " + stringToHash);
        String merchantSecureHash = generateSecureHash(stringToHash, MERCHANT_HASH_CODE);
        param.put("vpc_SecureHash", merchantSecureHash);
        String requestParam = "";
        for (Map.Entry<String, Object> items : param.entrySet()) {
            String key = items.getKey();
            String value = items.getValue().toString();
            requestParam += key + "=" + URLEncoder.encode(value, "UTF-8") + "&";
        }
        return requestParam;
    }

    private static void onePayVerifySecureHash(String merchantUrlRequest, String merchantHashCode) throws Exception {
        System.out.println("merchant send request: " + merchantUrlRequest);
        Map<String, Object> queryParam = parseQueryParams(merchantUrlRequest);
        TreeMap<String, Object> paramSorted = sortMap(queryParam);
        String stringToHash = generateStringToHash(paramSorted);
        System.out.println("onepay's string to hash: " + stringToHash + "|");
        String onePayVpcSecureHash = generateSecureHash(stringToHash, merchantHashCode);
        String merchantVpcSecureHash = queryParam.get("vpc_SecureHash").toString();
        System.out.println("merchant's vpc_SecureHash: " + merchantVpcSecureHash);
        System.out.println("OnePay's vpc_Securehash: " + onePayVpcSecureHash);
        if (onePayVpcSecureHash.equals(merchantVpcSecureHash)) {
            System.out.println("vpc_SecureHash is valid");
        } else {
            System.out.println("vpc_SecureHash is invalid");
        }
    }

    public static String printHexBinary(byte[] data) {
        StringBuilder r = new StringBuilder(data.length * 2);
        for (byte b : data) {
            r.append(hexCode[(b >> 4) & 0xF]);
            r.append(hexCode[(b & 0xF)]);
        }
        return r.toString();
    }

    private static int hexToBin(char ch) {
        if ('0' <= ch && ch <= '9')
            return ch - '0';
        if ('A' <= ch && ch <= 'F')
            return ch - 'A' + 10;
        if ('a' <= ch && ch <= 'f')
            return ch - 'a' + 10;
        return -1;
    }

    public static byte[] parseHexBinary(String s) {
        final int len = s.length();

        // "111" is not a valid hex encoding.
        if (len % 2 != 0)
            throw new IllegalArgumentException("hexBinary needs to be even-length: " + s);

        byte[] out = new byte[len / 2];

        for (int i = 0; i < len; i += 2) {
            int h = hexToBin(s.charAt(i));
            int l = hexToBin(s.charAt(i + 1));
            if (h == -1 || l == -1)
                throw new IllegalArgumentException("contains illegal character for hexBinary: " + s);

            out[i / 2] = (byte) (h * 16 + l);
        }

        return out;
    }

    private static TreeMap<String, Object> sortMap(Map<String, Object> queryParamMap) {
        return new TreeMap<>(queryParamMap);
    }

    private static String generateStringToHash(Map<String, Object> queryParamSorted) {
        String stringToHash = "";
        for (Map.Entry<String, Object> items : queryParamSorted.entrySet()) {
            String key = items.getKey();
            String value = items.getValue().toString();
            String pref4 = key.substring(0, 4);
            String pref5 = key.substring(0, 5);
            if ("vpc_".equals(pref4) || "user_".equals(pref5)) {
                if (!"vpc_SecureHash".equals(key) && !"vpc_SecureHashType".equals(key)) {
                    if (value.length() > 0) {
                        if (stringToHash.length() > 0) {
                            stringToHash += "&";
                        }
                        stringToHash += key + "=" + value;
                    }
                }
            }
        }
        return stringToHash;
    }

    private static String generateSecureHash(String stringToHash, String merchantHashCode) throws Exception {
        byte[] merchantHashHex = parseHexBinary(merchantHashCode);
        SecretKeySpec signingKey = new SecretKeySpec(merchantHashHex, "HMACSHA256");
        Mac mac = Mac.getInstance("HMACSHA256");
        mac.init(signingKey);
        String secureHash = printHexBinary(mac.doFinal(stringToHash.getBytes(UTF_8)));
        return secureHash;
    }

    public static Map<String, Object> parseQueryParams(String url) throws Exception {
        String queryString = new URL(url).getQuery();
        if (queryString == null) {
            return new HashMap<>();
        }
        Map<String, Object> params = new HashMap<>();
        if (queryString != null && queryString.length() > 0) {
            for (String kv : queryString.split("&")) {
                if (kv != null && kv.contains("=")) {
                    String[] aryKV = kv.split("=");
                    if (aryKV.length == 2) {
                        params.put(URLDecoder.decode(aryKV[0], "UTF-8"), URLDecoder.decode(aryKV[1], "UTF-8"));
                    } else {
                        params.put(URLDecoder.decode(aryKV[0], "UTF-8"), "");
                    }
                }
            }
        }
        return params;
    }

    private static String createHttpsRequest() {
        try {
            String parameter = merchantSendRequestDynamic();
            return executeGet(BASE_URL, parameter);
        } catch (Exception e) {
            System.out.println(e);
            return "";
        }
    }

    public static String executeGet(String targetURL, String urlParameters) {
        HttpURLConnection connection = null;
        try {
            // Create connection
            URL url = new URL(targetURL);
            connection = (HttpURLConnection) url.openConnection();
            connection.setRequestMethod("GET");
            connection.setRequestProperty("Content-Type",
                    "*/*");
            connection.setRequestProperty("Host", "mtf.onepay.vn");
            connection.setInstanceFollowRedirects(false);
            connection.setUseCaches(false);
            connection.setDoOutput(true);
            System.out.println("url parameter: " + urlParameters);
            // Send request
            DataOutputStream wr = new DataOutputStream(
                    connection.getOutputStream());
            wr.writeBytes(urlParameters);
            wr.close();

            // Get Response
            String headerLocation = connection.getHeaderField("location");
            return headerLocation;
        } catch (Exception e) {
            e.printStackTrace();
            return null;
        } finally {
            if (connection != null) {
                connection.disconnect();
            }
        }
    }

    public static String checkHash() throws Exception {
        {
            Map<String, Object> param = new HashMap<>();
            param.put("vpc_AccessCode", "D0608B06");
            param.put("vpc_Amount", "100000");
            param.put("vpc_Command", "refund");
            param.put("vpc_MerchTxnRef", "2023112210522019671");
            param.put("vpc_Merchant", "OP_SHOPDUNK");
            param.put("vpc_Operator", "Administrator");
            param.put("vpc_OrgMerchTxnRef", "2023112210510819671");
            param.put("vpc_Version", "2");

            TreeMap<String, Object> queryParamSorted = sortMap(param);
            String stringToHash = generateStringToHash(queryParamSorted);
            System.out.println("merchant's string to hash: " + stringToHash);
            String merchantSecureHash = generateSecureHash(stringToHash, MERCHANT_HASH_CODE);

            param.put("vpc_SecureHash", merchantSecureHash);
            String requestParam = "";
            for (Map.Entry<String, Object> items : param.entrySet()) {
                String key = items.getKey();
                String value = items.getValue().toString();
                requestParam += key + "=" + value + "&";
            }

            String requestUrl = BASE_URL + "?" + requestParam;

            return requestUrl;
        }
    }
}
