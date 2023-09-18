-- Execute by SSMS --> Query --> SQLCMD Mode
-- Change path value to your project/folder path
:setvar path "D:\CodeHq\ForecastingSystem.Backend\Infrastructure.Data\_SQLScripts"

:r $(path)\001_ClientDummyData.sql
:r $(path)\002_ProjectAndProjectRateDummyData.sql
:r $(path)\003_SkillsetCategoriesAndSkillsetsDummydata.sql
:r $(path)\004_RoleDummyData.sql
:r $(path)\005_EmployeesAndRolesDummydata.sql
:r $(path)\006_PublicHolidayDummyData.sql