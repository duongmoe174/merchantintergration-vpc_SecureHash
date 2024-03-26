package main

import (
	"fmt"
	"net/url"
	"time"
)

func createInvoiceInstallment(merchantId string, merchantAccessCode string, merchantHashCode string) {

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
		"vpc_Amount":         "1000000000", //amount must >= 3.000.000 VND
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

func createInvoiceInstallmentThemeITA(merchantId string, merchantAccessCode string, merchantHashCode string, amount string, cardList string, itaTime string, itaFeeAmount string, itaBankSwiftCode string) {
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
		"vpc_Amount":         amount, // amount must >= 3.000.000 VND
		"vpc_TicketNo":       "192.168.166.149",
		"AgainLink":          "https://mtf.onepay.vn/client/qt/",
		"Title":              "PHP VPC 3-Party",
		"vpc_Customer_Phone": "84987654321",
		"vpc_Customer_Email": "test@onepay.vn",
		"vpc_Customer_Id":    "test",
		"vpc_CardList":       cardList,
		"vpc_ItaTime":        itaTime,
		"vpc_ItaFeeAmount":   itaFeeAmount,
		"vpc_ItaBank":        itaBankSwiftCode,
		"vpc_Theme":          "ita",
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

func getInstallment(amount string, merchantId string, merchantHashCode string) {
	signedHeaderNames := []string{"(request-target)", "(created)", "host", "accept"}
	accept := ""
	uriPath := "/msp/api/v1/merchants/" + merchantId + "/installments?amount=" + amount
	headerSign := map[string]string{
		"Host":   AppConfig.HOST,
		"Accept": accept,
	}
	method := "GET"
	signature := createRequestSignatureITA(method, uriPath, headerSign, signedHeaderNames, merchantId, merchantHashCode)

	headerRequest := map[string]string{
		"accept":    accept,
		"Host":      AppConfig.HOST,
		"signature": signature,
	}

	urlRequets := AppConfig.BaseURL + uriPath

	sendHttpGetRequestWithHeader(urlRequets, headerRequest)
}
