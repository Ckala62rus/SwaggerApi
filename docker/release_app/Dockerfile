#FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
FROM mcr.microsoft.com/dotnet/sdk:5.0
WORKDIR /src

#Begin Sonar
RUN dotnet tool install -g dotnet-sonarscanner
RUN dotnet tool install --global dotnet-ef --version 5.0.0

ENV PATH="$PATH:/root/.dotnet/tools"

ENTRYPOINT [ "dotnet", "Architecture.dll" ]
