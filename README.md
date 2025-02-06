# Дипломная работа по .NET для STEP IT Academy Баку

## Описание проекта

StepBook - это социальная сеть, которая объединяет людей, учащихся в STEP IT Academy. Пользователи могут создавать
профили, обмениваться сообщениями и ставить лайки друг-другу.

## Технологии

- ASP.NET Core
- Entity Framework Core
- SignalR
- AutoMapper
- FluentValidation
- PostgresSQL
- Docker
- Swagger
- JWT
- Serilog
- Amazon S3 (для хранения файлов)
- Azure (Azure PostgresSQL)
- CloudinaryData (для хранения фотографий пользователей)

## Архитектура проекта

Проект построен на основе N-Tier архитектуры. В решении присутствуют следующие проекты:

- StepBook.API - проект API
- StepBook.BLL - бизнес-логика
- StepBook.DAL - доступ к данным
- StepBook.DTO - Data Transfer Objects
- StepBook.BuildingBlocks - общие классы и методы

## Клиентское приложение

Одностраничное приложение на Angular

## Запуск проекта

1. Установите Docker
2. Склонируйте репозиторий
3. Перейдите в корень проекта и настройте строку подключения к базе данных в файле `appsettings.json`
4. В корне проекта выполните команду `docker-compose up`
5. Перейдите по адресу `https://localhost:7035/swagger` для просмотра документации API

## API Endpoints

### Account

- POST /api/account/register - регистрация пользователя
- POST /api/account/login - авторизация пользователя
- POST /api/account/refresh-token - обновление токена
- POST /api/account/logout - выход из аккаунта
- GET /api/account/email-confirm-code - подтверждение email
- PUT /api/account/change-password - изменение пароля
- PUT /api/account/change-username - изменение логина пользователя
- POST /api/account/forgot-password - восстановление пароля
- POST /api/account/reset-password - сброс пароля
- DELETE /api/account/delete-account - удаление аккаунта
- POST /api/account/request-confirmation-code - запрос кода подтверждения

### Admin

- POST /api/admin/add-to-blacklist/{username} - добавить пользователя в черный список
- POST /api/admin/remove-from-blacklist/{username} - удалить пользователя из черного списка
- GET /api/admin/blacklist - получить список пользователей в черном списке
- GET /api/admin/reports - получить список жалоб
- DELETE /api/admin/delete-user-account/{username} - удалить аккаунт пользователя
- GET /api/admin/users - получить список пользователей

### Report

- POST /api/report/add-report-to-user/{username} - отправить жалобу на пользователя

### Buckets

- POST /api/buckets/upload-file - загрузить файл
- GET /api/buckets/get-file-url - получить URL файла
- GET /api/buckets/get-all-files - получить список файлов
- DELETE /api/buckets/delete-file - удалить файл

### Likes

- POST /api/likes/{username} - поставить лайк пользователю
- GET /api/likes - получить список лайкнутых пользователей

### Messages

- POST /api/messages - отправить сообщение
- GET /api/messages - получить список сообщений
- GET /api/messages/thread/{username} - получить список сообщений с пользователем
- DELETE /api/messages/{id} - удалить сообщение

### Users

- GET /api/users - получить список пользователей
- PUT /api/users - обновить пользователя
- GET /api/users/{id} - получить пользователя по id
- GET /api/users/{username} - получить пользователя по username
- POST /api/users/add-photo - добавить фото
- PUT /api/users/set-main-photo/{photoId} - установить главное фото
- DELETE /api/users/delete-photo/{photoId} - удалить фото