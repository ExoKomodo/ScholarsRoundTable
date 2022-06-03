#! /bin/bash

set -ex

cd /app

export ENV=production
export FLASK_APP=auth
export FLASK_ENV=production

. ./env/bin/activate

flask run --host=0.0.0.0 --port 5001
