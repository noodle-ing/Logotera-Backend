# 🚀 Logotera Backend - Инструкция по развертыванию

## 📋 Что было настроено

### ✅ Подключение к PostgreSQL
- **Сервер**: 176.123.179.219:5432  
- **База данных**: logotera  
- **Пользователь**: postgres  
- **Автоматическое применение миграций** при запуске

### ✅ Docker Configuration
- Многоэтапный Dockerfile для оптимальной сборки
- Автоматическая инициализация базы данных
- Создание администратора по умолчанию
- Настройка безопасности (непривилегированный пользователь)

### ✅ Функциональность
- ASP.NET Core 8.0 с Entity Framework
- JWT аутентификация
- Identity система с ролями (Admin/User)
- CORS настроен для фронтенда
- Swagger UI для документации API

## 🚀 Команды для запуска

### Сборка Docker образа
```bash
docker build -t logotera-backend .
```

### Запуск контейнера
```bash
# Основной запуск на порту 8080
docker run -d --name logotera-backend -p 8080:5000 logotera-backend

# Или через docker-compose
docker-compose up -d
```

### Проверка работы
```bash
# Проверить статус
docker ps

# Посмотреть логи
docker logs logotera-backend

# Тестировать API
curl http://localhost:8080/weatherforecast
```

## 🔧 Управление контейнером

```bash
# Остановить
docker stop logotera-backend

# Удалить
docker rm logotera-backend

# Пересобрать при изменениях
docker build -t logotera-backend . && docker run -d --name logotera-backend -p 8080:5000 logotera-backend
```

## 📊 API Endpoints

| Endpoint | Описание |
|----------|----------|
| `GET /weatherforecast` | Тестовый endpoint |
| `GET /swagger` | Документация API (в Development) |
| `/api/auth/*` | Аутентификация |
| `/api/users/*` | Управление пользователями |
| `/api/tasks/*` | Задачи |
| `/api/weeklyreports/*` | Еженедельные отчеты |

## 🔐 Администратор по умолчанию

- **Логин**: admin
- **Пароль**: Admin@123
- **Email**: admin@admin
- **Роль**: Admin

## 🌐 Доступ к приложению

После запуска приложение будет доступно:
- **API**: http://localhost:8080
- **Swagger UI**: http://localhost:8080/swagger (в Development режиме)

## 🔍 Диагностика

### Проверка подключения к БД
```bash
# Тест подключения к PostgreSQL
nc -zv 176.123.179.219 5432
```

### Просмотр логов
```bash
# Просмотр всех логов
docker logs logotera-backend

# Просмотр логов в реальном времени
docker logs -f logotera-backend
```

## 📝 Настройки

Основные настройки находятся в `appsettings.json`:
- Строка подключения к БД
- JWT секреты
- CORS политики
- Настройки администратора

## 🚨 Безопасность

- Пароли не хранятся в открытом виде
- JWT токены с ограниченным временем жизни
- CORS настроен только для разрешенных доменов
- Контейнер работает под непривилегированным пользователем

## 📞 Поддержка

При возникновении проблем:
1. Проверьте логи: `docker logs logotera-backend`
2. Убедитесь в доступности БД: `nc -zv 176.123.179.219 5432`
3. Перезапустите контейнер: `docker restart logotera-backend` 