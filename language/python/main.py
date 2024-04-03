import create_invoice as cInvoice
import create_invoice_token as cInvoiceToken
import create_invoice_installment as cInvoiceInstallment
import verify_secure_hash as VeriySign
from config import Config
import query_dr as QueryApi


def main():
    verify_sign()


def make_invoice():
    merchant_id = Config.MERCHANT_PAYNOW_ID
    merchant_access_code = Config.MERCHANT_PAYNOW_ACCESS_CODE
    merchant_hash_code = Config.MERCHANT_PAYNOW_HASH_CODE
    cInvoice.create_invocie(merchant_id, merchant_access_code, merchant_hash_code)


def make_invoice_and_create_token():
    merchant_id = Config.MERCHANT_PAYNOW_ID
    merchant_access_code = Config.MERCHANT_PAYNOW_ACCESS_CODE
    merchant_hash_code = Config.MERCHANT_PAYNOW_HASH_CODE
    cInvoiceToken.create_invocie_and_create_token(
        merchant_id, merchant_access_code, merchant_hash_code
    )


def make_invoice_and_payment_with_token():
    merchant_id = Config.MERCHANT_PAYNOW_ID
    merchant_access_code = Config.MERCHANT_PAYNOW_ACCESS_CODE
    merchant_hash_code = Config.MERCHANT_PAYNOW_HASH_CODE
    token_num = "5123451517076481"
    token_exp = "1225"
    cInvoiceToken.create_invocie_and_payment_with_token(
        merchant_id, merchant_access_code, merchant_hash_code, token_num, token_exp
    )


def make_invoice_intsallment_at_onepay_site():
    merchant_id = Config.MERCHANT_INSTALLMENT_ID
    merchant_access_code = Config.MERCHANT_INSTALLMENT_ACCESS_CODE
    merchant_hash_code = Config.MERCHANT_INSTALLMENT_HASH_CODE
    cInvoiceInstallment.create_invocie_intsallment(
        merchant_id, merchant_access_code, merchant_hash_code
    )


def make_invoice_installment_at_merchant_site():
    merchant_id = Config.MERCHANT_INSTALLMENT_ID
    merchant_access_code = Config.MERCHANT_INSTALLMENT_ACCESS_CODE
    merchant_hash_code = Config.MERCHANT_INSTALLMENT_HASH_CODE
    amount = "1000000000"
    card_list = "VC"
    ita_time = "3"
    ita_fee_amount = "20000200"
    ita_bank_swift_code = "BFTVVNVX"
    cInvoiceInstallment.create_invocie_intsallment_theme_ita(
        merchant_id,
        merchant_access_code,
        merchant_hash_code,
        amount,
        card_list,
        ita_time,
        ita_fee_amount,
        ita_bank_swift_code,
    )


def get_intsallment_by_merchant_id():
    amount = "30000000"
    merchant_id = Config.MERCHANT_INSTALLMENT_ID
    merchant_hash_code = Config.MERCHANT_INSTALLMENT_HASH_CODE
    cInvoiceInstallment.get_installment(amount, merchant_id, merchant_hash_code)


def query_transaction():
    merchant_id = Config.MERCHANT_PAYNOW_ID
    merchant_access_code = Config.MERCHANT_PAYNOW_ACCESS_CODE
    merchant_hash_code = Config.MERCHANT_PAYNOW_HASH_CODE
    merch_txn_ref = "TEST_638466347228355308"
    QueryApi.query_dr_api(
        merchant_id, merchant_access_code, merchant_hash_code, merch_txn_ref
    )


def verify_sign():
    url = "https://webhook.site/6ddd97bd-d51c-4c36-b16b-7755ce68a010?vpc_Amount=10000000&vpc_Command=pay&vpc_MerchTxnRef=TEST_1711444940129917200&vpc_Merchant=DUONGTT&vpc_Message=Canceled&vpc_OrderInfo=Ma+Don+Hang&vpc_TxnResponseCode=99&vpc_Version=2&vpc_SecureHash=E083AFF7100E679678A68E6FDCB4751A75C2775EB42A5D24209E42B7665313D0"
    merchant_hash_code = Config.MERCHANT_PAYNOW_HASH_CODE
    VeriySign.onepay_verify_secure_hash(url, merchant_hash_code)


main()
