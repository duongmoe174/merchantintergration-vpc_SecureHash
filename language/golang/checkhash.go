package main

import (
	"crypto/hmac"
	"crypto/sha256"
	"encoding/hex"
	"fmt"
	"net/http"
	"net/url"
	"sort"
	"strings"
	"time"
)

type MapSort struct {
	Key   string
	Value string
}

const (
	merchantHashCode = "6D0870CDE5F24F34F3915FB0045120DB"
	BASE_URL         = "https://mtf.onepay.vn/paygate/vpcpay.op"
)

func main() {
	// urlMerchant := merchantSendRequest()
	// onePayVerifySecureHash(urlMerchant, merchantHashCode)
	sendHttpGetRequest()
}

func merchantSendRequestStatic() string {
	merchantQueryMap := map[string]string{
		"vpc_Version":        "2",
		"vpc_Currency":       "VND",
		"vpc_Command":        "pay",
		"vpc_AccessCode":     "6BEB2546",
		"vpc_Merchant":       "TESTONEPAY",
		"vpc_Locale":         "vn",
		"vpc_ReturnURL":      "https://mtf.onepay.vn/client/qt/dr/",
		"vpc_MerchTxnRef":    "TEST_1698291680035",
		"vpc_OrderInfo":      "Ma Don Hang",
		"vpc_Amount":         "10000000",
		"vpc_TicketNo":       "192.168.166.149",
		"vpc_CardList":       "VC",
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
	requestUrl := fmt.Sprintf("%s?%s", BASE_URL, params.Encode())
	return requestUrl

}

func merchantSendRequestDynamic() string {
	time := time.Now().UnixNano()
	vpcMerTxnRef := "TEST_" + fmt.Sprintf("%d", time)
	merchantQueryMap := map[string]string{
		"vpc_Version":        "2",
		"vpc_Currency":       "VND",
		"vpc_Command":        "pay",
		"vpc_AccessCode":     "6BEB2546",
		"vpc_Merchant":       "TESTONEPAY",
		"vpc_Locale":         "vn",
		"vpc_ReturnURL":      "https://mtf.onepay.vn/client/qt/dr/?duongtt=duongtt&mode=TEST_PAYGATE",
		"vpc_MerchTxnRef":    vpcMerTxnRef,
		"vpc_OrderInfo":      "Ma Don Hang",
		"vpc_Amount":         "10000000",
		"vpc_TicketNo":       "192.168.166.149",
		"vpc_CardList":       "VC",
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
	requestUrl := fmt.Sprintf("%s?%s", BASE_URL, params.Encode())
	return requestUrl

}

func onePayVerifySecureHash(urlMerchant string, merchantHashCode string) {
	u, err := url.Parse(urlMerchant)
	if err != nil {
		panic(err)
	}
	queryParams := u.Query()
	queryParamsMap := make(map[string]string)
	for k, v := range queryParams {
		queryParamsMap[k] = strings.Join(v, "")
	}
	queryMapSorted := sortParams(queryParamsMap)
	stringToHash := generateStringToHash(queryMapSorted)
	onePaySecureHash := generateSecureHash(stringToHash, merchantHashCode)
	merchantSecureHash := queryParamsMap["vpc_SecureHash"]
	fmt.Println("OnePay's Hash: ", onePaySecureHash)
	fmt.Println("Merchant's Hash: ", merchantSecureHash)
	if onePaySecureHash == merchantSecureHash {
		fmt.Println("vpc_SecureHash is valid")
	} else {
		fmt.Println("vpc_SecureHash invalid")
	}

}

func sortParams(paramMap map[string]string) []MapSort {
	keys := make([]string, 0, len(paramMap))
	for k := range paramMap {
		keys = append(keys, k)
	}
	sort.Strings(keys)
	mapSorted := []MapSort{}

	//var paramMapSorted map[string]string
	for _, k := range keys {
		value := paramMap[k]
		for n := range paramMap {
			if k == n {
				mapSorted = append(mapSorted, MapSort{Key: k, Value: value})
			}
		}
	}
	return mapSorted
}

func generateStringToHash(paramMapSorted []MapSort) string {
	stringToHash := ""
	for _, items := range paramMapSorted {
		key := items.Key
		value := items.Value
		pref4 := key[0:4]
		pref5 := key[0:5]
		if pref4 == "vpc_" || pref5 == "user_" {
			if key != "vpc_SecureHashType" && key != "vpc_SecureHash" {
				if len(value) > 0 {
					if len(stringToHash) > 0 {
						stringToHash += "&"
					}
					stringToHash += key + "=" + value
				}
			}
		}
	}
	return stringToHash
}

func generateSecureHash(stringToHash string, merchantHashCode string) string {
	keyHashHex, err := hex.DecodeString(merchantHashCode)
	if err != nil {
		panic(err)
	}
	secureHash := hmac.New(sha256.New, keyHashHex)
	secureHash.Write([]byte(stringToHash))
	secureHashToString := hex.EncodeToString(secureHash.Sum(nil))
	signUpper := strings.ToUpper(secureHashToString)
	return signUpper
}

func sendHttpGetRequest() {
	requestUrl := merchantSendRequestDynamic()
	client := &http.Client{
		CheckRedirect: func(req *http.Request, via []*http.Request) error {
			return http.ErrUseLastResponse
		},
	}
	req, err := http.NewRequest(http.MethodGet, requestUrl, nil)
	if err != nil {
		fmt.Println(err)
		return
	}
	resp, err := client.Do(req)
	if err != nil {
		fmt.Println(err)
		return
	}
	link := resp.Header.Get("Location")
	fmt.Println("Link redirect to invoice page: ", link)
}
