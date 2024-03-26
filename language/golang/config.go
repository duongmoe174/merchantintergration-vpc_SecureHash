package main

import (
	"fmt"

	"github.com/spf13/viper"
)

type Config struct {
	MerchantPaynowId              string `mapstructure:"merchant_paynow_id"`
	MerchantPaynowAccessCode      string `mapstructure:"merchant_paynow_access_code"`
	MerchantPaynowHashCode        string `mapstructure:"merchant_paynow_hash_code"`
	MerchantInstallmentId         string `mapstructure:"merchant_installment_id"`
	MerchantInstallmentAccessCode string `mapstructure:"merchant_installment_access_code"`
	MerchantInstallmentHashCode   string `mapstructure:"merchant_installment_hash_code"`
	BaseURL                       string `mapstructure:"base_url"`
	URLPrefix                     string `mapstructure:"url_prefix"`
	HOST                          string `mapstructure:"host"`
}

var AppConfig Config

func LoadConfig() {
	viper.SetConfigName("config")
	viper.AddConfigPath(".")
	viper.SetConfigType("yaml")

	if err := viper.ReadInConfig(); err != nil {
		panic(err)
	}

	// Unmarshal the AppConfig

	if err := viper.UnmarshalKey("Config", &AppConfig); err != nil {
		fmt.Printf("Error unmarshaling config: %s\n", err)
		return
	}
}
