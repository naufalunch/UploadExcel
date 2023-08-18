using System;
using System.Data;
using System.IO;
using System.Text;
using ExcelDataReader;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExcelFileUploader.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
       public IActionResult UploadFile(IFormFile file)
    {
    if (file == null || file.Length <= 0)
    {
        ViewBag.Message = "No file uploaded.";
        return View("Index");
    }

    using (var stream = file.OpenReadStream())
    {
        using (var reader = ExcelReaderFactory.CreateReader(stream, new ExcelReaderConfiguration()
        {
            FallbackEncoding = Encoding.GetEncoding("utf-8")
        }))
        {
            DataSet result = new DataSet();
            do
            {
                DataTable table = new DataTable();
                while (reader.Read())
                {
                    DataRow row = table.NewRow();
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        row[i] = reader.GetValue(i);
                    }
                    table.Rows.Add(row);
                }
                result.Tables.Add(table);
            } while (reader.NextResult());

            ViewBag.Message = "File uploaded successfully.";
            ViewBag.Table = result.Tables[0];
        }
    }

    return View("Index");
}


    }
}
