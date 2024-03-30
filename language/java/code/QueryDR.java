package code;

import java.net.URLEncoder;
import java.util.HashMap;
import java.util.Map;
import java.util.TreeMap;

public class QueryDR {
    protected void QueryDRApi(String merchantId, String merchantAccessCode, String merchantHashCode,
            String merchTxnRef) {
        try {
            Map<String, Object> param = new HashMap<>();
            param.put("vpc_Version", "2");
            param.put("vpc_Command", "queryDR");
            param.put("vpc_AccessCode", merchantAccessCode);
            param.put("vpc_Merchant", merchantId);
            param.put("vpc_Password", "admin@123456");
            param.put("vpc_User", "Administrator");
            param.put("vpc_MerchTxnRef", merchTxnRef);
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
            String urlRequest = Config.BASE_URL + "/msp/api/v1/vpc/invoices/queries";
            Util.executePost(urlRequest, requestParam);

        } catch (Exception e) {
            System.out.println(e.getMessage());
        }
    }
}
