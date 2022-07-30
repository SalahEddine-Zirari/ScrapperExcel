﻿using ScrapperExcel;
using System.Text.RegularExpressions;



var provider = new ExcelDataProvider();

var stream = await provider.GetData(DateTime.Now.AddDays(-15));

var parser = new ExcelDataParser();

var data = parser.ReadExcelFile(stream);


Console.ReadKey();


