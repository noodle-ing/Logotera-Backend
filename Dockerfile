# Используем официальный образ .NET 8.0 SDK для сборки
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

# Копируем файл проекта и восстанавливаем зависимости
COPY Logotera/*.csproj ./Logotera/
COPY *.sln ./
RUN dotnet restore

# Копируем весь исходный код
COPY . ./

# Собираем приложение
RUN dotnet publish Logotera/Logotera.csproj -c Release -o out

# Используем runtime образ для финального контейнера
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Копируем собранное приложение из предыдущего этапа
COPY --from=build-env /app/out .

# Создаем пользователя для безопасности
RUN addgroup --gid 1001 --system dotnetgroup && \
    adduser --uid 1001 --system --gid 1001 dotnetuser && \
    chown -R dotnetuser:dotnetgroup /app

USER dotnetuser

# Открываем порт 5000 для HTTP и 5001 для HTTPS
EXPOSE 5000
EXPOSE 5001

# Настраиваем переменные окружения
ENV ASPNETCORE_URLS=http://+:5000
ENV ASPNETCORE_ENVIRONMENT=Production

# Запускаем приложение
ENTRYPOINT ["dotnet", "Logotera.dll"] 