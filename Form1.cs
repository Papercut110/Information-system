using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InformationSystem
{
    public partial class Form1 : Form
    {
        Emploees emploees;
        Emploees emploeesDell;
        Dep dep;
        public List<object[]> list;
        int? row = null;
        Sorting.SortingOptions sort;
        Sorting.SortingOptions sort2;
        IOrderedEnumerable<Emploees> order;
        public string serverSettings;

        public Form1()
        {
            InitializeComponent();
        }
        
        private void Form1_Load(object sender, EventArgs e)
        {
            dataGridView1.CellClick += DataGridView1_CellClick;
        }

        private void DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            row = e.RowIndex;
            object[] obj;
            if (e.RowIndex != 0)
                obj = list.ElementAt(e.RowIndex);
            else
                return;

            emploeesDell = new Emploees()
            {
                Name = Convert.ToString(obj[0]),
                SureName = Convert.ToString(obj[1]),
                Age = Convert.ToInt32(obj[2]),
                Department_id = new DataBaseService().DepartmentSearch((string)obj[3]),
                Salary = Convert.ToInt32(obj[4]),
                Projects = Convert.ToInt32(obj[5])
            };
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dep = new Dep() { Departament = textBox4.Text };

            dep.Id = new DataBaseService().AddToBase(dep, dep.Departament);

            emploees = new Emploees()
            {
                Name = textBox1.Text,
                SureName = textBox6.Text,
                Age = Convert.ToInt32(textBox5.Text),
                Department_id = dep.Id,
                Salary = Convert.ToInt32(textBox3.Text),
                Projects = Convert.ToInt32(textBox2.Text)
            };

            new DataBaseService().AddToBase(emploees);

            MessageBox.Show("Запись добавлена");

            TextClear();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();

            if (list != null)
                list.Clear();

            if (textBox1.Text == "" && textBox6.Text == "" && textBox5.Text == "" && textBox3.Text == "" && textBox2.Text == "" && textBox4.Text == "")
            {
                MessageBox.Show("Не заполнено не одно поле");
                return;
            }
                

            DataBaseService dataBaseService = new DataBaseService();

            Emploees emploees = new Emploees();

            if (textBox1.Text != "")
                emploees.Name = textBox1.Text;
            if (textBox6.Text != "")
                emploees.SureName = textBox6.Text;
            if (textBox5.Text != "")
                emploees.Age = Convert.ToInt32(textBox5.Text);
            if (textBox3.Text != "")
                emploees.Salary = Convert.ToInt32(textBox3.Text);
            if (textBox2.Text != "")
                emploees.Projects = Convert.ToInt32(textBox2.Text);


            if (textBox4.Text != "")
            {
                dep = new Dep() { Departament = textBox4.Text };
                dep.Id = dataBaseService.DepartmentSearch(dep.Departament);
                if (dep.Id != 0)
                    emploees.Department_id = dep.Id;
            }

            list = new DataBaseService().Search(emploees);

            foreach (var n in list)
            {
                dataGridView1.Rows.Add(n);
            }
            list.Clear();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            new DataBaseService().DeleteRow(emploeesDell);

            if (row != null)
            {
                dataGridView1.Rows.RemoveAt((int)row);
                row = null;
            }
            else
            {
                MessageBox.Show("Косяк с индексом списка отображения DataGridView");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            new DataBaseService().ClearBase();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();

            if (list != null)
                list.Clear();

            list = new DataBaseService().ShowAll();

            foreach (var n in list)
            {
                dataGridView1.Rows.Add(n);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows != null)
            {
                dataGridView1.Rows.Clear();
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (list != null)
            {
                new XmlProcessing().Serialize(list);
                MessageBox.Show("Данные выгружены в файл");
            }
            else
            {
                MessageBox.Show("Нет данных для выгрузки");
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            new XmlProcessing().Deserialize();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (list != null)
            {
                new JsonProcessing().Serialize(list);
                MessageBox.Show("Данные выгружены в файл");
            }
            else
            {
                MessageBox.Show("Нет данных для выгрузки");
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            new JsonProcessing().Deserialize();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();

            if (file.ShowDialog() == DialogResult.OK)
            {
                DataBaseService.path = file.FileName;
            }


            //using (Stream stream = file.OpenFile())
            //{
            //    using (StreamReader reader = new StreamReader(stream))
            //    {
            //        serverSettings = reader.ReadToEnd();
            //    }
            //}
        }



        private void button12_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text == "Пусто" && comboBox2.Text == "Пусто") return;                

            var sorting = new Sorting();


            if (list != null && comboBox1.Text != "Пусто" && comboBox2.Text == "Пусто")
            {
                sort = SpecifySettings(comboBox1.Text);
                order = sorting.Ordering(sort, list);

                dataGridView1.Rows.Clear();

                foreach (var item in order)
                {
                    dataGridView1.Rows.Add(new object[] { item.Name, item.SureName, item.Age, new DataBaseService().DepartmentNameSearch(item.Department_id), item.Salary, item.Projects });
                }
            }
            else if (list != null && comboBox1.Text != "Пусто" && comboBox2.Text != "Пусто")
            {
                sort = SpecifySettings(comboBox1.Text);
                sort2 = SpecifySettings(comboBox2.Text);

                order = sorting.OrderingThen(sort2, sorting.Ordering(sort, list));

                dataGridView1.Rows.Clear();

                foreach (var item in order)
                {
                    dataGridView1.Rows.Add(new object[] { item.Name, item.SureName, item.Age, new DataBaseService().DepartmentNameSearch(item.Department_id), item.Salary, item.Projects });
                }
            }
            else
            {
                MessageBox.Show("Таблица пуста");
            }
        }


        #region TextBoxes
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            LetterFieldCheck(textBox1);
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            LetterFieldCheck(textBox6);
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            DigitFieldCheck(textBox5);
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            DigitFieldCheck(textBox3);
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            DigitFieldCheck(textBox2);
        }

        #endregion


        #region Helper methods
        private void LetterFieldCheck(TextBox textBox)
        {
            bool flag = false;
            char[] symbols = textBox.Text.ToCharArray();

            foreach (char i in symbols)
            {
                if (!char.IsLetter(i))
                {
                    textBox.Clear();
                    flag = true;
                }
            }
            if (flag)
            {
                MessageBox.Show("Некорректный символ");
            }
        }

        private void DigitFieldCheck(TextBox textBox)
        {
            bool flag = false;
            char[] symbols = textBox.Text.ToCharArray();

            foreach (char i in symbols)
            {
                if (char.IsLetter(i) || char.IsWhiteSpace(i))
                {
                    textBox.Clear();
                    flag = true;
                }
            }
            if (flag)
            {
                MessageBox.Show("Некорректный символ");
            }
        }

        private void TextClear()
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            textBox6.Clear();
        }

        private Sorting.SortingOptions SpecifySettings(string text)
        {
            Sorting.SortingOptions sorting = default;

            if (text != "")
            {
                switch (text)
                {
                    case "Имя":
                        sorting = Sorting.SortingOptions.Name;
                        break;
                    case "Фамилия":
                        sorting = Sorting.SortingOptions.SureName;
                        break;
                    case "Возраст":
                        sorting = Sorting.SortingOptions.Age;
                        break;
                    case "Департамент":
                        sorting = Sorting.SortingOptions.Department;
                        break;
                    case "Оплата труда":
                        sorting = Sorting.SortingOptions.Salary;
                        break;
                    case "Колл-во проектов":
                        sorting = Sorting.SortingOptions.Projects;
                        break;
                }
            }
            else
            {
                MessageBox.Show("Укажите критерий для сортировки");
            }
            return sorting;
        }






        #endregion

        
    }
}
