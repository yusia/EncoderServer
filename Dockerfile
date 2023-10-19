FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore "./Encoder/Encoder.Server.csproj" --disable-parallel

RUN dotnet publish "./Encoder/Encoder.Server.csproj" -c Release -o /app --no-restore


FROM mcr.microsoft.com/dotnet/sdk:7.0
WORKDIR /app
COPY --from=build /app ./


ENTRYPOINT ["dotnet", "Encoder.Server.dll"]