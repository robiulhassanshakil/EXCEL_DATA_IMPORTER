#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["DataImporter.ExcelToDatabaseService/DataImporter.ExcelToDatabaseService.csproj", "DataImporter.ExcelToDatabaseService/"]
COPY ["DataImporter.Importing/DataImporter.Importing.csproj", "DataImporter.Importing/"]
COPY ["DataImporter.Common/DataImporter.Common.csproj", "DataImporter.Common/"]
COPY ["DataImporter.Membership/DataImporter.Membership.csproj", "DataImporter.Membership/"]
COPY ["DataImporter.Data/DataImporter.Data.csproj", "DataImporter.Data/"]
RUN dotnet restore "DataImporter.ExcelToDatabaseService/DataImporter.ExcelToDatabaseService.csproj"
COPY . .
WORKDIR "/src/DataImporter.ExcelToDatabaseService"
RUN dotnet build "DataImporter.ExcelToDatabaseService.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "DataImporter.ExcelToDatabaseService.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "DataImporter.ExcelToDatabaseService.dll"]