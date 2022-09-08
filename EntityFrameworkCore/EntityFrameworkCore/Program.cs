// Entity Framework Core (databases) - https://metanit.com/sharp/efcore/1.1.php
using Microsoft.EntityFrameworkCore; // основной пакет EF Core
// using Microsoft.EntityFrameworkCore.SqlServer; // представляет функциональность провайдера для Microsoft SQL Server и SQL Azure
// using Microsoft.EntityFrameworkCore.SqlServer.NetTopologySuite; // предоставляет поддержку географических типов (spatial types) для SQL Server
using Microsoft.EntityFrameworkCore.Sqlite; // представляет функциональность провайдера для SQLite и включает нативные бинарные файлы для движка базы данных
// using Microsoft.EntityFrameworkCore.Sqlite.Core; // представляет функциональность провайдера для SQLite, но в отличие от предыдущего пакета не содержит нативные бинарные файлы для движка базы данных
// using Microsoft.EntityFrameworkCore.Sqlite.NetTopologySuite; // предоставляет поддержку географических типов (spatial types) для SQLite
// using Microsoft.EntityFrameworkCore.Cosmos; // представляет функциональность провайдера для Azure Cosmos DB
// using Microsoft.EntityFrameworkCore.InMemory; // представляет функциональность провайдера базы данных в памяти
// using Microsoft.EntityFrameworkCore.Tools; // содержит команды EF Core PowerShell для Visual Studio Package Manager Console; применяется в Visual Studio для миграций и генерации классов по готовой бд
using Microsoft.EntityFrameworkCore.Design; // содержит вспомогательные компоненты EF Core, применяемые в процессе разработки
// using Microsoft.EntityFrameworkCore.Proxies; // хранит функциональность для так называемой "ленивой загрузки" (lazy loading) и прокси остлеживания изменений
// using Microsoft.EntityFrameworkCore.Abstractions; // содержит набор абстракций EF Core, которые не зависят от конкретной СУБД
// using Microsoft.EntityFrameworkCore.Relational; // хранит компоненты EF Core для провайдеров реляционных СУБД
// using Microsoft.EntityFrameworkCore.Analyzers; // содержит функционал анализаторов C# для EF Core
using Microsoft.EntityFrameworkCore.Metadata;
using System.Collections.Generic;
using System;
using System.Drawing;
using Microsoft.Extensions.Configuration;
// using Microsoft.Extensions.Configuration.Binder;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.Configuration.EnvironmentVariables;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore.Migrations;
using static System.Collections.Specialized.BitVector32;
using System.Runtime.Intrinsics.X86;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Globalization;
using System.Linq;
using System.ComponentModel.DataAnnotations; // Аннотации - настройка классов сущностей в Fluent API с помощью атрибутов, большинство из них располагаются в этом пространстве
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Reflection.Emit;
// using Microsoft.EntityFrameworkCore.SqlServer // to use SQL Server in EF
// using Pomelo.EntityFrameworkCore.MySql // to use MySql (from Pomelo; official MySql from Oracle is worse)))
// using Npgsql.EntityFrameworkCore.PostgreSQL // to use PostgreSQL in EF
// Scaffold-DbContext "Host=localhost;Port=5432;Database=usersdb;Username=postgres;Password=123456789"
// enter Provider: Npgsql.EntityFrameworkCore.PostgreSQL
// (для выполнения этой команды тоже необходим пакет Microsoft.EntityFrameworkCore.Tools)

// for Northwind.db:
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.IO;
using static System.Console; // now we can call Read/Write/ReadLine/WriteLine without "Console."
using System.Text;
using System.Diagnostics.Metrics;
using System.Data;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

InputEncoding = Encoding.Unicode;
OutputEncoding = Encoding.Unicode;

// ApplicationContext db = new(); // DB deletion and creation
ForeignLoading(5);


// Study Methods
static void CreateDB() // Creating and filling up a DB
{
    // Так как класс ApplicationContext через базовый класс DbContext реализует интерфейс IDisposable,
    // то для работы с ApplicationContext с автоматическим закрытием данного объекта мы можем использовать конструкцию using.
    // !! Операции с контекстом (создание БД, открытие, подключение, удаление...) могут занимать где-то 200-250 мс каждая
    using (ApplicationContext db = new())
    {
        // создаем два объекта User
        User tom = new User("Tom", 33); // { Name = "Tom", Age = 33 };
        User alice = new User("Alice", 26); // { Name = "Alice", Age = 26 };

        // добавляем их в БД
        db.Users.Add(tom);
        db.Users.Add(alice);
        // сохранение в БД
        db.SaveChanges();
        Console.WriteLine("Объекты успешно сохранены");

        // проверяем: получаем объекты из БД и выводим на консоль
        Console.WriteLine("Список объектов:");
        ListUsersDB();

        // удаляем БД
        // if (db.Database.EnsureDeleted())
        //     Console.WriteLine($"БД Users удалена");
        // else Console.WriteLine($"БД Users уже была удалена ранее");
    }
}

static void ListUsersDB() // Reading an existing DB
{
using (ApplicationContext db = new())
{
ListUsersDBContext(db);
}
}

static void ListUsersDBContext(ApplicationContext db) // Reading an existing DB
{
    // получаем объекты из БД и выводим на консоль
    // Users имеет тип DBSet<User>, users = List<User>
    // ToList calls User() constructors for User, that is, the properties are set authomatically from DB
    var users = db.Users.ToList();  // или await db.Users.ToListAsync();
    foreach (User u in users)
        u.Print();
}

// Основные операции с данными. Большинство операций с данными так или иначе представляют собой CRUD операции (Create, Read, Update, Delete),
// то есть создание, получение, обновление и удаление. Entity Framework Core позволяет легко выполнять все эти действия.
static void CRUD()
{
    // Добавление

    using (ApplicationContext db = new ApplicationContext())
    {
        User oleg = new User("Oleg", 50);
        User angela = new User("Angela", 53);

        // any number of parameters or an IEnumerable parameter allowed
        db.Users.AddRange(oleg, angela); // = db.Users.Add(oleg); db.Users.Add(angela);
                                         // или await db.Users.AddRangeAsync(tom, alice);

        db.SaveChanges(); // или await db.SaveChangesAsync();
    }

    // чтение (получение)
    using (ApplicationContext db = new ApplicationContext())
    {
        // получаем объекты из бд и выводим на консоль
        Console.WriteLine("Данные после добавления:");
        ListUsersDBContext(db);
    }

    // Редактирование
    using (ApplicationContext db = new ApplicationContext())
    {
        // получаем первый объект
        User? user = db.Users.FirstOrDefault(); // или await db.Users.FirstOrDefaultAsync();
        if (user != null)
        {
            user.SetName("Bob");
            user.SetAge(44);
            db.SaveChanges(); // или await db.SaveChangesAsync();

            // обновить объект user НЕзависимо от контекста: db.Users.Update(user);
            // также обновление множества: db.Users.UpdateRange(tom, alice); 
        }
        // выводим данные после обновления
        Console.WriteLine("\nДанные после редактирования:");
        ListUsersDBContext(db);
    }

    // Удаление
    using (ApplicationContext db = new ApplicationContext())
    {
        // получаем первый объект
        User? firstUser = db.Users.FirstOrDefault();
        User? secondUser = firstUser is not null ? db.Users.FirstOrDefault(u => u.Id != firstUser.Id) : null;

        //удаляем объекты
        if (firstUser is not null) db.Users.Remove(firstUser);
        if (secondUser is not null) db.Users.Remove(secondUser);
        db.SaveChanges();

        // выводим данные после обновления
        Console.WriteLine($"\nДанные после удаления первой ({firstUser?.Id}) и второй ({secondUser?.Id}) строк:");
        ListUsersDBContext(db);
    }
}

// настройка с помощью DbContextOptionsBuilder & ConfigurationBuilder
static void Config()
{
    // configuration through options builder
    // DbContextOptionsBuilder<ApplicationContext> optionsBuilder0 = 
    //    new DbContextOptionsBuilder<ApplicationContext>() // конструктор по умолчанию
    //        .UseSqlite("Data Source=helloapp.db"); // это декоратор, метод UseSqlite тоже возвращает DbContextOptionsBuilder

    // DbContextOptions<ApplicationContext> options0 = optionsBuilder0.Options;

    // configuration through JSON file:
    IConfigurationBuilder builder = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json") // получаем конфигурацию из файла appsettings.json - устаревшая функция
        .SetBasePath(Directory.GetCurrentDirectory()); // установка пути к текущему каталогу - устаревшая функция
    Console.WriteLine("builder has the type: " + builder.GetType()); // Microsoft.Extensions.Configuration.ConfigurationBuilder

    // создаем конфигурацию
    IConfiguration config = builder.Build();
    Console.WriteLine("config has the type: " + config.GetType()); // Microsoft.Extensions.Configuration.ConfigurationRoot

    // получаем строку подключения
    // GetConnectionString(string name) is the shorthand for GetSection("ConnectionStrings")[name]
    string connectionString = config.GetConnectionString("DefaultConnection"); // the name of the string from JSON file
    Console.WriteLine("connectionString is: " + connectionString); // Data Source = helloapp.db

    // different method:
    // Get values from the config given their key and their target type.
    JsonSettings settings = config.GetRequiredSection("ConnectionStrings").Get<JsonSettings>();
    Console.WriteLine("DefaultConnection: " + settings.DefaultConnection); // Data Source = helloapp.db

    DbContextOptionsBuilder<ApplicationContext> optionsBuilder =
        new DbContextOptionsBuilder<ApplicationContext>() // конструктор по умолчанию
            .UseSqlite(connectionString); // это декоратор, метод UseSqlite тоже возвращает DbContextOptionsBuilder

    DbContextOptions<ApplicationContext> options = optionsBuilder.Options;

    using (ApplicationContext db = new ApplicationContext(options))
        ListUsersDBContext(db);
}

