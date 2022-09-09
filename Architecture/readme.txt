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

6) Просмотр скрипта в консоли, какой код SQL будет сформирован
	dotnet ef dbcontext script -p Architecture.DAL -s Architecture

7) Список миграций
	dotnet ef migrations list -p Architecture.DAL -s Architecture

8) Чтобы отменить конкретную(ые) миграцию(и) :
	dotnet ef database update LastGoodMigrationName
	or
	PM> Update-Database -Migration LastGoodMigrationName
	
	Чтобы отменить все миграции :
	 dotnet ef database update 0
	 or
	 PM> Update-Database -Migration 0
	
	Чтобы удалить последнюю миграцию:
	 dotnet ef migrations remove
	 or
	 PM> Remove-Migration
	
	Чтобы удалить все миграции:
	просто удалите Migrationsпапку.

	Чтобы удалить последние несколько миграций (не все):
	Не существует команды для удаления множества миграций, и мы не можем просто удалить эти 
	несколько миграций и migrations их *.designer.cs файлы, поскольку нам нужно сохранить файл 
	моментального снимка в согласованном состоянии. Нам нужно удалять миграции одну за другой (см. To remove last migration выше).
	
	Чтобы отменить и удалить последнюю миграцию:
	 dotnet ef migrations remove --force
	 or
	 PM> Remove-Migration -Force
