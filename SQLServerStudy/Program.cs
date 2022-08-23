// https://metanit.com/sharp/adonetcore/2.1.php
// В данном случае мы будем использовать к локальному серверу. Если мы подключаемся к полноценному серверу MS SQL Server (например, версия Developer Edition), то в качестве адреса сервера, как правило, выступает localhost:

using Microsoft.Data.SqlClient;
using System.Data;
using System;
using System.Threading.Tasks;
 
namespace HelloApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string connectionString = "Server=(localdb)\\mssqllocaldb;Database=master;Trusted_Connection=True;";
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

            Console.WriteLine("Программа завершила работу.");
            Console.Read();
        }
    }
}