import urllib.parse
import time
import util as Util
from config import Config
import hashlib
import hmac


def create_invocie_intsallment(merchant_id, merchant_access_code, merchant_hash_code):
    vpc_mer_txn_ref = "TEST_" + str(round(time.time() * 1000))
    merchant_param = {
        "vpc_Version": "2",
        "vpc_Currency": "VND",
        "vpc_Command": "pay",
        "vpc_AccessCode": merchant_access_code,
        "vpc_Merchant": merchant_id,
        "vpc_Locale": "vn",
        "vpc_ReturnURL": "https://mtf.onepay.vn/client/qt/dr/?id=1&mode=TEST_PAYGATE",
        "vpc_MerchTxnRef": vpc_mer_txn_ref,
        "vpc_OrderInfo": "Ma Don Hang",
        "vpc_Amount": "1000000000",
        "vpc_TicketNo": "192.168.166.149",
        "AgainLink": "https://mtf.onepay.vn/client/qt/",
        "Title": "PHP VPC 3-Party",
        "vpc_Customer_Phone": "84987654321",
        "vpc_Customer_Email": "test@onepay.vn",
        "vpc_Customer_Id": "test",
    }

    params_sorted = Util.sort_param(merchant_param)
    string_to_hash = Util.generate_string_to_hash(params_sorted)
    print("merchant's string to hash: " + string_to_hash)
    secure_hash = Util.generate_secure_hash(string_to_hash, merchant_hash_code)
    merchant_param["vpc_SecureHash"] = secure_hash
    url_request = Config.BASE_URL + Config.URL_PREFIX
    Util.send_get_http_request(url_request, merchant_param)


def create_invocie_intsallment_theme_ita(
    merchant_id,
    merchant_access_code,
    merchant_hash_code,
    amount,
    card_list,
    ita_time,
    ita_fee_amount,
    ita_bank_swift_code,
):
    vpc_mer_txn_ref = "TEST_" + str(round(time.time() * 1000))
    merchant_param = {
        "vpc_Version": "2",
        "vpc_Currency": "VND",
        "vpc_Command": "pay",
        "vpc_AccessCode": merchant_access_code,
        "vpc_Merchant": merchant_id,
        "vpc_Locale": "vn",
        "vpc_ReturnURL": "https://mtf.onepay.vn/client/qt/dr/?id=1&mode=TEST_PAYGATE",
        "vpc_MerchTxnRef": vpc_mer_txn_ref,
        "vpc_OrderInfo": "Ma Don Hang",
        "vpc_Amount": amount,
        "vpc_TicketNo": "192.168.166.149",
        "AgainLink": "https://mtf.onepay.vn/client/qt/",
        "Title": "PHP VPC 3-Party",
        "vpc_Customer_Phone": "84987654321",
        "vpc_Customer_Email": "test@onepay.vn",
        "vpc_Customer_Id": "test",
        "vpc_CardList": card_list,
        "vpc_ItaTime": ita_time,
        "vpc_ItaFeeAmount": ita_fee_amount,
        "vpc_ItaBank": ita_bank_swift_code,
        "vpc_theme": "ita",
    }

    params_sorted = Util.sort_param(merchant_param)
    string_to_hash = Util.generate_string_to_hash(params_sorted)
    print("merchant's string to hash: " + string_to_hash)
    secure_hash = Util.generate_secure_hash(string_to_hash, merchant_hash_code)
    merchant_param["vpc_SecureHash"] = secure_hash
    url_request = Config.BASE_URL + Config.URL_PREFIX
    Util.send_get_http_request(url_request, merchant_param)


def get_installment(amount, merchant_id, merchant_hash_code):
    signed_header_names = ["(request-target)", "(created)", "host", "accept"]
    accept = "*/*"
    header = {
        "Host": Config.HOST,
        "Accept": accept,
    }
    uri = "/msp/api/v1/merchants/" + merchant_id + "/installments?amount=" + str(amount)

    signature = Util.create_request_signature_ita(
        "GET", uri, header, signed_header_names, merchant_id, merchant_hash_code
    )

    header_request = {
        "Host": Config.HOST,
        "signature": signature,
        "accept": accept,
    }
    url_request = Config.BASE_URL + uri
    Util.send_get_http_request_with_header(url_request, header_request)
