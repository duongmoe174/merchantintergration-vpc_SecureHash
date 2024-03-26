package main

func queryDRApi(merchantId string, merchantAccessCode string, merchantHashCode string, merchTxnRef string) {
	merchantQueryMap := map[string]string{
		"vpc_Version":     "2",
		"vpc_Command":     "queryDR",
		"vpc_AccessCode":  merchantAccessCode,
		"vpc_Merchant":    merchantId,
		"vpc_Password":    "admin@123456",
		"vpc_User":        "Administrator",
		"vpc_MerchTxnRef": merchTxnRef,
	}
	queryParamSorted := sortParams(merchantQueryMap)
	stringTohash := generateStringToHash(queryParamSorted)
	merchantGenSecureHash := generateSecureHash(stringTohash, merchantHashCode)
	merchantQueryMap["vpc_SecureHash"] = merchantGenSecureHash
	urlRequest := AppConfig.BaseURL + "/msp/api/v1/vpc/invoices/queries"
	sendHttpPostRequest(urlRequest, merchantQueryMap)
}
