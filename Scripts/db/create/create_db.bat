@echo off
if "%1" EQU "" goto BadParams

SET PGHOST=localhost
SET PGPORT=5432
SET PGUSER=postgres
SET PGPASSWORD=%1
SET PGCLIENTENCODING=UTF8
SET ON_ERROR_STOP=0

@rem Удаление висящих подключений
@rem psql  -U postgres -c "SELECT pg_terminate_backend(pg_stat_activity.pid) FROM pg_stat_activity WHERE pg_stat_activity.datname = 'greeny'  AND pid <> pg_backend_pid(); " 1>log00.out 2>log00.err  
psql   --dbname=postgres -U postgres -c "SELECT pg_stat_activity.pid FROM pg_stat_activity WHERE pg_stat_activity.datname = 'greeny' ;" 1>pg_stat.log 2>pg_stat.err
for /f "skip=5 tokens=3" %%a in ('"dir pg_stat.log /-c"') do set i=%%a  & goto 00
:00
if %i% GTR 27 (
del /f /s /q pg_stat.err
del /f /s /q pg_stat.log
@Echo  !!!!!There are connections to the database  "greeny" !!
 GoTo ErrorBuild
 
) else (
del /f /s /q pg_stat.err
del /f /s /q pg_stat.log
 GoTo 01
 )
:01
psql --file=create_db.sql
psql --file=create_schema.sql -w --dbname=greeny
psql --file=create_user.sql -w --dbname=greeny
psql --file=grant.sql -w --dbname=greeny

GoTo endJob

:BadParams
@Echo Parameter - password for postgres not found!!
@Echo Usage: create_db.bat psw_for_postgree
pause
goto endJob:

:ErrorBuild
Echo !!!!!!!!!!!!!Createing database failed!!!!!!!!!!!
pause
goto endJob:

:endJob

