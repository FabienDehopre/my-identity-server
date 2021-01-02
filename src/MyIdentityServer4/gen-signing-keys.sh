#!/bin/bash

echo "Generating signing keys..."
echo -n "Checking for ECDsa signing key..."
if [[ ! -e ./certs/ec/3594ab85ad5d4784a5c862fc28e53a6b.pem ]]; then
  mkdir -p ./certs/ec
  openssl ecparam -name prime256v1 -genkey -noout -out ./certs/ec/3594ab85ad5d4784a5c862fc28e53a6b.pem
  echo -e "\tGENERATED"
else
  echo -e "\tALREADY EXISTS"
fi

echo -n "Checking for PSA signing key..."
if [[ ! -e ./certs/rsa/d327491fa24349389fcf87ea53d2b1d2.pem ]]; then
  mkdir -p ./certs/rsa
  openssl genrsa -out ./certs/rsa/d327491fa24349389fcf87ea53d2b1d2.pem 2048
  echo -e "\t\tGENERATED"
else
  echo -e "\t\tALREADY EXISTS"
fi

echo -n "Checking for RSA signing key..."
if [[ ! -e ./certs/rsa/e3626be31f7f48eca843ded4abc4cbf1.pem ]]; then
  mkdir -p ./certs/rsa
  openssl genrsa -out ./certs/rsa/e3626be31f7f48eca843ded4abc4cbf1.pem 2048
  echo -e "\t\tGENERATED"
else
  echo -e "\t\tALREADY EXISTS"
fi

echo "DONE."
