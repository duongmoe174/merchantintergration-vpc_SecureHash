package code;

import java.util.*;

public class VerifyVpcSecureHash {
    protected void onePayVerifySecureHash(String merchantUrlRequest, String merchantHashCode) {
        try {
            System.out.println("merchant send request: " + merchantUrlRequest);
            Map<String, Object> queryParam = Util.parseQueryParams(merchantUrlRequest);
            TreeMap<String, Object> paramSorted = Util.sortMap(queryParam);
            String stringToHash = Util.generateStringToHash(paramSorted);
            System.out.println("onepay's string to hash: " + stringToHash + "|");
            String onePayVpcSecureHash = Util.generateSecureHash(stringToHash, merchantHashCode);
            String merchantVpcSecureHash = queryParam.get("vpc_SecureHash").toString();
            System.out.println("merchant's vpc_SecureHash: " + merchantVpcSecureHash);
            System.out.println("OnePay's vpc_Securehash: " + onePayVpcSecureHash);
            if (onePayVpcSecureHash.equals(merchantVpcSecureHash)) {
                System.out.println("vpc_SecureHash is valid");
            } else {
                System.out.println("vpc_SecureHash is invalid");
            }
        } catch (Exception e) {
            System.out.println(e.getMessage());
        }

    }
}
