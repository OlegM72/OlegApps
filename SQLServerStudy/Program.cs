using Microsoft.Data.SqlClient;
using Microsoft.Data.Sqlite;
using System.Data;
using System;
using System.Threading.Tasks;
 
namespace HelloApp
{
    class Program
    {
        static string connectionString = "Server=(localdb)\\mssqllocaldb;Database=adonetdb;Trusted_Connection=True;";
        static async Task Main(string[] args)
        {
            // SQLServer study - https://metanit.com/sharp/adonetcore/2.1.php
            #region Connection Open/Close, Properties
            /*
            string connectionString = "Server=(localdb)\\mssqllocaldb;Database=master;Trusted_Connection=True;";
            // В данном случае мы будем использовать к локальному серверу. Если мы подключаемся к полноценному серверу MS SQL Server (например, версия Developer Edition), то в качестве адреса сервера, как правило, выступает localhost:
            // string connectionString = "Server=localhost;Database=master;Trusted_Connection=True;";
            // альтернатива: string connectionString = "Server=.;Database=master;Trusted_Connection=True;";

            // Создание подключения
            SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                // Открываем подключение
                await connection.OpenAsync(); // можно использовать синхронный Open();
                Console.WriteLine("Подключение открыто");
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                // если подключение открыто
                if (connection.State == ConnectionState.Open)
                {
                    // закрываем подключение
                    await connection.CloseAsync(); // Close() or Dispose()
                    Console.WriteLine($"\tClientConnectionId: {connection.ClientConnectionId}"); // 41d64fb6-4a71-45b0-9081-07e9c69a3ae7
                    Console.WriteLine("Подключение закрыто...");
                }
            }

            // alternative method:
            using (SqlConnection connection2 = new SqlConnection(connectionString))
            {
                await connection2.OpenAsync();
                Console.WriteLine($"\tClientConnectionId: {connection2.ClientConnectionId}"); // the same because Pooling = true by default
                Console.WriteLine("Подключение открыто");
            }
            Console.WriteLine("Подключение закрыто...");

            // test
            try { throw null; }
            catch (Exception ex) { Console.WriteLine(ex is null ? "null" : ex.Message); } // Object reference not set to an instance of an object.

            // Получение информации о подключении
            // Объект SqlConnection обладает рядом свойств, которые позволяют получить информацию о подключении:

            using (SqlConnection connection3 = new SqlConnection(connectionString))
            {
                await connection3.OpenAsync();
                Console.WriteLine("Подключение открыто");
                // Вывод информации о подключении
                Console.WriteLine("Свойства подключения:");
                Console.WriteLine($"\tСтрока подключения: {connection3.ConnectionString}"); // Server=(localdb)\mssqllocaldb; Database=master; Trusted_Connection=True;
                Console.WriteLine($"\tБаза данных: {connection3.Database}"); // master
                Console.WriteLine($"\tСервер: {connection3.DataSource}"); // (localdb)\mssqllocaldb
                Console.WriteLine($"\tВерсия сервера: {connection3.ServerVersion}"); // 15.00.4153
                Console.WriteLine($"\tСостояние: {connection3.State}"); // Open
                Console.WriteLine($"\tWorkstationld: {connection3.WorkstationId}"); // OLEG
                Console.WriteLine($"\tAccessToken: {connection3.AccessToken}"); // 
                Console.WriteLine($"\tCanCreateBatch: {connection3.CanCreateBatch}"); // False
                Console.WriteLine($"\tClientConnectionId: {connection3.ClientConnectionId}"); // 41d64fb6-4a71-45b0-9081-07e9c69a3ae7
                Console.WriteLine($"\tCommandTimeout: {connection3.CommandTimeout}"); // 30
                Console.WriteLine($"\tConnectionTimeout: {connection3.ConnectionTimeout}"); // 15
                Console.WriteLine($"\tContainer: {connection3.Container}"); // 
                Console.WriteLine($"\tFireInfoMessageEventOnUserErrors: {connection3.FireInfoMessageEventOnUserErrors}"); // False
                Console.WriteLine($"\tPacketSize: {connection3.PacketSize}"); // 8000
                Console.WriteLine($"\tServerProcessId: {connection3.ServerProcessId}"); // 51
                Console.WriteLine($"\tSite: {connection3.Site}"); // 
                Console.WriteLine($"\tStatisticsEnabled: {connection3.StatisticsEnabled}"); // False

                SqlConnection.ClearPool(connection3); // removing the connection from the Pool forces new connection with the same string
            }
            Console.WriteLine("Подключение закрыто...");

            using (SqlConnection connection4 = new SqlConnection(connectionString))
            {
                await connection4.OpenAsync();
                Console.WriteLine("Подключение открыто");
                Console.WriteLine($"\tClientConnectionId: {connection4.ClientConnectionId}"); // different because the connection was removed from Pool
            }
            Console.WriteLine("Подключение закрыто...");
            */
            #endregion Connection Open/Close, Properties

            #region SqlCommand
            /*
            // SqlCommand
            string connectionString = "Server=(localdb)\\mssqllocaldb;Database=adonetdb;Trusted_Connection=True;";
            string sqlExpression = "CREATE DATABASE adonetdb";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();  // открываем подключение
                // SqlCommand command = new SqlCommand(sqlExpression, connection);
                    // the same is:
                    // SqlCommand command = new SqlCommand();
                    // command.CommandText = sqlExpression; // определяем выполняемую команду
                    // command.Connection = connection; // определяем используемое подключение
                // выполняем команду
                // try { await command.ExecuteNonQueryAsync(); Console.WriteLine("База данных создана"); }
                // catch (Exception ex) { Console.WriteLine(ex.Message); }
                // ExecuteNonQuery()/ExecuteNonQueryAsync(): просто выполняет sql-выражение и возвращает количество измененных записей.
                //      Подходит для sql-выражений INSERT, UPDATE, DELETE, CREATE.
                // ExecuteReader()/ExecuteReaderAsync(): выполняет sql-выражение и возвращает строки из таблицы. Подходит для sql-выражения SELECT.
                // ExecuteScalar()/ExecuteScalarAsync(): выполняет sql-выражение и возвращает одно скалярное значение, например, число.
                //      Подходит для sql-выражения SELECT в паре с одной из встроенных функций SQL, как например, Min, Max, Sum, Count.
                SqlCommand command = new("CREATE TABLE Users (Id INT PRIMARY KEY IDENTITY, Age INT NOT NULL, Name NVARCHAR(100) NOT NULL)", connection);
                // Id INT IDENTITY(100,5) PRIMARY KEY --> Id would be int: 100, 105, 110, ... autoincremented
                try { await command.ExecuteNonQueryAsync(); Console.WriteLine("Таблица Users создана"); }
                catch (Exception ex) { Console.WriteLine(ex.Message); }
                command.CommandText = "INSERT INTO Users (Name, Age) VALUES ('Tom', 36)";
                try {
                    int number = await command.ExecuteNonQueryAsync();
                    Console.WriteLine($"Добавлено объектов: {number}");
                }
                catch (Exception ex) { Console.WriteLine(ex.Message); }
                command.CommandText = "INSERT INTO Users (Name, Age) VALUES ('Alice', 32), ('Bob', 28)";
                try
                {
                    int number = await command.ExecuteNonQueryAsync();
                    Console.WriteLine($"Добавлено объектов: {number}");
                }
                catch (Exception ex) { Console.WriteLine(ex.Message); }
                command.CommandText = "UPDATE Users SET Age=20 WHERE Name='Tom'";
                try
                {
                    int number = await command.ExecuteNonQueryAsync();
                    Console.WriteLine($"Обновлено объектов: {number}");
                }
                catch (Exception ex) { Console.WriteLine(ex.Message); }
                command.CommandText = "DELETE FROM Users WHERE Name='Tom'";
                try
                {
                    int number = await command.ExecuteNonQueryAsync();
                    Console.WriteLine($"Удалено объектов: {number}");
                }
                catch (Exception ex) { Console.WriteLine(ex.Message); }
                Console.WriteLine("Введите новый возраст для всех Алис");
                int age = Int32.Parse(Console.ReadLine());
                command.CommandText = $"UPDATE Users SET Age={age} WHERE Name='Alice'";
                try
                {
                    int number = await command.ExecuteNonQueryAsync();
                    Console.WriteLine($"Обновлено объектов: {number}");
                }
                catch (Exception ex) { Console.WriteLine(ex.Message); }
               */
            #endregion SqlCommand

            #region SqlDataReader
            /*
            // получим все данные из таблицы Users, созданной в прошлой теме, и выведем их на консоль:
            string connectionString = "Server=(localdb)\\mssqllocaldb;Database=adonetdb;Trusted_Connection=True;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                SqlCommand command = new SqlCommand("SELECT * FROM Users", connection);
                SqlDataReader reader = await command.ExecuteReaderAsync();
                // Или using (SqlDataReader reader = await command.ExecuteReaderAsync()) { ... }

                if (reader.HasRows) // если есть данные
                {
                    // выводим названия столбцов
                    string[] columns = new string[reader.FieldCount];
                    for (int col = 0; col < reader.FieldCount; col++)
                    {
                        columns[col] = reader.GetName(col);
                        Console.Write(columns[col] + "\t");
                    }
                    Console.WriteLine();

                    // построчно считываем данные, ReadAsync = true, если есть данные (строка считана)
                    while (await reader.ReadAsync()) // or: while (reader.Read())
                    {
                        int id = reader.GetInt32(0); // object id = reader.GetValue(0); // or: reader[0]; // or: reader["id"];
                        int age = reader.GetInt32(1); // object age = reader.GetValue(1); // or: reader["name"];
                        string name = reader.GetString(2); // object name = reader.GetValue(2); // or: reader["age"];
                        Console.WriteLine($"{id}\t{age}\t{name}");
                        // Console.WriteLine($"{reader["id"]}\t{reader["age"]}\t{reader["name"]}");
                        // for (int col = 0; col < reader.FieldCount; col++) {
                        //    Console.Write(reader[columns[col]] + "\t"); }
                        // Console.WriteLine();
                    }
                }
                await reader.CloseAsync();

                command.CommandText = "CREATE TABLE SQLDataReaderMethods (Id INT PRIMARY KEY IDENTITY, SQLType NVARCHAR(20) NOT NULL, NETType NVARCHAR(20) NOT NULL, Method NVARCHAR(20) NOT NULL)";
                try { await command.ExecuteNonQueryAsync(); Console.WriteLine("Таблица SQLDataReaderMethods создана"); }
                catch (Exception ex) { Console.WriteLine(ex.Message); }
                command.CommandText = "INSERT INTO SQLDataReaderMethods (SQLType, NETType, Method) VALUES "+
                       "('bigint', 'Int64', 'GetInt64'), " +
                       "('binary', 'Byte[]', 'GetBytes'), " +
                       "('bit', 'Boolean', 'GetBoolean'), " +
                       "('char', 'String и Char[]', 'GetString и GetChars'), " +
                       "('date', 'DateTime', 'GetDateTime'), " +
                       "('datetime', 'DateTime', 'GetDateTime'), " +
                       "('datetime2', 'DateTime', 'GetDateTime'), " +
                       "('decimal', 'Decimal', 'GetDecimal'), " +
                       "('float', 'Double', 'GetDouble'), " +
                       "('image', 'Byte[]', 'GetBytes и GetStream'), " +
                       "('int', 'Int32', 'GetInt32'), " +
                       "('money', 'Decimal', 'GetDecimal'), " +
                       "('nchar', 'String и Char[]', 'GetString и GetChars'), " +
                       "('ntext', 'String и Char[]', 'GetString и GetChars'), " +
                       "('numeric', 'Decimal', 'GetDecimal'), " +
                       "('nvarchar', 'String и Char[]', 'GetString и GetChars'), " +
                       "('real', 'Single(float)', 'GetFloat'), " +
                       "('rowversion', 'Byte[]', 'GetBytes'), " +
                       "('smalldatetime', 'DateTime', 'GetDateTime'), " +
                       "('smallint', 'Intl6', 'GetInt16'), " +
                       "('smallmoney', 'Decimal', 'GetDecimal'), " +
                       "('sql_variant', 'Object', 'GetValue'), " +
                       "('time', 'TimeSpan', 'GetTimeSpan'), " +
                       "('timestamp', 'Byte[]', 'GetBytes'), " +
                       "('tinyint', 'Byte', 'GetByte'), " +
                       "('uniqueidentifier', 'Guid', 'GetGuid'), " +
                       "('varbinary', 'Byte[]', 'GetBytes'), " +
                       "('varchar', 'String и Char[]', 'GetString и GetChars')";
                try
                {
                    int number = await command.ExecuteNonQueryAsync();
                    Console.WriteLine($"Добавлено объектов: {number}");
                }
                catch (Exception ex) { Console.WriteLine(ex.Message); }
                */

            #endregion SqlDataReader

            #region ExecuteScalar
            /*
            // Получение скалярных значений
            string connectionString = "Server=(localdb)\\mssqllocaldb;Database=adonetdb;Trusted_Connection=True;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                SqlCommand command = new SqlCommand("SELECT COUNT(*) FROM Users", connection);
                object count = await command.ExecuteScalarAsync();

                command.CommandText = "SELECT MIN(Age) FROM Users";
                object minAge = await command.ExecuteScalarAsync();

                command.CommandText = "SELECT AVG(Age) FROM Users";
                object avgAge = await command.ExecuteScalarAsync();

                Console.WriteLine($"В таблице {count} объектa(ов)");
                Console.WriteLine($"Минимальный возраст: {minAge}");
                Console.WriteLine($"Средний возраст: {avgAge}");
            */
            #endregion ExecuteScalar

            #region SqlParameter
            /*
            string connectionString = "Server=(localdb)\\mssqllocaldb;Database=adonetdb;Trusted_Connection=True;";

            // данные для добавления
            int age = 27;
            string name = "Sam";
            // выражение SQL для добавления данных
            string sqlExpression = "INSERT INTO Users (Name, Age) VALUES (@name, @age)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                SqlCommand command = new SqlCommand(sqlExpression, connection);

                // создаем параметр для имени (размер 100 СИМВОЛОВ (для массивов - байт), т.к. такое ограничение)
                SqlParameter nameParam = new SqlParameter("@name", System.Data.SqlDbType.NVarChar, 100);
                // определяем значение
                nameParam.Value = name; // если там больше 100 символов, строка будет усечена
                // также можно определить тип и размер через свойства
                // nameParam.SqlDbType = System.Data.SqlDbType.NVarChar;
                // nameParam.Size = 100;
                // добавляем параметр к команде
                command.Parameters.Add(nameParam);
                // создаем параметр для возраста
                SqlParameter ageParam = new SqlParameter("@age", age);
                // добавляем параметр к команде
                command.Parameters.Add(ageParam);

                int number = await command.ExecuteNonQueryAsync();
                Console.WriteLine($"Добавлено объектов: {number}");

                // вывод данных
                command.CommandText = "SELECT * FROM Users";
                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    if (reader.HasRows) // если есть данные
                    {
                        // выводим названия столбцов
                        string[] columns = new string[reader.FieldCount];
                        for (int col = 0; col < reader.FieldCount; col++)
                        {
                            columns[col] = reader.GetName(col);
                            Console.Write(columns[col] + "\t");
                        }
                        Console.WriteLine();

                        // построчно считываем данные, ReadAsync = true, если есть данные (строка считана)
                        while (await reader.ReadAsync()) // or: while (reader.Read())
                        {
                            for (int col = 0; col < reader.FieldCount; col++)
                            {
                                Console.Write(reader[columns[col]] + "\t");
                            }
                            Console.WriteLine();
                        }
                    }
                }
            }
            */
            #endregion SqlParameter

            #region Выходные параметры запросов
            /*
            string connectionString = "Server=(localdb)\\mssqllocaldb;Database=adonetdb;Trusted_Connection=True;";

            int age = 23;
            string name = "Kenny";
            string sqlExpression = "INSERT INTO Users (Name, Age) VALUES (@name, @age);SET @id=SCOPE_IDENTITY()";
            //  операция присвоения параметру id идентификатора добавленной строки: SET @id=SCOPE_IDENTITY().
            //  SCOPE_IDENTITY() - встроенная функция MS SQL Server, возвращающая идентификатор добавленной строки.
            // https://docs.microsoft.com/en-us/sql/t-sql/functions/scope-identity-transact-sql?view=sql-server-ver16
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                SqlCommand command = new SqlCommand(sqlExpression, connection);

                // создаем параметр для имени
                SqlParameter nameParam = new SqlParameter("@name", name);
                // добавляем параметр к команде
                command.Parameters.Add(nameParam);
                // создаем параметр для возраста
                SqlParameter ageParam = new SqlParameter("@age", age);
                // добавляем параметр к команде
                command.Parameters.Add(ageParam);
                // параметр для id
                SqlParameter idParam = new SqlParameter
                {
                    ParameterName = "@id", // имя параметра
                    SqlDbType = SqlDbType.Int, // тип значения
                    Direction = ParameterDirection.Output // тип параметра: параметр выходной.
                    // Input (default): параметр является входным (input-only), то есть предназначен для передачи значений в sql-выражение запроса.
                    // InputOutput: параметр может быть как входным, так и выходным (bidirectional).
                    // Output: параметр является выходным (output-only), то есть используется для возвращения запросом каких-либо значений
                    // ReturnValue: параметр представляет результат выполнения выражения или хранимой процедуры (a stored procedure return value parameter).
                };
                command.Parameters.Add(idParam);

                await command.ExecuteNonQueryAsync();

                // получим значения выходного параметра
                Console.WriteLine($"Id нового объекта: {idParam.Value}"); // значение параметра
            }
            */
            #endregion Выходные параметры запросов

            #region Работа с хранимыми процедурами
            /*
            // command.CommandType = CommandType.StoredProcedure
            // command.CommandType = CommandType.Text
            // command.CommandType = CommandType.TableDirect

            string connectionString = "Server=(localdb)\\mssqllocaldb;Database=adonetdb;Trusted_Connection=True;";

            // Хранимая процедура sp_InsertUser, которая добавляет объект в БД и возвращает id добавленной строки
            // @name & @age - входные параметры
            // после выражения AS идет стандартное sql-выражение INSERT, которое выполняет добавление данных
            // И в конце с помощью выражения SELECT возвращается результат.
            // Выражение SCOPE_IDENTITY() возвращает id добавленной записи, поэтому на выходе из процедуры мы получим id новой записи.
            // И завершается процедура ключевым словом GO.
            string proc1 = @"CREATE PROCEDURE [dbo].[sp_InsertUser]
                                @name nvarchar(50),
                                @age int
                            AS
                                INSERT INTO Users (Name, Age)
                                VALUES (@name, @age)

                                SELECT SCOPE_IDENTITY()
                            GO";

            // Хранимая процедура sp_GetUsers, которая будет возвращать все строки из таблицы
            string proc2 = @"CREATE PROCEDURE [dbo].[sp_GetUsers]
                                AS
                                    SELECT * FROM Users 
                                GO";

            // создание процедур
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                SqlCommand command = new SqlCommand(proc1, connection);

                // добавляем первую процедуру
                try { await command.ExecuteNonQueryAsync(); }
                catch (Exception ex) { Console.WriteLine(ex.GetType() + ": " + ex.Message); }
                // Microsoft.Data.SqlClient.SqlException: There is already an object named 'sp_InsertUser' in the database.

                // добавляем вторую процедуру
                command.CommandText = proc2;

                try { await command.ExecuteNonQueryAsync(); }
                catch (Exception ex) { Console.WriteLine(ex.GetType() + ": " + ex.Message); }
                Console.WriteLine("Хранимые процедуры добавлены в базу данных.");

                // работа с параметрами процедур и их выполнение
                Console.Write("Введите имя пользователя: ");
                string name = Console.ReadLine();

                Console.Write("Введите возраст пользователя: ");
                int age = Int32.Parse(Console.ReadLine());

                await AddUserAsync(name, age);

                Console.WriteLine();

                await GetUsersAsync();

                // Выходные параметры хранимых процедур

                // Процедуру для получения диапазона возрастов по имени:
                string proc = @"CREATE PROCEDURE [dbo].[sp_GetAgeRange]
                                @name nvarchar(50),
                                @minAge int out,
                                @maxAge int out
                            AS
                                SELECT @minAge = MIN(Age), @maxAge = MAX(Age) FROM Users WHERE Name LIKE '%' + @name + '%';";

                command = new SqlCommand(proc, connection);

                try { await command.ExecuteNonQueryAsync();
                    Console.WriteLine("Процедура sp_GetAgeRange добавлена в базу данных.");
                }
                catch (Exception ex) { Console.WriteLine(ex.GetType() + ": " + ex.Message); }

                await GetAllUsersAsync();   // сначала выводим всех пользователей

                Console.Write("Введите имя пользователя:");
                name = Console.ReadLine();

                await GetAgeRangeAsync(name);
            }
            */
            #endregion Работа с хранимыми процедурами

            #region Transactions
            /*
            string connectionString = "Server=(localdb)\\mssqllocaldb;Database=adonetdb;Trusted_Connection=True;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                SqlTransaction transaction = connection.BeginTransaction();

                SqlCommand command = connection.CreateCommand();
                command.Transaction = transaction;

                try
                {
                    // выполняем две отдельные команды
                    command.CommandText = "INSERT INTO Users (Name, Age) VALUES('Tanya', 34)";
                    await command.ExecuteNonQueryAsync();
                    command.CommandText = "INSERT INTO Users (Name, Age) VALUES('Katya', 31)";
                    await command.ExecuteNonQueryAsync();
                    // command.CommandText = "WRONG COMMAND"; // Microsoft.Data.SqlClient.SqlException: Could not find stored procedure 'WRONG'.
                    // await command.ExecuteNonQueryAsync();

                    // command.CommandText = "sp_GetUsers";
                    // PrintDB(command); // System.InvalidOperationException: The connection does not support MultipleActiveResultSets.
                    // System.InvalidOperationException: There is already an open DataReader associated with this Connection which must be closed first.

                    // подтверждаем транзакцию
                    await transaction.CommitAsync();

                    Console.WriteLine("Данные добавлены в базу данных");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.GetType() + ": " + ex.Message);
                    // если ошибка, откатываем назад все изменения
                    await transaction.RollbackAsync();
                }

                await PrintCommandResult(new SqlCommand("SELECT * FROM Users", connection)); // Print DB

            }
            */
            #endregion Transactions

            #region Сохранение и извлечение файлов из базы данных
            /*
            // Создание базы данных и таблицы для хранения файлов (изображений)
            string connectionString = "Server=(localdb)\\mssqllocaldb;Database=master;Trusted_Connection=True;";
            string sqlExpression = "CREATE DATABASE filesdb";
            // создаем базу данных
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                try { await command.ExecuteNonQueryAsync();
                    Console.WriteLine("База данных создана"); }
                catch (Exception ex) { Console.WriteLine(ex.GetType() + ": " + ex.Message); }
            }

            connectionString = "Server=(localdb)\\mssqllocaldb;Database=filesdb;Trusted_Connection=True;";
            sqlExpression = @"CREATE TABLE Files
                                (Id INT PRIMARY KEY IDENTITY, 
                                 Title NVARCHAR(50) NOT NULL, 
                                 FileName NVARCHAR(50) NOT NULL,
                                 ImageData varbinary(MAX))";
            // создаем таблицу
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                try { await command.ExecuteNonQueryAsync();
                    Console.WriteLine("Таблица Files создана"); }
                catch (Exception ex) { Console.WriteLine(ex.GetType() + ": " + ex.Message); }
            }

            // Сохранение файла
            await SaveFileToDatabaseAsync();

            // Извлечение файла из базы данных
            await ReadFileFromDatabaseAsync();
            */
            #endregion Сохранение и извлечение файлов из базы данных

            // DataSet study https://metanit.com/sharp/adonetcore/3.1.php
            #region SqlDataAdapter и загрузка данных в DataSet
            /*
            string connectionString = "Server=(localdb)\\mssqllocaldb;Database=adonetdb;Trusted_Connection=True;";
            string sql = "SELECT * FROM Users";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Создаем объект DataAdapter
                SqlDataAdapter adapter = new SqlDataAdapter(sql, connection);
                // SqlDataAdapter adapter = new SqlDataAdapter();
                // SqlDataAdapter adapter = new SqlDataAdapter(command);
                // SqlDataAdapter adapter = new SqlDataAdapter(sql, connectionString);

                // Создаем объект DataSet
                DataSet ds = new DataSet();
                // Заполняем Dataset данными из ДБ Users
                adapter.Fill(ds); // Fill uses the current connection or opens & closes new connection

                // Объект DataSet состоит из набора объектов DataTable. Для доступа к таблицам в DataSet определено свойство Tables,
                // которое представляет объект DataTableCollection. Объект DataTable, в свою очередь, состоит из набора объектов DataColumn -
                // описания столбцов таблицы, которые определяют схему таблицы и которые можно получить через свойство Columns.
                // Кроме столбцов, класс DataTable определяет свойство Rows, которое представляет тип DataRowCollection
                // и которое хранит строки таблицы. Каждая строка представляет класс DataRow.

                // Отображаем данные
                ShowDataSet(ds);

                // Отображение свойств всех таблиц
                ShowDataSetProperties(ds);
                // Output:
                // DataSet NewDataSet: 1 таблиц
                // Таблица Table: 3 столбцов, 31 строк
                // Namespace = , CaseSensitive = False, DisplayExpression =
                // HasErrors = False, IsInitialized = True, Locale = ru-UA
                // MinimumCapacity = 50, Prefix = , PrimaryKey = System.Data.DataColumn[][0]
                // RemotingFormat = Xml

                // ColumnName = Id: Expression = , Unique = False, Ordinal = 0,
                //      AllowDBNull = True, AutoIncrement = False, AutoIncrementSeed = 0, AutoIncrementStep = 1
                //      Caption = Id, ColumnMapping = Element, DataType = System.Int32, DateTimeMode = UnspecifiedLocal
                //      DefaultValue = , DesignMode = False, Expression = , MaxLength = -1
                //      Prefix = , ReadOnly = False, Unique = False

                // ColumnName = Age: Expression = , Unique = False, Ordinal = 1,
                //      AllowDBNull = True, AutoIncrement = False, AutoIncrementSeed = 0, AutoIncrementStep = 1
                //      Caption = Age, ColumnMapping = Element, DataType = System.Int32, DateTimeMode = UnspecifiedLocal
                //      DefaultValue = , DesignMode = False, Expression = , MaxLength = -1
                //      Prefix = , ReadOnly = False, Unique = False

                // ColumnName = Name: Expression = , Unique = False, Ordinal = 2,
                //      AllowDBNull = True, AutoIncrement = False, AutoIncrementSeed = 0, AutoIncrementStep = 1
                //      Caption = Name, ColumnMapping = Element, DataType = System.String, DateTimeMode = UnspecifiedLocal
                //      DefaultValue = , DesignMode = False, Expression = , MaxLength = -1
                //      Prefix = , ReadOnly = False, Unique = False

            }
            */
            #endregion SqlDataAdapter и загрузка данных в DataSet

            #region DataSet without DB
            /*
            // ручное создание таблицы в DataSet, определение ее структуры и добавление данных
            DataSet usersSet = new DataSet("UsersSet");
            DataTable users = new DataTable("Users");
            // добавляем таблицу в dataset
            usersSet.Tables.Add(users);

            // создаем столбцы для таблицы Users
            DataColumn idColumn = new DataColumn("Id", Type.GetType("System.Int32"));
            idColumn.Unique = true; // столбец будет иметь уникальное значение
            idColumn.AllowDBNull = false; // не может принимать null
            idColumn.AutoIncrement = true; // будет автоинкрементироваться
            idColumn.AutoIncrementSeed = 1; // начальное значение
            idColumn.AutoIncrementStep = 1; // приращении при добавлении новой строки

            DataColumn nameColumn = new DataColumn("Name", Type.GetType("System.String"));
            DataColumn ageColumn = new DataColumn("Age", Type.GetType("System.Int32"));
            ageColumn.DefaultValue = 1; // значение по умолчанию

            users.Columns.Add(idColumn);
            users.Columns.Add(nameColumn);
            users.Columns.Add(ageColumn);
            // определяем первичный ключ таблицы Users
            users.PrimaryKey = new DataColumn[] { users.Columns["Id"] };

            DataRow row = users.NewRow();
            row.ItemArray = new object?[] { null, "Tom", 36 }; // null is ignored because the column is autogenerated
            users.Rows.Add(row); // добавляем первую строку
            users.Rows.Add(new object?[] { null, "Bob", 29 }); // добавляем вторую строку

            row = users.Rows[0]; // первая строка
            row[1] = "Dad"; // второй ячейке первой строки присваивается значение Dad
            users.Rows[0][2] = 30; //третьей ячейке первой строки присваивается значение 30
            users.Rows[1]["Age"] = 50; // во второй строке меняем возраст

            ShowDataSet(usersSet);
            // Id      Name    Age
            // 1       Dad     30
            // 2       Bob     50
            ShowDataSetProperties(usersSet);
            // DataSet UsersSet: 1 таблиц
            // Таблица Users: 3 столбцов, 2 строк
            // Namespace = , CaseSensitive = False, DisplayExpression =
            // HasErrors = False, IsInitialized = True, Locale = ru-UA
            // MinimumCapacity = 50, Prefix = , PrimaryKey = System.Data.DataColumn[][1]-> [0] = Id
            // RemotingFormat = Xml

            // ColumnName = Id: Expression = , Unique = True, Ordinal = 0,
            //      AllowDBNull = False, AutoIncrement = True, AutoIncrementSeed = 1, AutoIncrementStep = 1
            //      Caption = Id, ColumnMapping = Element, DataType = System.Int32, DateTimeMode = UnspecifiedLocal
            //      DefaultValue = , DesignMode = False, Expression = , MaxLength = -1
            //      Prefix = , ReadOnly = False, Unique = True

            // ColumnName = Name: Expression = , Unique = False, Ordinal = 1,
            //      AllowDBNull = True, AutoIncrement = False, AutoIncrementSeed = 0, AutoIncrementStep = 1
            //      Caption = Name, ColumnMapping = Element, DataType = System.String, DateTimeMode = UnspecifiedLocal
            //      DefaultValue = , DesignMode = False, Expression = , MaxLength = -1
            //      Prefix = , ReadOnly = False, Unique = False

            // ColumnName = Age: Expression = , Unique = False, Ordinal = 2,
            //      AllowDBNull = True, AutoIncrement = False, AutoIncrementSeed = 0, AutoIncrementStep = 1
            //      Caption = Age, ColumnMapping = Element, DataType = System.Int32, DateTimeMode = UnspecifiedLocal
            //      DefaultValue = 1, DesignMode = False, Expression = , MaxLength = -1
            //      Prefix = , ReadOnly = False, Unique = False

            var selectedUsers = users.Select("Age > 30"); // selectedUsers = DataRow[]?
            Console.WriteLine("Age > 30:");
            foreach (var user in selectedUsers)
                Console.WriteLine($"{user["Name"]} - {user["Age"]}"); // Bob - 50

            users.Rows.RemoveAt(1); // удаление второй строки по индексу
            // другой сопосб удаления
            row = users.Rows[0];
            users.Rows.Remove(row); // удаляем первую строку

            ShowDataSet(usersSet);  // Id      Name    Age
            */
            #endregion DataSet without DB

            #region Сохранение изменений DataSet в базе данных / SqlCommandBuilder, метод Update объекта SqlDataAdapter
            /*
            string connectionString = "Server=(localdb)\\mssqllocaldb;Database=adonetdb;Trusted_Connection=True;";
            string sql = "SELECT * FROM Users";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Создаем объект DataAdapter
                SqlDataAdapter adapter = new SqlDataAdapter(sql, connection);
                // Создаем объект Dataset
                DataSet ds = new DataSet();
                // Заполняем Dataset
                adapter.Fill(ds);

                DataTable dt = ds.Tables[0]; // таблица Users
                // создадим новую строку с той же схемой, как в этой таблице
                DataRow newRow = dt.NewRow();
                newRow["Name"] = "Rick";
                newRow["Age"] = 24;
                // добавим новую строку
                dt.Rows.Add(newRow);

                // Изменим значение в столбце Age для первой строки
                dt.Rows[0]["Age"] = 17;

                // создаем объект SqlCommandBuilder (он также является обработчиком события adapter.RowUpdating)
                SqlCommandBuilder commandBuilder = new SqlCommandBuilder(adapter);
                adapter.Update(ds);
                // альтернативный способ - обновление только одной таблицы
                // adapter.Update(dt);

                // При вызове у адаптера метода Update() происходит анализ изменений, которые произошли.
                // И после этого выполняется соответствующая команда. В данном случае, так как идет добавление новой строки и обновление одной,
                // то буду выполняться команды InsertCommand и UpdateCommand. Однако в данном коде мы нигде явным образом не задаем эти команды,
                // за нас все автоматически делает SqlCommandBuilder. Причем больше нигде в коде мы этот объект не вызываем.
                // При необходимости мы можем получить sql-выражения используемых команд:

                Console.WriteLine(commandBuilder.GetUpdateCommand().CommandText);
                Console.WriteLine(commandBuilder.GetInsertCommand().CommandText);
                Console.WriteLine(commandBuilder.GetDeleteCommand().CommandText);

                // В нашем случае команда обновления будет выглядеть так:
                // UPDATE [Users] SET [Age] = @p1, [Name] = @p2 WHERE (([Id] = @p3) AND ([Age] = @p4) AND ([Name] = @p5))
                // Команда вставки: INSERT INTO[Users] ([Age], [Name]) VALUES(@p1, @p2)
                // Команда удаления: DELETE FROM[Users] WHERE(([Id] = @p1) AND([Age] = @p2) AND([Name] = @p3))

                // проверка: заново получаем данные из БД
                // очищаем полностью DataSet
                ds.Clear();
                // перезагружаем данные
                adapter.Fill(ds);

                // Отображаем данные
                ShowDataSet(ds);
            }
            */
            #endregion Сохранение изменений DataSet в базе данных

            // SQLite study - https://metanit.com/sharp/adonetcore/4.1.php
            #region Подключение к базе данных SQLite, выполнение запросов к БД и SqliteCommand
            /*
            // Для создания подключения к БД SQLite применяется класс SqliteConnection, конструктор () или (connectionString)
            // параметр Mode устанавливает режим подключения и может принимать следующие значения:
            //  ReadWriteCreate (по умолчанию): открывает базу данных для чтения и записи. Если БД не существует, то она автоматически создается.
            //  ReadWrite: открывает базу данных для чтения и записи.
            //  ReadOnly: открывает базу данных только для чтения.
            //  Memory: создает и открывает базу данных в памяти.
            // параметр Cache устанавливает режим кэширования. Может принимать следующие значения:
            //  Default (по умолчанию): применяет режим по умолчанию используемой библиотеки SQLite.
            //  Private: для каждого подключения создается свой приватный кэш.
            //  Shared: подключения используют один общий кэш.
            // Foreign Keys указывает, будет ли база данных поддерживать внешние ключи (PRAGMA foreign_keys = 1 или 0)
            // Recursive triggers указывает, будут ли поддерживаться рекурсивные триггеры (если true, то посылается PRAGMA recursive_triggers)

            // SQLite НЕ поддерживает асинхронные операции чтения/записи!

            using (var connection = new SqliteConnection("Data Source=usersdata.db"))
            {
                // открытие подключения
                connection.Open(); // если ее нет, то создается

                // Выполнение запросов к БД SQLite и SqliteCommand.
                SqliteCommand command = new SqliteCommand(); // Создание команды - альтернативы:
                // SqliteCommand command = connection.CreateCommand(); // метод SqliteConnection
                // SqliteCommand command = new SqliteCommand(sql); // String
                // SqliteCommand command = new SqliteCommand(sql, connection); // + SqliteConnection
                // SqliteCommand command = new SqliteCommand(sql, connection, transaction); // + SqliteTransaction

                // свойства SqliteCommand:
                // CommandText: хранит выполняемую команду SQL
                // CommandTimeout: хранит временной интервал в секундах, после которого SqliteCommand прекращает попытки выполнить команду.
                // По умолчанию равен 30 секундам. Значение 0 представляет отстутсвие интервала.
                // Parameters: предствляет параметры команды
                // Connection: предоставляет используемое подключение SqliteConnection
                command.Connection = connection;
                command.CommandText = "CREATE TABLE Users(_id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE, Name TEXT NOT NULL, Age INTEGER NOT NULL)";

                // Чтобы выполнить команду (sql-выражение), необходимо применить один из методов SqliteCommand:
                // ExecuteNonQuery: возвращает количество измененных записей. Подходит для sql-выражений INSERT, UPDATE, DELETE, CREATE.
                // ExecuteReader(): возвращает считанные из таблицы строки. Подходит для sql-выражения SELECT.
                // ExecuteScalar(): возвращает одно скалярное значение, например, число. Подходит для SELECT в паре с одной из встроенных функций SQL,
                // как, например, Min, Max, Sum, Count.

                // Создание таблицы
                try
                {
                    command.ExecuteNonQuery();
                    Console.WriteLine("Таблица Users создана");
                }
                catch (Exception ex) { Console.WriteLine(ex.GetType() + ": " + ex.Message); }

                // View: http://sqlitebrowser.org/

                // добавление данных
                command.CommandText = "INSERT INTO Users (Name, Age) VALUES ('Tom', 36)";
                try
                {
                    int number = command.ExecuteNonQuery();
                    Console.WriteLine($"В таблицу Users добавлено объектов: {number}");
                }
                catch (Exception ex) { Console.WriteLine(ex.GetType() + ": " + ex.Message); }

                string sqlExpression = "INSERT INTO Users (Name, Age) VALUES ('Alice', 32), ('Bob', 28)";
                command.CommandText = sqlExpression;
                try { int number = command.ExecuteNonQuery();
                      Console.WriteLine($"В таблицу Users добавлено объектов: {number}"); }
                catch (Exception ex) { Console.WriteLine(ex.GetType() + ": " + ex.Message); }

                Console.WriteLine("Введите имя:");
                string name = Console.ReadLine();

                Console.WriteLine("Введите возраст:");
                int age = Int32.Parse(Console.ReadLine());

                command.CommandText = $"INSERT INTO Users (Name, Age) VALUES ('{name}', {age})";
                try { int number = command.ExecuteNonQuery();
                      Console.WriteLine($"В таблицу Users добавлено объектов: {number}"); }
                catch (Exception ex) { Console.WriteLine(ex.GetType() + ": " + ex.Message); }

                // Обновление объектов
                command.CommandText = "UPDATE Users SET Age=20 WHERE Name='Tom'";
                try { int number = command.ExecuteNonQuery();
                      Console.WriteLine($"Обновлено объектов Tom -> 20: {number}"); }
                catch (Exception ex) { Console.WriteLine(ex.GetType() + ": " + ex.Message); }

                // обновление ранее добавленного объекта
                Console.WriteLine($"Введите новое имя для всех, у кого возраст {age}:");
                name = Console.ReadLine();
                command.CommandText = $"UPDATE Users SET Name='{name}' WHERE Age={age}";
                try { int number = command.ExecuteNonQuery(); 
                      Console.WriteLine($"Обновлено объектов: {number}"); }
                catch (Exception ex) { Console.WriteLine(ex.GetType() + ": " + ex.Message); }

                // Удаление данных
                sqlExpression = "DELETE FROM Users WHERE Name='Tom'";
                command = new SqliteCommand(sqlExpression, connection);
                try { int number = command.ExecuteNonQuery();
                      Console.WriteLine($"Удалено объектов Tom: {number}"); }
                catch (Exception ex) { Console.WriteLine(ex.GetType() + ": " + ex.Message); }
            }
            */
            #endregion Подключение к базе данных SQLite, выполнение запросов к БД и SqliteCommand

            #region Чтение результатов запроса и SqliteDataReader
            /*
            string sqlExpression = "SELECT * FROM Users";
            using (var connection = new SqliteConnection("Data Source=usersdata.db"))
            {
                connection.Open();

                SqliteCommand command = new SqliteCommand(sqlExpression, connection);

                await PrintSQLiteCommandResult(command); // uses ReadAsync() but also can use Read()

                // alternative methods of getting the values:
                // var id = reader.GetValue(0); var name = reader.GetValue(1);     var age = reader.GetValue(2);
                // object id = reader[0]        object name = reader[1];           object age = reader[2];
                // object id = reader["_id"];   object name = reader["Name"];      object age = reader["Age"];
                // int id = reader.GetInt32(0); string name = reader.GetString(1); int age = reader.GetInt32(2);

                // типы данных SQLite:
                // SQLite имеет только четыре примитивных типа: INTEGER, REAL, TEXT и BLOB.
                // С помощью пакета Microsoft.Data.Sqlite типы данных C# и SQLite сопоставляются следующим образом:
                // Тип C#           Тип SQLite
                // bool             INTEGER(0 или 1)
                // sbyte            INTEGER
                // byte             INTEGER
                // byte[]           BLOB
                // char             TEXT(UTF-8)
                // DateTime         TEXT(формат yyyy-MM-dd HH: mm:ss.FFFFFFF)
                // DateTimeOffset   TEXT(формат yyyy-MM-dd HH: mm:ss.FFFFFFF)
                // TimeSpan         TEXT(формат d.hh:mm: ss.fffffff)
                // decimal          TEXT(формат 0.0###########################)
                // float            REAL
                // double           REAL
                // Guid             TEXT(формат 00000000-0000-0000-0000-000000000000)
                // int              INTEGER
                // short            INTEGER
                // long             INTEGER
                // uint             INTEGER
                // ushort           INTEGER
                // ulong            INTEGER
                // string           TEXT
            }
            */
            #endregion Чтение результатов запроса и SqliteDataReader

            #region Параметризация запросов к БД Sqlite - SqliteParameter
            /*
            // SqliteParameter() - конструктор с инициализацией через свойств
            // SqliteParameter(String, Object): имя и значение параметра
            // SqliteParameter(String, SqliteType): имя и тип параметра
            // SqliteParameter(String, SqliteType, Int32): имя и тип параметра, максимальный размер значения параметра в байтах
            // SqliteParameter(String, SqliteType, Int32, String): имя, типа, максимальный размер параметра и имя столбца в таблице

            // Для конфигурации параметров можно использовать их свойства (все они входные), например:
            // ParameterName: имя параметра
            // SqliteType: тип параметра:
            //    SqliteType.Integer
            //    SqliteType.Real
            //    SqliteType.Text
            //    SqliteType.Blob
            // Value: значение параметра
            // Size: максимальный размер данных параметра в байтах
            // IsNullable: указывает, допускает ли параметр значение null

            // После определения параметров они добавляются в коллекцию Parameters объекта SqliteCommand.

            // данные для добавления
            int userage = 23;
            string username = "Daniel";
            // выражение SQL для добавления данных
            string sqlExpression = "INSERT INTO Users (Name, Age) VALUES (@name, @age)";
            using (var connection = new SqliteConnection("Data Source=usersdata.db"))
            {
                connection.Open();

                SqliteCommand command = new SqliteCommand(sqlExpression, connection);
                // создаем параметр для имени
                // SqliteParameter nameParam = new SqliteParameter("@name", username);
                SqliteParameter nameParam = new SqliteParameter("@name", SqliteType.Text, 4); // if > 4 than will be truncated
                nameParam.Value = username;
                    // также можно определить тип и размер через свойства:
                    // nameParam.SqliteType = SqliteType.Text;
                    // nameParam.Size = 4;
                // добавляем параметр к команде
                command.Parameters.Add(nameParam);
                // добавляем параметр для возраста к команде
                command.Parameters.Add(new SqliteParameter("@age", userage));

                try { int number = command.ExecuteNonQuery();
                      Console.WriteLine($"Добавлено объектов: {number}"); }
                catch (Exception ex) { Console.WriteLine(ex.GetType() + ": " + ex.Message); }

                // вывод данных
                command.CommandText = "SELECT * FROM Users";
                await PrintSQLiteCommandResult(command);
            }
            */
            #endregion Параметризация запросов к БД Sqlite - SqliteParameter

            #region Получение скалярных значений в SQLite
            /*
            using (var connection = new SqliteConnection("Data Source=usersdata.db"))
            {
                connection.Open();

                SqliteCommand command = new SqliteCommand();
                command.Connection = connection;
                // Для получения идентификатора добавленнего объекта на стороне базы данных применяется встроенная функция last_insert_rowid().
                // И для ее выполнения в код SQL-запроса добавляет подзапрос "SELECT last_insert_rowid();"
                command.CommandText = @"INSERT INTO Users (Name, Age) 
                                        VALUES ('Sam', 45);
                                        SELECT last_insert_rowid();";
                try { object id = command.ExecuteScalar();
                      Console.WriteLine($"Идентификатор добавленного объекта: {id}"); } // 7
                catch (Exception ex) { Console.WriteLine(ex.GetType() + ": " + ex.Message); }

                // other useful scalar functions: Min, Max, Sum, Count...
                command.CommandText = "SELECT COUNT(*) FROM Users";
                try { object count = command.ExecuteScalar();
                      Console.WriteLine($"В таблице {count} объектa(ов)"); } // 6
                catch (Exception ex) { Console.WriteLine(ex.GetType() + ": " + ex.Message); }

                command.CommandText = "SELECT MIN(Age) FROM Users";
                try { object minAge = command.ExecuteScalar();
                      Console.WriteLine($"Минимальный возраст: {minAge}"); } // 23
                catch (Exception ex) { Console.WriteLine(ex.GetType() + ": " + ex.Message); }

                command.CommandText = "SELECT AVG(Age) FROM Users";
                try { object avgAge = command.ExecuteScalar();
                      Console.WriteLine($"Средний возраст: {avgAge}"); } // 34,333333333333336
                catch (Exception ex) { Console.WriteLine(ex.GetType() + ": " + ex.Message); }

                // вывод данных
                // command.CommandText = "SELECT * FROM Users";
                // await PrintSQLiteCommandResult(command);
            }
            */
            #endregion Получение скалярных значений в SQLite

            #region Сохранение и извлечение файлов из базы данных SQLite
            /*
            // Создание таблицы Files для хранения файлов (изображений)

            // выражение SQL для добавления данных
            string sqlExpression = @"CREATE TABLE Files 
                                (_id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
                                 Title TEXT NOT NULL, 
                                 FileName TEXT NOT NULL,
                                 ImageData BLOB)";
            using (var connection = new SqliteConnection("Data Source=filesdata.db"))
            {
                connection.Open();

                SqliteCommand command = new SqliteCommand(sqlExpression, connection);
                try { command.ExecuteNonQuery();
                      Console.WriteLine("Таблица Files создана"); }
                catch (Exception ex) { Console.WriteLine(ex.GetType() + ": " + ex.Message); }
            }

            // Сохранение файлов: метод в качестве параметров получает полный путь к файлу и его название
            SaveFile($"../../../Sky.jpg", "Небо");

            // Извлечение файлов из базы данных
            GetFiles();
            */
            #endregion Сохранение и извлечение файлов из базы данных SQLite

        } // Main

