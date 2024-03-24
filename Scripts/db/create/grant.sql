--Грантуем пользователям полный доступ к таблицам нашей схемы
GRANT USAGE ON SCHEMA greeny TO greeny_user;
GRANT ALL PRIVILEGES ON SCHEMA greeny TO greeny_user;
GRANT ALL ON ALL SEQUENCES IN SCHEMA greeny TO greeny_user;
GRANT ALL ON ALL TABLES IN SCHEMA greeny TO greeny_user;
GRANT ALL ON ALL FUNCTIONS IN SCHEMA greeny TO greeny_user;