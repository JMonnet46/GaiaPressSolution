IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [LesChantiersDb] (
    [RefChantier] nvarchar(450) NOT NULL,
    [NomChantier] nvarchar(max) NULL,
    [DescriptifChantier] nvarchar(max) NULL,
    [DateChantier] datetime2 NOT NULL,
    [RefClient] int NULL,
    [Entreprise] nvarchar(max) NULL,
    [Adresse] nvarchar(max) NULL,
    [Telephone] nvarchar(max) NULL,
    [Reference] nvarchar(max) NULL,
    [IsSaved] bit NOT NULL,
    [FilePath] nvarchar(max) NULL,
    [FileTitle] nvarchar(max) NULL,
    [EventsSupendus] bit NOT NULL,
    CONSTRAINT [PK_LesChantiersDb] PRIMARY KEY ([RefChantier])
);
GO

CREATE TABLE [Forages] (
    [RefForage] nvarchar(450) NOT NULL,
    [ChantierRefChantier] nvarchar(450) NULL,
    CONSTRAINT [PK_Forages] PRIMARY KEY ([RefForage]),
    CONSTRAINT [FK_Forages_LesChantiersDb_ChantierRefChantier] FOREIGN KEY ([ChantierRefChantier]) REFERENCES [LesChantiersDb] ([RefChantier])
);
GO

CREATE TABLE [ListeSolDb] (
    [RefSol] nvarchar(450) NOT NULL,
    [nomSol] nvarchar(max) NULL,
    [zoneGeo] nvarchar(max) NULL,
    [famille] nvarchar(max) NULL,
    [ClasseDuSol] nvarchar(max) NULL,
    [SousClasse] int NOT NULL,
    [ClasseInférieure] nvarchar(max) NULL,
    [SousClasseInférieure] int NOT NULL,
    [nomSolNormalise] nvarchar(max) NULL,
    [Dmax] real NOT NULL,
    [Tamisat50mm] real NOT NULL,
    [Tamisat2mm] real NOT NULL,
    [tamisat80microns] real NOT NULL,
    [VBS] real NOT NULL,
    [Ip] real NOT NULL,
    [Es] real NOT NULL,
    [Phimu] real NOT NULL,
    [ChantierRefChantier] nvarchar(450) NULL,
    CONSTRAINT [PK_ListeSolDb] PRIMARY KEY ([RefSol]),
    CONSTRAINT [FK_ListeSolDb_LesChantiersDb_ChantierRefChantier] FOREIGN KEY ([ChantierRefChantier]) REFERENCES [LesChantiersDb] ([RefChantier])
);
GO

CREATE TABLE [ListeSondeDb] (
    [RefSonde] nvarchar(450) NOT NULL,
    [NomSonde] nvarchar(max) NULL,
    [ChantierRefChantier] nvarchar(450) NULL,
    CONSTRAINT [PK_ListeSondeDb] PRIMARY KEY ([RefSonde]),
    CONSTRAINT [FK_ListeSondeDb_LesChantiersDb_ChantierRefChantier] FOREIGN KEY ([ChantierRefChantier]) REFERENCES [LesChantiersDb] ([RefChantier])
);
GO

CREATE TABLE [ListeEssaiExpansionsDb] (
    [RefEssai] nvarchar(450) NOT NULL,
    [RefSonde] nvarchar(450) NULL,
    [ClassPrecis] int NOT NULL,
    [typeEssai] nvarchar(max) NULL,
    [nomEssai] nvarchar(max) NULL,
    [dateEssai] datetime2 NOT NULL,
    [difPresEauAir] real NOT NULL,
    [hauteurCpv] real NOT NULL,
    [Di] real NOT NULL,
    [Vc] real NOT NULL,
    [Vs] real NOT NULL,
    [EpMembrane] real NOT NULL,
    [Prp] real NOT NULL,
    [PrV] real NOT NULL,
    [coefficientA] real NOT NULL,
    [coefficientB] real NOT NULL,
    [PrecisVc] real NOT NULL,
    [PrecisVs] real NOT NULL,
    [PrEpMembrane] real NOT NULL,
    [PrecisA] real NOT NULL,
    [PrecisB] real NOT NULL,
    CONSTRAINT [PK_ListeEssaiExpansionsDb] PRIMARY KEY ([RefEssai]),
    CONSTRAINT [FK_ListeEssaiExpansionsDb_ListeSondeDb_RefSonde] FOREIGN KEY ([RefSonde]) REFERENCES [ListeSondeDb] ([RefSonde])
);
GO

