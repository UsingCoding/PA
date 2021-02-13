FROM library/ubuntu:20.04

RUN apt-get update && apt-get install -y curl make

WORKDIR /tmp/builer

ADD dotnet-install.sh . 

RUN ./dotnet-install.sh -c 5.0
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=1

RUN apt clean autoclean && \
    apt autoremove -y && \
    rm -rf /var/lib/{apt,dpkg,cache,log}/ && \
    rm -rf /tmp/builer
    
WORKDIR /app
RUN ln -s /root/.dotnet/dotnet /usr/local/bin/dotnet