// Логирование
static void Logging()
{
    // Метод LogTo
    using (ApplicationContext db = new ApplicationContext())
    {
        User user1 = new User("Tom", 33);
        User user2 = new User("Alice", 26);

        db.Users.Add(user1);
        db.Users.Add(user2);
        db.SaveChanges();

        ListUsersDBContext(db);

        // Output see the file log.txt
    }
}

// Northwind.db queries
static void QueryingCategories() 
{
    using (var db = new Northwind())
    {
        Console.WriteLine("Categories and how many products they have:");
        
        // запрос для получения всех категорий и связанных с ними товаров
        IQueryable<Category> cats = db.Categories
            .Include(c => c.Products);

        Console.WriteLine($"cats.ToQueryString: {cats.ToQueryString()}");
        // cats.ToQueryString:
        // SELECT "c"."CategoryID", "c"."CategoryName", "c"."Description", "p"."ProductID", "p"."CategoryID", "p"."UnitPrice", "p"."Discontinued", "p"."ProductName", "p"."UnitsInStock"
        // FROM "Categories" AS "c"
        // LEFT JOIN "Products" AS "p" ON "c"."CategoryID" = "p"."CategoryID"
        // ORDER BY "c"."CategoryID"

        foreach (Category c in cats)
            Console.WriteLine($"{c.CategoryName} has {c.Products.Count} products.");
        // Output:
        // Categories and how many products they have:
        // Beverages has 12 products.
        // Condiments has 12 products.
        // Confections has 13 products.
        // Dairy Products has 10 products.
        // Grains/Cereals has 7 products.
        // Meat/Poultry has 6 products.
        // Produce has 5 products.
        // Seafood has 12 products.
    }
}

static void FilteredIncludes()
{
    using (var db = new Northwind())
    {
        Console.Write("Enter a minimum for units in stock: ");
        string unitsInStock = Console.ReadLine();
        int stock = int.Parse(unitsInStock);
        IQueryable<Category> cats = db.Categories
            .Include(c => c.Products.Where(p => p.Stock >= stock));

        Console.WriteLine($"cats.ToQueryString: {cats.ToQueryString()}");
        // cats.ToQueryString: .param set @__stock_0 100
        // SELECT "c"."CategoryID", "c"."CategoryName", "c"."Description", "t"."ProductID", "t"."CategoryID", "t"."UnitPrice", "t"."Discontinued", "t"."ProductName", "t"."UnitsInStock"
        // FROM "Categories" AS "c"
        // LEFT JOIN(
        //      SELECT "p"."ProductID", "p"."CategoryID", "p"."UnitPrice", "p"."Discontinued", "p"."ProductName", "p"."UnitsInStock"
        //      FROM "Products" AS "p"
        //      WHERE "p"."UnitsInStock" >= @__stock_0
        // ) AS "t" ON "c"."CategoryID" = "t"."CategoryID"
        // ORDER BY "c"."CategoryID"

        foreach (Category c in cats) {
            Console.WriteLine($"{c.CategoryName} has {c.Products.Count} products with a minimum of {stock} units in stock.");
        
            foreach (Product p in c.Products)
                Console.WriteLine($" {p.ProductName} has {p.Stock} units in stock.");
        }
        // Output:
        // Beverages has 2 products with a minimum of 100 units in stock.
        //  Sasquatch Ale has 111 units in stock.
        //  Rhönbräu Klosterbier has 125 units in stock.
        // Condiments has 2 products with a minimum of 100 units in stock.
        //  Grandma's Boysenberry Spread has 120 units in stock.
        //  Sirop d'érable has 113 units in stock.
        // Confections has 0 products with a minimum of 100 units in stock.
        // Dairy Products has 1 products with a minimum of 100 units in stock.
        //  Geitost has 112 units in stock.
        // Grains/Cereals has 1 products with a minimum of 100 units in stock.
        //  Gustaf's Knäckebröd has 104 units in stock.
        // Meat/Poultry has 1 products with a minimum of 100 units in stock.
        //  Pâté chinois has 115 units in stock.
        // Produce has 0 products with a minimum of 100 units in stock.
        // Seafood has 3 products with a minimum of 100 units in stock.
        //  Inlagd Sill has 112 units in stock.
        //  Boston Crab Meat has 123 units in stock.
        //  Röd Kaviar has 101 units in stock.
    }
}

static void QueryingProducts()
{
    using (var db = new Northwind())
    {
        WriteLine("Products that cost more than a price, highest at top.");
        string input;
        decimal price;
        do
        {
            Write("Enter a product price: ");
            input = Console.ReadLine();
        } while (!decimal.TryParse(input, out price));

        IQueryable<Product> prods = db.Products
            .Where(product => product.Cost > price)
            .OrderByDescending(product => product.Cost);

        Console.WriteLine($"prods.ToQueryString: {prods.ToQueryString()}");
        // .param set @__price_0 50.0
        // SELECT "p"."ProductID", "p"."CategoryID", "p"."UnitPrice", "p"."Discontinued", "p"."ProductName", "p"."UnitsInStock"
        // FROM "Products" AS "p"
        // WHERE "p"."UnitPrice" > @__price_0
        // ORDER BY "p"."UnitPrice" DESC

        // the same is:
        IQueryable<Product> prods2 = from product in db.Products
                                     where product.Cost > price
                                     orderby product.Cost descending
                                     select product;

        foreach (Product item in prods)
            Console.WriteLine("{0}: {1} costs {2:$#,##0.00} and has {3} in stock.", item.ProductID, item.ProductName, item.Cost, item.Stock);
    }
    // Output:
    // Products that cost more than a price, highest at top.
    // Enter a product price: 50
    // 38: Côte de Blaye costs $263,50 and has 17 in stock.
    // 29: Thüringer Rostbratwurst costs $123,79 and has 0 in stock.
    // 9: Mishi Kobe Niku costs $97,00 and has 29 in stock.
    // 20: Sir Rodney's Marmalade costs $81,00 and has 40 in stock.
    // 18: Carnarvon Tigers costs $62,50 and has 42 in stock.
    // 59: Raclette Courdavault costs $55,00 and has 79 in stock.
    // 51: Manjimup Dried Apples costs $53,00 and has 20 in stock.
}

// Конструкторы сущностей
static void UserConstructor()
{
    using (ApplicationContext db = new())
    {
        // db.Database.EnsureDeleted();
        // db.Database.EnsureCreated();

        User tom = new User("Tom", 37); // Constructor for Tom (37) has been called
        User bob = new User("Bob", 41); // Constructor for Bob (41) has been called
        User tim = new User("Tim");     // Constructor for Tim has been called
        db.Users.Add(tom);
        db.Users.Add(bob);
        db.Users.Add(tim);
        db.SaveChanges();

        // ListUsersDBContext(db); // output: only DB values
    }

    using (ApplicationContext db = new())
        ListUsersDBContext(db); // output: EMPTY constructor calls and DB values
        // Empty constructor has been called // 3 or 6 times (in second run)
        // 1. Tom - 37
        // 2. Bob - 41
        // 3. Tim - 18
        // 4. Tom - 37
        // 5. Bob - 41
        // 6. Tim - 18
}

// Генерация значений свойств и столбцов
// Если для свойства явным образом не установлено значение при вставке в таблицу, то устанавливается значение по умолчанию
// (null для nullable-типов, 0 для int, Guid.Empty для Guid и т.д.). В зависимости от используемого провайдера базы данных,
// значения для свойств могут генерироваться на стороне клиента с помощью EF, либо же на стороне базы данных при добавлении.
// Если значение генерируется базой данных, тогда при добавлении объекта в контекст EF может назначить временное значение.
// Это временное значение будет заменено значением, сгенерированным базой данных при вызове метода SaveChanges().
// Генерация ключей
// По умолчанию для свойств первичных ключей, которые представляют типы int или GUID и которые имеют значение по умолчанию,
// генерируется значение при вставке в базу данных. Для всех остальных свойств значения по умолчанию не генерируется.
static void ValuesGeneration()
{
    using (ApplicationContext db = new ApplicationContext())
    {
        User user = new User("Tom"); // Constructor for Tom has been called
        Console.WriteLine($"Перед добавлением в контекст: User1 Id = {user.Id}, Age = {user.Age}");    // Id = 0, Age = 18
        db.Users.Add(user);
        Console.WriteLine($"После добавления в контекст: User1 Id = {user.Id}, Age = {user.Age}");   // Id = 0, Age = 18

        User user2 = new User("Max"); // Constructor for Max has been called
        user2.SetAge(19);
        Console.WriteLine($"Перед добавлением в контекст: User2 Id = {user2.Id}, Age = {user2.Age}");  // Id = 0, Age = 19
        db.Users.Add(user2);
        // if [DatabaseGenerated(DatabaseGeneratedOption.None)] Id: System.InvalidOperationException: The instance of entity type 'User' cannot be tracked because another instance with the key value '{Id: 0}' is already being tracked. When attaching existing entities, ensure that only one entity instance with a given key value is attached.
        // If [DatabaseGenerated(DatabaseGeneratedOption.Identity)]: default (as written here)
        // If [DatabaseGenerated(DatabaseGeneratedOption.Computed)]: System.InvalidOperationException: The property 'Id' cannot be configured as 'ValueGeneratedOnUpdate' or 'ValueGeneratedOnAddOrUpdate' because it's part of a key and its value cannot be changed after the entity has been added to the store.
        Console.WriteLine($"После добавления в контекст: User2 Id = {user2.Id}, Age = {user2.Age}");   // Id = 0, Age = 19

        db.SaveChanges();
        Console.WriteLine($"После добавления в базу данных: User1 Id = {user.Id}, Age = {user.Age}");  // Id = 1, Age = 18
        Console.WriteLine($"После добавления в базу данных: User2 Id = {user2.Id}, Age = {user2.Age}");  // Id = 2, Age = 19
        ListUsersDBContext(db);
        // 1. Tom - 18
        // 2. Max - 19
    }
}

