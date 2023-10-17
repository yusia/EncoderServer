FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore "./Encoder/EncoderServer.csproj" --disable-parallel

RUN dotnet publish "./Encoder/EncoderServer.csproj" -c Release -o /app --no-restore


FROM mcr.microsoft.com/dotnet/sdk:7.0
WORKDIR /app
COPY --from=build /app ./


ENTRYPOINT ["dotnet", "EncoderServer.dll"]