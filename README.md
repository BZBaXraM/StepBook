# Дипломная работа по .NET для STEP IT Academy Баку

## Описание проекта

StepBook - это социальная сеть, которая объединяет людей, учащихся в STEP IT Academy. Пользователи могут создавать
профили, добавлять других пользователей в друзья, обмениваться сообщениями и ставить лайки друг-другу.

## Технологии

- ASP.NET Core
- Entity Framework Core
- SignalR
- AutoMapper
- FluentValidation
- PostgreSQL
- Docker
- Swagger

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
- GET /api/account/email-confirm - подтверждение email
- PUT /api/account/change-password - изменение пароля
- POST /api/account/forgot-password - восстановление пароля
- POST /api/account/reset-password - сброс пароля
- GET /api/account/sign-google - авторизация через Google
- GET /api/account/login-google - вход через Google

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