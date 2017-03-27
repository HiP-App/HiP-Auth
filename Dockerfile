FROM microsoft/dotnet:1.1.1-sdk
RUN dotnet nuget locals all --clear
RUN mkdir -p /dotnetapp
COPY src /dotnetapp
WORKDIR /dotnetapp

EXPOSE 5001

RUN dotnet restore --no-cache
WORKDIR /dotnetapp/Auth
ENTRYPOINT ["dotnet", "run"]