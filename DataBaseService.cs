using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Linq;
using System.Runtime.Serialization.Formatters;
using System.Diagnostics;
using System.Windows.Forms;
using System.IO;
using System.Net.Configuration;

namespace InformationSystem
{
    public class DataBaseService : LinqToBaseDataContext
    {
        public static string path; // = "Data Source=DESKTOP-JKVR2L0\\SQLEXPRESS; Initial Catalog = InfoDB; User ID = sa; Password = raw1";
        static string connection;

        DataContext data;

        public DataBaseService()
        {
            if (path != "" && File.Exists(path))
            {
                connection = File.ReadAllText(path);
                data = new DataContext(connection);
            }
            else
            {
                MessageBox.Show("Укажите расположение файла конфигурации");
                OpenFileDialog file = new OpenFileDialog();

                if (file.ShowDialog() == DialogResult.OK)
                {
                    path = file.FileName;
                    connection = File.ReadAllText(path);
                    data = new DataContext(connection);
                }
            }
        }

        public int AddToBase(Dep obj, string department)
        {
            int id = 0;
            bool flag = false;
            IQueryable<bool> trigger = data.GetTable<Dep>().Select(dep => dep.Departament == department);

            foreach (bool e in trigger)
            {
                if (e)
                {
                    flag = true;
                }
            }

            if (!flag)
            {
                data.GetTable<Dep>().InsertOnSubmit(obj);
                data.SubmitChanges();
                
                IQueryable<int> deps = data.GetTable<Dep>().Select(dep => dep.Id);

                foreach (var idx in deps)
                {
                    id = idx;
                }

                return id;
            }
            else
            {   
                var table = data.GetTable<Dep>().Select((x) => new { Number = x.Departament, Idx = x.Id }).Where(x => x.Number == department);

                foreach (var e in table)
                {
                    id = e.Idx;
                    break;
                }

                flag = false;

                return id;
            }
        }

        public void AddToBase(Emploees obj)
        {
            bool trigger = false;
            var table = data.GetTable<Emploees>().Select(x => x.Name == obj.Name && x.SureName == obj.SureName && x.Age == obj.Age);
            
            foreach (var e in table)
            {
                if (e == true)
                {
                    trigger = true;
                }
            }

            if (!trigger)
            {
                data.GetTable<Emploees>().InsertOnSubmit(obj);
                data.SubmitChanges();
            }
            else
            {
                MessageBox.Show("Запись присутствует в базе");
            }
        }

        public int DepartmentSearch(string department)
        {
            int id = 0;
            bool flag = false;
            IQueryable<bool> trigger = data.GetTable<Dep>().Select(dep => dep.Departament == department);

            foreach (bool e in trigger)
            {
                if (e)
                {
                    flag = true;
                }
            }

            if (!flag)
            {
                return id = 0;
            }
            else
            {
                var table = data.GetTable<Dep>().Select((x) => new { Number = x.Departament, Idx = x.Id }).Where(x => x.Number == department);

                foreach (var e in table)
                {
                    id = e.Idx;
                    break;
                }

                flag = false;

                return id;
            }
        }

        public bool DepartmentSearchById(int id)
        {
            bool flag = false;

            var table = data.GetTable<Dep>().Where(x => x.Id == id);

            foreach (var n in table)
            {
                if (n != null)
                {
                    flag = true;
                    break;
                }
            }
            return flag;
        }

        public string DepartmentNameSearch(int id)
        {
            string depName = string.Empty;
            var table = data.GetTable<Dep>().Where(x => x.Id == id);

            foreach (var n in table)
            {
                depName = n.Departament;
            }

            return depName;
        }

        public List<object[]> Search(Emploees emp)
        {
            List<object[]> list = new List<object[]>();
            IQueryable<Emploees> table = data.GetTable<Emploees>();

            if (emp.Department_id != 0)
            {
                if (emp.Name != null)
                    table = table.Where(x => x.Name == emp.Name);
                if (emp.SureName != null)
                    table = table.Where(x => x.SureName == emp.SureName);
                if (emp.Age != 0)
                    table = table.Where(x => x.Age == emp.Age);
                if (emp.Salary != 0)
                    table = table.Where(x => x.Salary == emp.Salary);
                if (emp.Projects != 0)
                    table = table.Where(x => x.Projects == emp.Projects);

                table = table.Where(x => x.Department_id == emp.Department_id);
            }
            else
            {
                if (emp.Name != null)
                    table = table.Where(x => x.Name == emp.Name);
                if (emp.SureName != null)
                    table = table.Where(x => x.SureName == emp.SureName);
                if (emp.Age != 0)
                    table = table.Where(x => x.Age == emp.Age);
                if (emp.Salary != 0)
                    table = table.Where(x => x.Salary == emp.Salary);
                if (emp.Projects != 0)
                    table = table.Where(x => x.Projects == emp.Projects);

                
            }

            
            if (table != null)
            {
                foreach (var n in table)
                {
                    object[] obj = new object[6];

                    obj[0] = n.Name;
                    obj[1] = n.SureName;
                    obj[2] = n.Age;
                    obj[3] = n.Dep.Departament;
                    obj[4] = n.Salary;
                    obj[5] = n.Projects;
                    list.Add(obj);

                }


                return list;
            }
            else
            {
                MessageBox.Show("По вашему запросу ничего не найдено");

                return null;
            }
        }

