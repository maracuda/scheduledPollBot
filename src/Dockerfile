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
ENV PORT=10000
ENV VOSTOK_IDENTITY_PROJECT=PollBot
ENV VOSTOK_IDENTITY_APPLICATION=PollBot
ENV VOSTOK_IDENTITY_INSTANCE=1
ENTRYPOINT ["dotnet", "TelegramInteraction.dll"]
