-- Script Date: 7/15/2017 9:18 PM  - ErikEJ.SqlCeScripting version 3.5.2.69
CREATE TABLE [Users] (
  [Id] INTEGER NOT NULL
, [Name] nvarchar(4000) NULL
, [Description] nvarchar(4000) NULL
, [CreateOn] datetime NOT NULL
, [UpdateOn] datetime NOT NULL
, CONSTRAINT [sqlite_master_PK_Users] PRIMARY KEY ([Id])
);
CREATE UNIQUE INDEX [sqlite_autoindex_Users_1_Users] ON [Users] ([Id] ASC);