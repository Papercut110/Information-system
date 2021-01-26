using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace InformationSystem
{
    public class SourceForXmlJson
    {
        public int EmployerId { get; set; }
        public string EmployerName { get; set; }
        public string EmloyerSureName { get; set; }
        public int EmployerAge { get; set; }
        public int EmployerDepId { get; set; }
        public decimal EmployerSalary { get; set; }
        public int EmployerProjects { get; set; }
        public string DepName { get; set; }

        public Emploees Convert(SourceForXmlJson source)
        {
            return new Emploees()
            {
                Name = source.EmployerName,
                SureName = source.EmloyerSureName,
                Age = source.EmployerAge,
                Department_id = source.EmployerDepId,
                Salary = source.EmployerSalary,
                Projects = source.EmployerProjects
            };
        }
    }
}
