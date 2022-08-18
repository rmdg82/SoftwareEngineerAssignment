FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["SoftwareEngineerAssignment.Api.csproj", "SoftwareEngineerAssignment.Api/"]
RUN dotnet restore "SoftwareEngineerAssignment.Api.csproj"
COPY . .
WORKDIR "/src/SoftwareEngineerAssignment.Api"
RUN dotnet build "SoftwareEngineerAssignment.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SoftwareEngineerAssignment.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SoftwareEngineerAssignment.Api.dll"]