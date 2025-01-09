FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src

# copy all the layers' csproj files into respective folders
COPY ["./Architecture/Architecture.csproj", "src/Architecture/"]
COPY ["./Architecture.Core/Architecture.Core.csproj", "src/Architecture.Core/"]
COPY ["./Architecture.DAL/Architecture.DAL.csproj", "src/Architecture.DAL/"]
COPY ["./Architecture.Domain/Architecture.Domain.csproj", "src/Architecture.Domain/"]

# run restore over API project - this pulls restore over the dependent projects as well
RUN dotnet restore "src/Architecture/Architecture.csproj"

COPY . .

# run build over the API project
WORKDIR "/src/Architecture/"
RUN dotnet build -c Release -o /app/build

# run publish over the API project
FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

FROM base AS runtime
WORKDIR /app

COPY --from=publish /app/publish .
RUN ls -l
ENTRYPOINT [ "dotnet", "Architecture.dll" ]
