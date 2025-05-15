FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["UserServices.csproj", "UserServices/"]
RUN dotnet restore "UserServices/UserServices.csproj"

COPY . ./UserServices
WORKDIR "/src/UserServices"
RUN dotnet build "UserServices.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "UserServices.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "UserServices.dll"]
