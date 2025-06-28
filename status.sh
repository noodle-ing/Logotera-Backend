#!/bin/bash

# 📊 Скрипт для проверки статуса Logotera Backend
# Использование: ./status.sh

CONTAINER_NAME="logotera-backend"
PORT="8080"

echo "📊 Статус Logotera Backend"
echo "=========================="

# Проверка контейнера
if docker ps -q -f name=$CONTAINER_NAME | grep -q .; then
    echo "✅ Контейнер: ЗАПУЩЕН"
    
    # Информация о контейнере
    echo "📋 Информация о контейнере:"
    docker ps --format "table {{.Names}}\t{{.Status}}\t{{.Ports}}" -f name=$CONTAINER_NAME
    
    # Проверка API
    echo ""
    echo "🔍 Проверка API..."
    if curl -f -s http://localhost:$PORT/weatherforecast > /dev/null 2>&1; then
        echo "✅ API: ДОСТУПЕН (http://localhost:$PORT)"
    else
        echo "❌ API: НЕДОСТУПЕН"
    fi
    
    # Использование ресурсов
    echo ""
    echo "📈 Использование ресурсов:"
    docker stats $CONTAINER_NAME --no-stream --format "table {{.Container}}\t{{.CPUPerc}}\t{{.MemUsage}}\t{{.NetIO}}"
    
else
    echo "❌ Контейнер: НЕ ЗАПУЩЕН"
    
    # Проверка наличия остановленного контейнера
    if docker ps -a -q -f name=$CONTAINER_NAME | grep -q .; then
        echo "ℹ️ Найден остановленный контейнер"
        docker ps -a --format "table {{.Names}}\t{{.Status}}" -f name=$CONTAINER_NAME
    fi
    
    echo "💡 Для запуска выполните: ./deploy.sh"
fi

echo ""
echo "🛠️ Полезные команды:"
echo "  ./deploy.sh   - Полная пересборка и запуск"
echo "  ./restart.sh  - Быстрый перезапуск"
echo "  ./logs.sh     - Просмотр логов"
echo "  ./stop.sh     - Остановка" 