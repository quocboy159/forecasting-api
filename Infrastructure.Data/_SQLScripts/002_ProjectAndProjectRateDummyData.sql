INSERT [dbo].[Project] ([ProjectName], [ClientId], [ProjectCode], [Description], [ProjectType], [StartDate], [EndDate], [ProjectValue], [Confident], [ProjectManagerId], [Status], [UpdatedBy], [UpdatedDateTime]) VALUES ('Skillet', 1, '100150', NULL, 'Opportunity', NULL, NULL, 309000, 75, NULL, N'Active', NULL, NULL)

DECLARE @projectId INT
SELECT @projectId = SCOPE_IDENTITY()

INSERT [dbo].[ProjectRate] ([RateName], [ProjectId]) VALUES ('NZ Project Manager', @projectId)
INSERT [dbo].[ProjectRateHistory] ([ProjectRateId], [ExternalRateHistoryId], [Rate], [StartDate]) VALUES (SCOPE_IDENTITY(), 0, 185, GETDATE())

INSERT [dbo].[ProjectRate] ([RateName], [ProjectId]) VALUES ('VN Developer', @projectId)
INSERT [dbo].[ProjectRateHistory] ([ProjectRateId], [ExternalRateHistoryId], [Rate], [StartDate]) VALUES (SCOPE_IDENTITY(), 0, 105, GETDATE())

INSERT [dbo].[ProjectRate] ([RateName], [ProjectId]) VALUES ('VN Tester', @projectId)
INSERT [dbo].[ProjectRateHistory] ([ProjectRateId], [ExternalRateHistoryId], [Rate], [StartDate]) VALUES (SCOPE_IDENTITY(), 0, 115, GETDATE())

INSERT [dbo].[ProjectRate] ([RateName], [ProjectId]) VALUES ('NZ Developer', @projectId)
INSERT [dbo].[ProjectRateHistory] ([ProjectRateId], [ExternalRateHistoryId], [Rate], [StartDate]) VALUES (SCOPE_IDENTITY(), 0, 165, GETDATE())

