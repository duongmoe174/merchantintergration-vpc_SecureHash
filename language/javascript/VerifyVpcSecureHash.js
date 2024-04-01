const util = require("./Util");

function onePayVerifySecureHash(urlResponse, merHashCode) {
  let newURL = new URL(urlResponse);
  let params = new URLSearchParams(newURL.search);
  let hashFromMerchant = params.get("vpc_SecureHash");
  let paramObject = Object.fromEntries(params);
  let paramsSorted = util.sortObj(paramObject);
  let stringToHash = util.generateStringToHash(paramsSorted);
  let OnePaySign = util.genSecureHash(stringToHash, merHashCode);
  console.log("onepay's hash: " + OnePaySign);
  console.log("merchant's hash: " + hashFromMerchant);
  if (OnePaySign == hashFromMerchant) {
    console.log("vpc_SecureHash PASSED!!!");
  } else {
    console.error("invalid vpc_SecureHash");
  }
}

module.exports = {
  onePayVerifySecureHash,
};
