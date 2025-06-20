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

CREATE TABLE [Trigger] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Trigger] PRIMARY KEY ([Id])
);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250602164557_AddTriggerTable', N'8.0.16');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [Trigger] DROP CONSTRAINT [PK_Trigger];
GO

EXEC sp_rename N'[Trigger]', N'Triggers';
GO

ALTER TABLE [Triggers] ADD CONSTRAINT [PK_Triggers] PRIMARY KEY ([Id]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250602172130_TriggerTableRename', N'8.0.16');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [Treatments] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Treatments] PRIMARY KEY ([Id])
);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250602185809_AddTreatmentTable', N'8.0.16');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

DROP TABLE [Triggers];
GO

CREATE TABLE [TriggerCategories] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_TriggerCategories] PRIMARY KEY ([Id])
);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250603082836_TriggerTabToTriggerCategoryTab', N'8.0.16');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [Medications] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Medications] PRIMARY KEY ([Id])
);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250603084102_AddMedicationTab', N'8.0.16');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

DROP TABLE [Medications];
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250604192438_RemoveMedicationTabl', N'8.0.16');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

DROP TABLE [TriggerCategories];
GO

CREATE TABLE [Triggers] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Triggers] PRIMARY KEY ([Id])
);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250604193732_TriggerCategoryToTriggerTab', N'8.0.16');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [Triggers] ADD [HeadacheEntryId] int NULL;
GO

ALTER TABLE [Treatments] ADD [HeadacheEntryId] int NULL;
GO

CREATE TABLE [HeadacheEntries] (
    [Id] int NOT NULL IDENTITY,
    [Date] datetime2 NOT NULL,
    [Duration] int NOT NULL,
    [Intensity] int NOT NULL,
    [Note] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_HeadacheEntries] PRIMARY KEY ([Id])
);
GO

CREATE INDEX [IX_Triggers_HeadacheEntryId] ON [Triggers] ([HeadacheEntryId]);
GO

CREATE INDEX [IX_Treatments_HeadacheEntryId] ON [Treatments] ([HeadacheEntryId]);
GO

ALTER TABLE [Treatments] ADD CONSTRAINT [FK_Treatments_HeadacheEntries_HeadacheEntryId] FOREIGN KEY ([HeadacheEntryId]) REFERENCES [HeadacheEntries] ([Id]);
GO

ALTER TABLE [Triggers] ADD CONSTRAINT [FK_Triggers_HeadacheEntries_HeadacheEntryId] FOREIGN KEY ([HeadacheEntryId]) REFERENCES [HeadacheEntries] ([Id]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250604194933_AddHeadacheEntryTab', N'8.0.16');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250604195633_MissingTriggerTreatmentInHeadacheEnntryTab', N'8.0.16');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [Treatments] DROP CONSTRAINT [FK_Treatments_HeadacheEntries_HeadacheEntryId];
GO

ALTER TABLE [Triggers] DROP CONSTRAINT [FK_Triggers_HeadacheEntries_HeadacheEntryId];
GO

DROP INDEX [IX_Triggers_HeadacheEntryId] ON [Triggers];
GO

DROP INDEX [IX_Treatments_HeadacheEntryId] ON [Treatments];
GO

DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Triggers]') AND [c].[name] = N'HeadacheEntryId');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [Triggers] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [Triggers] DROP COLUMN [HeadacheEntryId];
GO

DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Treatments]') AND [c].[name] = N'HeadacheEntryId');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [Treatments] DROP CONSTRAINT [' + @var1 + '];');
ALTER TABLE [Treatments] DROP COLUMN [HeadacheEntryId];
GO

CREATE TABLE [HeadacheEntryTreatment] (
    [HeadacheEntriesId] int NOT NULL,
    [TreatmentsId] int NOT NULL,
    CONSTRAINT [PK_HeadacheEntryTreatment] PRIMARY KEY ([HeadacheEntriesId], [TreatmentsId]),
    CONSTRAINT [FK_HeadacheEntryTreatment_HeadacheEntries_HeadacheEntriesId] FOREIGN KEY ([HeadacheEntriesId]) REFERENCES [HeadacheEntries] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_HeadacheEntryTreatment_Treatments_TreatmentsId] FOREIGN KEY ([TreatmentsId]) REFERENCES [Treatments] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [HeadacheEntryTrigger] (
    [HeadacheEntriesId] int NOT NULL,
    [TriggersId] int NOT NULL,
    CONSTRAINT [PK_HeadacheEntryTrigger] PRIMARY KEY ([HeadacheEntriesId], [TriggersId]),
    CONSTRAINT [FK_HeadacheEntryTrigger_HeadacheEntries_HeadacheEntriesId] FOREIGN KEY ([HeadacheEntriesId]) REFERENCES [HeadacheEntries] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_HeadacheEntryTrigger_Triggers_TriggersId] FOREIGN KEY ([TriggersId]) REFERENCES [Triggers] ([Id]) ON DELETE CASCADE
);
GO

