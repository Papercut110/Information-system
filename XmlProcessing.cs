using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace InformationSystem
{
    class XmlProcessing : Form1
    {
        string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\XmlEmploees.xml";
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
            XmlSerializer serializer = new XmlSerializer(typeof(List<SourceForXmlJson>), new XmlRootAttribute("SourceForXmlJson"));

            using (Stream stream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                serializer.Serialize(stream, emp);
            }
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
            List<SourceForXmlJson> list = new List<SourceForXmlJson>();

            XmlSerializer serializer = new XmlSerializer(typeof(List<SourceForXmlJson>), new XmlRootAttribute("SourceForXmlJson"));

            using (Stream stream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                list = serializer.Deserialize(stream) as List<SourceForXmlJson>;
            }

            Debug.WriteLine(list.Count);

            return list;
        }
    }
}

