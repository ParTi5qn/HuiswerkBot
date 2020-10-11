using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace HuiswerkBot.Database
{
    public class DatabaseHandler
    {
        private readonly MySqlConnection _conn;
        private readonly IConfigurationRoot _config;
        public bool isConnected { get {return _conn.State == System.Data.ConnectionState.Open;} }   

        public DatabaseHandler()
        {
            _conn = new MySqlConnection();
        }

        public DatabaseHandler(IConfigurationRoot config)
        {
            _config = config;
            _conn = new MySqlConnection();
        }

        public async Task<MySqlConnection> Connect(string DBConnectionString = "server=192.168.2.63;uid=decodos;" + "password=toor;database=huiswerk")
        {
            await Task.Run( () => {
                try{
                    _conn.ConnectionString = DBConnectionString;
                    _conn.Open();
                    if(_conn.State == System.Data.ConnectionState.Open)
                    {
                        Console.WriteLine($"Connected to: {_conn.Database}");
                    }
                }
                catch(MySqlException e){
                    Console.WriteLine(e.Message);
                }
            });
            return _conn;
        }

        public async Task InsertHuiswerk(string Huiswerk, string Username)
        {
             if(! (_conn.State == System.Data.ConnectionState.Open)) return;

            try{
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _conn;
                cmd.CommandText = $"INSERT INTO huiswerk.huiswerk (huiswerk.huiswerk_text, huiswerk.made_by) VALUES('{Huiswerk}', '{Username}')";
                cmd.Prepare();

                if(await cmd.ExecuteNonQueryAsync() > 0)
                {
                    Console.WriteLine($"Qeury: {cmd.CommandText} executed succesfully. ");
                }
            }
            catch(MySqlException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public async Task<EmbedBuilder> GetHuiswerk()
        {
            EmbedBuilder reply = new EmbedBuilder();   
            if(! (_conn.State == System.Data.ConnectionState.Open)) return null;

            try{
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _conn;
                cmd.CommandText = $"SELECT huiswerk.huiswerk_text FROM huiswerk";
                cmd.Prepare();
    
                MySqlDataReader reader = cmd.ExecuteReader();
                while(await reader.ReadAsync())
                {
                    reply.AddField(x => {
                        x.Value = reader["huiswerk_text"];
                    });
                    Console.WriteLine(reader["huiswerk_text"]);
                }
                reader.Close();
                Console.WriteLine($"Query: \"{cmd.CommandText}\" executed succesfully.");
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            
            _conn.Close();
            Console.WriteLine("GetFromTable is done.");
            return reply;
        }
    }
}