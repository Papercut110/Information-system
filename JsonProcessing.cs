using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InformationSystem
{
    class JsonProcessing : Form1
    {
        string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\JsonEmploees.json";

        public void Serialize(List<object[]> list)
        {
            List<SourceForXmlJson> emploeesList = new List<SourceForXmlJson>();
            foreach (var n in list)
            {
                object[] obj = n;

                SourceForXmlJson source = new SourceForXmlJson()
                {
                    EmployerId = new DataBaseService().SearchEmploeesRowId(new Emploees()
                    {
                        Name = Convert.ToString(obj[0]),
                        SureName = Convert.ToString(obj[1]),
                        Age = Convert.ToInt32(obj[2]),
                        Department_id = new DataBaseService().DepartmentSearch((string)obj[3]),
                        Salary = Convert.ToInt32(obj[4]),
                        Projects = Convert.ToInt32(obj[5])
                    }),
                    EmployerName = Convert.ToString(obj[0]).StringProcessing(),
                    EmloyerSureName = Convert.ToString(obj[1]).StringProcessing(),
                    EmployerAge = Convert.ToInt32(obj[2]),
                    EmployerDepId = new DataBaseService().DepartmentSearch(((string)obj[3]).StringProcessing()),
                    EmployerSalary = Convert.ToInt32(obj[4]),
                    EmployerProjects = Convert.ToInt32(obj[5]),
                    DepName = ((string)obj[3]).StringProcessing()
                };
                emploeesList.Add(source);
            }
            ProcessSerialize(emploeesList);
        }

        private void ProcessSerialize(List<SourceForXmlJson> emp)
        {
            string json = JsonConvert.SerializeObject(emp);

            File.WriteAllText(path, json);
        }

        public void Deserialize()
        {
            List<SourceForXmlJson> list = ProcessDeserialize();

            if (list != null)
            {
                DialogResult dialogResult = MessageBox.Show("Желаете ли вы добавить отсутствующие записи в базу?", "Сообщение", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    foreach (var n in list)
                    {
                        if (!new DataBaseService().SearchEmploeesById(n.EmployerId))
                        {
                            if (!new DataBaseService().DepartmentSearchById(n.EmployerDepId) && new DataBaseService().DepartmentSearch(n.DepName) == 0)
                            {
                                Dep dep = new Dep()
                                {
                                    Departament = n.DepName
                                };

                                int id = new DataBaseService().AddToBase(dep, n.DepName);
                                new DataBaseService().AddToBase(new Emploees
                                {
                                    Name = n.EmployerName,
                                    SureName = n.EmloyerSureName,
                                    Age = n.EmployerAge,
                                    Department_id = id,
                                    Salary = n.EmployerSalary,
                                    Projects = n.EmployerProjects
                                });
                            }
                            else
                            {
                                new DataBaseService().AddToBase(new Emploees
                                {
                                    Name = n.EmployerName,
                                    SureName = n.EmloyerSureName,
                                    Age = n.EmployerAge,
                                    Department_id = n.EmployerDepId,
                                    Salary = n.EmployerSalary,
                                    Projects = n.EmployerProjects
                                });
                            }
                        }
                    }
                }
            }
        }

        private List<SourceForXmlJson> ProcessDeserialize()
        {
            List<SourceForXmlJson> list;
            string json = string.Empty;

            if (File.Exists(path))
            {
                json = File.ReadAllText(path);
                list = JsonConvert.DeserializeObject<List<SourceForXmlJson>>(json);
                return list;
            }
            else
            {
                MessageBox.Show("Необнаружено файла для десериализации");
                return null;
            }
        }
    }
}
