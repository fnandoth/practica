FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["ProductServices.csproj", "ProductServices/"]
RUN dotnet restore "ProductServices/ProductServices.csproj"

COPY . ./ProductServices
WORKDIR "/src/ProductServices"
RUN dotnet build "ProductServices.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ProductServices.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "ProductServices.dll"]
