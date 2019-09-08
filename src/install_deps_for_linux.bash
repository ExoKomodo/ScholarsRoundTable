#! /bin/bash

set -ex

wget https://packages.microsoft.com/config/ubuntu/18.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
sudo dpkg -i packages-microsoft-prod.deb
rm packages-microsoft-prod.deb

sudo add-apt-repository universe
sudo apt-get -y update

sudo apt-get install -y apt-transport-https python3 python3-pip python3-venv nginx

sudo apt-get -y update
sudo apt-get install -y dotnet-sdk-6.0
