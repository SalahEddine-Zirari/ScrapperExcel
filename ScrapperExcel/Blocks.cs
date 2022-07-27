using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ScrapperExcel
{
    public class Blocks :  Generic<Blocks>
    {
        public DateOnly Date { get; set; }
        public string OffPeak1 { get; set; }
        public string OffPeak2 { get; set; }
        public string MiddleNight { get; set; }
        public string Night { get; set; }
        public string EarlyMorning { get; set; }
        public string Morning { get; set; }
        public string LateMorning { get; set; }
        public string Business { get; set; }
        public string HighNoon { get; set; }
        public string EarlyAfternoon { get; set; }
        public string Afternoon { get; set; }
        public string RushHour { get; set; }
        public string Evening { get; set; }



        public List<Blocks> DtToObj(DataTable dt)
        {
            var Data = new List<Blocks>();

            var row = 4;
            var date = dt.Rows[1][0].ToString();

            DateOnly OgDate = DateOnly.ParseExact(date, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            OgDate = OgDate.AddDays(-6);

            for (int i = 1; i < dt.Columns.Count; i++)
            {
                var obj = new Blocks();
                obj.Date = OgDate;
                foreach (PropertyInfo prop in obj.GetType().GetProperties())
                {
                   
                    var type = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
                    if (type == typeof(string))
                    {
                        prop.SetValue(obj, dt.Rows[row][i].ToString());
                        row++;
                    }

                };

                Data.Add(obj);
                OgDate = OgDate.AddDays(1);
                row = 4;
            }
            return Data;
        }


        public override string ToString()
        {
            return $"{Date} \n{OffPeak1} \n{OffPeak2} \n{MiddleNight}\n{Night}\n.\n.\n. \n\n  ";
        }
    }
}

