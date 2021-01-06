FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build-env
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:3000

COPY *.csproj ./
RUN dotnet restore
COPY . ./
#ENTRYPOINT ["dotnet","run"]
RUN dotnet publish -c Release -o out --no-restore
FROM mcr.microsoft.com/dotnet/core/aspnet:2.2

WORKDIR /app
COPY --from=build-env /app/out ./
ENTRYPOINT ["dotnet", "EmailService.dll"]
