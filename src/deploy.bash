#! /bin/bash

set -ex

bash ./run_auth_prod.bash &

bash ./run_server_prod.bash