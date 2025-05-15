FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["OrderServices.csproj", "OrderServices/"]
RUN dotnet restore "OrderServices/OrderServices.csproj"

COPY . ./OrderServices
WORKDIR "/src/OrderServices"
RUN dotnet build "OrderServices.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OrderServices.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "OrderServices.dll"]
