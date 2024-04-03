import hmac
import hashlib
import time
import requests
import base64


def send_get_http_request(url, param):
    response = requests.get(url, params=param, allow_redirects=False)
    print(response.headers.get("location"))


def send_get_http_request_with_header(url, header_request):
    response = requests.get(url, headers=header_request)
    print("Response: " + response.text)


def send_post_request(url, data):
    headers = {"Content-Type": "application/x-www-form-urlencoded"}
    response = requests.post(url, data=data, headers=headers)
    print("Response:" + response.text)


def sort_param(params: dict):
    params_sorted = dict(sorted(params.items(), key=lambda x: x[0]))
    return params_sorted


def generate_string_to_hash(params_sorted: dict):
    string_to_hash = ""
    for key, value in params_sorted.items():
        prefix_key = key[0:4]
        if prefix_key == "vpc_" or prefix_key == "user":
            if key != "vpc_SecureHashType" and key != "vpc_SecureHash":
                if len(value) > 0:
                    if len(string_to_hash) > 0:
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
    return hmac.new(key, msg.encode("utf-8"), hashlib.sha256).digest()


def create_request_signature_ita(
    method, uri, http_headers, signed_header_names, merchant_id, merchant_hash_code
):
    created = str(int(time.time()))
    lowercase_headers = {}
    for key, value in http_headers.items():
        lowercase_headers[key.lower()] = value
    lowercase_headers["(request-target)"] = method.lower() + " " + uri
    lowercase_headers["(created)"] = created

    signing_string = ""

    header_names = ""
    for element in signed_header_names:
        header_name = element
        if header_name not in lowercase_headers:
            raise Exception("MissingRequiredHeaderException: " + header_name)
        if signing_string:
            signing_string += "\n"
        signing_string += header_name + ": " + lowercase_headers[header_name]

        if header_names:
            header_names += " "
        header_names += header_name

    print("signingString=" + signing_string)

    hmac_key = bytes.fromhex(merchant_hash_code)
    signature = base64.b64encode(
        hmac.new(hmac_key, signing_string.encode("utf-8"), hashlib.sha512).digest()
    ).decode("utf-8")
    signing_algorithm = "hs2019"
    return (
        'algorithm="'
        + signing_algorithm
        + '"'
        + ', keyId="'
        + merchant_id
        + '"'
        + ', headers="'
        + header_names
        + '"'
        + ", created="
        + str(created)
        + ', signature="'
        + signature
        + '"'
    )
