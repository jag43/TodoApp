CREATE TABLE TodoItem(
    Id INT NOT NULL IDENTITY (1, 1),
    UserId NVARCHAR (1024) NOT NULL,
    Created_UnixTicks BIGINT NOT NULL,
    Created_ZoneId NVARCHAR(256) NOT NULL,
    Title NVARCHAR(4000) NOT NULL,
    Done BIT NOT NULL DEFAULT 0,
    Due_UnixTicks BIGINT NULL,
    Due_ZoneId NVARCHAR(256) NULL,
    Notes NVARCHAR(MAX) NULL,
    CONSTRAINT [PK_TodoItem] PRIMARY KEY CLUSTERED (Id ASC),
    INDEX [IX_TodoItem_UserId]
    (
        [UserId] ASC
    ),
    INDEX [IX_TodoItem_UserId_Title]
    (
        [UserId] ASC,
        [Title] ASC
    ),
    INDEX [IX_TodoItem_UserId_Created_UnixTicks]
    (
        [UserId] ASC,
        [Created_UnixTicks] ASC
    ),
    INDEX [IX_TodoItem_UserId_Due_UnixTicks]
    (
        [UserId] ASC,
        [Due_UnixTicks] ASC
    )
)