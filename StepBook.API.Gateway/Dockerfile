FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["StepBook.API.Gateway/StepBook.API.Gateway.csproj", "StepBook.API.Gateway/"]
RUN dotnet restore "StepBook.API.Gateway/StepBook.API.Gateway.csproj"
COPY . .
WORKDIR "/src/StepBook.API.Gateway"
RUN dotnet build "StepBook.API.Gateway.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "StepBook.API.Gateway.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "StepBook.API.Gateway.dll"]
