FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY *.sln .
COPY EventManager.Web/*.csproj ./EventManager.Web/
COPY EventManager.Application/*.csproj ./EventManager.Application/
COPY EventManager.Infrastructure/*.csproj ./EventManager.Infrastructure/

RUN dotnet restore "EventManager.Web/EventManager.Web.csproj"

COPY . .

RUN dotnet publish -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app .
ENV ASPNETCORE_URLS=http://*:8080
EXPOSE 8080
ENTRYPOINT ["dotnet", "EventManager.Web.dll"]
