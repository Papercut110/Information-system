using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InformationSystem
{
    public class Sorting : Form1
    {
        public enum SortingOptions
        {
            Name,
            SureName,
            Age,
            Department,
            Salary,
            Projects
        }

        public IEnumerable<Emploees> GetAllEmploees(List<object[]> list)
        {
            List<Emploees> empList = new List<Emploees>();

            foreach (var n in list)
            {
                empList.Add(n.ConvertArray());
            }

            return empList;
        }

        public IOrderedEnumerable<Emploees> Ordering(SortingOptions options, List<object[]> list)
        {
            IOrderedEnumerable<Emploees> filtered = default;

            var empList = GetAllEmploees(list);
            switch (options)
            {
                case SortingOptions.Age:
                    filtered = empList.OrderBy(x => x.Age);
                    break;
                case SortingOptions.Name:
                    filtered = empList.OrderBy(x => x.Name);
                    break;
                case SortingOptions.SureName:
                    filtered = empList.OrderBy(x => x.SureName);
                    break;
                case SortingOptions.Department:
                    filtered = empList.OrderBy(x => x.Department_id);
                    break;
                case SortingOptions.Salary:
                    filtered = empList.OrderBy(x => x.Salary);
                    break;
                case SortingOptions.Projects:
                    filtered = empList.OrderBy(x => x.Projects);
                    break;
            }
            

            return filtered;
        }

        public IOrderedEnumerable<Emploees> OrderingThen(SortingOptions options, IOrderedEnumerable<Emploees> emp)
        {
            IOrderedEnumerable<Emploees> filtered = default;

            switch (options)
            {
                case SortingOptions.Age:
                    filtered = emp.ThenBy(x => x.Age);
                    break;
                case SortingOptions.Name:
                    filtered = emp.ThenBy(x => x.Name);
                    break;
                case SortingOptions.SureName:
                    filtered = emp.ThenBy(x => x.SureName);
                    break;
                case SortingOptions.Department:
                    filtered = emp.ThenBy(x => x.Department_id);
                    break;
                case SortingOptions.Salary:
                    filtered = emp.ThenBy(x => x.Salary);
                    break;
                case SortingOptions.Projects:
                    filtered = emp.ThenBy(x => x.Projects);
                    break;
            }


            return filtered;
        }
    }
}
