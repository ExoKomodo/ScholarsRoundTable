###########
# Builder #
###########
FROM mcr.microsoft.com/dotnet/sdk:6.0-bullseye-slim as builder

RUN apt-get update -y
RUN apt-get install -y python3
RUN apt-get install -y python3-pip
RUN apt-get install -y python3-venv

COPY ./src /app

WORKDIR /app/server

RUN dotnet publish --configuration Release

WORKDIR /app

RUN python3 -m venv env
RUN . ./env/bin/activate

RUN pip3 install -e .

############
# Deployer #
############
FROM mcr.microsoft.com/dotnet/aspnet:6.0-bullseye-slim as deployer

RUN apt-get update -y
RUN apt-get install -y nginx
RUN apt-get install -y python3
RUN apt-get install -y python3-venv

RUN apt-get autoremove

COPY --from=builder /app /app

RUN rm -f /etc/nginx/sites-enabled/*
RUN ln -f /app/server/nginx/server.conf /etc/nginx/sites-available/server.conf
RUN ln -s /etc/nginx/sites-available/server.conf /etc/nginx/sites-enabled/server.conf

EXPOSE 80
ENV ASPNETCORE_URLS=http://+:5000

WORKDIR /app
CMD ["bash", "./deploy.bash"]
