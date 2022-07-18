using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SQLite;

namespace GPACalculator
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }
        public static SQLiteConnection connector = new SQLiteConnection(@"Data source = C:\Users\beyza\source\repos\GPACalculator\courses.sqlite;Version=3;");

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            String c_name = textBox1.Text;
            int grade_1 = Convert.ToInt32(textBox3.Text);
            int grade_2 = Convert.ToInt32(textBox4.Text);
            int ects = Convert.ToInt32(textBox5.Text);
            int term = Convert.ToInt32(comboBox1.SelectedItem);
            MessageBox.Show(c_name + grade_1 + grade_2 + ects);
            connector.Open();
            SQLiteCommand query = new SQLiteCommand("insert into courses (coursename,courseterm,ects,grade1,grade2) values ('" + c_name + "','" + term + "','" + ects + "','" + grade_1 + "','" + grade_2 + "')", connector);
            object res = query.ExecuteNonQuery();
            connector.Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
