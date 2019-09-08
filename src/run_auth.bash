#! /bin/bash

set -ex

python3 -m venv env
source ./env/bin/activate

export FLASK_APP=auth
export FLASK_ENV=development

pip3 install --upgrade pip
pip3 install -e .

flask run --host=0.0.0.0 --port $@
