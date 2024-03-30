package code;

import java.net.URLEncoder;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Date;
import java.util.HashMap;
import java.util.List;
import java.util.Map;
import java.util.TreeMap;

public class CreateInvoiceInstallment {
    protected void createInvoiceInstallment(String merchantId, String merchantAccessCode, String merchantHashCode) {
        try {
            SimpleDateFormat sdfDate = new SimpleDateFormat("yyyyMMddHHmmss");// dd/MM/yyyy
            Date now = new Date();
            String strDate = sdfDate.format(now);
            String vpcMerchTxnRef = "TEST_" + strDate;
            Map<String, Object> param = new HashMap<>();
            param.put("vpc_Version", "2");
            param.put("vpc_Currency", "VND");
            param.put("vpc_Command", "pay");
            param.put("vpc_AccessCode", merchantAccessCode);
            param.put("vpc_Merchant", merchantId);
            param.put("vpc_Locale", "vn");
            param.put("vpc_ReturnURL", "https://mtf.onepay.vn/client/qt/dr/?duongtt=duongtt&mode=TEST_PAYGATE");
            param.put("vpc_MerchTxnRef", vpcMerchTxnRef);
            param.put("vpc_OrderInfo", "Ma Don Hang");
            param.put("vpc_Amount", "1000000000"); // amount must >= 3.000.000 VND
            param.put("vpc_TicketNo", "192.168.166.149");
            param.put("AgainLink", "https://mtf.onepay.vn/client/qt/");
            param.put("Title", "PHP VPC 3-Party");
            param.put("vpc_Customer_Phone", "84987654321");
            param.put("vpc_Customer_Email", "test@onepay.vn");
            param.put("vpc_Customer_Id", "test");

            TreeMap<String, Object> queryParamSorted = Util.sortMap(param);
            String stringToHash = Util.generateStringToHash(queryParamSorted);
            System.out.println("merchant's string to hash: " + stringToHash);
            String merchantSecureHash = Util.generateSecureHash(stringToHash, merchantHashCode);
            param.put("vpc_SecureHash", merchantSecureHash);
            String requestParam = "";
            for (Map.Entry<String, Object> items : param.entrySet()) {
                String key = items.getKey();
                String value = items.getValue().toString();
                requestParam += key + "=" + URLEncoder.encode(value, "UTF-8") + "&";
            }
            String requestUrl = Config.BASE_URL + Config.URL_PREFIX + requestParam;
            Util.executeGet(requestUrl);

        } catch (Exception e) {
            System.out.println(e.getMessage());
        }
    }

    protected void createInvoiceInstallmentThemeIta(String merchantId, String merchantAccessCode,
            String merchantHashCode, String amount, String cardList, String itaTime, String itaFeeAmount,
            String itaBankSwiftCode) {
        try {
            SimpleDateFormat sdfDate = new SimpleDateFormat("yyyyMMddHHmmss");// dd/MM/yyyy
            Date now = new Date();
            String strDate = sdfDate.format(now);
            String vpcMerchTxnRef = "TEST_" + strDate;
            Map<String, Object> param = new HashMap<>();
            param.put("vpc_Version", "2");
            param.put("vpc_Currency", "VND");
            param.put("vpc_Command", "pay");
            param.put("vpc_AccessCode", merchantAccessCode);
            param.put("vpc_Merchant", merchantId);
            param.put("vpc_Locale", "vn");
            param.put("vpc_ReturnURL", "https://mtf.onepay.vn/client/qt/dr/?duongtt=duongtt&mode=TEST_PAYGATE");
            param.put("vpc_MerchTxnRef", vpcMerchTxnRef);
            param.put("vpc_OrderInfo", "Ma Don Hang");
            param.put("vpc_Amount", amount); // amount must >= 3.000.000 VND
            param.put("vpc_TicketNo", "192.168.166.149");
            param.put("AgainLink", "https://mtf.onepay.vn/client/qt/");
            param.put("Title", "PHP VPC 3-Party");
            param.put("vpc_Customer_Phone", "84987654321");
            param.put("vpc_Customer_Email", "test@onepay.vn");
            param.put("vpc_Customer_Id", "test");
            param.put("vpc_CardList", cardList);
            param.put("vpc_ItaTime", itaTime);
            param.put("vpc_ItaFeeAmount", itaFeeAmount);
            param.put("vpc_ItaBank", itaBankSwiftCode);
            param.put("vpc_Theme", "ita");

            TreeMap<String, Object> queryParamSorted = Util.sortMap(param);
            String stringToHash = Util.generateStringToHash(queryParamSorted);
            System.out.println("merchant's string to hash: " + stringToHash);
            String merchantSecureHash = Util.generateSecureHash(stringToHash, merchantHashCode);
            param.put("vpc_SecureHash", merchantSecureHash);
            String requestParam = "";
            for (Map.Entry<String, Object> items : param.entrySet()) {
                String key = items.getKey();
                String value = items.getValue().toString();
                requestParam += key + "=" + URLEncoder.encode(value, "UTF-8") + "&";
            }
            String requestUrl = Config.BASE_URL + Config.URL_PREFIX + requestParam;
            Util.executeGet(requestUrl);

        } catch (Exception e) {
            System.out.println(e.getMessage());
        }
    }

    protected void getInstallment(String amount, String merchantId, String merchantHashCode) {
        List<String> signedHeaderNames = new ArrayList<>();
        signedHeaderNames.add("(request-target)");
        signedHeaderNames.add("(created)");
        signedHeaderNames.add("host");
        signedHeaderNames.add("accept");

        String accept = "";
        String uriPath = "/msp/api/v1/merchants/" + merchantId + "/installments?amount=" + amount;
        Map<String, String> headerSign = new HashMap<>();
        headerSign.put("Host", Config.HOST);
        headerSign.put("Accept", accept);

        String method = "GET";
        String signature = Util.createRequestSignatureITA(method, uriPath, signedHeaderNames, headerSign, merchantId,
                merchantHashCode);

        Map<String, String> headerRequest = new HashMap<>();
        headerRequest.put("accept", accept);
        headerRequest.put("Host", Config.HOST);
        headerRequest.put("signature", signature);

        String urlRequest = Config.BASE_URL + uriPath;

        Util.executeGetWithHeader(urlRequest, headerRequest);
    }
}
