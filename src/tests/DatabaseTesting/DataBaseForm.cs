using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DatabaseTesting
{
    public partial class DataBaseForm : Form
    {
        private readonly DatabaseHandler _dbHandler;

        public DataBaseForm()
        {
            InitializeComponent();
            _dbHandler = new DatabaseHandler();
        }

        private async void btn_OpenConnection_Click(object sender, EventArgs e)
        {
            await _dbHandler.OpenAsync();
            output.Text += "Connection opened \r\n";
        }

        private async void btn_Insert_Click(object sender, EventArgs e)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("huiswerk_text", "DATABASETESTING");
            parameters.Add("made_by", "DATABSETESTING");

            await _dbHandler.Insert("huiswerk");
            output.Text += "Insertion complete \r\n";
        }
    }
}
