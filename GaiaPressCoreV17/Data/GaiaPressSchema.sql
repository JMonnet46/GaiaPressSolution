-- =============================================
-- Script: 01_CreateDatabaseSchema.sql
-- Description: Crée la base de données GaiaPressDB et ses tables
-- Auteur: [Votre nom]
-- Date: 19/08/2026
-- =============================================

-- 🔹 1. Créer la base de données (si elle n'existe pas)
USE [master];
GO

IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'GaiaPressDB')
BEGIN
    CREATE DATABASE [GaiaPressDB];
    PRINT 'Base de données GaiaPressDB créée.';
END
GO

-- 🔹 2. Se connecter à la base de données
USE [GaiaPressDB];
GO

-- 🔹 3. Créer les tables (schéma complet)
-- (Copiez ici tout le schéma SQL que je vous ai fourni précédemment)
CREATE TABLE [Users] (
    UserId NVARCHAR(450) PRIMARY KEY,
    Username NVARCHAR(256) NOT NULL UNIQUE,
    Email NVARCHAR(256) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(MAX) NOT NULL,
    Salt NVARCHAR(MAX) NOT NULL,
    CreatedAt DATETIME2 DEFAULT GETUTCDATE(),
    LastLogin DATETIME2 NULL
);
-- ... (autres tables)
GO

-- 🔹 4. Créer les index et contraintes
-- (Exemple : index sur Email pour les recherches rapides)
CREATE INDEX IX_UserEmail ON [Users](Email);
GO

-- 🔹 5. Insérer des données de test (optionnel)
-- INSERT INTO [Users] (UserId, Username, Email, PasswordHash, Salt)
-- VALUES ('123e4567-e89b-12d3-a456-426614174000', 'admin', 'admin@gaiapress.com', '...', '...');
-- GO