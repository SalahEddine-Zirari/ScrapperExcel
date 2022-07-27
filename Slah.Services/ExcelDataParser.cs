using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Slah.Domain;

namespace ScrapperExcel
{
    
    
    public class ExcelDataParser
    {
        static ExcelDataParser()
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
        }
        
        public object ReadExcelFile(Stream stream)
        {
            var reader = ExcelReaderFactory.CreateReader(stream);

            var result = reader.AsDataSet();

            var tables = result.Tables.Cast<DataTable>();

            var baseLoadDataTable = tables.ElementAt(0);
            var peakLoadDataTable = tables.ElementAt(1);
            var blocksDataTable = tables.ElementAt(2);
            var hourlyPricesAndVolumes = tables.ElementAt(3);


            return new
            {
                BaseLoad = ParseBaseLoad(baseLoadDataTable),
                PeakLoadDataTable = ParseBaseLoad(peakLoadDataTable),
                blocksDataTable = ParseBlocks(blocksDataTable)
            };
        }

        private IEnumerable<TimePriceVolume> ParseBaseLoad(DataTable dt)
        {
            var data = new List<TimePriceVolume>();

            var date = dt.Rows[1][0].ToString();
            var ogDate = DateTime.ParseExact(date, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            ogDate = ogDate.AddDays(-6);

            for (int i = 1; i < dt.Columns.Count; i++)
            {
                var obj = new TimePriceVolume
                {
                    Date = ogDate,
                    Price = (double)dt.Rows[4][i],
                    Volume = (double)dt.Rows[5][i]
                };

                data.Add(obj);
                ogDate = ogDate.AddDays(1);
            }

            return data.ToArray();
        }        
        
        private IEnumerable<Block> ParseBlocks(DataTable dt)
        {
            var Data = new List<Block>();


            var date = dt.Rows[1][0].ToString();

            var OgDate = DateTime.ParseExact(date, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            OgDate = OgDate.AddDays(-6);

            dt.Columns.Cast<DataColumn>().Skip(1).Select((c, i) =>
            {
                OgDate = OgDate.AddDays(1);
                return new Block
                {
                    Date = OgDate,
                    Periods = dt.Rows.Cast<DataRow>().Skip(3).Select(x => new PeriodicPrice()
                    {
                        Name = "REGEX", // put the real value
                        Period = new Period()
                        {
                            Start = DateTime.Today, // put the real value
                            End = DateTime.Today, // put the real value
                        },
                        Price = decimal.Parse(x[i].ToString())
                    })
                    
                };
            });
            return Data.ToArray();
        }


    }
}
