using ExcelDataReader;
using System.Data;
using System.Text.Json;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using System.IO;
using System.Text;

System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

string DtToJson(DataTable table)
{
    var JSONString = new StringBuilder();
    var title = "";
    if (table.Rows.Count > 0)
    {
        JSONString.Append("[");
        for (int i = 1; i < table.Columns.Count; i++)
        {
            
            JSONString.Append("{");
            for (int j = 3; j < table.Rows.Count; j++)
            {
                if(table.Rows[j][i].ToString() != "")
                {
                    if (j == 3)
                        title = "Date";

                    else
                        title = table.Rows[j][0].ToString();

                    if (j < table.Rows.Count - 1) 
                    {
                       
                        JSONString.Append("\"" + title + "\": " + "\"" + table.Rows[j][i].ToString() + "\",");
                        
                    }
                    else 
                    {
                        JSONString.Append("\"" + title + "\": " + "\"" + table.Rows[j][i].ToString() + "\"");
                    }   
                }              
            }
            if (i == table.Columns.Count - 1)
            {
                JSONString.Append("}");
            }
            else
            {
                JSONString.Append("},");
            }

            
        }
        
        JSONString.Append("]");
    }
    return JSONString.ToString();
}



void efr(string path)
{
    var stream = File.Open(path, FileMode.Open, FileAccess.Read);

    var reader = ExcelReaderFactory.CreateReader(stream);


    var result = reader.AsDataSet();

    var tables = result.Tables.Cast<DataTable>();

    var table1 = tables.FirstOrDefault();

    var Json = DtToJson(table1);    

    File.WriteAllText("Data", DtToJson(table1));

}
efr(Path.GetFullPath(@"C:\Users\Cheato\Downloads\data.xlsx"));