static void ExternalKeys()
{
    // Установка главной сущности по навигационному свойству зависимой сущности
    using (ApplicationContext db = new ApplicationContext())
    {
        Company company1 = new Company { Name = "Google" }; // Empty Company constructor has been called (2x)
        Company company2 = new Company { Name = "Microsoft" };
        User user1 = new User { Name = "Tom", Company = company1 }; // Empty User constructor has been called (3x)
        User user2 = new User { Name = "Bob", Company = company2 };
        User user3 = new User { Name = "Sam", Company = company2 };

        db.Companies.AddRange(company1, company2);  // добавление компаний 
        db.Users.AddRange(user1, user2, user3);     // добавление пользователей

        db.SaveChanges();

        ListUsersDBContext(db);
        // 1. Tom (age 0) works in Google (Id 1) // 1.Tom (age 0) works in Google (Id 0) if CompanyName instead of Company.Name
        // 2. Bob (age 0) works in Microsoft (Id 2)
        // 3. Sam (age 0) works in Microsoft (Id 2)
        // Empty Company constructor has been called (2x) - only if this is the end of the routine (Dispose?)
        // Empty User constructor has been called (3x) - only if this is the end of the routine (Dispose?)
    }

    Console.WriteLine();

    // Установка главной сущности по свойству-внешнему ключу зависимой сущности
    using (ApplicationContext db = new ApplicationContext())
    {
        Company company1 = new Company { Name = "Google" }; // Empty Company constructor has been called (2x)
        Company company2 = new Company { Name = "Microsoft" };
        db.Companies.AddRange(company1, company2);  // добавление компаний
        db.SaveChanges();
        // Company*.Id is generated in SaveChanges so we had to call it first

        User user1 = new User { Name = "Tom", CompanyId = company1.Id }; // Empty User constructor has been called (3x)
        User user2 = new User { Name = "Bob", CompanyId = company1.Id };
        User user3 = new User { Name = "Sam", CompanyId = company2.Id };

        db.Users.AddRange(user1, user2, user3);     // добавление пользователей
        db.SaveChanges();

        ListUsersDBContext(db);
        // 1.Tom (age 0) works in Google (Id 1) // 1.Tom(age 0) works in  (Id 1)  if CompanyName instead of Company.Name
        // 2.Bob (age 0) works in Google (Id 1)
        // 3.Sam (age 0) works in Microsoft (Id 2)
        // Empty Company constructor has been called (2x) - only if this is the end of the routine (Dispose?)
        // Empty User constructor has been called (3x) - only if this is the end of the routine (Dispose?)
    }

    Console.WriteLine();

    // Установка зависимой сущности через навигационное свойство главной сущности
    using (ApplicationContext db = new ApplicationContext())
    {
        User user1 = new User { Name = "Tom" }; // Empty User constructor has been called (3x)
        User user2 = new User { Name = "Bob" };
        User user3 = new User { Name = "Sam" };

        Company company1 = new Company { Name = "Google", Users = { user1, user2 } }; // Empty Company constructor has been called (2x)
        Company company2 = new Company { Name = "Microsoft", Users = { user3 } };

        db.Companies.AddRange(company1, company2);  // добавление компаний
        db.Users.AddRange(user1, user2, user3);     // добавление пользователей
        db.SaveChanges();

        ListUsersDBContext(db);
        // 1. Tom (age 0) works in Google (Id 1) // 1. Tom (age 0) works in Google (Id 0)  if CompanyName instead of Company.Name
        // 2. Bob (age 0) works in Google (Id 1)
        // 3. Sam (age 0) works in Microsoft (Id 2)
        // Empty Company constructor has been called (2x) - only if this is the end of the routine (Dispose?)
        // Empty User constructor has been called (3x) - only if this is the end of the routine (Dispose?)
    }

    Console.WriteLine();

    // Установка в качестве внешнего ключа произвольного свойства (CompanyName)
    using (ApplicationContext db = new ApplicationContext())
    {
        Company company1 = new Company { Name = "Google" };
        Company company2 = new Company { Name = "Microsoft" };
        User user1 = new User { Name = "Tom", Company = company1 };
        User user2 = new User { Name = "Bob", Company = company2 }; // CompanyName = "Microsoft" };
        User user3 = new User { Name = "Sam", Company = company2 }; // CompanyName = company2.Name };

        db.Companies.AddRange(company1, company2);
        db.Users.AddRange(user1, user2, user3);
        db.SaveChanges();

        ListUsersDBContext(db); // System.NullReferenceException: Object reference not set to an instance of an object (in Print method with Company.Name)
        // 1. Tom (age 0) works in Google (Id 0)  // CompanyName instead of Company.Name
        // 2. Bob (age 0) works in Microsoft (Id 0)
        // 3. Sam (age 0) works in Microsoft (Id 0)
        // Empty Company constructor has been called (2x) - only if this is the end of the routine (Dispose?)
        // Empty User constructor has been called (3x) - only if this is the end of the routine (Dispose?)
    }
}

// Каскадное удаление представляет автоматическое удаление зависимой сущности после удаления главной
static void DeleteCascade()
{
    // if User.Company is [Required] or .OnDelete(DeleteBehavior.Cascade) in Fluent API ->
    // CONSTRAINT "PK_Users" PRIMARY KEY("Id" AUTOINCREMENT),
    // CONSTRAINT "FK_Users_Companies_CompanyId" FOREIGN KEY("CompanyId") REFERENCES "Companies"("Id") ON DELETE CASCADE
    using (ApplicationContext db = new ApplicationContext())
    {
        // добавляем начальные данные
        Company microsoft = new Company { Name = "Microsoft" }; // Empty Company constructor has been called (2x)
        Company google = new Company { Name = "Google" };
        db.Companies.AddRange(microsoft, google);
        db.SaveChanges();

        User tom = new User { Name = "Tom", Company = microsoft }; // Empty User constructor has been called (4x)
        User bob = new User { Name = "Bob", Company = google };
        User alice = new User { Name = "Alice", Company = microsoft };
        User kate = new User { Name = "Kate", Company = google };
        db.Users.AddRange(tom, bob, alice, kate);
        db.SaveChanges();

        Console.WriteLine("\nСписок пользователей до удаления первой компании");
        ListUsersDBContext(db);
        // 1. Tom (age 0) works in Microsoft (Id 1)
        // 2. Bob (age 0) works in Google (Id 2)
        // 3. Alice (age 0) works in Microsoft (Id 1)
        // 4. Kate (age 0) works in Google (Id 2)

        // Удаляем первую компанию
        Company comp = db.Companies.FirstOrDefault();
        if (comp != null) db.Companies.Remove(comp);
        db.SaveChanges();

        Console.WriteLine("\nСписок пользователей после удаления первой компании");
        ListUsersDBContext(db);
        // 2. Bob (age 0) works in Google (Id 2)
        // 4. Kate (age 0) works in Google (Id 2)
    }

    Console.WriteLine();

    // if User.Company is Nullable and User.CompanyId is absent or is Nullable int
    // or  .OnDelete(DeleteBehavior.Restrict) or .OnDelete(DeleteBehavior.SetNull) in Fluent API ->
    // CONSTRAINT "FK_Users_Companies_CompanyId" FOREIGN KEY("CompanyId") REFERENCES "Companies"("Id"),
    // CONSTRAINT "PK_Users" PRIMARY KEY("Id" AUTOINCREMENT)
    // or CONSTRAINT "FK_Users_Companies_CompanyId" FOREIGN KEY("CompanyId") REFERENCES "Companies"("Id"), ON DELETE SET NULL

    // (Company.Name in Print) -> System.NullReferenceException: Object reference not set to an instance of an object.
    // (Company?.Name in Print) -> Список пользователей после удаления первой компании:
    // 1. Tom (age 0) works in  (Id)
    // 2. Bob (age 0) works in Google (Id 2)
    // 3. Alice (age 0) works in  (Id)
    // 4. Kate (age 0) works in Google (Id 2)
}

