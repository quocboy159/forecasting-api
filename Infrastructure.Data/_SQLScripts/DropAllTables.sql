-- Drop all the constraints
while(
    exists(
        select 1
        from INFORMATION_SCHEMA.TABLE_CONSTRAINTS
        where CONSTRAINT_TYPE = 'FOREIGN KEY'
        AND CONSTRAINT_SCHEMA <> 'HangFire'
    )
) begin
        declare @sql1 nvarchar(2000)
        SELECT TOP 1 @sql1 =(
                                'ALTER TABLE ' + TABLE_SCHEMA + '.[' + TABLE_NAME + '] DROP CONSTRAINT [' + CONSTRAINT_NAME + ']'
                            )
                        FROM information_schema.table_constraints
                        WHERE CONSTRAINT_TYPE = 'FOREIGN KEY'  AND CONSTRAINT_SCHEMA <> 'HangFire'
        exec (@sql1) 
        PRINT @sql1
end 

-- Drop all the tables
while(
    exists(
        select 1
        from INFORMATION_SCHEMA.TABLES
        where  TABLE_TYPE = 'BASE TABLE'AND TABLE_SCHEMA <> 'HangFire'
    )
) begin
        declare @sql2 nvarchar(2000)
        SELECT TOP 1 @sql2 =(
                                'DROP TABLE ' + TABLE_SCHEMA + '.[' + TABLE_NAME + ']'
                            )
                        FROM INFORMATION_SCHEMA.TABLES
                        WHERE TABLE_TYPE = 'BASE TABLE'  AND TABLE_SCHEMA <> 'HangFire'
        exec (@sql2)
        /* you dont need this line, it just shows what was executed */
        PRINT @sql2
end