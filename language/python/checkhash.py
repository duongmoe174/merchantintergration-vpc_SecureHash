import urllib.parse
import hmac
import hashlib
import time
import requests

MERCHANT_HASH_CODE: str = "6D0870CDE5F24F34F3915FB0045120DB"
BASE_URL: str = "https://mtf.onepay.vn/paygate/vpcpay.op"



def run_main():
    url = "https://mtf.onepay.vn/paygate/vpcpay.op?AgainLink=https%3A%2F%2Fmtf.onepay.vn%2Fclient%2Fqt%2F&Title=PHP+VPC+3-Party&vpc_AccessCode=6BEB2546&vpc_Amount=10000000&vpc_CardList=VC&vpc_Command=pay&vpc_Currency=VND&vpc_Customer_Email=test%40onepay.vn&vpc_Customer_Id=test&vpc_Customer_Phone=84987654321&vpc_Locale=vn&vpc_MerchTxnRef=TEST_1706240061&vpc_Merchant=TESTONEPAY&vpc_OrderInfo=Ma+Don+Hang&vpc_ReturnURL=https%3A%2F%2Fportal.lanit.com.vn%3A4083%2FsessYBzPl9UJVn4bm2uE%2Findex.php%3Fact%3Dpayment%26process_payment%3Donepay&vpc_TicketNo=192.168.166.149&vpc_Version=2&vpc_SecureHash=11396C01BC33F204691399900DED2D34AB4F9499FF0BA1651E6F7A1F19DAFE3B"
    onepay_verify_secure_hash(url, MERCHANT_HASH_CODE)
    
def run_main2():
    send_http_request()

def send_http_request():
    merchant_param = merchant_send_request_dynamic()
    response = requests.get(BASE_URL, params=merchant_param, allow_redirects=False)
    print(response.headers.get('location'))

def merchant_send_request_static():
    merchant_param = {
        "vpc_Version": "2",
        "vpc_Currency": "VND",
        "vpc_Command": "pay",
        "vpc_AccessCode": "6BEB2546",
        "vpc_Merchant": "TESTONEPAY",
        "vpc_Locale": "vn",
        "vpc_ReturnURL": "https://mtf.onepay.vn/client/qt/dr/",
        "vpc_MerchTxnRef": "TEST_1698291680035",
        "vpc_OrderInfo": "Ma Don Hang",
        "vpc_Amount": "10000000",
        "vpc_TicketNo": "192.168.166.149",
        "vpc_CardList": "VC",
        "AgainLink": "https://mtf.onepay.vn/client/qt/",
        "Title": "PHP VPC 3-Party",
        "vpc_Customer_Phone": "84987654321",
        "vpc_Customer_Email": "test@onepay.vn",
        "vpc_Customer_Id": "test"
    }

    params_sorted = sort_param(merchant_param)
    string_to_hash = generate_string_to_hash(params_sorted)
    print("merchant's string to hash: " + string_to_hash)
    secure_hash = generate_secure_hash(string_to_hash, MERCHANT_HASH_CODE)
    merchant_param['vpc_SecureHash'] = secure_hash
    return merchant_param

def merchant_send_request_dynamic():
    vpc_mer_txn_ref = "TEST_" + str(round(time.time()*1000))
    merchant_param = {
        "vpc_Version": "2",
        "vpc_Currency": "VND",
        "vpc_Command": "pay",
        "vpc_AccessCode": "6BEB2546",
        "vpc_Merchant": "TESTONEPAY",
        "vpc_Locale": "vn",
        "vpc_ReturnURL": "https://mtf.onepay.vn/client/qt/dr/?id=1&mode=TEST_PAYGATE",
        "vpc_MerchTxnRef": vpc_mer_txn_ref,
        "vpc_OrderInfo": "Ma Don Hang",
        "vpc_Amount": "10000000",
        "vpc_TicketNo": "192.168.166.149",
        "vpc_CardList": "VC",
        "AgainLink": "https://mtf.onepay.vn/client/qt/",
        "Title": "PHP VPC 3-Party",
        "vpc_Customer_Phone": "84987654321",
        "vpc_Customer_Email": "test@onepay.vn",
        "vpc_Customer_Id": "test"
    }

    params_sorted = sort_param(merchant_param)
    string_to_hash = generate_string_to_hash(params_sorted)
    print("merchant's string to hash: " + string_to_hash)
    secure_hash = generate_secure_hash(string_to_hash, MERCHANT_HASH_CODE)
    merchant_param['vpc_SecureHash'] = secure_hash
    return merchant_param


def onepay_verify_secure_hash(url, hash_code):
    params = dict(urllib.parse.parse_qsl(urllib.parse.urlsplit(url).query))
    params_sorted = sort_param(params)
    vpc_securehash_merchant = params_sorted['vpc_SecureHash']
    string_to_hash = generate_string_to_hash(params_sorted)
    onepay_sign = generate_secure_hash(string_to_hash, hash_code)
    print("onepay sign: " + onepay_sign)
    print("merchant sign: " + vpc_securehash_merchant)
    if (onepay_sign == vpc_securehash_merchant):
        print("vpc_SecureHash PASSED!")
    else:
        print("vpc_SecureHash is Invalid!")


def sort_param(params: dict):
    params_sorted = dict(sorted(params.items(), key=lambda x: x[0]))
    return params_sorted


def generate_string_to_hash(params_sorted: dict):
    string_to_hash = ""
    for key, value in params_sorted.items():
        prefix_key = key[0: 4]
        if (prefix_key == "vpc_" or prefix_key == "user"):
            if (key != "vpc_SecureHashType" and key != "vpc_SecureHash"):
                if (len(value) > 0):
                    if (len(string_to_hash) > 0):
                        string_to_hash += "&"
                    string_to_hash += key + "=" + value
    return string_to_hash


def generate_secure_hash(string_to_hash: str, merchant_hash_code: str):
    return vpc_auth(string_to_hash, merchant_hash_code)


def vpc_auth(msg, key):
    vpc_key = bytes.fromhex(key)
    vpc_hash = hmac_sha256(vpc_key, msg)
    return vpc_hash.hex().upper()


def hmac_sha256(key, msg):
    return hmac.new(key, msg.encode('utf-8'), hashlib.sha256).digest()

run_main2()