        // Отображение данных всех таблиц из DataSet
        private static void ShowDataSet(DataSet ds)
        {
            // Перебор всех таблиц
            foreach (DataTable dt in ds.Tables) // Tables = DataTableCollection
            {
                foreach (DataColumn column in dt.Columns) // Columns = DataColumnCollection
                    Console.Write($"{column.ColumnName}\t");
                Console.WriteLine();
                // перебор всех строк таблицы
                foreach (DataRow row in dt.Rows) // Rows = DataRowCollection
                {
                    // получаем все ячейки строки
                    // отдельную ячейку можно получить как Item[String] по имени столбца или Item[int] по индексу
                    object?[]? cells = row.ItemArray; // возвращает или устанавливает значения всех ячеек строки в виде объекта object?[]?
                    foreach (object? cell in cells)
                        Console.Write($"{cell}\t");
                    Console.WriteLine();
                }
            }
        }

        // Отображение свойств всех таблиц из DataSet
        private static void ShowDataSetProperties(DataSet ds)
        {
            // Перебор всех таблиц
            Console.WriteLine($"DataSet {ds.DataSetName}: {ds.Tables.Count} таблиц");
            foreach (DataTable dt in ds.Tables) // Tables = DataTableCollection
            {
                Console.WriteLine($"Таблица {dt.TableName}: {dt.Columns.Count} столбцов, {dt.Rows.Count} строк");
                Console.WriteLine($"Namespace = {dt.Namespace}, CaseSensitive = {dt.CaseSensitive}, DisplayExpression = {dt.DisplayExpression}");
                Console.WriteLine($"HasErrors = {dt.HasErrors}, IsInitialized = {dt.IsInitialized}, Locale = {dt.Locale}");
                Console.WriteLine($"MinimumCapacity = {dt.MinimumCapacity}, Prefix = {dt.Prefix}, "+
                    $"PrimaryKey = {dt.PrimaryKey}[{dt.PrimaryKey.Length}]{(dt.PrimaryKey.Length == 0 ? "" : " -> [0] = " + dt.PrimaryKey[0].ColumnName)}");
                Console.WriteLine($"RemotingFormat = {dt.RemotingFormat}");

                foreach (DataColumn column in dt.Columns)
                { // Columns = DataColumnCollection
                    Console.WriteLine($"ColumnName = {column.ColumnName}: Expression = {column.Expression}, Unique = {column.Unique}, " +
                        $"Ordinal = {column.Ordinal},\r\n    AllowDBNull = {column.AllowDBNull}, AutoIncrement = {column.AutoIncrement}, " +
                        $"AutoIncrementSeed = {column.AutoIncrementSeed}, AutoIncrementStep = {column.AutoIncrementStep}\r\n" +
                        $"    Caption = {column.Caption}, ColumnMapping = {column.ColumnMapping}, DataType = {column.DataType}, DateTimeMode = {column.DateTimeMode}");
                    Console.WriteLine($"    DefaultValue = {column.DefaultValue}, DesignMode = {column.DesignMode}, Expression = {column.Expression}, MaxLength = {column.MaxLength}\r\n" +
                        $"    Prefix = {column.Prefix}, ReadOnly = {column.ReadOnly}, Unique = {column.Unique}");
                }
            }
        }

