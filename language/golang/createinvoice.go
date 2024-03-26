package main

import (
	"fmt"
	"net/url"
	"time"
)

func createInvoice(merchantId string, merchantAccessCode string, merchantHashCode string) {

	time := time.Now().UnixNano()
	vpcMerTxnRef := "TEST_" + fmt.Sprintf("%d", time)
	merchantQueryMap := map[string]string{
		"vpc_Version":        "2",
		"vpc_Currency":       "VND",
		"vpc_Command":        "pay",
		"vpc_AccessCode":     merchantAccessCode,
		"vpc_Merchant":       merchantId,
		"vpc_Locale":         "vn",
		"vpc_ReturnURL":      "https://mtf.onepay.vn/client/qt/dr/?duongtt=duongtt&mode=TEST_PAYGATE",
		"vpc_MerchTxnRef":    vpcMerTxnRef,
		"vpc_OrderInfo":      "Ma Don Hang",
		"vpc_Amount":         "10000000",
		"vpc_TicketNo":       "192.168.166.149",
		"AgainLink":          "https://mtf.onepay.vn/client/qt/",
		"Title":              "PHP VPC 3-Party",
		"vpc_Customer_Phone": "84987654321",
		"vpc_Customer_Email": "test@onepay.vn",
		"vpc_Customer_Id":    "test",
	}

	queryParamSorted := sortParams(merchantQueryMap)
	stringTohash := generateStringToHash(queryParamSorted)
	merchantGenSecureHash := generateSecureHash(stringTohash, merchantHashCode)
	merchantQueryMap["vpc_SecureHash"] = merchantGenSecureHash

	params := url.Values{}
	for key, value := range merchantQueryMap {
		params.Add(key, value)
	}
	requestUrl := AppConfig.BaseURL + AppConfig.URLPrefix + params.Encode()
	sendHttpGetRequest(requestUrl)
}
