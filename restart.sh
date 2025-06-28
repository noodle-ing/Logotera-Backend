#!/bin/bash

# 🔄 Скрипт для перезапуска Logotera Backend (без пересборки)
# Использование: ./restart.sh

CONTAINER_NAME="logotera-backend"
IMAGE_NAME="logotera-backend"
PORT="8080"

echo "🔄 Перезапуск контейнера $CONTAINER_NAME..."

# Остановка
if docker ps -q -f name=$CONTAINER_NAME | grep -q .; then
    echo "⏹️ Остановка контейнера..."
    docker stop $CONTAINER_NAME
    docker rm $CONTAINER_NAME
fi

# Запуск
echo "▶️ Запуск контейнера..."
docker run -d \
    --name $CONTAINER_NAME \
    --restart unless-stopped \
    -p $PORT:5000 \
    $IMAGE_NAME

if docker ps -q -f name=$CONTAINER_NAME | grep -q .; then
    echo "✅ Контейнер успешно перезапущен!"
    echo "🌐 API доступен по адресу: http://localhost:$PORT"
else
    echo "❌ Ошибка при перезапуске контейнера!"
    exit 1
fi 