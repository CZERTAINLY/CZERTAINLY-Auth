#!/usr/bin/env sh

APP_SCRIPTS_HOME="/opt/czertainly"


if [ -f ${APP_SCRIPTS_HOME}/trusted-certificates.pem ]
then
    PEM_FILE=${APP_SCRIPTS_HOME}/trusted-certificates.pem
    CERTS=$(grep 'END CERTIFICATE' $PEM_FILE| wc -l)

    echo "Copying certificates to ca cert..."

    # For every cert in the PEM file, extract it and store it to CA certificates
    # awk command: step 1, if line is in the desired cert, print the line
    #              step 2, increment counter when last line of cert is found
    for N in $(seq 0 $(($CERTS - 1))); do
      ALIAS="czertainly-trusted-$N"
      cat $PEM_FILE |
        awk "n==$N { print }; /END CERTIFICATE/ { n++ }" > /usr/local/share/ca-certificates/$ALIAS.crt
    done

    echo "Updating CA cert certificates..."
    update-ca-certificates

    echo "Running CZERTAINLY Auth service..."
    dotnet Czertainly.Auth.dll
else
    echo "ERROR" "No trusted certificates were provided!"
fi