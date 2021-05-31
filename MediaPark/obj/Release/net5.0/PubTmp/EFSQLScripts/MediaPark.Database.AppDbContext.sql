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

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210529213927_CreatingCountryRegionDateAndHolidayTypeEntitites')
BEGIN
    CREATE TABLE [Countries] (
        [CountryCode] nvarchar(450) NOT NULL,
        [FullName] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_Countries] PRIMARY KEY ([CountryCode])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210529213927_CreatingCountryRegionDateAndHolidayTypeEntitites')
BEGIN
    CREATE TABLE [HolidayTypes] (
        [Id] int NOT NULL IDENTITY,
        [Name] nvarchar(max) NULL,
        CONSTRAINT [PK_HolidayTypes] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210529213927_CreatingCountryRegionDateAndHolidayTypeEntitites')
BEGIN
    CREATE TABLE [FromDates] (
        [Id] int NOT NULL IDENTITY,
        [Day] int NOT NULL,
        [Month] int NOT NULL,
        [Year] int NOT NULL,
        [CountryCode] nvarchar(450) NULL,
        CONSTRAINT [PK_FromDates] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_FromDates_Countries_CountryCode] FOREIGN KEY ([CountryCode]) REFERENCES [Countries] ([CountryCode]) ON DELETE NO ACTION
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210529213927_CreatingCountryRegionDateAndHolidayTypeEntitites')
BEGIN
    CREATE TABLE [Regions] (
        [Id] int NOT NULL IDENTITY,
        [Name] nvarchar(max) NULL,
        [CountryCode] nvarchar(450) NULL,
        CONSTRAINT [PK_Regions] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Regions_Countries_CountryCode] FOREIGN KEY ([CountryCode]) REFERENCES [Countries] ([CountryCode]) ON DELETE NO ACTION
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210529213927_CreatingCountryRegionDateAndHolidayTypeEntitites')
BEGIN
    CREATE TABLE [ToDates] (
        [Id] int NOT NULL IDENTITY,
        [Day] int NOT NULL,
        [Month] int NOT NULL,
        [Year] int NOT NULL,
        [CountryCode] nvarchar(450) NULL,
        CONSTRAINT [PK_ToDates] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_ToDates_Countries_CountryCode] FOREIGN KEY ([CountryCode]) REFERENCES [Countries] ([CountryCode]) ON DELETE NO ACTION
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210529213927_CreatingCountryRegionDateAndHolidayTypeEntitites')
BEGIN
    CREATE TABLE [Country_HolidayTypes] (
        [Id] int NOT NULL IDENTITY,
        [CountryCode] nvarchar(450) NULL,
        [HolidayTypeId] int NOT NULL,
        CONSTRAINT [PK_Country_HolidayTypes] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Country_HolidayTypes_Countries_CountryCode] FOREIGN KEY ([CountryCode]) REFERENCES [Countries] ([CountryCode]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Country_HolidayTypes_HolidayTypes_HolidayTypeId] FOREIGN KEY ([HolidayTypeId]) REFERENCES [HolidayTypes] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210529213927_CreatingCountryRegionDateAndHolidayTypeEntitites')
BEGIN
    CREATE INDEX [IX_Country_HolidayTypes_CountryCode] ON [Country_HolidayTypes] ([CountryCode]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210529213927_CreatingCountryRegionDateAndHolidayTypeEntitites')
BEGIN
    CREATE INDEX [IX_Country_HolidayTypes_HolidayTypeId] ON [Country_HolidayTypes] ([HolidayTypeId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210529213927_CreatingCountryRegionDateAndHolidayTypeEntitites')
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [IX_FromDates_CountryCode] ON [FromDates] ([CountryCode]) WHERE [CountryCode] IS NOT NULL');
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210529213927_CreatingCountryRegionDateAndHolidayTypeEntitites')
BEGIN
    CREATE INDEX [IX_Regions_CountryCode] ON [Regions] ([CountryCode]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210529213927_CreatingCountryRegionDateAndHolidayTypeEntitites')
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [IX_ToDates_CountryCode] ON [ToDates] ([CountryCode]) WHERE [CountryCode] IS NOT NULL');
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210529213927_CreatingCountryRegionDateAndHolidayTypeEntitites')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20210529213927_CreatingCountryRegionDateAndHolidayTypeEntitites', N'5.0.6');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210529214109_CreatingHolidayEntities')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20210529214109_CreatingHolidayEntities', N'5.0.6');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210529214232_CreatingDayEntity')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20210529214232_CreatingDayEntity', N'5.0.6');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210529214341_CreatingFullYearOfHolidaysEntity')
BEGIN
    CREATE TABLE [Days] (
        [Id] int NOT NULL IDENTITY,
        [DayOfTheMonth] int NOT NULL,
        [Month] int NOT NULL,
        [Year] int NOT NULL,
        [DayStatus] nvarchar(max) NULL,
        CONSTRAINT [PK_Days] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210529214341_CreatingFullYearOfHolidaysEntity')
BEGIN
    CREATE TABLE [FullYearOfHolidays] (
        [Id] int NOT NULL IDENTITY,
        [Year] int NOT NULL,
        [CountryCode] nvarchar(450) NULL,
        CONSTRAINT [PK_FullYearOfHolidays] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_FullYearOfHolidays_Countries_CountryCode] FOREIGN KEY ([CountryCode]) REFERENCES [Countries] ([CountryCode]) ON DELETE NO ACTION
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210529214341_CreatingFullYearOfHolidaysEntity')
BEGIN
    CREATE TABLE [Holidays] (
        [Id] int NOT NULL IDENTITY,
        [HolidayTypeId] int NOT NULL,
        [CountryCode] nvarchar(450) NULL,
        CONSTRAINT [PK_Holidays] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Holidays_Countries_CountryCode] FOREIGN KEY ([CountryCode]) REFERENCES [Countries] ([CountryCode]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Holidays_HolidayTypes_HolidayTypeId] FOREIGN KEY ([HolidayTypeId]) REFERENCES [HolidayTypes] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210529214341_CreatingFullYearOfHolidaysEntity')
BEGIN
    CREATE TABLE [HolidayDates] (
        [Id] int NOT NULL IDENTITY,
        [Day] int NOT NULL,
        [Month] int NOT NULL,
        [Year] int NOT NULL,
        [DayOfWeek] int NOT NULL,
        [HolidayId] int NOT NULL,
        CONSTRAINT [PK_HolidayDates] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_HolidayDates_Holidays_HolidayId] FOREIGN KEY ([HolidayId]) REFERENCES [Holidays] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210529214341_CreatingFullYearOfHolidaysEntity')
BEGIN
    CREATE TABLE [HolidayNames] (
        [Id] int NOT NULL IDENTITY,
        [Lang] nvarchar(max) NULL,
        [Text] nvarchar(max) NULL,
        [HolidayId] int NOT NULL,
        CONSTRAINT [PK_HolidayNames] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_HolidayNames_Holidays_HolidayId] FOREIGN KEY ([HolidayId]) REFERENCES [Holidays] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210529214341_CreatingFullYearOfHolidaysEntity')
BEGIN
    CREATE INDEX [IX_FullYearOfHolidays_CountryCode] ON [FullYearOfHolidays] ([CountryCode]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210529214341_CreatingFullYearOfHolidaysEntity')
BEGIN
    CREATE UNIQUE INDEX [IX_HolidayDates_HolidayId] ON [HolidayDates] ([HolidayId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210529214341_CreatingFullYearOfHolidaysEntity')
BEGIN
    CREATE INDEX [IX_HolidayNames_HolidayId] ON [HolidayNames] ([HolidayId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210529214341_CreatingFullYearOfHolidaysEntity')
BEGIN
    CREATE INDEX [IX_Holidays_CountryCode] ON [Holidays] ([CountryCode]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210529214341_CreatingFullYearOfHolidaysEntity')
BEGIN
    CREATE INDEX [IX_Holidays_HolidayTypeId] ON [Holidays] ([HolidayTypeId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210529214341_CreatingFullYearOfHolidaysEntity')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20210529214341_CreatingFullYearOfHolidaysEntity', N'5.0.6');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210530091536_addingDateAndDayOfWeekToHoliday')
BEGIN
    DROP TABLE [HolidayDates];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210530091536_addingDateAndDayOfWeekToHoliday')
BEGIN
    ALTER TABLE [Holidays] ADD [Date] nvarchar(max) NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210530091536_addingDateAndDayOfWeekToHoliday')
BEGIN
    ALTER TABLE [Holidays] ADD [DayOfTheWeek] int NOT NULL DEFAULT 0;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210530091536_addingDateAndDayOfWeekToHoliday')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20210530091536_addingDateAndDayOfWeekToHoliday', N'5.0.6');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210530153105_CreatingRelationshipBetweenDayAndHoliday')
BEGIN
    ALTER TABLE [Holidays] ADD [DayId] int NOT NULL DEFAULT 0;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210530153105_CreatingRelationshipBetweenDayAndHoliday')
BEGIN
    ALTER TABLE [Days] ADD [HolidayId] int NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210530153105_CreatingRelationshipBetweenDayAndHoliday')
BEGIN
    CREATE UNIQUE INDEX [IX_Holidays_DayId] ON [Holidays] ([DayId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210530153105_CreatingRelationshipBetweenDayAndHoliday')
BEGIN
    ALTER TABLE [Holidays] ADD CONSTRAINT [FK_Holidays_Days_DayId] FOREIGN KEY ([DayId]) REFERENCES [Days] ([Id]) ON DELETE CASCADE;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210530153105_CreatingRelationshipBetweenDayAndHoliday')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20210530153105_CreatingRelationshipBetweenDayAndHoliday', N'5.0.6');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210530160657_configuringDay')
BEGIN
    DECLARE @var0 sysname;
    SELECT @var0 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Days]') AND [c].[name] = N'HolidayId');
    IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [Days] DROP CONSTRAINT [' + @var0 + '];');
    ALTER TABLE [Days] DROP COLUMN [HolidayId];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210530160657_configuringDay')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20210530160657_configuringDay', N'5.0.6');
END;
GO

COMMIT;
GO

