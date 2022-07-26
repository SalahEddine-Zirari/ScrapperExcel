﻿using ExcelDataReader;
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
            var hourlyPricesAndVolumesFor = tables.ElementAt(3);


            return new
            {
                //BaseLoad = ParseBasePeakLoad(baseLoadDataTable),
                //PeakLoadDataTable = ParseBasePeakLoad(peakLoadDataTable),
                //blocksDataTable = ParseBlocks(blocksDataTable),
                HPVDataTable = ParseHourlyPriceAndVolume(hourlyPricesAndVolumes),
                HPVDataTableFor = ParseHourlyPriceAndVolumeFor(hourlyPricesAndVolumes)

            };
        }

        private IEnumerable<LoadExtremes> ParseBasePeakLoad(DataTable dt)
        {
            var data = new List<LoadExtremes>();

            var date = dt.Rows[1][0].ToString();
            var ogDate = DateTime.ParseExact(date, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            ogDate = ogDate.AddDays(-6);

            dt.Columns.Cast<DataColumn>().Skip(1).ToList().ForEach(col =>
            {
                var obj = new LoadExtremes
                {
                    Date = ogDate,
                    Price = (double)dt.Rows[4][col],
                    Volume = (double)dt.Rows[5][col]
                };

                data.Add(obj);
                ogDate = ogDate.AddDays(1);

            });

            //for (int i = 1; i < dt.Columns.Count; i++)
            //{
            //    var obj = new LoadExtremes
            //    {
            //        Date = ogDate,
            //        Price = (double)dt.Rows[4][i],
            //        Volume = (double)dt.Rows[5][i]
            //    };

            //    data.Add(obj);
            //    ogDate = ogDate.AddDays(1);
            //}

            return data.ToArray();
        }
        
        private IEnumerable<HourlyPriceAndVolume> ParseHourlyPriceAndVolume(DataTable dt)
        {
            
            var Data = new List<HourlyPriceAndVolume>();

            var date = dt.Rows[1][0].ToString();
            var OgDate = DateTime.ParseExact(date, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            OgDate = OgDate.AddDays(-6);

            dt.Columns.Cast<DataColumn>().Skip(2).ToList().ForEach(col =>
            {
                var HPVObject = new HourlyPriceAndVolume();
                var HourlyData = new List<HourlyData>();

                HPVObject.Date = OgDate;
                var PriceVolumeRows=ChunkList(dt.Rows.Cast<DataRow>().Skip(4).ToList(), 2);
                PriceVolumeRows.ForEach(rows =>
                {
                    var HDataObj = new HourlyData
                    {
                        HourOfDay = HPVHour(rows[0][0].ToString(), OgDate),
                        Price = decimal.Parse(rows[0][col].ToString()),
                        Volume = decimal.Parse(rows[1][col].ToString())
                    };
                    HourlyData.Add(HDataObj);

                });
                HPVObject.HData = HourlyData;
                Data.Add(HPVObject);
                OgDate = OgDate.AddDays(1);


            });
                     
            return Data.ToArray();

        }
       

        private IEnumerable<Block> ParseBlocks(DataTable dt)
        {
            var Data = new List<Block>();
            var date = dt.Rows[1][0].ToString();

            var OgDate = DateTime.ParseExact(date, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            OgDate = OgDate.AddDays(-6);
            dt.Columns.Cast<DataColumn>().Skip(1).ToList().ForEach(col =>
            {
                var block = new Block();
                var PeriodicPrices = new List<PeriodicPrice>();
                block.Date = OgDate;

                dt.Rows.Cast<DataRow>().Skip(4).ToList().ForEach(row =>
                {
                    var periodicPriceObj = new PeriodicPrice()
                    {
                        Name = PeriodNameRegexExtractor(row[0].ToString()),
                        Price = decimal.Parse(row[col].ToString()),
                        Period = new Period()
                        {
                            Start = PeriodHourSeparator(row[0].ToString(), OgDate)[0],
                            End = PeriodHourSeparator(row[0].ToString(), OgDate)[1]
                        }
                    };
                    PeriodicPrices.Add(periodicPriceObj);
                });
                block.Periods = PeriodicPrices;
                Data.Add(block);
                OgDate = OgDate.AddDays(1);
            });

            {
                //for (var i = 1; i < dt.Columns.Count; i++)
                //{
                //    var block = new Block();
                //    var PeriodicPrices = new List<PeriodicPrice>();

                //    block.Date = OgDate;
                //    for(var x=4;x<dt.Rows.Count;x++)
                //    {
                //        var periodicPriceObj = new PeriodicPrice()
                //        {
                //            Name = PeriodNameRegexExtractor(dt.Rows[x][0].ToString()),
                //            Price = decimal.Parse(dt.Rows[x][i].ToString()),
                //            Period = new Period()
                //            {
                //                Start = PeriodHourSeparator(dt.Rows[x][0].ToString(), OgDate)[0],
                //                End = PeriodHourSeparator(dt.Rows[x][0].ToString(), OgDate)[1]
                //            }
                //        };
                //        PeriodicPrices.Add(periodicPriceObj);
                //    }
                //    block.Periods = PeriodicPrices;

                //    Data.Add(block);

                //    OgDate = OgDate.AddDays(1);

                //}

            }
            return Data.ToArray();
        }


        //HourlyPriceAndVolume methods
        private List<List<T>> ChunkList<T>(IEnumerable<T> data, int size)
        {
            return data
              .Select((x, i) => new { Index = i, Value = x })
              .GroupBy(x => x.Index / size)
              .Select(x => x.Select(v => v.Value).ToList())
              .ToList();
        }
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
