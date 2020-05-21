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

RUN apt-get install -y python3-pip
RUN yes | pip3 install requests elasticsearch

ADD https://deb.nodesource.com/setup_10.x /home/install/setup_nodejs.sh
RUN chmod a+x /home/install/setup_nodejs.sh \
    && /home/install/setup_nodejs.sh \
    && apt-get install -y nodejs

RUN pwd && ls src && ls /home/install
COPY NewsBrowser /home/install/src
COPY NewsBrowser/import /home/data/import

WORKDIR /home/install/src

COPY ./docker/entrypoint.sh /entrypoint.sh
RUN chmod a+x /entrypoint.sh
ENTRYPOINT ["/entrypoint.sh"]