        // добавление пользователя
        private static async Task AddUserAsync(string name, int age)
        {
            // название процедуры
            string sqlExpression = "sp_InsertUser";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                SqlCommand command = new SqlCommand(sqlExpression, connection);
                // указываем, что команда представляет хранимую процедуру
                command.CommandType = CommandType.StoredProcedure;
                // параметр для ввода имени
                SqlParameter nameParam = new SqlParameter { ParameterName = "@name", Value = name }; // ("@name", name);
                // добавляем параметр
                command.Parameters.Add(nameParam);
                // параметр для ввода возраста
                command.Parameters.Add(new SqlParameter("@age", age));

                // выполняем процедуру
                try
                {
                    var id = await command.ExecuteScalarAsync();
                    Console.WriteLine($"Id добавленного объекта: {id}");
                }
                // если нам не надо возвращать id:
                // try
                // { var id = await command.ExecuteNonQueryAsync();
                //    Console.WriteLine($"Количество добавленных объектов: {id}");
                // }
                catch (Exception ex) { Console.WriteLine(ex.GetType() + ": " + ex.Message); }
            }
        }

        // вывод результата команды SQL Server на экран
        private static async Task PrintCommandResult(SqlCommand command)
        {
            using (SqlDataReader reader = await command.ExecuteReaderAsync())
            {
                if (reader.HasRows) // если есть данные
                {
                    // выводим названия столбцов
                    string[] columns = new string[reader.FieldCount];
                    for (int col = 0; col < reader.FieldCount; col++)
                    {
                        columns[col] = reader.GetName(col);
                        Console.Write(columns[col] + "\t");
                    }
                    Console.WriteLine();

                    // построчно считываем данные, ReadAsync = true, если есть данные (строка считана)
                    while (await reader.ReadAsync()) // or: while (reader.Read())
                    {
                        for (int col = 0; col < reader.FieldCount; col++)
                        {
                            Console.Write(reader[columns[col]] + "\t");
                        }
                        Console.WriteLine();
                    }
                }
            }

        }

