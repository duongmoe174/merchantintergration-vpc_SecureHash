import util as Util
from config import Config


def query_dr_api(merchant_id, merchant_access_code, merchant_hash_code, merch_txn_ref):
    merchant_param = {
        "vpc_Version": "2",
        "vpc_Command": "queryDR",
        "vpc_AccessCode": merchant_access_code,
        "vpc_Merchant": merchant_id,
        "vpc_Locale": "vn",
        "vpc_MerchTxnRef": merch_txn_ref,
        "vpc_Password": "admin@123456",
        "vpc_User": "Administrator",
    }
    params_sorted = Util.sort_param(merchant_param)
    string_to_hash = Util.generate_string_to_hash(params_sorted)
    print("merchant's string to hash: " + string_to_hash)
    secure_hash = Util.generate_secure_hash(string_to_hash, merchant_hash_code)
    merchant_param["vpc_SecureHash"] = secure_hash
    url_request = Config.BASE_URL + "/msp/api/v1/vpc/invoices/queries"
    Util.send_post_request(url_request, merchant_param)