CREATE INDEX [IX_HeadacheEntryTreatment_TreatmentsId] ON [HeadacheEntryTreatment] ([TreatmentsId]);
GO

CREATE INDEX [IX_HeadacheEntryTrigger_TriggersId] ON [HeadacheEntryTrigger] ([TriggersId]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250604200217_TriggerAndTreatmentTabAddColumn', N'8.0.16');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

DROP TABLE [HeadacheEntryTreatment];
GO

DROP TABLE [HeadacheEntryTrigger];
GO

ALTER TABLE [Triggers] ADD [HeadacheEntryId] int NULL;
GO

ALTER TABLE [Treatments] ADD [HeadacheEntryId] int NULL;
GO

CREATE TABLE [Medications] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Medications] PRIMARY KEY ([Id])
);
GO

CREATE INDEX [IX_Triggers_HeadacheEntryId] ON [Triggers] ([HeadacheEntryId]);
GO

CREATE INDEX [IX_Treatments_HeadacheEntryId] ON [Treatments] ([HeadacheEntryId]);
GO

ALTER TABLE [Treatments] ADD CONSTRAINT [FK_Treatments_HeadacheEntries_HeadacheEntryId] FOREIGN KEY ([HeadacheEntryId]) REFERENCES [HeadacheEntries] ([Id]);
GO

ALTER TABLE [Triggers] ADD CONSTRAINT [FK_Triggers_HeadacheEntries_HeadacheEntryId] FOREIGN KEY ([HeadacheEntryId]) REFERENCES [HeadacheEntries] ([Id]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250605143354_NewTab', N'8.0.16');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [HeadacheRecords] (
    [Id] int NOT NULL IDENTITY,
    [Date] datetime2 NOT NULL,
    [Duration] int NOT NULL,
    [Intensity] int NOT NULL,
    [MedicationId] int NOT NULL,
    [TreatmentId] int NOT NULL,
    [TriggerId] int NOT NULL,
    [Notes] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_HeadacheRecords] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_HeadacheRecords_Medications_MedicationId] FOREIGN KEY ([MedicationId]) REFERENCES [Medications] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_HeadacheRecords_Treatments_TreatmentId] FOREIGN KEY ([TreatmentId]) REFERENCES [Treatments] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_HeadacheRecords_Triggers_TriggerId] FOREIGN KEY ([TriggerId]) REFERENCES [Triggers] ([Id]) ON DELETE CASCADE
);
GO

CREATE INDEX [IX_HeadacheRecords_MedicationId] ON [HeadacheRecords] ([MedicationId]);
GO

CREATE INDEX [IX_HeadacheRecords_TreatmentId] ON [HeadacheRecords] ([TreatmentId]);
GO

CREATE INDEX [IX_HeadacheRecords_TriggerId] ON [HeadacheRecords] ([TriggerId]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250605144910_AddHeadacheRecordTab', N'8.0.16');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [Treatments] DROP CONSTRAINT [FK_Treatments_HeadacheEntries_HeadacheEntryId];
GO

ALTER TABLE [Triggers] DROP CONSTRAINT [FK_Triggers_HeadacheEntries_HeadacheEntryId];
GO

DROP TABLE [HeadacheEntries];
GO

DROP INDEX [IX_Triggers_HeadacheEntryId] ON [Triggers];
GO

DROP INDEX [IX_Treatments_HeadacheEntryId] ON [Treatments];
GO

DECLARE @var2 sysname;
SELECT @var2 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Triggers]') AND [c].[name] = N'HeadacheEntryId');
IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [Triggers] DROP CONSTRAINT [' + @var2 + '];');
ALTER TABLE [Triggers] DROP COLUMN [HeadacheEntryId];
GO

