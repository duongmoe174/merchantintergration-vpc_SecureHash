const CryptoJS = require("crypto-js");
let MERCHANT_HASH_CODE = "EB1B7F75EBB2FAABD6763FC37A3628AF";
let MERCHANT_ID = "TESTTRAGOP";
const BASE_URL = "https://mtf.onepay.vn";
const HOST = "mtf.onepay.vn";
const PREFIX_URL = "/msp/api/v1";
const axios = require("axios");
const https = require("https");

function main() {
  getInstallment(20000000);
}

function getInstallment(amount) {
  let signedHeaderNames = ["(request-target)", "(created)", "host", "accept"];
  let accept = "";
  let header = {
    Host: HOST,
    Accept: accept,
  };
  const uri =
    PREFIX_URL + "/merchants/" + MERCHANT_ID + "/installments?amount=" + amount;

  let signature = createRequestSignature(
    "GET",
    uri,
    header,
    signedHeaderNames,
    MERCHANT_ID,
    MERCHANT_HASH_CODE
  );

  const options = {
    hostname: HOST, // Replace with your URL
    path: uri, // Replace with the path to your resource
    method: "GET",
    headers: {
      Host: HOST,
      signature: signature,
      accept: "",
    },
  };
  // Send the GET request
  const req = https.request(options, (res) => {
    console.log(`statusCode: ${res.statusCode}`);

    // Accumulate response data
    let responseData = "";
    res.on("data", (chunk) => {
      responseData += chunk;
    });

    // Process response data
    res.on("end", () => {
      console.log(responseData);
      // Process responseData as needed
    });
  });

  // Handle errors
  req.on("error", (error) => {
    console.error(error);
  });

  // End the request
  req.end();
}

function createRequestSignature(
  method,
  uri,
  httpHeaders,
  signedHeaderNames,
  keyId,
  hexaHmacKey
) {
  let created = Math.floor(new Date().getTime() / 1000);
  let lowercaseHeaders = {};
  for (let key in httpHeaders) {
    if (httpHeaders.hasOwnProperty(key)) {
      lowercaseHeaders[key.toLowerCase()] = httpHeaders[key];
    }
  }
  lowercaseHeaders["(request-target)"] = method.toLowerCase() + " " + uri;
  lowercaseHeaders["(created)"] = created;

  let signingString = "";

  let headerNames = "";
  for (const element of signedHeaderNames) {
    let headerName = element;
    if (!lowercaseHeaders.hasOwnProperty(headerName)) {
      throw "MissingRequiredHeaderException: " + headerName;
    }
    if (signingString !== "") signingString += "\n";
    signingString += headerName + ": " + lowercaseHeaders[headerName];

    if (headerNames !== "") headerNames += " ";
    headerNames += headerName;
  }

  console.log("signingString=" + signingString);

  let hmacKey = CryptoJS.enc.Hex.parse(hexaHmacKey);
  let signature = CryptoJS.enc.Base64.stringify(
    CryptoJS.HmacSHA512(signingString, hmacKey)
  );
  let signingAlgorithm = "hs2019";
  return (
    'algorithm="' +
    signingAlgorithm +
    '"' +
    ', keyId="' +
    keyId +
    '"' +
    ', headers="' +
    headerNames +
    '"' +
    ", created=" +
    created +
    ', signature="' +
    signature +
    '"'
  );
}

main();