CREATE TABLE [ListeEssaiResistancesDb] (
    [RefEssai] nvarchar(450) NOT NULL,
    [RefSonde] nvarchar(450) NULL,
    [ClassPrecis] int NOT NULL,
    [nomEssai] nvarchar(max) NULL,
    [dateEssai] datetime2 NOT NULL,
    [typeEssai] real NOT NULL,
    [difPresEauAir] real NOT NULL,
    [hauteurCpv] real NOT NULL,
    [A0] real NOT NULL,
    [A1] real NOT NULL,
    [A2] real NOT NULL,
    [A3] real NOT NULL,
    [Pel] real NOT NULL,
    [Pe] real NOT NULL,
    [MySondeResistanceRefSonde] nvarchar(450) NULL,
    [MesureCount] int NOT NULL,
    CONSTRAINT [PK_ListeEssaiResistancesDb] PRIMARY KEY ([RefEssai]),
    CONSTRAINT [FK_ListeEssaiResistancesDb_ListeSondeDb_MySondeResistanceRefSonde] FOREIGN KEY ([MySondeResistanceRefSonde]) REFERENCES [ListeSondeDb] ([RefSonde]),
    CONSTRAINT [FK_ListeEssaiResistancesDb_ListeSondeDb_RefSonde] FOREIGN KEY ([RefSonde]) REFERENCES [ListeSondeDb] ([RefSonde])
);
GO

CREATE TABLE [ListeMesureExpansionsDb] (
    [Ind] int NOT NULL IDENTITY,
    [RefEssai] nvarchar(450) NOT NULL,
    [IndiceMesure] int NOT NULL,
    [Pression] real NOT NULL,
    [Volume] real NOT NULL,
    CONSTRAINT [PK_ListeMesureExpansionsDb] PRIMARY KEY ([Ind]),
    CONSTRAINT [FK_ListeMesureExpansionsDb_ListeEssaiExpansionsDb_RefEssai] FOREIGN KEY ([RefEssai]) REFERENCES [ListeEssaiExpansionsDb] ([RefEssai]) ON DELETE CASCADE
);
GO

