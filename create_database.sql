CREATE DATABASE csharp_database;

\c csharp_database

CREATE TABLE "JsonModels"(
    "Id" SERIAL PRIMARY KEY,
    "BsonData" JSON NOT NULL
);
