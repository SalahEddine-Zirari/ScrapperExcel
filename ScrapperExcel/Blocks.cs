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

            var date = dt.Rows[1][0].ToString();
            DateOnly OgDate = DateOnly.ParseExact(date, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            OgDate = OgDate.AddDays(-6);

            for (int i = 1; i < dt.Columns.Count; i++)
            {
            //    var obj = new Blocks();

            //    foreach (PropertyInfo prop in obj.GetType().GetProperties())
            //    {
            //        Date = OgDate;
            //        var row = 4;
            //        var type = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
            //        if (type == typeof(string))
            //        {
            //            prop.SetValue(obj, dt.Rows[row][i].ToString());
            //            row++;
            //        }
                    
            //    };

                var obj = new Blocks()

                {
                    Date = OgDate,
                    OffPeak1 = dt.Rows[4][i].ToString(),
                    OffPeak2 = dt.Rows[5][i].ToString(),
                    MiddleNight = dt.Rows[6][i].ToString(),
                    Night = dt.Rows[7][i].ToString(),
                    EarlyMorning = dt.Rows[8][i].ToString(),
                    Morning = dt.Rows[9][i].ToString(),
                    LateMorning = dt.Rows[10][i].ToString(),
                    Business = dt.Rows[11][i].ToString(),
                    HighNoon = dt.Rows[12][i].ToString(),
                    EarlyAfternoon = dt.Rows[13][i].ToString(),
                    Afternoon = dt.Rows[14][i].ToString(),
                    RushHour = dt.Rows[15][i].ToString(),
                    Evening = dt.Rows[16][i].ToString()
                };

                Data.Add(obj);
                OgDate = OgDate.AddDays(1);
            }
            return Data;
        }


        public override string ToString()
        {
            return $"{Date} \n{OffPeak1} \n{OffPeak2} \n{MiddleNight}\n{Night}\n \n  ";
        }
    }
}