CREATE TABLE [ListeEssaiForageDb] (
    [RefEssai] nvarchar(450) NOT NULL,
    [RefChantier] nvarchar(max) NULL,
    [RefForage] nvarchar(max) NULL,
    [MySolRefSol] nvarchar(450) NULL,
    [MaResistanceRefEssai] nvarchar(450) NULL,
    [MaExpansionRefEssai] nvarchar(450) NULL,
    [MonEssaiRefEssai] nvarchar(450) NULL,
    [refSonde] nvarchar(450) NULL,
    [refEssaiResistancePropre] nvarchar(max) NULL,
    [refEssaiExpansionPropre] nvarchar(max) NULL,
    [refEssaiPerteDeCharge] nvarchar(max) NULL,
    [RefSol] nvarchar(max) NULL,
    [nomEssai] nvarchar(max) NULL,
    [dateEssai] nvarchar(max) NULL,
    [heureEssai] nvarchar(max) NULL,
    [Enregistreur] nvarchar(max) NULL,
    [Operateur] nvarchar(max) NULL,
    [VolSonde] real NOT NULL,
    [ClassPrecis] int NOT NULL,
    [PrecisVolSonde] real NOT NULL,
    [difPresEauAir] real NOT NULL,
    [hauteurCpv] real NOT NULL,
    [presP0] real NOT NULL,
    [presFluage] real NOT NULL,
    [presLimite] real NOT NULL,
    [presLimiteNF] real NOT NULL,
    [presLimiteTheo] real NOT NULL,
    [PrecisVolume] real NOT NULL,
    [PrecisPression] real NOT NULL,
    [poidsVol] real NOT NULL,
    [poidsVolDejauge] real NOT NULL,
    [coefPoisson] real NOT NULL,
    [modPressio] real NOT NULL,
    [modPressioNf] real NOT NULL,
    [modElastique] real NOT NULL,
    [modElastiqueNf] real NOT NULL,
    [Cohesion] real NOT NULL,
    [Frottement] real NOT NULL,
    [FrottementIntergranulaire] real NOT NULL,
    [U0] real NOT NULL,
    [profNappe] real NOT NULL,
    [profSonde] real NOT NULL,
    [coteZeroGeomeca] real NOT NULL,
    [EventsSuspendus] bit NOT NULL,
    [Pe] real NOT NULL,
    [Ie] int NOT NULL,
    [ChantierRefChantier] nvarchar(450) NULL,
    [ForageRefForage] nvarchar(450) NULL,
    CONSTRAINT [PK_ListeEssaiForageDb] PRIMARY KEY ([RefEssai]),
    CONSTRAINT [FK_ListeEssaiForageDb_Forages_ForageRefForage] FOREIGN KEY ([ForageRefForage]) REFERENCES [Forages] ([RefForage]),
    CONSTRAINT [FK_ListeEssaiForageDb_LesChantiersDb_ChantierRefChantier] FOREIGN KEY ([ChantierRefChantier]) REFERENCES [LesChantiersDb] ([RefChantier]),
    CONSTRAINT [FK_ListeEssaiForageDb_ListeEssaiExpansionsDb_MaExpansionRefEssai] FOREIGN KEY ([MaExpansionRefEssai]) REFERENCES [ListeEssaiExpansionsDb] ([RefEssai]),
    CONSTRAINT [FK_ListeEssaiForageDb_ListeEssaiForageDb_MonEssaiRefEssai] FOREIGN KEY ([MonEssaiRefEssai]) REFERENCES [ListeEssaiForageDb] ([RefEssai]),
    CONSTRAINT [FK_ListeEssaiForageDb_ListeEssaiResistancesDb_MaResistanceRefEssai] FOREIGN KEY ([MaResistanceRefEssai]) REFERENCES [ListeEssaiResistancesDb] ([RefEssai]),
    CONSTRAINT [FK_ListeEssaiForageDb_ListeSolDb_MySolRefSol] FOREIGN KEY ([MySolRefSol]) REFERENCES [ListeSolDb] ([RefSol]),
    CONSTRAINT [FK_ListeEssaiForageDb_ListeSondeDb_refSonde] FOREIGN KEY ([refSonde]) REFERENCES [ListeSondeDb] ([RefSonde])
);
GO

CREATE TABLE [ListeMesureResistanceDb] (
    [Ind] int NOT NULL IDENTITY,
    [RefEssai] nvarchar(450) NOT NULL,
    [IndiceMesure] int NOT NULL,
    [Pression] real NOT NULL,
    [Volume] real NOT NULL,
    [EssaiResistanceRefEssai] nvarchar(450) NULL,
    CONSTRAINT [PK_ListeMesureResistanceDb] PRIMARY KEY ([Ind]),
    CONSTRAINT [FK_ListeMesureResistanceDb_ListeEssaiResistancesDb_EssaiResistanceRefEssai] FOREIGN KEY ([EssaiResistanceRefEssai]) REFERENCES [ListeEssaiResistancesDb] ([RefEssai]),
    CONSTRAINT [FK_ListeMesureResistanceDb_ListeEssaiResistancesDb_RefEssai] FOREIGN KEY ([RefEssai]) REFERENCES [ListeEssaiResistancesDb] ([RefEssai]) ON DELETE CASCADE
);
GO

