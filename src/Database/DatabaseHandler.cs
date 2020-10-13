using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Discord;
using MySql.Data.MySqlClient;
using System.Data;
using System.Diagnostics;
using HuiswerkBot.Modules;
using ConnectionState = System.Data.ConnectionState;

namespace HuiswerkBot.Database
{
    internal class DatabaseHandler
    {
        private readonly string _connectionString = "server=192.168.2.63;uid=decodos;password=toor;Database=hw_test;";
        private readonly MySqlConnection _connection;
        public bool Connected { get => (_connection.State == ConnectionState.Open); }

        public DatabaseHandler()
        {
            try
            {
                _connection = new MySqlConnection(_connectionString);
                _connection.StateChange += this.Connection_StateChange;
                _connection.InfoMessage += this.Connection_InfoMessage;
                _connection.Open();
                if (_connection.State == ConnectionState.Open)
                {
                    Console.WriteLine($"Succesfully connected to {_connection.Database}!");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public DatabaseHandler(object config)
        {
            Console.WriteLine($"Using non-supported constructor at: {new StackFrame().GetFileName()}: {new StackFrame().GetFileLineNumber()}");
            try
            {
                _connection = new MySqlConnection(_connectionString);
                _connection.StateChange += this.Connection_StateChange;
                _connection.InfoMessage += this.Connection_InfoMessage;
                _connection.Open();
                if (_connection.State == ConnectionState.Open)
                {
                    Console.WriteLine($"Succesfully connected to {_connection.Database}!");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private void Connection_InfoMessage(object sender, MySqlInfoMessageEventArgs args)
        {
            int i = 0;
            while (i < args.errors.Length)
            {
                Console.WriteLine($"InfoMessage: {args.errors[i]}");
                i++;
            }
        }

        private void Connection_StateChange(object sender, StateChangeEventArgs e)
        {
            Console.WriteLine($"Connection state changed to: {e.CurrentState}");
        }

        internal async Task OpenAsync()
        {
            try
            {
                if (!this.Connected) await _connection.OpenAsync();
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
        internal async Task Insert(string subject, string description, DateTime deadline, DateTime finisedDate, string author, string authorGroup = "", string authorAvatar = "", bool finised = false)
        {
            try
            {
                await OpenAsync();
                if (Connected)
                {
                    // Setup the command to execute
                    MySqlCommand command = new MySqlCommand
                    {
                        Connection = _connection,
                        CommandText =
                            $"INSERT INTO `huiswerk`(`subject`, `description`, `deadline`, `author`, `authorGroup`, `authorAvatar` ,`creation_date`, `finished`, `finished_date`) VALUES (?subject, ?description, ?deadline, ?author, ?authorGroup, ?authorAvatar, ?creation_date, ?finished, ?finished_date)"
                    };

                    command.Parameters.AddWithValue("?subject", subject);
                    command.Parameters.AddWithValue("?description", description);
                    command.Parameters.AddWithValue("deadline", deadline);
                    command.Parameters.AddWithValue("?author", author);
                    command.Parameters.AddWithValue("?authorGroup", authorGroup);
                    command.Parameters.AddWithValue("?authorAvatar", authorAvatar);
                    command.Parameters.AddWithValue("?creation_date", DateTime.Now);
                    command.Parameters.AddWithValue("?finished", finised);
                    command.Parameters.AddWithValue("?finished_date", new DateTime(DateTime.Now.Ticks + new DateTime(2099, 12, 31).Ticks));

                    await command.PrepareAsync();

                    if (await command.ExecuteNonQueryAsync() > 0)
                    {
                        // Console.WriteLine($"{command.ExecuteReader().RecordsAffected} rows have been inserted.");
                    }
                }
                _connection.Close();
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
                    Connection = _connection,
                    CommandText =
                        $"SELECT * FROM `huiswerk` WHERE deadline > CURDATE() ORDER BY `deadline` ASC LIMIT 0, ?amount"
                };

                // Add value to parameter
                command.Parameters.AddWithValue("?amount", amount);

                // Prepare query.
                await command.PrepareAsync();

                MySqlDataReader reader = command.ExecuteReader();
                int i = 0;
                while (await reader.ReadAsync())
                {
                    Huiswerk huiswerk = new Huiswerk();
                    huiswerk.ID = reader["hw_id"];
                    huiswerk.Subject = reader["subject"];
                    huiswerk.Description = reader["description"];
                    huiswerk.Deadline = reader["deadline"];
                    huiswerk.Author = reader["author"];
                    huiswerk.AuthorGroup = reader["author_group"];
                    huiswerk.AuthorAvatar = reader["author_avatar"];
                    huiswerk.CreationDate = reader["creation_date"];
                    huiswerk.Finished = reader["finished"];
                    huiswerk.FinishedDate = reader["finished_date"];
                    result[i] = huiswerk;

                    Console.WriteLine($"huiswerk: {huiswerk.ID}");
                    Console.WriteLine($"result: {result[i].ID}");
                    i++;
                }

                reader.Close();
                _connection.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return result;
        }
    }
}