        public bool SearchEmploees(Emploees emp)
        {
            List<object[]> list = new List<object[]>();
            IQueryable<Emploees> table = data.GetTable<Emploees>();

            if (emp.Department_id != 0)
            {
                if (emp.Name != null)
                    table = table.Where(x => x.Name == emp.Name);
                if (emp.SureName != null)
                    table = table.Where(x => x.SureName == emp.SureName);
                if (emp.Age != 0)
                    table = table.Where(x => x.Age == emp.Age);
                if (emp.Salary != 0)
                    table = table.Where(x => x.Salary == emp.Salary);
                if (emp.Projects != 0)
                    table = table.Where(x => x.Projects == emp.Projects);

                table = table.Where(x => x.Department_id == emp.Department_id);
            }
            else
            {
                if (emp.Name != null)
                    table = table.Where(x => x.Name == emp.Name);
                if (emp.SureName != null)
                    table = table.Where(x => x.SureName == emp.SureName);
                if (emp.Age != 0)
                    table = table.Where(x => x.Age == emp.Age);
                if (emp.Salary != 0)
                    table = table.Where(x => x.Salary == emp.Salary);
                if (emp.Projects != 0)
                    table = table.Where(x => x.Projects == emp.Projects);


            }


            if (table != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public int SearchEmploeesRowId(Emploees emp)
        {
            int id = 0;

            var table = data.GetTable<Emploees>().Where(x => x.Name == emp.Name && x.SureName == emp.SureName && x.Age == emp.Age && x.Department_id == emp.Department_id && x. Salary == emp.Salary && x.Projects == emp.Projects);

            foreach (Emploees e in table)
            {
                id = e.Id;
            }

            return id;
        }

        public bool SearchEmploeesById(int id)
        {
            bool flag = false;

            var table = data.GetTable<Emploees>().Where(x => x.Id == id);

            foreach (var n in table)
            {
                if (n.Id > 0)
                {
                    flag = true;
                    break;
                }
            }

            return flag;
        }

        public void DeleteRow(Emploees emp)
        {
            int id = SearchEmploeesRowId(emp);

            var table = data.GetTable<Emploees>().Where(x => x.Id == id);

            foreach (Emploees e in table)
            {
                data.GetTable<Emploees>().DeleteOnSubmit(e);
            }

            var idTable = data.GetTable<Emploees>().Where(x => x.Department_id == emp.Department_id);

            if (idTable.Count() == 1)
            {
                var depTable = data.GetTable<Dep>().Where(x => x.Id == emp.Department_id);

                foreach (Dep e in depTable)
                {
                    data.GetTable<Dep>().DeleteOnSubmit(e);
                    data.SubmitChanges();
                }
            }

            data.SubmitChanges();
        }

        public void ClearBase()
        {
            var empTable = data.GetTable<Emploees>();

            data.GetTable<Emploees>().DeleteAllOnSubmit(empTable);
            data.SubmitChanges();
            
            var depTable = data.GetTable<Dep>();

            data.GetTable<Dep>().DeleteAllOnSubmit(depTable);
            data.SubmitChanges();

            MessageBox.Show("База пуста");
        }

        public List<object[]> ShowAll()
        {
            List<object[]> list = new List<object[]>();
            IQueryable<Emploees> table = data.GetTable<Emploees>();

            if (table != null)
            {
                foreach (var n in table)
                {
                    object[] obj = new object[6];

                    obj[0] = n.Name;
                    obj[1] = n.SureName;
                    obj[2] = n.Age;
                    obj[3] = n.Dep.Departament;
                    obj[4] = n.Salary;
                    obj[5] = n.Projects;
                    list.Add(obj);

                }
            }
            return list;
        }
    }
}
