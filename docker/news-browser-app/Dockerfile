FROM ubuntu:16.04

RUN apt-get update && apt-get install -y apt-utils \
  apt-transport-https \
  software-properties-common \
  python-software-properties

RUN mkdir -p /home/install/src
RUN mkdir -p /home/data

WORKDIR /home/install
RUN pwd && ls
ADD https://packages.microsoft.com/config/ubuntu/16.04/packages-microsoft-prod.deb /home/install/packages-microsoft-prod.deb
RUN ls /home/install \
    && dpkg -i /home/install/packages-microsoft-prod.deb \
    && apt-get update \
    && apt-get install -y dotnet-sdk-3.1 \
    dotnet-runtime-3.1

RUN pwd && ls src
COPY NewsBrowser /home/install/src
COPY import /home/data/import

WORKDIR /home/install/src

COPY ./docker/entrypoint.sh /entrypoint.sh
RUN chmod +x /entrypoint.sh
ENTRYPOINT ["/docker-entrypoint.sh"]
