FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app

# Copy everything
COPY ./TelegramInteraction ./TelegramInteraction
COPY ./BusinessLogic ./BusinessLogic
# Restore as distinct layers
RUN dotnet restore TelegramInteraction/TelegramInteraction.csproj
# Build and publish a release
RUN dotnet publish -c Release -o out TelegramInteraction/TelegramInteraction.csproj

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "TelegramInteraction.dll"]