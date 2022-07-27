using ExcelDataReader;
using System.Data;
using System.Text.Json;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using System.IO;
using System.Text;
using System.Net.Http;
using ScrapperExcel;

System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

void ShowBaseLoad()
{
    BaseLoadPeakLoad bp = new BaseLoadPeakLoad();

    var dt = bp.GetLoad(0);
    var list = bp.DtToObj(dt);

    bp.CheckObjList(list);

}

void ShowPeakLoad()
{
    BaseLoadPeakLoad bp = new BaseLoadPeakLoad();

    var dt = bp.GetLoad(1);
    var list = bp.DtToObj(dt);

    bp.CheckObjList(list);
}

void ShowBlocks()
{
    Blocks blocks = new Blocks();

    var dt = blocks.GetLoad(2);
    var list = blocks.DtToObj(dt);

   blocks.CheckObjList(list);
}

//ShowBaseLoad();

ShowBlocks();