        // вывод результата команды SQLite на экран
        private static async Task PrintSQLiteCommandResult(SqliteCommand command)
        {
            using (SqliteDataReader reader = await command.ExecuteReaderAsync())
            {
                if (reader.HasRows) // если есть данные
                {
                    // выводим названия столбцов
                    string[] columns = new string[reader.FieldCount];
                    for (int col = 0; col < reader.FieldCount; col++)
                    {
                        columns[col] = reader.GetName(col);
                        Console.Write(columns[col] + "\t");
                    }
                    Console.WriteLine();

                    // построчно считываем данные, ReadAsync = true, если есть данные (строка считана)
                    while (await reader.ReadAsync()) // or: while (reader.Read())
                    {
                        for (int col = 0; col < reader.FieldCount; col++)
                        {
                            Console.Write(reader[columns[col]] + "\t");
                        }
                        Console.WriteLine();
                    }
                }
            }

        }

        // вывод всех пользователей
        private static async Task GetUsersAsync()
        {
            // название процедуры
            string sqlExpression = "sp_GetUsers";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                SqlCommand command = new SqlCommand(sqlExpression, connection);
                // указываем, что команда представляет хранимую процедуру
                command.CommandType = CommandType.StoredProcedure;
                await PrintCommandResult(command);
            }
        }

        // то же самое, но без хранимых процедур
        private static async Task GetAllUsersAsync()
        {
            string sqlExpression = "SELECT * FROM Users";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                await PrintCommandResult(command);
            }
        }

        // выполнение процедуры получения мин. и макс. возраста
        private static async Task GetAgeRangeAsync(string name)
        {
            string sqlExpression = "sp_GetAgeRange";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@name", name));

                // определяем первый выходной параметр
                SqlParameter minAgeParam = new SqlParameter
                {
                    ParameterName = "@minAge",
                    SqlDbType = SqlDbType.Int, // тип параметра
                    Direction = ParameterDirection.Output // указываем, что параметр будет выходным
                };

                command.Parameters.Add(minAgeParam);

                // определяем второй выходной параметр
                SqlParameter maxAgeParam = new SqlParameter
                {
                    ParameterName = "@maxAge",
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Output // указываем, что параметр будет выходным
                };
                command.Parameters.Add(maxAgeParam);

                try { await command.ExecuteNonQueryAsync(); }
                catch (Exception ex) { Console.WriteLine(ex.GetType() + ": " + ex.Message); }

                // если name = "", то анализируются ВСЕ записи. Если таких записей нет, то выдаются пустые значения {} (type System.DBNull)
                object minAge = command.Parameters["@minAge"].Value;
                object maxAge = command.Parameters["@maxAge"].Value;
                if (minAge is DBNull) Console.WriteLine("The value is empty string: " + DBNull.Value);
                Console.WriteLine($"Минимальный возраст: {minAge} (type: {minAge.GetType()})"); // например, 28 (type: System.Int32)
                Console.WriteLine($"Максимальный возраст: {maxAge} (type: {maxAge.GetType()})");
            }
        }

        // сохранение файла Sky.jpg в БД SQLServer
        private static async Task SaveFileToDatabaseAsync()
        {
            string connectionString = "Server=(localdb)\\mssqllocaldb;Database=filesdb;Trusted_Connection=True;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = @"INSERT INTO Files VALUES (@FileName, @Title, @ImageData)";
                command.Parameters.Add("@FileName", SqlDbType.NVarChar, 50);
                command.Parameters.Add("@Title", SqlDbType.NVarChar, 50);

                // путь к файлу для загрузки
                string filename = @"..\..\..\Sky.jpg";
                // заголовок файла
                string title = "Небо";
                // получаем короткое имя файла для сохранения в БД
                string shortFileName = filename.Substring(filename.LastIndexOf('\\') + 1); // Sky.jpg

                // массив для хранения бинарных данных файла
                byte[] imageData;
                using (FileStream fs = new FileStream(filename, FileMode.Open))
                {
                    imageData = new byte[fs.Length];
                    fs.Read(imageData, 0, imageData.Length);
                    command.Parameters.Add("@ImageData", SqlDbType.Image, Convert.ToInt32(fs.Length));
                }
                // передаем данные в команду через параметры
                command.Parameters["@FileName"].Value = shortFileName;
                command.Parameters["@Title"].Value = title;
                command.Parameters["@ImageData"].Value = imageData;

                try
                {
                    await command.ExecuteNonQueryAsync();
                    Console.WriteLine("Файл сохранен");
                }
                catch (Exception ex) { Console.WriteLine(ex.GetType() + ": " + ex.Message); }
            }
        }

        // чтение всех файлов из БД SQLServer и их сохранение
        private static async Task ReadFileFromDatabaseAsync()
        {
            string connectionString = "Server=(localdb)\\mssqllocaldb;Database=filesdb;Trusted_Connection=True;";
            List<Image> images = new List<Image>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                string sql = "SELECT * FROM Files";
                SqlCommand command = new SqlCommand(sql, connection);
                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        int id = reader.GetInt32(0);
                        string filename = reader.GetString(1);
                        string title = reader.GetString(2);
                        byte[] data = (byte[])reader.GetValue(3);

                        Image image = new Image(id, filename, title, data);
                        Console.WriteLine($"Прочитан файл {filename}");
                        images.Add(image);
                    }
                }
            }
            // сохраним все файлы из списка
            for (int i = 0; i < images.Count; i++)
            {
                using (FileStream fs = new FileStream(images[i].FileName, FileMode.OpenOrCreate))
                {
                    fs.Write(images[i].Data, 0, images[i].Data.Length);
                    Console.WriteLine($"Файл {images[i].Title} сохранен");
                }
            }
        }

        // сохранение файла в БД SQLite
        private static void SaveFile(string filename, string title)
        {
            // сначала считываем файл из файловой системы
            // получаем короткое имя файла для сохранения в бд
            string shortFileName = filename.Substring(filename.LastIndexOf('/') + 1); // Sky.jpg

            // массив для хранения бинарных данных файла
            byte[] imageData;
            using (FileStream fs = new FileStream(filename, FileMode.Open))
            {
                imageData = new byte[fs.Length];
                fs.Read(imageData, 0, imageData.Length);
            }

            using (var connection = new SqliteConnection("Data Source=filesdata.db"))
            {
                connection.Open();
                
                SqliteCommand command = new SqliteCommand();
                command.Connection = connection;
                command.CommandText = @"INSERT INTO Files (Title, FileName, ImageData)
                                        VALUES (@FileName, @Title, @ImageData)";
                command.Parameters.Add(new SqliteParameter("@FileName", shortFileName));
                command.Parameters.Add(new SqliteParameter("@Title", title));
                command.Parameters.Add(new SqliteParameter("@ImageData", imageData));
                try { int number = command.ExecuteNonQuery(); 
                      Console.WriteLine($"Добавлено объектов: {number}"); }
                catch (Exception ex) { Console.WriteLine(ex.GetType() + ": " + ex.Message); }
            }
        }

        // чтение всех файлов из БД SQLite и их сохранение
        private static void GetFiles()
        {
            List<Image> images = new List<Image>();
            string sql = "SELECT * FROM Files";

            using (var connection = new SqliteConnection("Data Source=filesdata.db"))
            {
                connection.Open();

                SqliteCommand command = new SqliteCommand(sql, connection);
                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows) // если есть данные
                    {
                        while (reader.Read())   // построчно считываем данные
                        {
                            int id = reader.GetInt32(0);
                            string filename = reader.GetString(1);
                            string title = reader.GetString(2);
                            byte[] data = (byte[])reader.GetValue(3);

                            Image image = new Image(id, filename, title, data);
                            images.Add(image);
                        }
                    }
                    Console.WriteLine($"Считано объектов: {images.Count}");
                }

                // сохраним все файлы из списка в папку приложения
                for (int i = 0; i < images.Count; i++)
                {
                    using (FileStream fs = new FileStream(images[i].FileName, FileMode.OpenOrCreate))
                    {
                        fs.Write(images[i].Data, 0, images[i].Data.Length);
                        Console.WriteLine($"Файл {images[i].Title} сохранен");
                    }
                }
            }
        }

        public class Image
        {
            public Image(int id, string filename, string title, byte[] data)
            {
                Id = id;
                FileName = filename;
                Title = title;
                Data = data;
            }
            public int Id { get; private set; }
            public string FileName { get; private set; }
            public string Title { get; private set; }
            public byte[] Data { get; private set; }
        }

    } // Program
} // namespace