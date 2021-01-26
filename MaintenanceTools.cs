using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InformationSystem
{
    public static class MaintenanceTools
    {
        public static string StringProcessing(this string text)
        {
            string word = string.Empty;
            char[] symbols = text.ToCharArray();

            foreach (var n in symbols)
            {
                if (!char.IsWhiteSpace(n))
                    word += n;
            }

            return word;
        }

        public static Emploees ConvertArray(this object[] obj)
        {
            var emp = new Emploees()
            {
                Name = Convert.ToString(obj[0]),
                SureName = Convert.ToString(obj[1]),
                Age = Convert.ToInt32(obj[2]),
                Department_id = new DataBaseService().DepartmentSearch((string)obj[3]),
                Salary = Convert.ToInt32(obj[4]),
                Projects = Convert.ToInt32(obj[5])
            };

            return emp;
        }
    }
}
