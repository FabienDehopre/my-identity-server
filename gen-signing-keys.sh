#!/bin/bash

echo "Generating signing keys..."
echo -n "Checking for ECDsa signing key..."
if [[ ! -e src/MyIdentityServer/certs/ec/3594ab85ad5d4784a5c862fc28e53a6b.pem ]]; then
  mkdir -p src/MyIdentityServer/certs/ec
  openssl ecparam -name prime256v1 -genkey -noout -out src/MyIdentityServer/certs/ec/3594ab85ad5d4784a5c862fc28e53a6b.pem
  echo -e "\tGENERATED"
else
  echo -e "\tALREADY EXISTS"
fi

echo -n "Checking for PSA signing key..."
if [[ ! -e src/MyIdentityServer/certs/rsa/d327491fa24349389fcf87ea53d2b1d2.pem ]]; then
  mkdir -p src/MyIdentityServer/certs/rsa
  openssl genrsa -out src/MyIdentityServer/certs/rsa/d327491fa24349389fcf87ea53d2b1d2.pem 2048
  echo -e "\t\tGENERATED"
else
  echo -e "\t\tALREADY EXISTS"
fi

echo -n "Checking for RSA signing key..."
if [[ ! -e src/MyIdentityServer/certs/rsa/e3626be31f7f48eca843ded4abc4cbf1.pem ]]; then
  mkdir -p src/MyIdentityServer/certs/rsa
  openssl genrsa -out src/MyIdentityServer/certs/rsa/e3626be31f7f48eca843ded4abc4cbf1.pem 2048
  echo -e "\t\tGENERATED"
else
  echo -e "\t\tALREADY EXISTS"
fi

echo "DONE."
