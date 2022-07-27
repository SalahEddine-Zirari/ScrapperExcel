namespace ScrapperExcel;

public class ExcelDataProvider
{
    public async Task<Stream> GetData(DateTime date)
    {
        var url = $"https://hupx.hu/en/dam/weekly-data/export.xlsx?date={date:yyyy-MM-dd}";
        var client = new HttpClient();
        var response = await client.GetAsync(url);
        response.EnsureSuccessStatusCode();
        var stream = await response.Content.ReadAsStreamAsync();
        return stream;
    }
}