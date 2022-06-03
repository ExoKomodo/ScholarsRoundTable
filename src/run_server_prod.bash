#! /bin/bash

set -ex

nginx -s reload

cd /app/server/bin/Release/net6.0

dotnet ./server.dll
