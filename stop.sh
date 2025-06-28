#!/bin/bash

# 🛑 Скрипт для остановки Logotera Backend
# Использование: ./stop.sh

CONTAINER_NAME="logotera-backend"

echo "🛑 Остановка контейнера $CONTAINER_NAME..."

if docker ps -q -f name=$CONTAINER_NAME | grep -q .; then
    docker stop $CONTAINER_NAME
    docker rm $CONTAINER_NAME
    echo "✅ Контейнер $CONTAINER_NAME остановлен и удален"
else
    if docker ps -a -q -f name=$CONTAINER_NAME | grep -q .; then
        docker rm $CONTAINER_NAME
        echo "✅ Контейнер $CONTAINER_NAME удален (уже был остановлен)"
    else
        echo "ℹ️ Контейнер $CONTAINER_NAME не найден"
    fi
fi 