// Загрузка связанных данных Через навигационные свойства
static void ForeignLoading(int method)
{
    Console.WriteLine("Method: " + method);
    // Eager loading (жадная загрузка) - Метод Include

    // 1. When Include is not required
    if (method == 1) using (ApplicationContext db = new ApplicationContext())
        {
            // добавляем начальные данные
            Company microsoft = new Company { Name = "Microsoft" }; // Empty Company constructor has been called (2x)
            Company google = new Company { Name = "Google" };
            db.Companies.AddRange(microsoft, google);

            User tom = new User { Name = "Tom", Company = microsoft }; // Empty User constructor has been called (4x)
            User bob = new User { Name = "Bob", Company = google };
            User alice = new User { Name = "Alice", Company = microsoft };
            User kate = new User { Name = "Kate", Company = google };
            db.Users.AddRange(tom, bob, alice, kate);

            db.SaveChanges();
            // since we have used AddRange and SaveChanges then we can skip Include: data is already in the context
            // the same is with double query: companies & users separately
            // получаем компании
            // List<Company> companies = db.Companies.ToList();

            // получаем пользователей
            List<User> users = db.Users
                .Include(u => u.Company)  // подгружаем данные по компаниям
                                          //.ThenInclude(c => c. ...) // если надо по иерархии
                .ToList();
            // SQL query for this is:
            // SELECT "u"."Id", "u"."CompanyId", "u"."Name", "c"."Id", "c"."Name"
            // FROM "Users" AS "u"
            // LEFT JOIN "Companies" AS "c" ON "u"."CompanyId" = "c"."Id"    // LEFT JOIN присоединяет данные из другой таблицы

            foreach (User user in users) user.Print();
            // 1. Tom (age 0) works in Microsoft (Id 1)
            // 2. Bob (age 0) works in Google (Id 2)
            // 3. Alice (age 0) works in Microsoft (Id 1)
            // 4. Kate (age 0) works in Google (Id 2)
        }

    // 2. When Include is required
    if (method == 2) using (ApplicationContext db = new ApplicationContext())
        {
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            // добавляем начальные данные
            Company microsoft = new Company { Name = "Microsoft" }; // Empty Company constructor has been called (2x)
            Company google = new Company { Name = "Google" };
            db.Companies.AddRange(microsoft, google);

            User tom = new User { Name = "Tom", Company = microsoft }; // Empty User constructor has been called (4x)
            User bob = new User { Name = "Bob", Company = google };
            User alice = new User { Name = "Alice", Company = microsoft };
            User kate = new User { Name = "Kate", Company = google };
            db.Users.AddRange(tom, bob, alice, kate);

            db.SaveChanges();
        }
    // separate context does not have the Companies data -> Include is required
    if (method == 2) using (ApplicationContext db = new ApplicationContext())
        {
            List<User> users = db.Users
                .Include(u => u.Company)  // добавляем данные по компаниям
                .ToList();
            foreach (User user in users) user.Print();
            // Without Include:
            // 1. Tom (age 0) works in  (Id 1)
            // 2. Bob (age 0) works in  (Id 2)
            // 3. Alice (age 0) works in  (Id 1)
            // 4. Kate (age 0) works in  (Id 2)
        }

    // 3. Загрузка сущностей со сложной многоуровневой структурой
    // добавление данных
    if (method == 3) using (ApplicationContext db = new ApplicationContext())
        {
            // пересоздадим базу данных
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            Country usa = new Country { Name = "USA" }; // Empty Country constructor has been called (2x)
            Country japan = new Country { Name = "Japan" };
            db.Countries.AddRange(usa, japan);

            // добавляем начальные данные
            Company microsoft = new Company { Name = "Microsoft", Country = usa }; // Empty Company constructor has been called (3x)
            Company google = new Company { Name = "Google", Country = usa };
            Company sony = new Company { Name = "Sony", Country = japan };
            db.Companies.AddRange(microsoft, google, sony);

            User tom = new User { Name = "Tom", Company = microsoft }; // Empty User constructor has been called (6x)
            User bob = new User { Name = "Bob", Company = sony };
            User oleg = new User { Name = "Oleg", Company = google };
            User alice = new User { Name = "Alice", Company = microsoft };
            User kate = new User { Name = "Kate", Company = sony };
            User angela = new User { Name = "Angela", Company = google };
            db.Users.AddRange(tom, bob, alice, kate, oleg, angela);

            db.SaveChanges();
        }
    // получение данных
    if (method == 3) using (ApplicationContext db = new ApplicationContext())
        {
            // получаем пользователей
            List<User> users = db.Users
                .Include(u => u.Company)  // подгружаем данные по компаниям
                    .ThenInclude(c => c!.Country)    // к компаниям подгружаем данные по странам // ! is to suppress the warning that c may be null
                                                     // if c is null then this property is ignored anyway
                                                     // .Include(u => u.Company!.Country)  // this is the same
                .ToList();
            // Empty User constructor has been called ; Empty Company constructor has been called ; Empty Country constructor has been called
            // Empty User constructor has been called ; Empty Company constructor has been called ; Empty Country constructor has been called
            // Empty User constructor has been called (3x) ; Empty Company constructor has been called ; Empty User constructor has been called
            // total: User 6x, Company 3x, Country 2x

            // SQL for this query is:
            // SELECT "u"."Id", "u"."CompanyId", "u"."Name", "c"."Id", "c"."CountryId", "c"."Name", "c0"."Id", "c0"."Name"
            // FROM "Users" AS "u"
            // LEFT JOIN "Companies" AS "c" ON "u"."CompanyId" = "c"."Id"
            // LEFT JOIN "Countries" AS "c0" ON "c"."CountryId" = "c0"."Id"

            foreach (User user in users) user.Print();
            // 1. Tom (age 0) works in Microsoft (Id 1, country USA)
            // 2. Bob (age 0) works in Sony (Id 3, country Japan)
            // 3. Alice (age 0) works in Microsoft (Id 1, country USA)
            // 4. Kate (age 0) works in Sony (Id 3, country Japan)
            // 5. Oleg (age 0) works in Google (Id 2, country USA)
            // 6. Angela (age 0) works in Google (Id 2, country USA)

            /*
            // For multiple levels hierarchy:
            List<User> users = db.Users
                        .Include(u => u.Company)  // добавляем данные по компаниям
                            .ThenInclude(comp => comp!.Country)      // к компании добавляем страну 
                                .ThenInclude(count => count!.Capital)    // к стране добавляем столицу
                        // .Include(u => u.Company!.Country!.Capital)  // this is the same
                        .Include(u => u.Position) // добавляем данные по должностям
                        .ToList();
            // SQL is:
            // SELECT u.Id, u.CompanyId, u.Name, u.PositionId, c.Id, c.CountryId, c.Name, c0.Id, c0.CapitalId, c0.Name, c1.Id, c1.Name, p.Id, p.Name
            // FROM Users AS u
            // LEFT JOIN Companies AS c ON u.CompanyId == c.Id
            // LEFT JOIN Countries AS c0 ON c.CountryId == c0.Id
            // LEFT JOIN Cities AS c1 ON c0.CapitalId == c1.Id
            // LEFT JOIN Positions AS p ON u.PositionId == p.Id)
            */
        }

    // 4. Explicit loading - явная загрузка данных с помощью метода Load()
    if (method == 4) using (ApplicationContext db = new ApplicationContext())
        {
            // пересоздадим базу данных
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            // добавляем начальные данные
            Country usa = new Country { Name = "USA" }; // Empty Country constructor has been called (1x)
            db.Countries.Add(usa);

            // добавляем начальные данные
            Company microsoft = new Company { Name = "Microsoft", Country = usa }; // Empty Company constructor has been called (2x)
            Company google = new Company { Name = "Google", Country = usa };
            db.Companies.AddRange(microsoft, google);

            User tom = new User { Name = "Tom", Company = microsoft }; // Empty User constructor has been called (4x)
            User bob = new User { Name = "Bob", Company = google };
            User alice = new User { Name = "Alice", Company = microsoft };
            User kate = new User { Name = "Kate", Company = google };
            db.Users.AddRange(tom, bob, alice, kate);

            db.SaveChanges();
        }
    if (method == 4) using (ApplicationContext db = new ApplicationContext())
        {   
            Company? company = db.Companies.FirstOrDefault();
            if (company != null)
            {
                db.Users
                    .Where(u => u.CompanyId == company.Id) // загружаются только те пользователи, у которых CompanyId соответствует Id ранее полученной компании.
                                                           // После этого нам не надо подгружать связанные данные, так как они уже есть в контексте.
                    .Load(); // загружает пользователей в контекст

                Console.WriteLine($"Company: {company.Name}");
                foreach (User u in company.Users) u.Print();
                // Empty Company constructor has been called
                // Empty User constructor has been called (2x)
                // Company: Microsoft
                // 1. Tom (age 0) works in Microsoft (Id 1, country ) // we do not load countries here
                // 3. Alice (age 0) works in Microsoft (Id 1, country )

                // Для загрузки связанных данных мы также можем использовать методы Collection() и Reference.
                // Метод Collection применяется, если навигационное свойство представляет коллекцию:
                db.Entry(company).Collection(c => c.Users).Load();
                // SELECT "u"."Id", "u"."CompanyId", "u"."Name"
                // FROM "Users" AS "u"
                // WHERE "u"."CompanyId" = @__p_0   // @__p_0 - в данном случае параметр, который представляет id компании и который автоматически передается инфаструктурой EF Core.

                Console.WriteLine($"Company: {company.Name}");
                foreach (User u in company.Users) u.Print();
                // (constructors do not called already)
                // Company: Microsoft
                // 1. Tom (age 0) works in Microsoft (Id 1, country )
                // 3. Alice (age 0) works in Microsoft (Id 1, country )
            }
            
            // Если навигационное свойство представляет одиночный объект, то можно применять метод Reference:
            User? user = db.Users.FirstOrDefault();  // получаем первого пользователя
            if (user != null)
            {
                db.Entry(user).Reference(u => u.Company).Load();  // load Company for this user
                // SELECT "c"."Id", "c"."Name"
                // FROM "Companies" AS "c"
                // WHERE "c"."Id" = @__p_0    // @__p_0 - в данном случае это автоматически передаваемый параметр, который представляет свойство CompanyId выбранного пользователя.

                db.Entry(user.Company).Reference(c => c.Country).Load(); // load also Country for this company
                // SELECT "c"."Id", "c"."Name"
                // FROM "Countries" AS "c"
                // WHERE "c"."Id" = @__p_0    // @__p_0 - в данном случае это свойство CountryId для текущей user.Company.

                user.Print();
                // without Country reference:
                // (constructors do not called already; if this block is the only here than User & Company are created once)
                // 1. Tom (age 0) works in Microsoft (Id 1, country )
                // with Country reference:
                // Empty Country constructor has been called
                // 1. Tom (age 0) works in Microsoft (Id 1, country USA)
            }
        }

    // Lazy loading - ленивая загрузка, предполагает неявную автоматическую загрузку связанных данных при обращении к навигационному свойству. Однако здесь есть ряд условий:
    // install and use Microsoft.EntityFrameworkCore.Proxies
    // При конфигурации контекста данных вызвать метод UseLazyLoadingProxies();
    // Все навигационные свойства должны быть определены как виртуальные (то есть с модификатором virtual), при этом сами классы моделей должны быть открыты для наследования
    using (ApplicationContext db = new ApplicationContext())
    {
        // пересоздадим базу данных
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        // добавляем начальные данные
        Company microsoft = new Company { Name = "Microsoft" };
        Company google = new Company { Name = "Google" };
        db.Companies.AddRange(microsoft, google);


        User tom = new User { Name = "Tom", Company = microsoft };
        User bob = new User { Name = "Bob", Company = google };
        User alice = new User { Name = "Alice", Company = microsoft };
        User kate = new User { Name = "Kate", Company = google };
        db.Users.AddRange(tom, bob, alice, kate);

        db.SaveChanges();
    }
    using (ApplicationContext db = new ApplicationContext())
    {
        var users = db.Users.ToList();
        foreach (User user in users) user.Print();
    }
    // получение всех пользователей
    //   SELECT "u"."Id", "u"."CompanyId", "u"."Name"
    // FROM "Users" AS "u"
    // идет обращение к свойству Company, его компании нет в контексте, поэтому выполняется sql-запрос
    //   SELECT "c"."Id", "c"."Name"
    //   FROM "Companies" AS "c"
    //   WHERE "c"."Id" = @__p_0
    // output: Tom - Microsoft
    // компания этого пользователя уже в контексте, не надо выполнять новый запрос
    // output: Alice - Microsoft
    // компании следующего пользователя нет в контексте, поэтому выполняется sql-запрос
    //   SELECT "c"."Id", "c"."Name"
    //   FROM "Companies" AS "c"
    //   WHERE "c"."Id" = @__p_0
    // output: Bob - Google
    // компания этого пользователя уже в контексте, не надо выполнять новый запрос
    // output: Kate - Google

    // Таким же образом можно загрузить компании и связанных с ними пользователей:
    using (ApplicationContext db = new ApplicationContext())
{
        var companies = db.Companies.ToList();
        foreach (Company company in companies)
        {
            Console.Write($"{company.Name}:");
            foreach (User user in company.Users) user.Print();
            Console.WriteLine();
        }
    }
}

