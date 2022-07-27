using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrapperExcel
{
    public class BaseLoadPeakLoad : Generic<BaseLoadPeakLoad>
    {

        public DateOnly Date { get; set; }
        public double Price { get; set; }
        public double Volume { get; set; }


       

       

        public  List<BaseLoadPeakLoad> DtToObj(DataTable dt)
        {
            var Data = new List<BaseLoadPeakLoad>();

            var date = dt.Rows[1][0].ToString();
            DateOnly OgDate = DateOnly.ParseExact(date, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            OgDate = OgDate.AddDays(-6);

            for (int i = 1; i < dt.Columns.Count; i++)
            {
                var obj = new BaseLoadPeakLoad
                {
                    Date = OgDate,
                    Price = (double)dt.Rows[4][i],
                    Volume = (double)dt.Rows[5][i]
                };

                Data.Add(obj);
                OgDate = OgDate.AddDays(1);
            }
            return Data;
        }


        public override string ToString()
        {
            return $"{Date} \n{Price} \n{Volume}\n \n  ";
        }

    }
}
