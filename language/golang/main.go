package main

func main() {
	LoadConfig()

	quereTransaction()
}

func makeInvoice() {
	merchantId := AppConfig.MerchantPaynowId
	merchantAccessCode := AppConfig.MerchantPaynowAccessCode
	merchantHashCode := AppConfig.MerchantPaynowHashCode
	createInvoice(merchantId, merchantAccessCode, merchantHashCode)
}

func makeInvoiceCreateToken() {
	merchantId := AppConfig.MerchantPaynowId
	merchantAccessCode := AppConfig.MerchantPaynowAccessCode
	merchantHashCode := AppConfig.MerchantPaynowHashCode
	createInvoiceCreateToken(merchantId, merchantAccessCode, merchantHashCode)
}

func makeInvoiceAndPaymentWithToken() {
	merchantId := AppConfig.MerchantPaynowId
	merchantAccessCode := AppConfig.MerchantPaynowAccessCode
	merchantHashCode := AppConfig.MerchantPaynowHashCode
	createInvoiceAndPaymentWithToken(merchantId, merchantAccessCode, merchantHashCode)
}

func makeInvoiceIntsallmentAtOnePaySite() {
	merchantId := AppConfig.MerchantInstallmentId
	merchantAccessCode := AppConfig.MerchantInstallmentAccessCode
	merchantHashCode := AppConfig.MerchantInstallmentHashCode
	createInvoiceInstallment(merchantId, merchantAccessCode, merchantHashCode)
}

func makeInvoiceInstallmentAtMerchantSite() {
	merchantId := AppConfig.MerchantInstallmentId
	merchantAccessCode := AppConfig.MerchantInstallmentAccessCode
	merchantHashCode := AppConfig.MerchantInstallmentHashCode
	amount := "1000000000"
	cardList := "VC"
	itaTime := "3"
	itaFeeAmount := "20000200"
	itaBankSwiftCode := "BFTVVNVX"
	createInvoiceInstallmentThemeITA(merchantId, merchantAccessCode, merchantHashCode, amount, cardList, itaTime, itaFeeAmount, itaBankSwiftCode)
}

func getInstallmentWithMerchantId() {
	merchantId := AppConfig.MerchantInstallmentId
	merchantHashCode := AppConfig.MerchantInstallmentHashCode
	amount := "3000000"
	getInstallment(amount, merchantId, merchantHashCode)
}

func quereTransaction() {
	merchantId := AppConfig.MerchantPaynowId
	merchantAccessCode := AppConfig.MerchantPaynowAccessCode
	merchantHashCode := AppConfig.MerchantPaynowHashCode
	merchTxnRef := "TEST_638466347228355308"
	queryDRApi(merchantId, merchantAccessCode, merchantHashCode, merchTxnRef)
}

func verifySign() {
	url := "https://webhook.site/6ddd97bd-d51c-4c36-b16b-7755ce68a010?vpc_Amount=10000000&vpc_Command=pay&vpc_MerchTxnRef=TEST_1711444940129917200&vpc_Merchant=DUONGTT&vpc_Message=Canceled&vpc_OrderInfo=Ma+Don+Hang&vpc_TxnResponseCode=99&vpc_Version=2&vpc_SecureHash=E083AFF7100E679678A68E6FDCB4751A75C2775EB42A5D24209E42B7665313D0"
	merchantHashCode := AppConfig.MerchantPaynowHashCode
	onePayVerifySecureHash(url, merchantHashCode)
}
