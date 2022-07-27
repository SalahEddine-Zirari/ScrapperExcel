using ScrapperExcel;

// var stream = File.Open(Path.GetFullPath(@"C:\Users\Cheato\Downloads\data.xlsx"), FileMode.Open, FileAccess.Read);


var provider = new ExcelDataProvider();

var stream = await provider.GetData(DateTime.Today.AddDays(-1));

var parser = new ExcelDataParser();

var data = parser.ReadExcelFile(stream);

Console.ReadKey();