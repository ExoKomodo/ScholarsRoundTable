#! /bin/bash

set -ex

export ENV=development
export FLASK_APP=auth
export FLASK_ENV=development

SSL_DIR=./ssl
SSL_NAME=localhost/localhost

./run_auth.bash 5001 --cert=${SSL_DIR}/${SSL_NAME}.crt --key=${SSL_DIR}/${SSL_NAME}.key
