# CurrencyApplication

# Currency Microservices Solution

## Overview

Пример простой микросервисной системы, которая позволяет управлять пользователями и их любимыми валютами,
демонстрирующий современные возможности платформы .NET.
Использует компоненты:

* **Docker + Docker Compose**
* **ASP.NET Core**
* **API Gateway (YARP)**
* **MediatR (CQRS pattern)**
* **OpenAPI + Scalar Web UI**
* **PostgreSQL**
* **Unit testing (xUnit + Moq)**

Обращение от клиентов происходит через **API Gateway**, который проксирует запросы до нужного сервиса.

---

## Gateway

ASP.NET Core Web API;
API Gateway и reverse proxy;

### Ответственность

* Визуализирует и описывает API имеющихся сервисов
* Позволяет вызывать методы из веб-интерфейса
* Проксирует запросы до ендпоинтов сервисов

### Технологии

* ASP.NET Core
* YARP Reverse Proxy
* Scalar UI

---

## FinanceService

ASP.NET Core Web API

### Ответственность

* Менеджмент валют и управление избранным валютами пользователя
* Позволяет получить список имеющихся валют
* Привязать или отвязать валюту для пользователя
* Получить курсы привязанных к пользователю валют

### Технологии

* ASP.NET Core
* MediatR / CQRS pattern
* PostgreSQL
* Entity Framework Core
* JWT Authorization

---

## UserService

ASP.NET Core Web API

### Ответственность

* Регистрация / аутентификация пользователя
* Генерация / валидация JWT

### Технологии

* ASP.NET Core
* MediatR / SQRS pattern
* Password hashing / verification
* PostgreSQL

---

## FinanceService.Tests

Unit Test Project (xUnit)
Набор unit-тестов для FinanceService

### Технологии

* xUnit
* Moq
* FluentAssertions

---

## UserService.Tests

Unit Test Project (xUnit)
Набор unit-тестов UserService

### Technologies

* xUnit
* Moq
* FluentAssertions

---

# Инфраструктура

## Docker

Общение между контейнерами происходит во внутренней docker сети.

### Сервисы

```
Gateway
FinanceService
UserService
PostgreSQL
```

### Межсервисное взаимодействие

```
Gateway -> FinanceService
Gateway -> UserService
Services -> PostgreSQL
```

---

# СУБД

**Database:** PostgreSQL 16

### Ответственность

* Хранение юзеров
* Хранение валют / их курсов
* Хранение сведений о валютах, привязанных к пользователю

### Возможности

* Автоматическая миграция в рамках работы MigrationService

---

# Аутентификация

Для аутентификации/авторизации используется  **JSON Web Tokens (JWT)**, сессия существует в рамках жизненного
цикла токена (по умолчанию - 4 часа)

# Запуск решения

## Собрать и запустить сервисы

```
docker-compose up --build
```

---

## Для допуска Access API Gateway / Documentation / Health check

```
http://localhost:5000 / http://localhost:5000/scalar / http://localhost:5000/health
```

---

# Testing

Для запуск unit тестов:

```
dotnet test
```

---
