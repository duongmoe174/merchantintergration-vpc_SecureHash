import urllib.parse
import util as Util


def onepay_verify_secure_hash(url, hash_code):
    params = dict(urllib.parse.parse_qsl(urllib.parse.urlsplit(url).query))
    params_sorted = Util.sort_param(params)
    vpc_securehash_merchant = params_sorted["vpc_SecureHash"]
    string_to_hash = Util.generate_string_to_hash(params_sorted)
    onepay_sign = Util.generate_secure_hash(string_to_hash, hash_code)
    print("onepay sign: " + onepay_sign)
    print("merchant sign: " + vpc_securehash_merchant)
    if onepay_sign == vpc_securehash_merchant:
        print("vpc_SecureHash PASSED!")
    else:
        print("vpc_SecureHash is Invalid!")