// =============
// Study Classes
class JsonSettings
{
    public string? DefaultConnection { get; set; }
}

// [Table("People")] // if the table name is different
// [Index("PhoneNumber", "Passport")] // if we want to create the index for these external keys (properties)
// [Index("PhoneNumber", IsUnique = true, Name = "Phone_Index")] // additional parameters for index: index name and its uniqueness
// [EntityTypeConfiguration(typeof(UserConfiguration))]
public class User
{
    // int id; // this private field will be set from DB
    // string name;
    // int age; // this private field will be set from DB

    // [Column("user_id")] // аннотация, если имя столбца отличается
    // [DatabaseGenerated(DatabaseGeneratedOption.None)] // отключить автогенерацию значения при добавлении (будет добавляться 0, поэтому надо устанавливать самому)
    // [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // чтобы база данных сама генерировала значение (это по умолчанию)
    // [DatabaseGenerated(DatabaseGeneratedOption.Computed)] // база генерирует значение и при добавлении, и при обновлении данных
    public int Id { get; set; } // => id; // { get => id } // it may be normal property with normal get & set, not automatic

    // [Required] // say that NULL is NOT allowed here. If not set -> Microsoft.EntityFrameworkCore.DbUpdateException
    // [StringLength(40)] // say that this is the maximum length (> 40 will be truncated)
    // public string ProductName { get; set; }

    // [Column("UnitPrice", TypeName = "money")] // if the title and type of the column is different
    public string? Name { get; set; } // { get => name; set => name = value; }

    // [Column(TypeName = "money")] // if there is no required DBtype, for example, decimal
    public int Age { get; set; } // => age;

    // public string? Position { get; set; }   // Новое свойство - должность пользователя

    // [Required] // in this case CompanyId can be absent (the foreign key is also created if Company is NOT nullable)
    // навигационное свойство (navigation property - представляет другую сущность и имеет собственный конструктор). EF Core does not set it automatically
    // [NotMapped] // we can say here that this field should not be created. Other way: "Ignore" in OnModelCreating
    // [ForeignKey("CompanyId")] // Чтобы установить другое свойство (не с именем по умолчанию) в качестве внешнего ключа для этого навигационного свойства
    public Company? Company { get; set; } // if it's absent here but Users is in Company class, it will create CompanyId field anyway
                                          // should be virtual for lazy loading
    // public string? CompanyName { get; set; }
    
    public int? CompanyId { get; set; } // внешний ключ (if is absent here, it will be created anyway in DB but it's better to create it for reference)
    // По умолчанию название внешнего ключа должно принимать одно из следующих вариантов имени:
    // Имя_навигационного_свойства + Имя ключа из связанной сущности - Company + ключ из модели Company Id = CompanyId
    // Имя_класса_связанной_сущности + Имя ключа из связанной сущности - Company + ключа из модели Company Id = CompanyId
    // CONSTRAINT "FK_Users_Companies_CompanyId" FOREIGN KEY("CompanyId") REFERENCES "Companies"("Id") ON DELETE CASCADE,
    // CONSTRAINT "PK_Users" PRIMARY KEY("Id" AUTOINCREMENT)

    // public DateTime CreatedAt { get; set; } = DateTime.Now;

    // Entity constructor: called e.g. when reading from DB. The values are set by EF Core directly (ctor without parameters) or through this ctor.
    public User(string name, int age) // names should be the same but can have different register. Ctors can be private
    {
        SetName(name);
        SetAge(age);
        // Id is set directly because it is not mentioned here
        Console.WriteLine($"User constructor for {name} ({age}) has been called");
    }
    public User(string name)
    {
        SetName(name);
        SetAge(18); // without this, Age is set to Default (0)
        Console.WriteLine($"User constructor for {name} has been called");
    }
    public User() // this ctor does nothing but is required for entity creating in case of direct setting of the property values
    { Console.WriteLine("Empty User constructor has been called"); }

    // public void Print() => Console.WriteLine($"{id}. {name} - {age}");
    // public void Print() => Console.WriteLine($"{Id}. {Name} (age {Age}) works in {Company?.Name} (Id {CompanyId})");
    public void Print() => Console.WriteLine($"{Id}. {Name} (age {Age}) works in {Company?.Name} (Id {CompanyId}, country {Company?.Country?.Name})");

    public void SetName(string name) => this.Name = name;
    public void SetAge(int age) => this.Age = age;
}

// сущность Company является главной сущностью, а класс User - зависимой, так как содержит ссылку на класс Company и зависит от этого класса.
// [NotMapped] // in this case, if the DB Company is not created in OnModelCreating, the Company field in Users is also not created
// [EntityTypeConfiguration(typeof(CompanyConfiguration))]
public class Company
{
    public int Id { get; set; } // CONSTRAINT "PK_Companies" PRIMARY KEY("Id" AUTOINCREMENT)
    public string? Name { get; set; }
    public List<User> Users { get; set; } = new(); // should be virtual for lazy loading
    public int CountryId { get; set; } // if it's not here, a runtime exception is thrown
    public Country? Country { get; set; }
    
    public Company() { Console.WriteLine("Empty Company constructor has been called"); }
    public Company(string name) // Constructor for Name in entities
    {
        Name = name;
        Console.WriteLine($"Company constructor for {name} has been called");
    }
}
public class Country
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public List<Company> Companies { get; set; } = new();

    public Country() { Console.WriteLine("Empty Country constructor has been called"); }
}

// A DbContext instance represents a session with the database and can be used to query and
// save instances of your entities. DbContext is a combination of the Unit Of Work and Repository patterns.
// (Comment: this means that all repositories (connections with operations) for the same database work together,
// and all the changes will be saved together. If some operation for one table fails, then all changes are
// rolled back for all tables. This ensures the database consistency.)
public class ApplicationContext : DbContext
{
    readonly StreamWriter logStream = new StreamWriter("../../../LogFile.txt", false); // append mode = false

    // Set = DbContext.Set (creates DbSet<T>) - this method setups the TEntity type (User here) for the repository pattern
    public DbSet<User> Users => Set<User>();
    // the same is:
    // public DbSet<User> Users { get => Set<User>(); }

    // alternative: public DbSet<User> Users { get; set; } = null!; // this just suppresses the compiler's warning
    // null! говорит, что данное свойство в принципе не будет иметь значение null (а это так, потому что конструктор базового класса
    // DbContext гарантирует, что все свойства типа DbSet будут инициализированы и соответственно в принципе не будут иметь значение null).

    // ! is the null-forgiving operator, telling the compiler that, even though it normally wouldn't allow it,
    // it should look the other way and allow it anyway, because we know better. null! itself has little practical use,
    // as it all but negates the usefulness of nullable reference types. It's more useful when you know an expression
    // can't be null, but the compiler doesn't. See https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/null-forgiving
    public DbSet<Company> Companies { get; set; } = null!;
    public DbSet<Country> Countries { get; set; } = null!;

    public string connectionString = "Data Source=helloapp.db"; // for SQLite: Data Source определяет файл базы данных - в данном случае "helloapp.db".
                                                                // for SQL Server: @"Server=(localdb)\mssqllocaldb;Database=helloappdb;Trusted_Connection=True;"

