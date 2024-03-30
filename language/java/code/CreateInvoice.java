package code;

import java.net.URLEncoder;
import java.text.SimpleDateFormat;
import java.util.Date;
import java.util.HashMap;
import java.util.Map;
import java.util.TreeMap;

public class CreateInvoice {

    protected void createInvoice(String merchantId, String merchantAccessCode, String merchantHashCode) {
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
            param.put("vpc_Amount", "10000000");
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
}
