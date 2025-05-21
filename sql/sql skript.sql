CREATE TABLE Platform (
    id SERIAL PRIMARY KEY,
    name VARCHAR(100) NOT NULL
);

CREATE TABLE Account (
    id SERIAL PRIMARY KEY,
    username VARCHAR(100) UNIQUE NOT NULL,
    email VARCHAR(100) UNIQUE NOT NULL
);

CREATE TABLE User (
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
