#!/bin/bash

# 📋 Скрипт для просмотра логов Logotera Backend
# Использование: ./logs.sh [количество_строк]

CONTAINER_NAME="logotera-backend"
LINES=${1:-50}  # По умолчанию 50 строк

echo "📋 Логи контейнера $CONTAINER_NAME (последние $LINES строк):"
echo "========================================================"

if docker ps -q -f name=$CONTAINER_NAME | grep -q .; then
    docker logs --tail $LINES -f $CONTAINER_NAME
else
    echo "❌ Контейнер $CONTAINER_NAME не запущен!"
    echo "💡 Запустите его командой: ./deploy.sh"
fi 