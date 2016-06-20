FROM microsoft/dotnet:onbuild

RUN printf "deb http://ftp.us.debian.org/debian jessie main\n" >> /etc/apt/sources.list

EXPOSE 5000

ENTRYPOINT ["dotnet", "run", "web"]

COPY . /src/Auth
WORKDIR /src/Auth

RUN ["dotnet", "restore"]