    // По умолчанию у нас нет базы данных. Поэтому в конструкторе класса контекста определен вызов метода Database.EnsureCreated(),
    // который при создании контекста автоматически проверит наличие базы данных и, если она отсуствует, создаст ее.
    // Если база данных имеется, но она НЕ имеет таблиц, то этот метод создает таблицы, которые соответствуют схеме данных.
    // Database = DatabaseFacade - Provides access to database related information and operations for this context.
    public ApplicationContext()  // => Database.EnsureCreated(); // асинхронная версия: Database.EnsureCreatedAsync()
    {
        /*
        bool isAvalaible = Database.CanConnect();
        // bool isAvalaible2 = await db.Database.CanConnectAsync();
        if (isAvalaible) Console.WriteLine("База данных доступна");
        else Console.WriteLine("База данных не доступна");

        bool isCreated = Database.EnsureCreated();
        // bool isCreated2 = await Database.EnsureCreatedAsync();
        if (isCreated) Console.WriteLine("База данных была создана");
        else Console.WriteLine("База данных уже существует");
        */
        // Database.EnsureDeleted();
        Database.EnsureCreated();
        // Для Country нет свойства DbSet в классе контекста, поэтому она не будет включена в контекст, и для нее не будет создаваться таблица в БД.
    }

    public ApplicationContext(string connectionString)
    {
        this.connectionString = connectionString;   // получаем извне строку подключения
        Database.EnsureCreated();
    }

    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
    {
        Database.EnsureCreated();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // настроить строку подключения для соединения с базой данных SQLite
        optionsBuilder
            // .UseLazyLoadingProxies()        // подключение lazy loading
            .UseSqlite(connectionString);
        // for SQL Server: optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=adonetdb;Trusted_Connection=True;");
        // for MySql: optionsBuilder.UseMySql("server=localhost;user=root;password=123456789;database=usersdb;", new MySqlServerVersion(new Version(8, 0, 25)));
        // for PostgreSQL: optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=usersdb;Username=postgres;Password=пароль_от_postgres");

        optionsBuilder.EnableSensitiveDataLogging(); // опция логирования с key values
                                                     // включить логирование:
                                                     // вывод на экран
                                                     //      уровень: Trace (наиболее детализированный, только для отладки), Debug (умолч.), Information,
                                                     //      Warning (неожиданные события и ошибки), Error (ошибки и исключения, которые не могут быть обработаны),
                                                     //      Critical (критические ошибки), None (вывод информации в лог не применяется)
                                                     // optionsBuilder.LogTo(Console.WriteLine, LogLevel.Trace); // the first parameter is Action<string>

        // фильтрация: вывод только данного типа сообщений (конкретизация по идентификатору события)
        // optionsBuilder.LogTo(Console.WriteLine, new[] { RelationalEventId.CommandExecuted });

        // фильтрация: вывод только данной категории сообщений:
        //      Database.Command: категория для выполняемых команд, позволяет получить выполняемый код SQL
        //      Database.Connection : категория для операций подключения к БД
        //      Database.Transaction : категория для транзакций с БД
        //      Migration: категория для миграций
        //      Model: категория для действий, совершаемых при привязке модели
        //      Query: категория для запросов за исключением тех, что генерируют исполняемый код SQL
        //      Scaffolding: категория для действий, выполняемых в поцессе обратного инжиниринга (то есть когда по базе данных генерируются классы и класс контекста)
        //      Update: категория для сообщений вызова DbContext.SaveChanges()
        //      Infrastructure: категория для всех остальных сообщений
        // optionsBuilder.LogTo(Console.WriteLine, new[] { DbLoggerCategory.Database.Command.Name }); // Update.Name });

        // Вывод на вкладку "Вывод" (Debug): only when running in Debug mode
        // optionsBuilder.LogTo(message => System.Diagnostics.Debug.WriteLine(message));

        // Вывод лога в файл:
        optionsBuilder.LogTo(logStream.WriteLine);
    }

