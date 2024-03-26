package main

import (
	"fmt"
	"net/url"
	"strings"
)

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
