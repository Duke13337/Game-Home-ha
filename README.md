# Game-Home-ha

## Описание проекта

**Game-home-ha** — это консольное C# приложение для управления игровой платформой, позволяющее пользователям отслеживать свои достижения в различных играх, добавлять друзей и взаимодействовать с игровыми платформами. Проект демонстрирует объектно-ориентированное проектирование с использованием баз данных, в котором реализованы основные концепции ООП — наследование, полиморфизм и инкапсуляция.

---

## Основные возможности

- Управление платформами (например, Steam, Epic Games и др.)
- Добавление и хранение информации об играх, относящихся к каждой платформе
- Учёт достижений, доступных в каждой игре
- Ведение списка пользователей с возможностью добавления друзей
- Отслеживание, какие достижения были разблокированы каждым пользователем и когда
- Реализация связей между пользователями, играми и достижениями с помощью базы данных

---

## Технические детали

- **Язык:** C#
- **Тип приложения:** Консольное приложение
- **База данных:** Любая реляционная СУБД (например, SQLite, SQL Server, PostgreSQL)
- **Объектно-ориентированное проектирование:**
  - 7+ классов: User, Game, Platform, Achievement, UserAchievement, FriendLink и др.
  - Использованы ключевые механизмы ООП: наследование, полиморфизм, инкапсуляция
- **Диаграммы UML:**
  - Диаграмма классов — для описания структуры и связей классов
  - Диаграмма последовательностей — для отображения взаимодействия объектов в ключевых сценариях
  - Диаграмма вариантов использования — для отображения функций и действий пользователей

---

## Структура базы данных

В базе данных реализовано не менее 7 таблиц с отношениями:
- Один-к-одному
- Один-ко-многим
- Многие-ко-многим

Пример таблиц:
- Platforms
- Games
- Achievements
- Users
- UserAchievements
- FriendLinks
- и другие

---

## Планы на будущее

- Добавление графического интерфейса пользователя (GUI)
- Расширение возможностей социального взаимодействия (чат, групповые достижения)
- Интеграция с внешними игровыми API для автоматического обновления достижений

---

![UML Diagram](https://github.com/user-attachments/assets/1998fd9c-b240-41df-99bd-ce5f6c090db3)


### Описание структуры базы данных 

**Таблицы и связи:**

1. **Platform**
    
    - `id` (PK)
        
    - `name` (название платформы)
        
    - Связь: один ко многим с таблицей **Game**.
        
2. **Account**
    
    - `id` (PK)
        
    - `username`, `email` (уникальные)
        
    - Связь: один к одному с таблицей **User**.
        
3. **User**
    
    - `id` (PK)
        
    - `account_id` (FK → Account.id)
        
    - `password`
        
    - Связи:
        
        - один к одному с **Account** и **EpicAccount**,
            
        - один ко многим с **GameSession**,
            
        - многие ко многим с **User** (через **FriendLink**) и **Achievement** (через **UserAchievement**).
            
4. **EpicAccount**
    
    - `id` (PK)
        
    - `user_id` (FK → User.id, уникальный)
        
    - Связь: один к одному с **User**.
        
5. **Game**
    
    - `id` (PK)
        
    - `platformid` (FK → Platform.id)
        
    - Связи:
        
        - один ко многим с **GameSession** и **Achievement**.
            
6. **GameSession**
    
    - `id` (PK)
        
    - `userid` (FK → User.id), `gameid` (FK → Game.id)
        
    - Связь: многие ко одному с **User** и **Game**.
        
7. **Achievement**
    
    - `id` (PK)
        
    - `gameid` (FK → Game.id)
        
    - Связь: многие ко многим с **User** через **UserAchievement**.
        
8. **UserAchievement**
    
    - Составной PK (`userid`, `achievementid`)
        
    - Связь: многие ко многим между **User** и **Achievement**.
        
9. **FriendLink**
    
    - Составной PK (`Userid`, `friendid`)
        
    - Связь: многие ко многим между **User** и **User**.


### SQL скрипт БД

```
CREATE TABLE Platform (
    id SERIAL PRIMARY KEY,
    name VARCHAR(100) NOT NULL
);

CREATE TABLE Account (
    id SERIAL PRIMARY KEY,
    username VARCHAR(100) UNIQUE NOT NULL,
    email VARCHAR(100) UNIQUE NOT NULL
);

CREATE TABLE "User" (
    id SERIAL PRIMARY KEY,
    account_id INT UNIQUE NOT NULL,
    password VARCHAR(100) NOT NULL,
    FOREIGN KEY (account_id) REFERENCES Account(id)
);

CREATE TABLE EpicAccount (
    id SERIAL PRIMARY KEY,
    name VARCHAR(100) NOT NULL,
    email VARCHAR(100) UNIQUE NOT NULL,
    user_id INT UNIQUE NOT NULL,
    FOREIGN KEY (user_id) REFERENCES "User"(id)
);

CREATE TABLE Game (
    id SERIAL PRIMARY KEY,
    title VARCHAR(100) NOT NULL,
    genre VARCHAR(50),
    platformid INT NOT NULL,
    FOREIGN KEY (platformid) REFERENCES Platform(id)
);

-- Создание таблицы GameSession
CREATE TABLE GameSession (
    id SERIAL PRIMARY KEY,
    userid INT NOT NULL,
    gameid INT NOT NULL,
    StartTime TIMESTAMP NOT NULL,
    durationMinutes INT NOT NULL,
    FOREIGN KEY (userid) REFERENCES "User"(id),
    FOREIGN KEY (gameid) REFERENCES Game(id)
);

CREATE TABLE Achievement (
    id SERIAL PRIMARY KEY,
    gameid INT NOT NULL,
    name VARCHAR(100) NOT NULL,
    description TEXT,
    FOREIGN KEY (gameid) REFERENCES Game(id)
);

CREATE TABLE UserAchievement (
    userid INT NOT NULL,
    achievementid INT NOT NULL,
    unlockedAt TIMESTAMP NOT NULL,
    PRIMARY KEY (userid, achievementid),
    FOREIGN KEY (userid) REFERENCES "User"(id),
    FOREIGN KEY (achievementid) REFERENCES Achievement(id)
);

CREATE TABLE FriendLink (
    Userid INT NOT NULL,
    friendid INT NOT NULL,
    PRIMARY KEY (Userid, friendid),
    FOREIGN KEY (Userid) REFERENCES "User"(id),
    FOREIGN KEY (friendid) REFERENCES "User"(id)
);
```