    // использование Fluent API
    protected override void OnModelCreating(ModelBuilder modelBuilder) // ModelBuilder implements Decorator pattern
    {
        base.OnModelCreating(modelBuilder); // if no other calls here

        // setup for external keys
        // Для настройки отношений между моделями с помощью Fluent API применяются специальные методы: HasOne / HasMany / WithOne / WithMany.
        // Методы HasOne и HasMany устанавливают навигационное свойство для сущности, для которой производится конфигурация.
        // Далее могут идти вызовы методов WithOne и WithMany, который идентифицируют навигационное свойство на стороне связанной сущности.
        // Методы HasOne/WithOne применяются для обычного навигационного свойства, представляющего одиночный объект,
        // а методы HasMany/WithMany используются для навигационных свойств, представляющих коллекции.
        // Сам же внешний ключ устанавливается с помощью метода HasForeignKey
        // -> CREATE TABLE "People" (
        //      "user_id" INTEGER NOT NULL CONSTRAINT "PK_People" PRIMARY KEY AUTOINCREMENT,
        //      "Name" TEXT NOT NULL,
        //      "Age" INTEGER NOT NULL,
        //      "CompanyId" INTEGER NOT NULL,
        //      CONSTRAINT "FK_People_Enterprises_CompanyId" FOREIGN KEY("CompanyId") REFERENCES "Enterprises"("Id") ON DELETE CASCADE
        //    CREATE TABLE "Enterprises" (
        //      "Id" INTEGER NOT NULL CONSTRAINT "PK_Enterprises" PRIMARY KEY AUTOINCREMENT,
        //      "Name" TEXT NOT NULL
        // Каскадное удаление представляет автоматическое удаление зависимой сущности после удаления главной

        // modelBuilder.Entity<User>()
        //    .HasOne(u => u.Company)
        //    .WithMany(c => c.Users)
        //    .OnDelete(DeleteBehavior.Cascade); // Cascade: зависимая сущность удаляется вместе с главной
        // SetNull: свойство-внешний ключ в зависимой сущности получает значение null - каскадное удаление отключено
        // Restrict: зависимая сущность никак не изменяется при удалении главной сущности

        // Метод HasPrincipalKey указывает на свойство связанной сущности, на которую будет ссылаться свойство-внешний ключ CompanyName.
        // Кроме того, для свойства, указанного в HasPrincipalKey(), будет создаваться альтернативный ключ.
        // -> CONSTRAINT "PK_Users" PRIMARY KEY("Id" AUTOINCREMENT),
        //    CONSTRAINT "FK_Users_Companies_CompanyName" FOREIGN KEY("CompanyName") REFERENCES "Companies"("Name")

        // modelBuilder.Entity<User>().HasOne(u => u.Company).WithMany(c => c.Users).HasForeignKey(u => u.CompanyId);
        // .HasForeignKey(u => u.CompanyName)
        // .HasPrincipalKey(c => c.Name);

        // Настройка отношения "один к одному"
        // modelBuilder
        //    .Entity<User>()
        //    .HasOne(u => u.Profile)
        //    .WithOne(p => p.User)
        //    .HasForeignKey<UserProfile>(p => p.UserKey);

        // 1:1 в одной таблице
        // modelBuilder.Entity<User>()
        //    .HasOne(u => u.Profile).WithOne(p => p.User)
        //    .HasForeignKey<UserProfile>(up => up.Id);
        // modelBuilder.Entity<User>().ToTable("Users");
        // modelBuilder.Entity<UserProfile>().ToTable("Users");
        // Однако несмотря на то, что данные хранятся в одной таблице, мы по прежнему с ними можем работать по отдельности через db.UserProfiles и db.Users.

        // Настройка отношения "многие ко многим"
        // modelBuilder.Entity<Course>()
        //        .HasMany(c => c.Students)
        //        .WithMany(s => s.Courses)
        //        .UsingEntity(j => j.ToTable("Enrollments")); // промежуточная таблица со связями между двумя таблицами

        /*  Самостоятельное создание промежуточной таблицы с дополнительными полями
        modelBuilder
            .Entity<Course>()
            .HasMany(c => c.Students)
            .WithMany(s => s.Courses)
            .UsingEntity<Enrollment>(
               j => j
                .HasOne(pt => pt.Student)
                .WithMany(t => t.Enrollments)
                .HasForeignKey(pt => pt.StudentId), // связь с таблицей Students через StudentId
            j => j
                .HasOne(pt => pt.Course)
                .WithMany(p => p.Enrollments)
                .HasForeignKey(pt => pt.CourseId),  // связь с таблицей Courses через CourseId
            j =>
            {   // настраиваем свойства сущности Enrollment, а также имя соответствующей таблицы и ее ключи
                j.Property(pt => pt.Mark).HasDefaultValue(3);
                j.HasKey(t => new { t.CourseId, t.StudentId });
                j.ToTable("Enrollments");
            });

            ==> tom.Enrollments.Add(new Enrollment { Course = algorithms, EnrollmentDate = DateTime.Now });
                tom.Courses.Add(basics);
                var courses = db.Courses.Include(c => c.Students).ToList();
        */

        // Комплексные типы
        // Атрибут класса [Owned] позволяет установить зависимый тип (он не должен содержать ключа; для него не создается отдельная таблица)
        // the same is:
        // modelBuilder.Entity<User>().OwnsOne(u => u.Profile); // указывается навигационное свойство, которое представляет зависимый тип
        // modelBuilder.Entity<User>().OwnsOne(typeof(UserProfile), "Profile"); // если зависимое свойство - приватное
        // modelBuilder.Entity<User>().OwnsOne(u => u.Profile, p => {  // если зависимый тип содержит собственные типы
        //    p.OwnsOne(c => c.Name);
        //    p.OwnsOne(c => c.Age); });

        // modelBuilder.Entity<User>().ToTable("People").Property(p => p.Name).IsRequired();
        // modelBuilder.Entity<User>().Property(p => p.Id).HasColumnName("user_id"); // "user_id" INTEGER NOT NULL

        // modelBuilder.Entity<Company>().ToTable("Enterprises").Property(c => c.Name).IsRequired(); // "Name" TEXT NOT NULL

        // this is the same as the following:
        // modelBuilder.ApplyConfiguration(new UserConfiguration()); // or attribute [EntityTypeConfiguration(typeof(UserConfiguration))] instead
        // modelBuilder.ApplyConfiguration(new CompanyConfiguration());  // or attribute [EntityTypeConfiguration(typeof(CompanyConfiguration))] instead

        // the same is:
        // modelBuilder.Entity<User>(UserConfigure);
        // modelBuilder.Entity<Company>(CompanyConfigure);

        // Инициализация БД начальными данными: this method is called during migration and in Database.EnsureCreated method when creating the DB
        // modelBuilder.Entity<User>().HasData(
        // new User { Id = 1, Name = "Tom", Age = 23 }, // Id field value is mandatory
        // new User { Id = 2, Name = "Alice", Age = 26 },
        // new User { Id = 3, Name = "Sam", Age = 28 }
        // );

        // определяем компании
        // Company microsoft = new Company { Id = 1, Name = "Microsoft" };
        // Company google = new Company { Id = 2, Name = "Google" };

        // определяем пользователей
        // User tom = new User { Id = 1, Name = "Tom", Age = 23, CompanyId = microsoft.Id }; 
        // User alice = new User { Id = 2, Name = "Alice", Age = 26, CompanyId = microsoft.Id };
        // User sam = new User { Id = 3, Name = "Sam", Age = 28, CompanyId = google.Id };
        // , Company = google }; --> System.InvalidOperationException: The seed entity for entity type 'User' with the key value 'Id:1'
        // cannot be added because it has the navigation 'Company' set. To seed relationships, add the entity seed to 'User' and
        // specify the foreign key values {'CompanyId'}.

        // добавляем данные для обеих сущностей
        // modelBuilder.Entity<Company>().HasData(microsoft, google);
        // modelBuilder.Entity<User>().HasData(tom, alice, sam);

        // modelBuilder.Entity<Country>(); // in this case, creating and performing migrations will create also the Country table
        // modelBuilder.Ignore<Company>(); // in this case, the Company table is not created
        // modelBuilder.Entity<User>().Ignore(u => u.Address); // if there is a property Address in User, we can say not to create it in DB

        // вместо атрибутов для полей класса можно использовать следующий оператор Fluent API в методе OnModelBuilding класса контекста:
        // modelBuilder.Entity<User>().ToTable("People"); // if the table of Users has the name People
        // modelBuilder.Entity<User>().ToTable("People", schema: "userstore"); // additionally we can define its schema

        // If Id property not included: System.InvalidOperationException: The entity type 'User' requires a primary key to be defined. If you intended to use a keyless entity type, call 'HasNoKey' in 'OnModelCreating'. For more information on keyless entity types, see https://go.microsoft.com/fwlink/?linkid=2141943.
        // modelBuilder.Entity<User>().Property("Id").HasField("id");   // Say that "id" is not needed, "Id" is instead of it // set User.id from Id field of DB
        // modelBuilder.Entity<User>().Property("Age").HasField("age"); // Age instead of age // set User.age from Age field of DB
        // modelBuilder.Entity<User>().Property("Name");                // set User.name from name field of DB (but the constructor is used)

        // say that the column user_id of DB has the property name Id in the Class
        // modelBuilder.Entity<User>().Property(u => u.Id).HasColumnName("user_id"); // the same is: [Column("user_id")] for Id property

        // modelBuilder.Entity<User>()
        //    .Property(user => user.ProductName)
        //    .IsRequired()      // instead of [Required] // we can show that this NOT NULL here
        //    .HasMaxLength(40); // instead of [StringLength(40)] or [MaxLength(40)] // we can say here that this is maximum length
        // Для SQLite MaxLength не имеет никакого значения
        // [MinLength] устанавливает минимальную длину, но на определение таблицы не влияет.

        // modelBuilder.Entity<User>().Property(b => b.Id).ValueGeneratedNever(); // Отключение автогенерации значения для свойства
        // other methods: ValueGeneratedOnAdd, ValueGeneratedOnUpdate, ValueGeneratedOnAddOrUpdate, ValueGeneratedOnUpdateSometimes

        // modelBuilder.Entity<User>().Property(u => u.Name).HasDefaultValue("Default"); // set the Default instead of NULL ("")
        // modelBuilder.Entity<User>().Property(u => u.Age).HasDefaultValue(18); // set the default instead of 0 // INTEGER NOT NULL DEFAULT 18,

        // modelBuilder.Entity<User>().Property(u => u.CreatedAt).HasDefaultValueSql("DATETIME('now')"); // SQL query for DEFAULT --> 2022-08-29 15:20:48
        // in case of setting = DateTime.Now for the property we get: 2022-08-29 17:23:11.3010678

        // modelBuilder.Entity<User>().HasKey(u => u.Ident); // the same is: [Key] for Ident property // if the key property is not "Id" or "(ClassName)Id"
        // modelBuilder.Entity<User>().HasKey(u => u.Ident).HasName("UsersPrimaryKey"); // if it also has different name in DB
        // modelBuilder.Entity<User>().HasKey(u => new { u.PassportSeria, u.PassportNumber} ); // complex key: Fluent API only
        // the table will have: CONSTRAINT "PK_Users" PRIMARY KEY("PassportSeria","PassportNumber")

        // Альтернативные ключи представляют свойства, которые также, как и первичный ключ, должны иметь уникальное значение.
        // В то же время альтернативные ключи не являются первичными. На уровне базы данных это выражается в установке для
        // соответствующих столбцов ограничения на уникальность.
        // modelBuilder.Entity<User>().HasAlternateKey(u => u.Passport);
        // CONSTRAINT "AK_Users_Passport" UNIQUE("Passport"),
        // CONSTRAINT "PK_Users" PRIMARY KEY("Id" AUTOINCREMENT)

        // Альтернативные ключи также могут быть составными:
        // modelBuilder.Entity<User>().HasAlternateKey(u => new { u.Passport, u.PhoneNumber });
        // CONSTRAINT "AK_Users_Passport_PhoneNumber" UNIQUE("Passport","PhoneNumber"),
        // CONSTRAINT "PK_Users" PRIMARY KEY("Id" AUTOINCREMENT)

        // Настройка индексов
        // Для увеличения производительности поиска в базе данных применяются индексы.
        // По умолчанию индекс создается для каждого свойства, которое используется в качестве внешнего ключа.
        // Однако Entity Framework также позволяет создавать свои индексы.

        // [Index("PhoneNumber", "Passport")] // if we want to create the index for these external keys (properties)
        // [Index("PhoneNumber", IsUnique = true, Name = "Phone_Index")] // additional parameters for index: index name and its uniqueness
        // modelBuilder.Entity<User>().HasIndex(u => u.Passport); // = [Index("PhoneNumber"]

        // Если индекс должен иметь уникальное значение: гарантируем, что в БД может быть только 1 объект с определенным значением для свойства-индекса
        // modelBuilder.Entity<User>().HasIndex(u => u.Passport).IsUnique(); // [Index("PhoneNumber", IsUnique = true]
        // modelBuilder.Entity<User>().HasIndex(u => new { u.Passport, u.PhoneNumber }); // [Index("Passport", "PhoneNumber")]
        // modelBuilder.Entity<User>().HasIndex(u => u.PhoneNumber).HasDatabaseName("PhoneIndex"); // [Index("PhoneNumber", Name = "PhoneIndex")]

        // Фильтры индексов - индексация только по ограниченному набору значений, что увеличивает производительность и уменьшает использование дискового простанства
        // modelBuilder.Entity<User>().HasIndex(u => u.PhoneNumber).HasFilter("[PhoneNumber] IS NOT NULL");

        // Вычисляемые столбцы
        // modelBuilder.Entity<User>().Property(u => u.Name).HasComputedColumnSql("'Person #' || Id || ' (' || Age || ' years)'"); // 1. Person #1 (18 years) - 18

        // установка ограничений
        // проверка значения перед добавлением в БД. Добавляется только, если удовлетворяет
        // modelBuilder.Entity<User>().HasCheckConstraint("Age", "Age > 0 AND Age < 12"); // DB: CONSTRAINT "CK_Users_Age" CHECK("Age" > 0 AND "Age" < 12)
        // modelBuilder.Entity<User>().HasCheckConstraint("Age", "Age > 0 AND Age < 120", c => c.HasName("CK_User_Age")); // задать имя ограничения
        // если не удовлетворяет, то: Microsoft.EntityFrameworkCore.DbUpdateException: An error occurred while saving the entity changes.
        // See the inner exception for details. ---> Microsoft.Data.Sqlite.SqliteException(0x80004005): SQLite Error 19: 'CHECK constraint failed: Age'.
    }

    // конфигурация для типа User
    public void UserConfigure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("People").Property(p => p.Name).IsRequired();
        builder.Property(p => p.Id).HasColumnName("user_id");
    }
    // конфигурация для типа Company
    public void CompanyConfigure(EntityTypeBuilder<Company> builder)
    {
        builder.ToTable("Enterprises").Property(c => c.Name).IsRequired();
    }
    public override void Dispose()
    {
        base.Dispose();
        logStream.Dispose();
    }

    public override async ValueTask DisposeAsync()
    {
        await base.DisposeAsync();
        await logStream.DisposeAsync();
    }
}

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        // corresponds to: ModelBuilder.Entity<User>().ToTable("People").Property(p => p.Name).IsRequired();
        builder.ToTable("People").Property(p => p.Name).IsRequired();
        // corresponds to: ModelBuilder.Entity<User>().Property(p => p.Id).HasColumnName("user_id"); // "user_id" INTEGER NOT NULL
        builder.Property(p => p.Id).HasColumnName("user_id");
    }
}
public class CompanyConfiguration : IEntityTypeConfiguration<Company>
{
    public void Configure(EntityTypeBuilder<Company> builder)
    {
        // corresponds to: ModelBuilder.Entity<Company>().ToTable("Enterprises").Property(c => c.Name).IsRequired(); // "Name" TEXT NOT NULL
        builder.ToTable("Enterprises").Property(c => c.Name).IsRequired();
    }
}

