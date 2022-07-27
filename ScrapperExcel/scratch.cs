
//HttpClient client = new HttpClient();
//var url = "https://hupx.hu/en/dam/weekly-data/export.xlsx?date=2022-07-15";
//var uri = new Uri(url);
//var content = await client.GetStreamAsync(uri);

//var request = CRequest(uri.ToString());
//var responseMsg = await client.SendAsync(request);
//responseMsg.EnsureSuccessStatusCode();
//var response = await responseMsg.Content.ReadAsStreamAsync();

//Console.WriteLine(response);

//static HttpRequestMessage CRequest(string url)
//{

//    var request = new HttpRequestMessage(HttpMethod.Get, url);
//    request.Headers.Add("Accept", "application/xlsx");
//    return request;
//}


//public string DtToJson(DataTable table)
//{
//    var JSONString = new StringBuilder();
//    var title = "";
//    if (table.Rows.Count > 0)
//    {
//        JSONString.Append("[");
//        for (int i = 1; i < table.Columns.Count; i++)
//        {

//            JSONString.Append("{");
//            for (int j = 3; j < table.Rows.Count; j++)
//            {
//                if (table.Rows[j][i].ToString() != "")
//                {
//                    if (j == 3)
//                        title = "Date";

//                    else
//                        title = table.Rows[j][0].ToString();

//                    if (j < table.Rows.Count - 1)
//                    {

//                        JSONString.Append("\"" + title + "\": " + "\"" + table.Rows[j][i].ToString() + "\",");

//                    }
//                    else
//                    {
//                        JSONString.Append("\"" + title + "\": " + "\"" + table.Rows[j][i].ToString() + "\"");
//                    }
//                }
//            }
//            if (i == table.Columns.Count - 1)
//            {
//                JSONString.Append("}");
//            }
//            else
//            {
//                JSONString.Append("},");
//            }


//        }

//        JSONString.Append("]");
//    }
//    return JSONString.ToString();
//}




//static HttpClient hClient()
//{
//    var client = new HttpClient();
//}
//HttpClient client = new HttpClient();

//async Task<bool> GetExc()
//{
//    var res = false;
//    var response = await client.GetStreamAsync("https://hupx.hu/en/dam/weekly-data/export.xlsx?date=2022-07-15");
//    if (response != null)
//        res = true;
//    return res;
//}
//Console.WriteLine(GetExc());


//Blocks obj = new Blocks();
//var i = 0;
//foreach (PropertyInfo prop in obj.GetType().GetProperties())
//{
//    var type = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
//    if (type == typeof(string))
//    {
//        i++;
//    }

//}
//Console.WriteLine(i);
//;



//var obj = new Blocks()

//{
//    Date = OgDate,
//    OffPeak1 = dt.Rows[4][i].ToString(),
//    OffPeak2 = dt.Rows[5][i].ToString(),
//    MiddleNight = dt.Rows[6][i].ToString(),
//    Night = dt.Rows[7][i].ToString(),
//    EarlyMorning = dt.Rows[8][i].ToString(),
//    Morning = dt.Rows[9][i].ToString(),
//    LateMorning = dt.Rows[10][i].ToString(),
//    Business = dt.Rows[11][i].ToString(),
//    HighNoon = dt.Rows[12][i].ToString(),
//    EarlyAfternoon = dt.Rows[13][i].ToString(),
//    Afternoon = dt.Rows[14][i].ToString(),
//    RushHour = dt.Rows[15][i].ToString(),
//    Evening = dt.Rows[16][i].ToString()
//};
