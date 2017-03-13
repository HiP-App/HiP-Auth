FROM microsoft/dotnet:1.0.1-sdk-projectjson

RUN mkdir -p /dotnetapp

COPY src /dotnetapp

WORKDIR /dotnetapp

RUN dotnet restore

EXPOSE 5001

WORKDIR /dotnetapp/Auth

ENTRYPOINT ["dotnet", "run", "-p", "project.json"]