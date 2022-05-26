FROM mcr.microsoft.com/dotnet/sdk:5.0 AS src
WORKDIR /src

COPY ../src .

RUN dotnet build Minibank.Web -c Release
RUN dotnet test Minibank.Core.Tests --no-build
RUN dotnet publish Minibank.Web -c Release --no-build -o /dist

FROM mcr.microsoft.com/dotnet/aspnet:5.0 as final
WORKDIR /app

COPY --from=src /dist .

ENV ASPNETCORE_URLS=http://*:5001;http://*:5000
EXPOSE 5001 5000

ENTRYPOINT ["dotnet", "Minibank.Web.dll"]