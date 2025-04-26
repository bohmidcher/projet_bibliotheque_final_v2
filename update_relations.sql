-- Drop existing foreign key constraints
IF EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Loans_Books_BookId')
    ALTER TABLE [Loans] DROP CONSTRAINT [FK_Loans_Books_BookId];
IF EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Loans_Members_MemberId')
    ALTER TABLE [Loans] DROP CONSTRAINT [FK_Loans_Members_MemberId];

-- Add new foreign key constraints
ALTER TABLE [Loans] ADD CONSTRAINT [FK_Loans_Books_BookId]
    FOREIGN KEY ([BookId]) REFERENCES [Books] ([Id])
    ON DELETE NO ACTION;

ALTER TABLE [Loans] ADD CONSTRAINT [FK_Loans_Members_MemberId]
    FOREIGN KEY ([MemberId]) REFERENCES [Members] ([Id])
    ON DELETE NO ACTION;