// this code was created with NuGet Package Manager Console command:
// Scaffold-DbContext "Data Source=helloapp.db" Microsoft.EntityFrameworkCore.Sqlite
// this line was added then: public virtual DbSet<User> Users { get; set; } = null!;
public partial class helloappContext : DbContext
{
    public helloappContext()
    {      // auto-created code is empty { }
           // при изменении схемы БД (класса User) или желании очистки БД перед использованием:
           // Удаляем бд со старой схемой (данные теряются!)
        Database.EnsureDeleted();
        // Если это нежелательно, то можно отредактировать базу в редакторе ИЛИ использовать миграцию:
        // в консоли NuGet: Add-Migration название_миграции, выполнить с помощью Update-Database
        // Add-Migration InitialCreate -Context helloappContext -> see Migration folder for the files created
        // To undo this action, use Remove-Migration.
        // start migration: Update-Database -Context helloappContext
        // (this constructor should be empty to use migration!)

        // Открываем или создаем БД с новой схемой - Это нужно всегда, если только не вызывается отдельно
        Database.EnsureCreated();
    }
    public helloappContext(DbContextOptions<helloappContext> options) : base(options) { }
    public virtual DbSet<User> Users { get; set; } = null!; // this line was added to auto-created code
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            // #warning To protect potentially sensitive information in your connection string, you should move it out of source code.
            // You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration -
            // see https://go.microsoft.com/fwlink/?linkid=2131148.
            // For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
            optionsBuilder.UseSqlite("Data Source=helloapp.db");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

#region NullNameShouldThrowTest 
// NullNameShouldThrowTest();
/*
static void Display<T>(T a, T backup) // testing null-coalescing operators ?? and ??=
{
    Console.WriteLine(a ?? backup); // if a != null then use a else use backup
                                    // This is equivalent to: a is not null ? a : backup
    // This is also equivalent to:
    a ??= backup; // the null-coalescing assignment operator: if a == null then a = backup else a is not changed.
    Console.WriteLine(a);           // a ??= backup is equivalent to: if (a is null) a = backup;
}

// [TestMethod, ExpectedException(typeof(ArgumentNullException))]
static void NullNameShouldThrowTest()
{
    // if Name is not nullable string:
    User? user1 = new User { Name = null }; // Warning CS8625 Литерал, равный NULL, не может быть преобразован в ссылочный тип, не допускающий значение NULL.
    User? user2 = new User { Name = null! }; // No warning
    Console.WriteLine($"user1: {user1.Name}"); // ""
    Console.WriteLine($"user2: {user2!.Name}"); // ""
    Display<string>(user2.Name, "Not Null"); // Not Null   Not Null
}
*/
#endregion NullNameShouldThrowTest

static class LinqTest
{
    static public void Test()
    {
        IEnumerable<int>
            source = new int[] { 5, 12, 3 },
            filtered = source.Where(n => n < 10),
            sorted = filtered.OrderBy(n => n),
            query1 = sorted.Select(n => n * 10);
        for (int i = 0; i < query1.Count(); i++) Console.Write(query1.ElementAt(i) + " ");
        Console.WriteLine(); // 30 50

        IEnumerable<int> query2 = new int[] { 5, 12, 3 }
            .Where(n => n < 10)
            .OrderBy(n => n)
            .Select(n => n * 10);
        for (int i = 0; i < query2.Count(); i++) Console.Write(query2.ElementAt(i) + " ");
        Console.WriteLine(); // 30 50

        IEnumerable<char> query = "Not what you might expect";
        foreach (char vowel in "aeiou")
        {
            // char temp = vowel;
            query = query.Where(c => c != vowel); // temp);
        }
        for (int i = 0; i < query.Count(); i++) Console.Write(query.ElementAt(i));
        Console.WriteLine(); // Nt wht y mght xpct

        List<int> some = new() { 1, 2, 3, 4, 5 };
        List<int> list = some.SelectMany(u => new List<int>() { u, u * 2, u * 3 }).ToList();
        for (int i = 0; i < list.Count(); i++) Console.Write(list.ElementAt(i) + " "); // 1 2 3 2 4 6 3 6 9 4 8 12 5 10 15
    }
}

// From Mark Price book, for SQLite database Northwind.db

#region Autogenerated classes for Northwind.db
/*
// autogen with: dotnet ef dbcontext scaffold "Filename=Northwind.db" Microsoft.EntityFrameworkCore.Sqlite --table Categories --table Products --namespace EFCore.AutoGen --data-annotations --context Northwind --project EntityFrameworkCOre
[Index("CategoryName", Name = "CategoryName")]
public partial class Category
{
    public Category() { Products = new HashSet<Product>(); }

    [Key]
    [Column("CategoryID")]
    public long CategoryId { get; set; }
    [Column(TypeName = "nvarchar (15)")]
    public string CategoryName { get; set; } = null!;
    [Column(TypeName = "ntext")]
    public string? Description { get; set; }
    [Column(TypeName = "image")]
    public byte[]? Picture { get; set; }

    [InverseProperty("Category")]
    public virtual ICollection<Product> Products { get; set; }
}

[Index("CategoryId", Name = "CategoriesProducts")]
[Index("CategoryId", Name = "CategoryID")]
[Index("ProductName", Name = "ProductName")]
[Index("SupplierId", Name = "SupplierID")]
[Index("SupplierId", Name = "SuppliersProducts")]
public partial class Product
{
    [Key]
    [Column("ProductID")]
    public long ProductId { get; set; }
    [Column(TypeName = "nvarchar (40)")]
    public string ProductName { get; set; } = null!;
    [Column("SupplierID", TypeName = "int")]
    public long? SupplierId { get; set; }
    [Column("CategoryID", TypeName = "int")]
    public long? CategoryId { get; set; }
    [Column(TypeName = "nvarchar (20)")]
    public string? QuantityPerUnit { get; set; }
    [Column(TypeName = "money")]
    public byte[]? UnitPrice { get; set; }
    [Column(TypeName = "smallint")]
    public long? UnitsInStock { get; set; }
    [Column(TypeName = "smallint")]
    public long? UnitsOnOrder { get; set; }
    [Column(TypeName = "smallint")]
    public long? ReorderLevel { get; set; }
    [Column(TypeName = "bit")]
    public byte[] Discontinued { get; set; } = null!;

    [ForeignKey("CategoryId")]
    [InverseProperty("Products")]
    public virtual Category? Category { get; set; }
}

public partial class Northwind : DbContext
{
    public Northwind() { }

    public Northwind(DbContextOptions<Northwind> options) : base(options) { }

    public virtual DbSet<Category> Categories { get; set; } = null!;
    public virtual DbSet<Product> Products { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            // #warning To protect potentially sensitive information in your connection string, you should move it out of source code. 
            // You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration -
            // see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings,
            // see http://go.microsoft.com/fwlink/?LinkId=723263.
            optionsBuilder.UseSqlite("Filename=Northwind.db");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.Property(e => e.CategoryId).ValueGeneratedNever();
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.Property(e => e.ProductId).ValueGeneratedNever();
            entity.Property(e => e.Discontinued).HasDefaultValueSql("0");
            entity.Property(e => e.ReorderLevel).HasDefaultValueSql("0");
            entity.Property(e => e.UnitPrice).HasDefaultValueSql("0");
            entity.Property(e => e.UnitsInStock).HasDefaultValueSql("0");
            entity.Property(e => e.UnitsOnOrder).HasDefaultValueSql("0");
        });
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
*/
#endregion Autogenerated classes for Northwind.db

// classes from Packt.Shared (Fluent API)
public class Category
{
    // эти свойства соотносятся со столбцами в базе данных
    public int CategoryID { get; set; }
    public string CategoryName { get; set; }
    [Column(TypeName = "ntext")]
    public string Description { get; set; }
    // определяет навигационное свойство для связанных строк
    public virtual ICollection<Product> Products { get; set; }
    public Category()
    {
        // чтобы позволить разработчикам добавлять товары в Category,
        // мы должны инициализировать навигационное свойство пустым списком
        this.Products = new HashSet<Product>();
    }
}
public class Product
{
    public int ProductID { get; set; }
    [Required]
    [StringLength(40)]
    public string ProductName { get; set; }
    [Column("UnitPrice", TypeName = "money")]
    public decimal? Cost { get; set; } // property name != field name
    [Column("UnitsInStock")]
    public short? Stock { get; set; }
    public bool Discontinued { get; set; }
    
    // эти две строки определяют связь внешнего ключа и таблицы Categories
    public int CategoryID { get; set; }
    public virtual Category Category { get; set; }
}

// управление соединением с базой данных
public class Northwind : DbContext
{
    // свойства, сопоставляемые с таблицами в базе данных
    public DbSet<Category> Categories { get; set; }
    public DbSet<Product> Products { get; set; }
    protected override void OnConfiguring(
    DbContextOptionsBuilder optionsBuilder)
    {
        string path = System.IO.Path.Combine(
        System.Environment.CurrentDirectory, "Northwind.db");
        optionsBuilder.UseSqlite($"Filename={path}");
    }
    protected override void OnModelCreating(
    ModelBuilder modelBuilder)
    {
        // пример использования Fluent API вместо атрибутов
        // для ограничения длины имени категории до 15 символов
        modelBuilder.Entity<Category>()
            .Property(category => category.CategoryName)
            .IsRequired() // не NULL
            .HasMaxLength(15);
        // добавлено "исправление" отсутствия поддержки
        // десятичных чисел в SQLite
        modelBuilder.Entity<Product>()
            .Property(product => product.Cost)
            .HasConversion<double>();
    }
}