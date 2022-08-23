// Entity Framework Core (databases)
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
// using Microsoft.EntityFrameworkCore.Proxies; // хранит функциональность для так называемой "ленивой загрузки" (lazy-loading) и прокси остлеживания изменений
// using Microsoft.EntityFrameworkCore.Abstractions; // содержит набор абстракций EF Core, которые не зависят от конкретной СУБД
// using Microsoft.EntityFrameworkCore.Relational; // хранит компоненты EF Core для провайдеров реляционных СУБД
// using Microsoft.EntityFrameworkCore.Analyzers; // содержит функционал анализаторов C# для EF Core
using Microsoft.EntityFrameworkCore.Metadata;
using System.Collections.Generic;
using System;
using System.Drawing;

ReadDB();


static void CreateDB() // Creating and filling up a DB
{
    // Так как класс ApplicationContext через базовый класс DbContext реализует интерфейс IDisposable,
    // то для работы с ApplicationContext с автоматическим закрытием данного объекта мы можем использовать конструкцию using.
    using (ApplicationContext db = new())
    {
        // создаем два объекта User
        User tom = new User { Name = "Tom", Age = 33 };
        User alice = new User { Name = "Alice", Age = 26 };

        // добавляем их в БД
        db.Users.Add(tom);
        db.Users.Add(alice);
        // сохранение в БД
        db.SaveChanges();
        Console.WriteLine("Объекты успешно сохранены");

        // проверяем: получаем объекты из БД и выводим на консоль
        var users = db.Users.ToList(); // Users имеет тип DBSet<User>
        Console.WriteLine("Список объектов:");
        foreach (User u in users)
        {
            Console.WriteLine($"{u.Id}. {u.Name} - {u.Age}");
        }
    }
}

static void ReadDB() // Reading an existing DB
{
    using (helloappContext db = new())
    {
        // получаем объекты из БД и выводим на консоль
        var users = db.Users.ToList(); // Users имеет тип DBSet<User>
        Console.WriteLine("Список объектов:");
        foreach (User u in users)
        {
            Console.WriteLine($"{u.Id}. {u.Name} - {u.Age}");
        }
    }
}

#region NullNameShouldThrowTest
// NullNameShouldThrowTest();

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

#endregion NullNameShouldThrowTest

public class User
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public int Age { get; set; }
}


//  A DbContext instance represents a session with the database and can be used to query and save instances of your entities.
//  DbContext is a combination of the Unit Of Work and Repository patterns.
public class ApplicationContext : DbContext
{
    public DbSet<User> Users => Set<User>(); // Set = DbContext.Set (creates DbSet<T>)
    // the same is:
    // public DbSet<User> Users { get => Set<User>(); }
    // alternative: public DbSet<User> Users { get; set; } = null!;
    // ! is the null-forgiving operator, telling the compiler that, even though it normally wouldn't allow it,
    // it should look the other way and allow it anyway, because we know better. null! itself has little practical use,
    // as it all but negates the usefulness of nullable reference types. It's more useful when you know an expression
    // can't be null, but the compiler doesn't. See https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/null-forgiving

    // По умолчанию у нас нет базы данных. Поэтому в конструкторе класса контекста определен вызов метода Database.EnsureCreated(),
    // который при создании контекста автоматически проверит наличие базы данных и, если она отсуствует, создаст ее.
    // Database = DatabaseFacade - Provides access to database related information and operations for this context.
    public ApplicationContext() => Database.EnsureCreated();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // настроить строку подключения для соединения с базой данных SQLite
        // Data Source определяет файл базы данных - в данном случае "helloapp.db".
        optionsBuilder.UseSqlite("Data Source=helloapp.db");
    }
}

// this code was created with NuGet Package Manager Console command:
// Scaffold-DbContext "Data Source=helloapp.db" Microsoft.EntityFrameworkCore.Sqlite
// this line was added then: public virtual DbSet<User> Users { get; set; } = null!;
public partial class helloappContext : DbContext
{
    public helloappContext() { }
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
