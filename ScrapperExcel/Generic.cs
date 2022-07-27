using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrapperExcel
{
    public class Generic<T> where T : class
    {
        public DataTable GetLoad(int sheetNumber)
        {
            var stream = File.Open(Path.GetFullPath(@"C:\Users\Cheato\Downloads\data.xlsx"), FileMode.Open, FileAccess.Read);

            var reader = ExcelReaderFactory.CreateReader(stream);

            var result = reader.AsDataSet();

            var tables = result.Tables.Cast<DataTable>();

            var table = tables.ElementAt(sheetNumber);

            return table;
        }
        public void CheckObjList(List<T> dtToObj)
        {
            foreach (var item in dtToObj)
            {
                Console.WriteLine(item.ToString());
            }
        }
    }
}
