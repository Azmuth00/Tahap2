using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Rotativa.AspNetCore;
using Tahap2.Repositories;
using System.IO;

namespace Tahap2.Controllers;

public class HomeController : Controller
{
    private readonly ILaporanRepository _repository;

    public HomeController(ILaporanRepository repository)
    {
        _repository = repository;
    }

    public IActionResult Index()
    {
        var data = _repository.GetAll();
        return View(data);
    }

    public IActionResult EksporPdf()
    {
        var data = _repository.GetAll();
        return new ViewAsPdf("Pdf", data);
    }

    public IActionResult EksporExcel()
    {
        var data = _repository.GetAll();

        using var workbook = new XLWorkbook();
        var sheet = workbook.Worksheets.Add("Laporan");

        sheet.Cell(1, 1).Value = "Id";
        sheet.Cell(1, 2).Value = "Nama";
        sheet.Cell(1, 3).Value = "Tanggal";
        sheet.Cell(1, 4).Value = "Total";

        for (int i = 0; i < data.Count; i++)
        {
            sheet.Cell(i + 2, 1).Value = data[i].Id;
            sheet.Cell(i + 2, 2).Value = data[i].Nama;
            sheet.Cell(i + 2, 3).Value = data[i].Tanggal.ToShortDateString();
            sheet.Cell(i + 2, 4).Value = data[i].Total;
        }

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        var content = stream.ToArray();
        return File(content,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    "laporan.xlsx");
    }
}
