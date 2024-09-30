FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Services/Messages.API/Messages.API.csproj", "Services/Messages.API/"]
COPY ["BuildingBlocks/BuildingBlocks/BuildingBlocks.csproj", "BuildingBlocks/BuildingBlocks/"]
COPY ["StepBook.Domain/StepBook.Domain.csproj", "StepBook.Domain/"]
COPY ["Services/Users.API/Users.API.csproj", "Services/Users.API/"]
RUN dotnet restore "Services/Messages.API/Messages.API.csproj"
COPY . .
WORKDIR "/src/Services/Messages.API"
RUN dotnet build "Messages.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Messages.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Messages.API.dll"]
