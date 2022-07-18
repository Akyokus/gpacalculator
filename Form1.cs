using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Data.SQLite;
using System.IO;

namespace GPACalculator
{
    public partial class Form1 : Form
    {
       
        public Form1()
        {
            InitializeComponent();
        }
        public static SQLiteConnection connector = new SQLiteConnection(@"Data source = C:\Users\beyza\source\repos\GPACalculator\courses.sqlite;Version=3;");


        public void calculateGPA()
        {
          double gpa =  calculateCGPA(1) + calculateCGPA(2) + calculateCGPA(3) + calculateCGPA(4) + calculateCGPA(5) + calculateCGPA(6);
            gpa = gpa / 11;
            label3.Text = gpa.ToString(); 
        }
        public void deleteCourse(int id)
        {
            SQLiteCommand delCom = new SQLiteCommand("DELETE FROM courses WHERE id="+id,connector);
            connector.Open();
            delCom.ExecuteNonQuery();
            connector.Close();
            MessageBox.Show("Succesfull !");
        }
        public double calculateCGPA(int terms)
        {
            connector.Open();
            SQLiteCommand getCom = new SQLiteCommand("select * from courses where courseterm=" + terms, connector);
            SQLiteDataAdapter adapter = new SQLiteDataAdapter(getCom);
            DataTable data = new DataTable();
            adapter.Fill(data);
            double totalgrade = 0;
            double totalflo = 0;
            for(int i = 0; i < data.Rows.Count; i++)
            {
                String letter = letterGrade(Convert.ToInt32(data.Rows[i]["id"]));
                double flo = 0;
                if (letter == "AA")
                {
                    flo = 4.0;
                }
                else if (letter == "BA")
                {
                    flo = 3.5;
                }
                else if (letter == "BB")
                {
                    flo = 3.0;
                }
                else if (letter == "CB")
                {
                    flo = 2.5;
                }
                else if (letter == "CC")
                {
                    flo = 2.0;
                }
                else if (letter == "FF")
                {
                    flo = 0;
                }
                int grade = (Convert.ToInt32(data.Rows[i]["grade1"]) + Convert.ToInt32(data.Rows[i]["grade2"])) / 2;
                totalgrade = totalgrade + grade * flo;
                totalflo = totalflo + flo;

            }
            totalgrade = totalgrade / totalflo;
            connector.Close();
            return Math.Round(totalgrade,2);
        }
        public string letterGrade(int id)
        {
            String letter;
            SQLiteCommand getCom = new SQLiteCommand("select * from courses where id="+id, connector);
            SQLiteDataAdapter adapter = new SQLiteDataAdapter(getCom);
            DataTable data = new DataTable();
            adapter.Fill(data);
            int grade =0 ;
            if (data != null)
            {
                if(data.Rows.Count > 0)
                {
                    grade = (Convert.ToInt32(data.Rows[0]["grade1"]) + Convert.ToInt32(data.Rows[0]["grade2"])) / 2;
                }
               
            }
            
            if(grade > 0 && grade <= 49)
            {
                letter = "FF";
            }else if(grade >= 50 && grade < 59)
            {
                letter = "CC";
            }else if(grade >= 60 && grade < 69)
            {
                letter = "CB";
            }else if(grade >= 70 && grade < 79){
                letter = "BB";
            }else if(grade >= 80 && grade < 89)
            {
                letter = "BA";
            }else if(grade >= 90 && grade <= 100)
            {
                letter = "AA";
            }
            else
            {
                letter = "NULL";
            }
            connector.Close();
            return letter;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
           
           
            connector.Open();
            SQLiteCommand getCom = new SQLiteCommand("select * from courses", connector);
            SQLiteDataAdapter adapter = new SQLiteDataAdapter(getCom);
            DataTable data = new DataTable();
            connector.Close();
           adapter.Fill(data);
            letterGrade(6);


            for (int i = 0; i < data.Rows.Count; i++)
            {
                dataGridView1.Rows.Add(data.Rows[i]["id"].ToString(), data.Rows[i]["coursename"].ToString(), data.Rows[i]["courseterm"].ToString(), data.Rows[i]["ects"].ToString(), data.Rows[i]["grade1"].ToString(), data.Rows[i]["grade2"].ToString(), letterGrade(Convert.ToInt32(data.Rows[i]["id"])));

            }
            

        }
        public void refresh()
        {

            connector.Open();
            SQLiteCommand getCom = new SQLiteCommand("select * from courses", connector);
            SQLiteDataAdapter adapter = new SQLiteDataAdapter(getCom);
            DataTable data = new DataTable();
            connector.Close();
            adapter.Fill(data);
            letterGrade(6);


            for (int i = 0; i < data.Rows.Count; i++)
            {
                dataGridView1.Rows.Add(data.Rows[i]["id"].ToString(), data.Rows[i]["coursename"].ToString(), data.Rows[i]["courseterm"].ToString(), data.Rows[i]["ects"].ToString(), data.Rows[i]["grade1"].ToString(), data.Rows[i]["grade2"].ToString(), letterGrade(Convert.ToInt32(data.Rows[i]["id"])));

            }

        }


        private void button1_Click(object sender, EventArgs e)
        {
           
            Form2 addForm = new Form2();
            addForm.Show();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            calculateGPA();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            int term = Convert.ToInt32(comboBox1.SelectedItem);
            double cgpa = calculateCGPA(term);
            label4.Text = "CGPA = ";
            cgpa = Math.Round(cgpa, 2);
            label5.Text = cgpa.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
           
            deleteCourse(Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value));
        }

        private void button5_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            refresh();

        }
    }
}
