1) Добавление миграции
	dotnet ef migrations add AddYouTube -p Architecture.DAL -s Architecture

	где флаг -p означает в каком проекте хранится контекст БД
	где флаг -s означает в каком проекте хранится строка подключениея к БД

2) Применение миграции
	dotnet ef database update -p Architecture.DAL -s Architecture

	где флаг -p означает в каком проекте хранится контекст БД
	где флаг -s означает в каком проекте хранится строка подключениея к БД

3) Запуск тестов через консоль
	dotnet test Architecture.Tests

4) Запуск Ngrok для телаграм бота Webhook
	ngrok http https://localhost:5001 --host-header=localhost:5001

5) Запуск проекта из консоли. (Запускать головной проект с веб)
	dotnet run
