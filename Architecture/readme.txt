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
	просто удалите Migrations папку.

	Чтобы удалить последние несколько миграций (не все):
	Не существует команды для удаления множества миграций, и мы не можем просто удалить эти 
	несколько миграций и migrations их *.designer.cs файлы, поскольку нам нужно сохранить файл 
	моментального снимка в согласованном состоянии. Нам нужно удалять миграции одну за другой (см. To remove last migration выше).
	
	Чтобы отменить и удалить последнюю миграцию:
	 dotnet ef migrations remove --force
	 or
	 PM> Remove-Migration -Force

 9) Пример как накатить миграцию и откатить её с удалением файла миграции
	
	// смотри список миграций и находим к примеру предпоследнюю миграцию
	dotnet ef migrations list -p Architecture.DAL -s Architecture

	/** 
	 *  обновляемся до предпоследний миграции(откатывемся)
	 *  после отката до предыдущей миграции, последняя миграция, которую мы хотим удалить, 
	/*  будет в состоянии "Panding"
	dotnet ef database update 20230207075615_FileTable  -p Architecture.DAL -s Architecture

	// Удаляем последнюю миграцию
	dotnet ef migrations remove -p Architecture.DAL -s Architecture

 10) Для подключения к локальной базе с виртуальной машины по логину и паролю, использовать следующие строки, как пример:
	Так же на локальной машине, в бд, разрешить удаленной подключение.

	"ConnectionStrings": {
    "DefaultConnection": "Server=DESKTOP-O3U1QA4;Database=Architecture;Integrated Security=false;Trusted_Connection=false;User ID=test;Password=123123;",
    "HangfireConnection": "Server=DESKTOP-O3U1QA4;Database=HangfireArchitecture;Trusted_Connection=true"
	 },