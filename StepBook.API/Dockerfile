FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["StepBook.API/StepBook.API.csproj", "StepBook.API/"]
COPY ["StepBook.BLL/StepBook.BLL.csproj", "StepBook.BLL/"]
COPY ["StepBook.DAL/StepBook.DAL.csproj", "StepBook.DAL/"]
COPY ["StepBook.BuildingBlocks/StepBook.BuildingBlocks.csproj", "StepBook.BuildingBlocks/"]
COPY ["StepBook.DTO/StepBook.DTO.csproj", "StepBook.DTO/"]
RUN dotnet restore "StepBook.API/StepBook.API.csproj"
COPY . .
WORKDIR "/src/StepBook.API"
RUN dotnet build "StepBook.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "StepBook.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "StepBook.API.dll"]
