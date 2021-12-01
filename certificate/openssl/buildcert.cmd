"C:\Program Files\OpenSSL-Win64\bin\openssl.exe" genrsa -out ca_privatekey.pem 2048
"C:\Program Files\OpenSSL-Win64\bin\openssl.exe" req -new -x509 -days 3650 -nodes -key ca_privatekey.pem -sha256 -out ca.crt -config ca.cnf
"C:\Program Files\OpenSSL-Win64\bin\openssl.exe" req -new -nodes -out server.csr -keyout serverkey.key -config server.cnf
"C:\Program Files\OpenSSL-Win64\bin\openssl.exe" x509 -req -in server.csr -CA ca.crt -CAkey ca_privatekey.pem -CAcreateserial -out servercert.crt -days 365 -extfile server_v3.ext