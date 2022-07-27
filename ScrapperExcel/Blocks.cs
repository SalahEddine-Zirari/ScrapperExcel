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
            DateTime OgDate = DateTime.ParseExact(date, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            OgDate = OgDate.AddDays(-6);

            for (int i = 0; i < dt.Columns.Count; i++)
            {
                    var obj = new Blocks();
                
                    foreach(PropertyInfo prop in obj.GetType().GetProperties())
                    {
                        
                        var row = 4;
                        var type = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
                        if (type == typeof(string))
                        {
                            prop.SetValue(obj,dt.Rows[row][i].ToString());
                            row++;
                        }


                    };

               //OffPeak1 = (double)dt.Rows[][i],
               //OffPeak2 = (double)dt.Rows[][i],
               //MiddleNight = (double)dt.Rows[][i],
               //Night = (double)dt.Rows[][i],
               //EarlyMorning = (double)dt.Rows[][i],
               //Morning = (double)dt.Rows[][i],
               //LateMorning = (double)dt.Rows[][i],
               //Business = (double)dt.Rows[][i],
               //HighNoon = (double)dt.Rows[][i],
               //EarlyAfternoon = (double)dt.Rows[][i],
               //Afternoon = (double)dt.Rows[][i],
               //RushHour = (double)dt.Rows[][i],
               //Evening = (double)dt.Rows[][i]

               ;

                Data.Add(obj);
                OgDate = OgDate.AddDays(1);
            }
            return Data;
        }


        public override string ToString()
        {
            return $"{OffPeak1} \n{HighNoon} \n{Evening}\n \n  ";
        }
    }
}
