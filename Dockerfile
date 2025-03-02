FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0.406 AS build
WORKDIR /src
COPY ["src/Bootstrapper/Api/Api.csproj", "src/Bootstrapper/Api/"]
COPY ["src/Modules/Memberships/Memberships.csproj", "src/Modules/Memberships/"]
COPY ["src/Shared/Shared.csproj", "src/Shared/"]
RUN dotnet restore "src/Bootstrapper/Api/Api.csproj"
COPY . .
WORKDIR "/src/src/Bootstrapper/Api"
RUN dotnet build "Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Api.dll"]
