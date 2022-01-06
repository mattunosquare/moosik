FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore moosik.api/moosik.api.csproj
RUN dotnet publish moosik.api/moosik.api.csproj -o /app --no-restore
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build /app ./
ENV ASPNETCORE_URLS=http://*:5000
ARG API_CONNECTION_STRING
ENV CONNECTION_STRING=$API_CONNECTION_STRING
EXPOSE 5000
ENTRYPOINT ["dotnet", "moosik.api.dll"]