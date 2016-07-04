FROM microsoft/dotnet:latest

RUN mkdir -p /dotnetapp
WORKDIR /dotnetapp/Auth

COPY src /dotnetapp
RUN dotnet restore

ENTRYPOINT ["dotnet", "run", "-p", "project.json"]

