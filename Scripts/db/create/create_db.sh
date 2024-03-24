#!/bin/bash
export PGUSER=postgres
export PGCLIENTENCODING=UTF8
export PGHOST=localhost
export PGPASSWORD=$1
# export PGPASSWORD=11111111
if [ $# -lt 1 ] 
then
    echo "Использование: create_db.sh Пароль"
    exit 1
fi    
#
psql   --dbname=postgres -U postgres -c "SELECT pg_stat_activity.pid FROM pg_stat_activity WHERE pg_stat_activity.datname = 'greeny' ;" 1>pg_stat.log 2>pg_stat.err
filesize=$(stat -c%s "pg_stat.log");
 if [[ "$filesize" != "28"  ]] 
then
    echo !!!!There are connections to the database  "greeny" !!
    rm ./pg_stat.log
    rm ./pg_stat.err
    exit 1
fi    
    rm ./pg_stat.log
    rm ./pg_stat.err


psql  -v ON_ERROR_STOP=1 -f create_db.sql
psql  -v ON_ERROR_STOP=1 -f create_schema.sql -w -d greeny
psql  -v ON_ERROR_STOP=1 -f create_user.sql -w -d greeny
psql  -v ON_ERROR_STOP=1 -f grant.sql -w -d greeny
