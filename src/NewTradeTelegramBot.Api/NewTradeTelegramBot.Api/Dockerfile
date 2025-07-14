# 1️⃣ Base image — faqat runtime uchun (Debug uchun ham ishlaydi)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# 2️⃣ Build stage — SDK bilan build qilish
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# csproj faylni nusxalash
COPY ["src/NewTradeTelegramBot.Api/NewTradeTelegramBot.Api/NewTradeTelegramBot.Api.csproj", "src/NewTradeTelegramBot.Api/"]
RUN dotnet restore "src/NewTradeTelegramBot.Api/NewTradeTelegramBot.Api.csproj"

# Qolgan fayllarni nusxalash
COPY . .

WORKDIR "src/NewTradeTelegramBot.Api"
RUN dotnet build "NewTradeTelegramBot.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# 3️⃣ Publish stage — faqat release tayyorlash
FROM build AS publish
WORKDIR "src/NewTradeTelegramBot.Api"
RUN dotnet publish "NewTradeTelegramBot.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# 4️⃣ Final runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NewTradeTelegramBot.Api.dll"]