DECLARE @var3 sysname;
SELECT @var3 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Treatments]') AND [c].[name] = N'HeadacheEntryId');
IF @var3 IS NOT NULL EXEC(N'ALTER TABLE [Treatments] DROP CONSTRAINT [' + @var3 + '];');
ALTER TABLE [Treatments] DROP COLUMN [HeadacheEntryId];
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250605153711_DropHeadacheEntryTab', N'8.0.16');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [AspNetRoles] (
    [Id] nvarchar(450) NOT NULL,
    [Name] nvarchar(256) NULL,
    [NormalizedName] nvarchar(256) NULL,
    [ConcurrencyStamp] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetRoles] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [AspNetUsers] (
    [Id] nvarchar(450) NOT NULL,
    [UserName] nvarchar(256) NULL,
    [NormalizedUserName] nvarchar(256) NULL,
    [Email] nvarchar(256) NULL,
    [NormalizedEmail] nvarchar(256) NULL,
    [EmailConfirmed] bit NOT NULL,
    [PasswordHash] nvarchar(max) NULL,
    [SecurityStamp] nvarchar(max) NULL,
    [ConcurrencyStamp] nvarchar(max) NULL,
    [PhoneNumber] nvarchar(max) NULL,
    [PhoneNumberConfirmed] bit NOT NULL,
    [TwoFactorEnabled] bit NOT NULL,
    [LockoutEnd] datetimeoffset NULL,
    [LockoutEnabled] bit NOT NULL,
    [AccessFailedCount] int NOT NULL,
    CONSTRAINT [PK_AspNetUsers] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [AspNetRoleClaims] (
    [Id] int NOT NULL IDENTITY,
    [RoleId] nvarchar(450) NOT NULL,
    [ClaimType] nvarchar(max) NULL,
    [ClaimValue] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [AspNetUserClaims] (
    [Id] int NOT NULL IDENTITY,
    [UserId] nvarchar(450) NOT NULL,
    [ClaimType] nvarchar(max) NULL,
    [ClaimValue] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [AspNetUserLogins] (
    [LoginProvider] nvarchar(450) NOT NULL,
    [ProviderKey] nvarchar(450) NOT NULL,
    [ProviderDisplayName] nvarchar(max) NULL,
    [UserId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY ([LoginProvider], [ProviderKey]),
    CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [AspNetUserRoles] (
    [UserId] nvarchar(450) NOT NULL,
    [RoleId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY ([UserId], [RoleId]),
    CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [AspNetUserTokens] (
    [UserId] nvarchar(450) NOT NULL,
    [LoginProvider] nvarchar(450) NOT NULL,
    [Name] nvarchar(450) NOT NULL,
    [Value] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY ([UserId], [LoginProvider], [Name]),
    CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO

CREATE INDEX [IX_AspNetRoleClaims_RoleId] ON [AspNetRoleClaims] ([RoleId]);
GO

CREATE UNIQUE INDEX [RoleNameIndex] ON [AspNetRoles] ([NormalizedName]) WHERE [NormalizedName] IS NOT NULL;
GO

CREATE INDEX [IX_AspNetUserClaims_UserId] ON [AspNetUserClaims] ([UserId]);
GO

CREATE INDEX [IX_AspNetUserLogins_UserId] ON [AspNetUserLogins] ([UserId]);
GO

CREATE INDEX [IX_AspNetUserRoles_RoleId] ON [AspNetUserRoles] ([RoleId]);
GO

CREATE INDEX [EmailIndex] ON [AspNetUsers] ([NormalizedEmail]);
GO

CREATE UNIQUE INDEX [UserNameIndex] ON [AspNetUsers] ([NormalizedUserName]) WHERE [NormalizedUserName] IS NOT NULL;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250605174832_AddAddUserTab', N'8.0.16');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [Triggers] ADD [UserId] nvarchar(450) NULL;
GO

ALTER TABLE [Treatments] ADD [UserId] nvarchar(450) NULL;
GO

ALTER TABLE [Medications] ADD [UserId] nvarchar(450) NULL;
GO

CREATE INDEX [IX_Triggers_UserId] ON [Triggers] ([UserId]);
GO

CREATE INDEX [IX_Treatments_UserId] ON [Treatments] ([UserId]);
GO

CREATE INDEX [IX_Medications_UserId] ON [Medications] ([UserId]);
GO

ALTER TABLE [Medications] ADD CONSTRAINT [FK_Medications_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]);
GO

ALTER TABLE [Treatments] ADD CONSTRAINT [FK_Treatments_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]);
GO

ALTER TABLE [Triggers] ADD CONSTRAINT [FK_Triggers_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250608091449_AddUserIdToMedicationTreatmentTrigger', N'8.0.16');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

DECLARE @var4 sysname;
SELECT @var4 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[HeadacheRecords]') AND [c].[name] = N'Notes');
IF @var4 IS NOT NULL EXEC(N'ALTER TABLE [HeadacheRecords] DROP CONSTRAINT [' + @var4 + '];');
ALTER TABLE [HeadacheRecords] ALTER COLUMN [Notes] nvarchar(max) NULL;
GO

ALTER TABLE [HeadacheRecords] ADD [UserId] nvarchar(max) NOT NULL DEFAULT N'';
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250608184158_AddUserIdAndFKsToHeadacheRecord', N'8.0.16');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [HeadacheRecords] DROP CONSTRAINT [FK_HeadacheRecords_Medications_MedicationId];
GO

ALTER TABLE [HeadacheRecords] DROP CONSTRAINT [FK_HeadacheRecords_Treatments_TreatmentId];
GO

ALTER TABLE [HeadacheRecords] DROP CONSTRAINT [FK_HeadacheRecords_Triggers_TriggerId];
GO

DECLARE @var5 sysname;
SELECT @var5 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[HeadacheRecords]') AND [c].[name] = N'TriggerId');
IF @var5 IS NOT NULL EXEC(N'ALTER TABLE [HeadacheRecords] DROP CONSTRAINT [' + @var5 + '];');
ALTER TABLE [HeadacheRecords] ALTER COLUMN [TriggerId] int NULL;
GO

DECLARE @var6 sysname;
SELECT @var6 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[HeadacheRecords]') AND [c].[name] = N'TreatmentId');
IF @var6 IS NOT NULL EXEC(N'ALTER TABLE [HeadacheRecords] DROP CONSTRAINT [' + @var6 + '];');
ALTER TABLE [HeadacheRecords] ALTER COLUMN [TreatmentId] int NULL;
GO

DECLARE @var7 sysname;
SELECT @var7 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[HeadacheRecords]') AND [c].[name] = N'MedicationId');
IF @var7 IS NOT NULL EXEC(N'ALTER TABLE [HeadacheRecords] DROP CONSTRAINT [' + @var7 + '];');
ALTER TABLE [HeadacheRecords] ALTER COLUMN [MedicationId] int NULL;
GO

ALTER TABLE [HeadacheRecords] ADD CONSTRAINT [FK_HeadacheRecords_Medications_MedicationId] FOREIGN KEY ([MedicationId]) REFERENCES [Medications] ([Id]);
GO

ALTER TABLE [HeadacheRecords] ADD CONSTRAINT [FK_HeadacheRecords_Treatments_TreatmentId] FOREIGN KEY ([TreatmentId]) REFERENCES [Treatments] ([Id]);
GO

ALTER TABLE [HeadacheRecords] ADD CONSTRAINT [FK_HeadacheRecords_Triggers_TriggerId] FOREIGN KEY ([TriggerId]) REFERENCES [Triggers] ([Id]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250609202634_HeadacheRecordTabNullableMedicationTreatmentTrigger', N'8.0.16');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [Medications] DROP CONSTRAINT [FK_Medications_AspNetUsers_UserId];
GO

ALTER TABLE [Treatments] DROP CONSTRAINT [FK_Treatments_AspNetUsers_UserId];
GO

ALTER TABLE [Triggers] DROP CONSTRAINT [FK_Triggers_AspNetUsers_UserId];
GO

ALTER TABLE [Medications] ADD CONSTRAINT [FK_Medications_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE;
GO

ALTER TABLE [Treatments] ADD CONSTRAINT [FK_Treatments_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE;
GO

ALTER TABLE [Triggers] ADD CONSTRAINT [FK_Triggers_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250615193750_AddCascadeDeleteToUserRelations', N'8.0.16');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [HeadacheRecords] DROP CONSTRAINT [FK_HeadacheRecords_Medications_MedicationId];
GO

ALTER TABLE [HeadacheRecords] ADD CONSTRAINT [FK_HeadacheRecords_Medications_MedicationId] FOREIGN KEY ([MedicationId]) REFERENCES [Medications] ([Id]) ON DELETE SET NULL;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250615194844_SetNullOnDeleteForHeadacheRecordRelations2', N'8.0.16');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [Triggers] ADD [IsDeleted] bit NOT NULL DEFAULT CAST(0 AS bit);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250616072543_AddIsDeletedToTrigger', N'8.0.16');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

EXEC sp_rename N'[Triggers].[IsDeleted]', N'IsActive', N'COLUMN';
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250616080040_AddIsActiveOnTrigger', N'8.0.16');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [Triggers] ADD [IsDeleted] bit NOT NULL DEFAULT CAST(0 AS bit);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250616091344_AddIsDeletedOnTrigger', N'8.0.16');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [Treatments] ADD [IsDeleted] bit NOT NULL DEFAULT CAST(0 AS bit);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250616100410_AddIsDeletedToTreatment', N'8.0.16');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

DECLARE @var8 sysname;
SELECT @var8 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Triggers]') AND [c].[name] = N'IsActive');
IF @var8 IS NOT NULL EXEC(N'ALTER TABLE [Triggers] DROP CONSTRAINT [' + @var8 + '];');
ALTER TABLE [Triggers] DROP COLUMN [IsActive];
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250616110032_RemoveIsActiveForTrigger', N'8.0.16');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [Medications] ADD [IsDeleted] bit NOT NULL DEFAULT CAST(0 AS bit);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250616121521_AddIsDeletedToMedication', N'8.0.16');
GO

COMMIT;
GO

