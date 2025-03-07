# Event Manager API

Этот проект представляет собой API для управления событиями, построенный на ASP.NET Core с использованием чистой архитектуры (Clean Architecture).

## Требования

- .NET 8 SDK
- Docker
- PostgreSQL

## Запуск проекта

### 1. Установите Docker
Убедитесь, что Docker установлен на вашем компьютере. 

###  2. Запустите проект с Docker Compose
Выполните следующие команды:
```
docker-compose build
docker-compose up
```
### 3. Откройте Swagger UI

После запуска проекта откройте Swagger UI в браузере:  
[http://localhost:5000/swagger](http://localhost:5000/swagger)

## Инициализация базы данных

При первом запуске проекта база данных автоматически инициализируется с администратором:

- Email: `administrator@example.com`
- Пароль: `AdminPassword123`
