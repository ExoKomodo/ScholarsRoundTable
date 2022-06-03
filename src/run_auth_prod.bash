#! /bin/bash

set -ex

cd /app

export ENV=production
export FLASK_APP=auth
export FLASK_ENV=production

python3 -m venv env
. ./env/bin/activate

python3 -m flask run --host=0.0.0.0 --port 5001
