using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace DatabaseTesting
{
    public class DatabaseHandler
    {
        private readonly string connectionString = "server=192.168.2.63;uid=decodos;password=toor;Database=hw_test;";
        private readonly MySqlConnection connection;
        private bool Connected { get => (connection.State == ConnectionState.Open); }

        public DatabaseHandler()
        {
            try
            {
                connection = new MySqlConnection(connectionString);
                connection.StateChange += this.Connection_StateChange;
                connection.InfoMessage += this.Connection_InfoMessage;
                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    Debug.WriteLine($"Succesfully connected to {connection.Database}!");
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        private void Connection_InfoMessage(object sender, MySqlInfoMessageEventArgs args)
        {
            int i = 0;
            while (i < args.errors.Length)
            {
                Debug.WriteLine($"InfoMessage: {args.errors[i]}");
                i++;
            }
        }

        private void Connection_StateChange(object sender, StateChangeEventArgs e)
        {
            Debug.WriteLine($"Connection state changed to: {e.CurrentState}");
        }

        public async Task OpenAsync()
        {
            try
            {
                if (!this.Connected) await connection.OpenAsync();
                else Debug.WriteLine("Connection already open!");
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }


        /// <summary>
        /// Perform INSERT query in specified Database
        /// </summary>
        /// <param name="Table"></param>
        /// <returns></returns>
        public async Task Insert(string Table)
        {
            try
            {
                await OpenAsync();
                if (Connected)
                {
                    // Setup the command to execute
                    MySqlCommand command = new MySqlCommand
                    {
                        Connection = connection,
                        CommandText =
                            $"INSERT INTO {Table} (huiswerk_text, made_by) VALUES (?huiswerk_text, ?made_by)"
                    };

                    command.Parameters.AddWithValue("?huiswerk_text", "test");
                    command.Parameters.AddWithValue("?made_by", "test");

                    await command.PrepareAsync();

                    if (await command.ExecuteNonQueryAsync() > 0)
                    {
                        Debug.WriteLine($"{command.ExecuteReader().RecordsAffected} rows have been inserted.");
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        public async Task Select(string Table)
        {
            try
            {
                MySqlCommand command = new MySqlCommand
                {
                    Connection = connection,
                    CommandText =
                        $"SELECT * from {Table} WHERE finished = 0"
                };


                // command.Parameters.AddWithValue("@table", Table);

                await command.PrepareAsync();
                MySqlDataReader reader = command.ExecuteReader();
                while (await reader.ReadAsync())
                {
                    Debug.WriteLine(reader["description"]);
                }

                reader.Close();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }
    }
}