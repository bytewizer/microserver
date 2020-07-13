# Self-signed SSL certificate

Install the following browsers certs from the "bytewizer.local" directory by clicking on both files:

* bytewizer.local.cer - Install domain certificate to the current user store.
* bytewizer.local.pfx - Install CA certificate to the Trusted Root Certification Authorities (required elevated privileges) with password of "bytewizer.local".

To create your own self signed certs install OpenSSL (https://slproweb.com/products/Win32OpenSSL.html) and check out the create_certificate.ps1 powershell script. See
https://github.com/henrikstengaard/hst-tools/tree/master/ssl for more information on this script.

```console
.\create_certificate.ps1 -rootCaName 'Bytewizer' -domainDnsName '*.bytewizer.local' -outputDir '.\'
```

Running script will create the following files that can be added as Binary Resources "files" in your Visual Studio Project.

* bytewizer.local_key.pem - Private key used to generate domain certificate in pkcs12 format.
* bytewizer.local.pem - Domain certificate in pkcs12 format.
