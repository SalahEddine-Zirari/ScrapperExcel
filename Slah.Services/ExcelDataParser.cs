using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Slah.Domain;
using System.Text.RegularExpressions;
using Salah.Domain;

namespace ScrapperExcel
{
    
    
    public class ExcelDataParser
    {
        static ExcelDataParser()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
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
                //BaseLoad = ParseBaseLoad(baseLoadDataTable),
                //PeakLoadDataTable = ParseBaseLoad(peakLoadDataTable),
                blocksDataTable = ParseBlocks(blocksDataTable),
                HPVDataTable = ParseHourlyPriceAndVolume(hourlyPricesAndVolumes)

            };
        }

        private IEnumerable<LoadExtremes> ParseBaseLoad(DataTable dt)
        {
            var data = new List<LoadExtremes>();

            var date = dt.Rows[1][0].ToString();
            var ogDate = DateTime.ParseExact(date, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            ogDate = ogDate.AddDays(-6);

            for (int i = 1; i < dt.Columns.Count; i++)
            {
                var obj = new LoadExtremes
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
        
        private IEnumerable<HourlyPriceAndVolume> ParseHourlyPriceAndVolume(DataTable dt)
        {
            
            var Data = new List<HourlyPriceAndVolume>();
            var date = dt.Rows[1][0].ToString();
            var OgDate = DateTime.ParseExact(date, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            OgDate = OgDate.AddDays(-6);
            dt.Columns.Cast<DataColumn>().Skip(2).ToList().ForEach(i =>
            {
                var HPVObject = new HourlyPriceAndVolume()
                {
                    Date = OgDate,
                    HData = dt.Rows.Cast<DataRow>().Skip(4).Take(2).ToList().ForEach(x =>
                    {
                        var HDataObj = new HourlyData()
                        {
                            HourOfDay = HPVHour(x[0].ToString(), OgDate),
                            Price = decimal.Parse(x[i].ToString()),
                            Volume = decimal.Parse(x[i].ToString())

                        }
                    })
                };

                Data.Add(HPVObject);
                OgDate = OgDate.AddDays(1);
            });
           

            return Data.ToArray();

        }
        

        private IEnumerable<Block> ParseBlocks(DataTable dt)
        {
            var Data = new List<Block>();
            var periods = new List<PeriodicPrice>();
            var date = dt.Rows[1][0].ToString();

            var OgDate = DateTime.ParseExact(date, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            OgDate = OgDate.AddDays(-6);

            dt.Columns.Cast<DataColumn>().Skip(1).ToList().ForEach( i =>
            {
                var block = new Block()
                {
                    Date = OgDate,
                    Periods = dt.Rows.Cast<DataRow>().Skip(4).Select(x => new PeriodicPrice()
                    {
                        Name = PeriodNameRegexExtractor(x[0].ToString()),
                        Period = new Period()
                        {
                            Start = PeriodHourSeparator(x[0].ToString(),OgDate)[0],
                            End = PeriodHourSeparator(x[0].ToString(),OgDate)[1],
                        },
                        Price = decimal.Parse(x[i].ToString())


                    })

                };
                Data.Add(block);
                OgDate = OgDate.AddDays(1);


            });
            return Data.ToArray();
        }

        //HourlyPriceAndVolume methods
        private DateTime HPVHour(string str, DateTime date)
        {
            Regex HourMatchingPattern = new Regex(@"[^H]*$");

            var HourStr = HourMatchingPattern.Match(str);

            var Hour = Convert.ToDouble(HourStr.Value);

            var CurrentHour = date;

            CurrentHour=CurrentHour.AddHours(Hour);

            if (HourStr.Value != "24")
                return CurrentHour;
            else
                return CurrentHour.AddSeconds(-1);

            

        }

        //Blocks only  methods
        private DateTime[] PeriodHourSeparator(string str,DateTime date)
        {
            char[] ch = str.ToCharArray();
            var StartHr = ch[0].ToString() + ch[1].ToString();
            var EndHr = ch[3].ToString() + ch[4].ToString();

           

            var StartDateTime = date;
            StartDateTime = StartDateTime.AddHours(Convert.ToDouble(StartHr));

            var EndDateTime = date;
            EndDateTime = EndDateTime.AddHours(Convert.ToDouble(EndHr));

            if (EndHr == "24")
                EndDateTime = EndDateTime.AddSeconds(-1);

            var StartEndTimes = new DateTime[] { StartDateTime, EndDateTime };

            return StartEndTimes;

        }

        private string PeriodNameRegexExtractor(string str)
        {
            Regex PeriodNamePattern = new Regex(@"\(([^()]*)\)");
            //Regex PeriodHourPattern = new Regex(@"^.{5}");

            var Name = PeriodNamePattern.Match(str);

            return Name.Groups[1].Value;

        }

    }
}
