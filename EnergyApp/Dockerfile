FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["EnergyApp/EnergyApp.csproj", "EnergyApp/"]
RUN dotnet restore "EnergyApp/EnergyApp.csproj"
COPY . .
WORKDIR "/src/EnergyApp"
RUN dotnet build "EnergyApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EnergyApp.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EnergyApp.dll"]
