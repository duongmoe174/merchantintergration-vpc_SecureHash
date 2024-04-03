import urllib.parse
import time
import util as Util
from config import Config


def create_invocie_and_create_token(
    merchant_id, merchant_access_code, merchant_hash_code
):
    vpc_mer_txn_ref = "TEST_" + str(round(time.time() * 1000))
    merchant_param = {
        "vpc_Customer_Id": "test",
        "vpc_CreateToken": "true",
        "vpc_Version": "2",
        "vpc_Currency": "VND",
        "vpc_Command": "pay",
        "vpc_AccessCode": merchant_access_code,
        "vpc_Merchant": merchant_id,
        "vpc_Locale": "vn",
        "vpc_ReturnURL": "https://mtf.onepay.vn/client/qt/dr/?id=1&mode=TEST_PAYGATE",
        "vpc_MerchTxnRef": vpc_mer_txn_ref,
        "vpc_OrderInfo": "Ma Don Hang",
        "vpc_Amount": "10000000",
        "vpc_TicketNo": "192.168.166.149",
        "AgainLink": "https://mtf.onepay.vn/client/qt/",
        "Title": "PHP VPC 3-Party",
        "vpc_Customer_Phone": "84987654321",
        "vpc_Customer_Email": "test@onepay.vn",
    }

    params_sorted = Util.sort_param(merchant_param)
    string_to_hash = Util.generate_string_to_hash(params_sorted)
    print("merchant's string to hash: " + string_to_hash)
    secure_hash = Util.generate_secure_hash(string_to_hash, merchant_hash_code)
    merchant_param["vpc_SecureHash"] = secure_hash
    url_request = Config.BASE_URL + Config.URL_PREFIX
    Util.send_get_http_request(url_request, merchant_param)


import urllib.parse
import time
import util as Util
from config import Config


def create_invocie_and_payment_with_token(
    merchant_id, merchant_access_code, merchant_hash_code, token_num, token_exp
):
    vpc_mer_txn_ref = "TEST_" + str(round(time.time() * 1000))
    merchant_param = {
        "vpc_TokenNum": token_num,
        "vpc_TokenExp": token_exp,
        "vpc_Customer_Id": "test",
        "vpc_Version": "2",
        "vpc_Currency": "VND",
        "vpc_Command": "pay",
        "vpc_AccessCode": merchant_access_code,
        "vpc_Merchant": merchant_id,
        "vpc_Locale": "vn",
        "vpc_ReturnURL": "https://mtf.onepay.vn/client/qt/dr/?id=1&mode=TEST_PAYGATE",
        "vpc_MerchTxnRef": vpc_mer_txn_ref,
        "vpc_OrderInfo": "Ma Don Hang",
        "vpc_Amount": "10000000",
        "vpc_TicketNo": "192.168.166.149",
        "AgainLink": "https://mtf.onepay.vn/client/qt/",
        "Title": "PHP VPC 3-Party",
        "vpc_Customer_Phone": "84987654321",
        "vpc_Customer_Email": "test@onepay.vn",
    }
    params_sorted = Util.sort_param(merchant_param)
    string_to_hash = Util.generate_string_to_hash(params_sorted)
    print("merchant's string to hash: " + string_to_hash)
    secure_hash = Util.generate_secure_hash(string_to_hash, merchant_hash_code)
    merchant_param["vpc_SecureHash"] = secure_hash
    url_request = Config.BASE_URL + Config.URL_PREFIX
    Util.send_get_http_request(url_request, merchant_param)