CREATE TABLE [ListeMesureEssaiForageDb] (
    [Ind] int NOT NULL IDENTITY,
    [RefEssai] nvarchar(450) NOT NULL,
    [IndiceMesure] int NOT NULL,
    [Pression] real NULL,
    [Volume15] real NULL,
    [Volume30] real NULL,
    [Volume60] real NULL,
    [ContrainteRadiale] real NULL,
    [ContrainteVerticale] real NULL,
    [Deformation] real NULL,
    [PNF] real NULL,
    [VNF] real NULL,
    [Qi] real NULL,
    [Pi] real NULL,
    [Qp] real NULL,
    [Pp] real NULL,
    [ChantierRefChantier] nvarchar(450) NULL,
    CONSTRAINT [PK_ListeMesureEssaiForageDb] PRIMARY KEY ([Ind]),
    CONSTRAINT [FK_ListeMesureEssaiForageDb_LesChantiersDb_ChantierRefChantier] FOREIGN KEY ([ChantierRefChantier]) REFERENCES [LesChantiersDb] ([RefChantier]),
    CONSTRAINT [FK_ListeMesureEssaiForageDb_ListeEssaiForageDb_RefEssai] FOREIGN KEY ([RefEssai]) REFERENCES [ListeEssaiForageDb] ([RefEssai]) ON DELETE CASCADE
);
GO

CREATE INDEX [IX_Forages_ChantierRefChantier] ON [Forages] ([ChantierRefChantier]);
GO

CREATE INDEX [IX_ListeEssaiExpansionsDb_RefSonde] ON [ListeEssaiExpansionsDb] ([RefSonde]);
GO

CREATE INDEX [IX_ListeEssaiForageDb_ChantierRefChantier] ON [ListeEssaiForageDb] ([ChantierRefChantier]);
GO

CREATE INDEX [IX_ListeEssaiForageDb_ForageRefForage] ON [ListeEssaiForageDb] ([ForageRefForage]);
GO

CREATE INDEX [IX_ListeEssaiForageDb_MaExpansionRefEssai] ON [ListeEssaiForageDb] ([MaExpansionRefEssai]);
GO

CREATE INDEX [IX_ListeEssaiForageDb_MaResistanceRefEssai] ON [ListeEssaiForageDb] ([MaResistanceRefEssai]);
GO

CREATE INDEX [IX_ListeEssaiForageDb_MonEssaiRefEssai] ON [ListeEssaiForageDb] ([MonEssaiRefEssai]);
GO

CREATE INDEX [IX_ListeEssaiForageDb_MySolRefSol] ON [ListeEssaiForageDb] ([MySolRefSol]);
GO

CREATE INDEX [IX_ListeEssaiForageDb_refSonde] ON [ListeEssaiForageDb] ([refSonde]);
GO

CREATE INDEX [IX_ListeEssaiResistancesDb_MySondeResistanceRefSonde] ON [ListeEssaiResistancesDb] ([MySondeResistanceRefSonde]);
GO

CREATE INDEX [IX_ListeEssaiResistancesDb_RefSonde] ON [ListeEssaiResistancesDb] ([RefSonde]);
GO

CREATE INDEX [IX_ListeMesureEssaiForageDb_ChantierRefChantier] ON [ListeMesureEssaiForageDb] ([ChantierRefChantier]);
GO

CREATE INDEX [IX_ListeMesureEssaiForageDb_RefEssai] ON [ListeMesureEssaiForageDb] ([RefEssai]);
GO

CREATE INDEX [IX_ListeMesureExpansionsDb_RefEssai] ON [ListeMesureExpansionsDb] ([RefEssai]);
GO

CREATE INDEX [IX_ListeMesureResistanceDb_EssaiResistanceRefEssai] ON [ListeMesureResistanceDb] ([EssaiResistanceRefEssai]);
GO

CREATE INDEX [IX_ListeMesureResistanceDb_RefEssai] ON [ListeMesureResistanceDb] ([RefEssai]);
GO

CREATE INDEX [IX_ListeSolDb_ChantierRefChantier] ON [ListeSolDb] ([ChantierRefChantier]);
GO

CREATE INDEX [IX_ListeSondeDb_ChantierRefChantier] ON [ListeSondeDb] ([ChantierRefChantier]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20261105035128_InitialCreate', N'8.0.11');
GO

COMMIT;
GO

