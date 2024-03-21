CREATE TABLE [Genres] (
    [GenreType] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_Genres] PRIMARY KEY ([GenreType])
);
GO


CREATE TABLE [Persons] (
    [Nconst] nvarchar(450) NOT NULL,
    [PrimaryName] nvarchar(max) NOT NULL,
    [BirthYear] date NULL,
    [DeathYear] date NULL,
    CONSTRAINT [PK_Persons] PRIMARY KEY ([Nconst])
);
GO


CREATE TABLE [Professions] (
    [PrimaryProfession] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_Professions] PRIMARY KEY ([PrimaryProfession])
);
GO


CREATE TABLE [TitleTypes] (
    [Type] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_TitleTypes] PRIMARY KEY ([Type])
);
GO


CREATE TABLE [PersonalCareers] (
    [Nconst] nvarchar(450) NOT NULL,
    [PrimProf] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_PersonalCareers] PRIMARY KEY ([Nconst], [PrimProf]),
    CONSTRAINT [FK_PersonalCareers_Persons_Nconst] FOREIGN KEY ([Nconst]) REFERENCES [Persons] ([Nconst]) ON DELETE CASCADE,
    CONSTRAINT [FK_PersonalCareers_Professions_PrimProf] FOREIGN KEY ([PrimProf]) REFERENCES [Professions] ([PrimaryProfession]) ON DELETE CASCADE
);
GO


CREATE TABLE [MovieBases] (
    [Tconst] nvarchar(450) NOT NULL,
    [TitleTypeType] nvarchar(450) NOT NULL,
    [PrimaryTitle] nvarchar(max) NOT NULL,
    [OriginalTitle] nvarchar(max) NULL,
    [IsAdult] bit NOT NULL,
    [StartYear] date NULL,
    [EndYear] date NULL,
    [RuntimeMins] int NULL,
    CONSTRAINT [PK_MovieBases] PRIMARY KEY ([Tconst]),
    CONSTRAINT [FK_MovieBases_TitleTypes_TitleTypeType] FOREIGN KEY ([TitleTypeType]) REFERENCES [TitleTypes] ([Type]) ON DELETE CASCADE
);
GO


CREATE TABLE [Directors] (
    [Tconst] nvarchar(450) NOT NULL,
    [Nconst] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_Directors] PRIMARY KEY ([Tconst], [Nconst]),
    CONSTRAINT [FK_Directors_MovieBases_Tconst] FOREIGN KEY ([Tconst]) REFERENCES [MovieBases] ([Tconst]) ON DELETE CASCADE,
    CONSTRAINT [FK_Directors_Persons_Nconst] FOREIGN KEY ([Nconst]) REFERENCES [Persons] ([Nconst]) ON DELETE CASCADE
);
GO


CREATE TABLE [MovieGenres] (
    [Tconst] nvarchar(450) NOT NULL,
    [GenreType] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_MovieGenres] PRIMARY KEY ([Tconst], [GenreType]),
    CONSTRAINT [FK_MovieGenres_Genres_GenreType] FOREIGN KEY ([GenreType]) REFERENCES [Genres] ([GenreType]) ON DELETE CASCADE,
    CONSTRAINT [FK_MovieGenres_MovieBases_Tconst] FOREIGN KEY ([Tconst]) REFERENCES [MovieBases] ([Tconst]) ON DELETE CASCADE
);
GO


CREATE TABLE [PersonalBlockbusters] (
    [Nconst] nvarchar(450) NOT NULL,
    [Tconst] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_PersonalBlockbusters] PRIMARY KEY ([Nconst], [Tconst]),
    CONSTRAINT [FK_PersonalBlockbusters_MovieBases_Tconst] FOREIGN KEY ([Tconst]) REFERENCES [MovieBases] ([Tconst]) ON DELETE CASCADE,
    CONSTRAINT [FK_PersonalBlockbusters_Persons_Nconst] FOREIGN KEY ([Nconst]) REFERENCES [Persons] ([Nconst]) ON DELETE CASCADE
);
GO


CREATE TABLE [Writers] (
    [Tconst] nvarchar(450) NOT NULL,
    [Nconst] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_Writers] PRIMARY KEY ([Tconst], [Nconst]),
    CONSTRAINT [FK_Writers_MovieBases_Tconst] FOREIGN KEY ([Tconst]) REFERENCES [MovieBases] ([Tconst]) ON DELETE CASCADE,
    CONSTRAINT [FK_Writers_Persons_Nconst] FOREIGN KEY ([Nconst]) REFERENCES [Persons] ([Nconst]) ON DELETE CASCADE
);
GO


CREATE INDEX [IX_Directors_Nconst] ON [Directors] ([Nconst]);
GO


CREATE INDEX [IX_MovieBases_TitleTypeType] ON [MovieBases] ([TitleTypeType]);
GO


CREATE INDEX [IX_MovieGenres_GenreType] ON [MovieGenres] ([GenreType]);
GO


CREATE INDEX [IX_PersonalBlockbusters_Tconst] ON [PersonalBlockbusters] ([Tconst]);
GO


CREATE INDEX [IX_PersonalCareers_PrimProf] ON [PersonalCareers] ([PrimProf]);
GO


CREATE INDEX [IX_Writers_Nconst] ON [Writers] ([Nconst]);
GO