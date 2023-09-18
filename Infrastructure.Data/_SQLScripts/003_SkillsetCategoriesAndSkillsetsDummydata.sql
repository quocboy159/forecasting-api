Declare @insertedId int

INSERT INTO [dbo].[SkillsetCategory] ([CategoryName], [Description]) VALUES  ('.Net', 'Backend skills')
SET @insertedId = (SELECT SCOPE_IDENTITY())
INSERT INTO [dbo].[Skillset] ([SkillsetCategoryId], [SkillsetName]) VALUES (@insertedId, '.Net Core')
INSERT INTO [dbo].[Skillset] ([SkillsetCategoryId], [SkillsetName]) VALUES (@insertedId, 'Entity Framework')
GO

Declare @insertedId int
INSERT INTO [dbo].[SkillsetCategory] ([CategoryName], [Description]) VALUES  ('Database', 'Data Accessment skills')
SET @insertedId = (SELECT SCOPE_IDENTITY())
INSERT INTO [dbo].[Skillset] ([SkillsetCategoryId], [SkillsetName]) VALUES (@insertedId, 'SQL Server')
INSERT INTO [dbo].[Skillset] ([SkillsetCategoryId], [SkillsetName]) VALUES (@insertedId, 'MongoDB')
INSERT INTO [dbo].[Skillset] ([SkillsetCategoryId], [SkillsetName]) VALUES (@insertedId, 'MySQL')

GO

Declare @insertedId int
INSERT INTO [dbo].[SkillsetCategory] ([CategoryName], [Description]) VALUES  ('Frontend', 'Frontend skills')
SET @insertedId = (SELECT SCOPE_IDENTITY())
INSERT INTO [dbo].[Skillset] ([SkillsetCategoryId], [SkillsetName]) VALUES (@insertedId, 'VueJs')
INSERT INTO [dbo].[Skillset] ([SkillsetCategoryId], [SkillsetName]) VALUES (@insertedId, 'Angular Js')
INSERT INTO [dbo].[Skillset] ([SkillsetCategoryId], [SkillsetName]) VALUES (@insertedId, 'React')
INSERT INTO [dbo].[Skillset] ([SkillsetCategoryId], [SkillsetName]) VALUES (@insertedId, 'Ruby on rail')
GO

Declare @insertedId int
INSERT INTO [dbo].[SkillsetCategory] ([CategoryName], [Description]) VALUES  ('Python', 'Backend skills')
SET @insertedId = (SELECT SCOPE_IDENTITY())
INSERT INTO [dbo].[Skillset] ([SkillsetCategoryId], [SkillsetName]) VALUES (@insertedId, 'Django')
INSERT INTO [dbo].[Skillset] ([SkillsetCategoryId], [SkillsetName]) VALUES (@insertedId, 'Pyramid')
GO

Declare @insertedId int
INSERT INTO [dbo].[SkillsetCategory] ([CategoryName], [Description]) VALUES  ('CICD', 'Automatic skills')
SET @insertedId = (SELECT SCOPE_IDENTITY())
INSERT INTO [dbo].[Skillset] ([SkillsetCategoryId], [SkillsetName]) VALUES (@insertedId, 'Azure DevOps')
INSERT INTO [dbo].[Skillset] ([SkillsetCategoryId], [SkillsetName]) VALUES (@insertedId, 'Octopus')
Go

Declare @insertedId int
INSERT INTO [dbo].[SkillsetCategory] ([CategoryName], [Description]) VALUES  ('Mobile', 'Frontend skills')
SET @insertedId = (SELECT SCOPE_IDENTITY())
INSERT INTO [dbo].[Skillset] ([SkillsetCategoryId], [SkillsetName]) VALUES (@insertedId, 'iOS')
INSERT INTO [dbo].[Skillset] ([SkillsetCategoryId], [SkillsetName]) VALUES (@insertedId, 'Android')

GO


