using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Discord;
using MySql.Data.MySqlClient;
using System.Data;
using System.Diagnostics;
using HuiswerkBot.Modules;
using HuiswerkBot.Helpers;
using Microsoft.Extensions.Configuration;
using ConnectionState = System.Data.ConnectionState;
using MySqlX.XDevAPI.Common;
using Microsoft.Extensions.DependencyInjection;
using HuiswerkBot.Services;

namespace HuiswerkBot.Database
{
    internal class DatabaseHandler
    {
        private readonly MySqlConnection _connection;
        private readonly IServiceProvider _serivces;

        public bool Connected => (this._connection.State == ConnectionState.Open);

        public DatabaseHandler(IServiceProvider services, IConfiguration config)
        {
            try
            {

                this._serivces = services;
                // ReSharper disable once UseObjectOrCollectionInitializer
                this._connection = new MySqlConnection();
                this._connection.ConnectionString = config["dbConfig:connectionString"];
                this._connection.StateChange += Connection_StateChange;
                this._connection.InfoMessage += Connection_InfoMessage;
                Open(this._connection);
                if (this._connection.State == ConnectionState.Open)
                {
                    Console.WriteLine($"Successfully connected to {this._connection.Database}!");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private static void Open(MySqlConnection connection)
        {
            connection.Open();
        }

        private static void Connection_InfoMessage(object sender, MySqlInfoMessageEventArgs args)
        {
            int i = 0;
            while (i < args.errors.Length)
            {
                Console.WriteLine($"InfoMessage: {args.errors[i]}");
                i++;
            }
        }

        private static void Connection_StateChange(object sender, StateChangeEventArgs e)
        {
            Console.WriteLine($"Connection state changed to: {e.CurrentState}");
        }

        internal async Task OpenAsync()
        {
            try
            {
                if (!this.Connected) await this._connection.OpenAsync();
                else Console.WriteLine("Connection already open!");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// Perform INSERT of Huiswerk into the database
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="description"></param>
        /// <param name="deadline"></param>
        /// <param name="finisedDate"></param>
        /// <param name="author"></param>
        /// <param name="authorGroup"></param>
        /// <param name="finised"></param>
        /// <returns></returns>
        internal async Task Insert(string subject, string description, string deadline, DateTime finisedDate,
            string author, string authorGroup = "", string authorAvatar = "", bool finised = false)
        {
            try
            {
                await this.OpenAsync();
                if (this.Connected)
                {


                    // Setup the command to execute
                    MySqlCommand command = new MySqlCommand
                    {
                        Connection = this._connection,
                        CommandText =
                            $"INSERT INTO `huiswerk`(`subject`, `description`, `deadline`, `author`, `author_group`, `author_avatar` ,`creation_date`, `finished`, `finished_date`) VALUES (?subject, ?description, ?deadline, ?author, ?authorGroup, ?authorAvatar, ?creation_date, ?finished, ?finished_date)"
                    };

                    DateTime ffs = DateTimeHelper.FormatDateTime(deadline, "MM/dd/yyyy");

                    command.Parameters.AddWithValue("?subject", subject);
                    command.Parameters.AddWithValue("?description", description);
                    command.Parameters.AddWithValue("deadline", ffs);
                    command.Parameters.AddWithValue("?author", author);
                    command.Parameters.AddWithValue("?authorGroup", authorGroup);
                    command.Parameters.AddWithValue("?authorAvatar", authorAvatar);
                    command.Parameters.AddWithValue("?creation_date", DateTime.Now);
                    command.Parameters.AddWithValue("?finished", finised);
                    command.Parameters.AddWithValue("?finished_date", new DateTime(2099, 12, 31));

                    await command.PrepareAsync();

                    if (await command.ExecuteNonQueryAsync() > 0)
                    {
                        // Console.WriteLine($"{command.ExecuteReader().RecordsAffected} rows have been inserted.");
                    }
                }

                this._connection.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        internal async Task<HuiswerkList> Read(int amount = 5)
        {
            HuiswerkList result = new HuiswerkList(5);
            try
            {
                await OpenAsync();
                MySqlCommand command = new MySqlCommand
                {
                    Connection = this._connection,
                    CommandText =
                        $"SELECT * FROM `huiswerk` WHERE deadline > CURDATE() AND finished = 0 ORDER BY `deadline` ASC LIMIT 0,  ?amount"
                };

                // Add value to parameter
                command.Parameters.AddWithValue("?amount", amount);

                // Prepare query.
                await command.PrepareAsync();

                MySqlDataReader reader = command.ExecuteReader();
                int i = 0;
                while (await reader.ReadAsync())
                {
                    Huiswerk huiswerk = new Huiswerk
                    {
                        ID = reader["hw_id"],
                        Subject = reader["subject"],
                        Description = reader["description"],
                        Deadline = reader["deadline"],
                        Author = reader["author"],
                        AuthorGroup = reader["author_group"],
                        AuthorAvatar = reader["author_avatar"],
                        CreationDate = reader["creation_date"],
                        Finished = reader["finished"],
                        FinishedDate = reader["finished_date"],
                    };
                    result[i] = huiswerk;

                    Console.WriteLine($"huiswerk: {huiswerk.ID}");
                    Console.WriteLine($"result: {result[i].ID}");
                    i++;
                }

                reader.Close();
                this._connection.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return result;
        }

        internal async Task Delete(int hwID)
        {
            int result;
            try
            {
                await this.OpenAsync();
                if (this.Connected)
                {
                    MySqlCommand command = new MySqlCommand
                    {
                        Connection = this._connection,
                        CommandText =
                            $"UPDATE huiswerk SET finished = 1 WHERE hw_id = ?hw_id"
                    };

                    command.Parameters.AddWithValue("?hw_id", hwID);
                    result = await command.ExecuteNonQueryAsync();
                    await this._serivces.GetRequiredService<Logging>().Write($"result of deleting: {result}");
                }
                
                this._connection.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}