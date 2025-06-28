#!/bin/bash

# 🚀 Logotera Backend - Скрипт автоматического развертывания
# Использование: ./deploy.sh

set -e  # Остановить выполнение при ошибке

# Цвета для вывода
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
NC='\033[0m' # No Color

# Настройки
CONTAINER_NAME="logotera-backend"
IMAGE_NAME="logotera-backend"
PORT="8080"

echo -e "${BLUE}🚀 Запуск развертывания Logotera Backend...${NC}"
echo "=================================================="

# Функция для вывода с временной меткой
log() {
    echo -e "${GREEN}[$(date '+%H:%M:%S')]${NC} $1"
}

error() {
    echo -e "${RED}[ERROR]${NC} $1"
}

warning() {
    echo -e "${YELLOW}[WARNING]${NC} $1"
}

# Проверка наличия Docker
if ! command -v docker &> /dev/null; then
    error "Docker не установлен! Установите Docker и повторите попытку."
    exit 1
fi

# Остановка и удаление существующего контейнера
log "Остановка существующих контейнеров..."
if docker ps -q -f name=$CONTAINER_NAME | grep -q .; then
    warning "Найден работающий контейнер $CONTAINER_NAME. Останавливаю..."
    docker stop $CONTAINER_NAME
    docker rm $CONTAINER_NAME
    log "Контейнер $CONTAINER_NAME остановлен и удален"
else
    if docker ps -a -q -f name=$CONTAINER_NAME | grep -q .; then
        warning "Найден остановленный контейнер $CONTAINER_NAME. Удаляю..."
        docker rm $CONTAINER_NAME
        log "Контейнер $CONTAINER_NAME удален"
    else
        log "Существующих контейнеров не найдено"
    fi
fi

# Удаление старого образа (опционально)
if docker images -q $IMAGE_NAME | grep -q .; then
    log "Удаление старого образа $IMAGE_NAME..."
    docker rmi $IMAGE_NAME || warning "Не удалось удалить старый образ (возможно, используется)"
fi

# Сборка нового образа
log "Сборка Docker образа..."
docker build -t $IMAGE_NAME . || {
    error "Ошибка при сборке Docker образа!"
    exit 1
}

log "Docker образ $IMAGE_NAME успешно собран!"

# Запуск контейнера
log "Запуск нового контейнера..."
docker run -d \
    --name $CONTAINER_NAME \
    --restart unless-stopped \
    -p $PORT:5000 \
    $IMAGE_NAME || {
    error "Ошибка при запуске контейнера!"
    exit 1
}

# Ожидание запуска
log "Ожидание запуска приложения..."
sleep 10

# Проверка статуса
if docker ps -q -f name=$CONTAINER_NAME | grep -q .; then
    log "✅ Контейнер $CONTAINER_NAME успешно запущен!"
    
    # Показать логи за последние 10 строк
    echo -e "\n${BLUE}📋 Последние логи:${NC}"
    docker logs --tail 10 $CONTAINER_NAME
    
    # Проверка доступности API
    echo -e "\n${BLUE}🔍 Проверка API...${NC}"
    sleep 5
    
    if curl -f -s http://localhost:$PORT/weatherforecast > /dev/null 2>&1; then
        log "✅ API доступен по адресу: http://localhost:$PORT"
    else
        warning "API пока недоступен. Проверьте логи: docker logs $CONTAINER_NAME"
    fi
    
else
    error "Контейнер не запущен! Проверьте логи: docker logs $CONTAINER_NAME"
    exit 1
fi

# Финальная информация
echo -e "\n${GREEN}🎉 Развертывание завершено успешно!${NC}"
echo "=================================================="
echo -e "${BLUE}📊 Информация о сервисе:${NC}"
echo "  🐳 Контейнер: $CONTAINER_NAME"
echo "  🌐 API URL: http://localhost:$PORT"
echo "  🧪 Тест API: http://localhost:$PORT/weatherforecast"
echo "  👤 Админ: admin / Admin@123"
echo ""
echo -e "${BLUE}📝 Полезные команды:${NC}"
echo "  Проверить статус: docker ps"
echo "  Посмотреть логи: docker logs $CONTAINER_NAME"
echo "  Остановить: docker stop $CONTAINER_NAME"
echo "  Перезапустить: docker restart $CONTAINER_NAME"
echo ""
echo -e "${GREEN}✅ Готово к работе!${NC}" 