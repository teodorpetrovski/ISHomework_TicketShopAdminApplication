using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using TicketShopAdminApplication.Models;

namespace TicketShopAdminApplication.Controllers
{
    public class TicketController : Controller
    {
        public IActionResult Index(string? genre)
        {
            HttpClient client = new HttpClient();

            string URL = "https://localhost:5001/api/Admin/GetTickets";

            if (!string.IsNullOrEmpty(genre))
            {
                URL += "?genre=" + Uri.EscapeDataString(genre);
            }

            HttpResponseMessage response = client.GetAsync(URL).Result;

            var result = response.Content.ReadAsAsync<List<Ticket>>().Result;

            return View(result);
        }


        public FileContentResult ExportTickets(string? genre)
        {
            HttpClient client = new HttpClient();

            string URL = "https://localhost:5001/api/Admin/GetTickets";

            if (!string.IsNullOrEmpty(genre))
            {
                URL += "?genre=" + Uri.EscapeDataString(genre);
            }

            HttpResponseMessage response = client.GetAsync(URL).Result;

            var result = response.Content.ReadAsAsync<List<Ticket>>().Result;

            string fileName = "Tickets.xlsx";
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            using (var workBook = new XLWorkbook())
            {
                IXLWorksheet worksheet = workBook.Worksheets.Add("All Orders");

                worksheet.Cell(1, 1).Value = "#";
                worksheet.Cell(1, 2).Value = "Movie Name";
                worksheet.Cell(1, 3).Value = "Movie Description";
                worksheet.Cell(1, 4).Value = "Ticket Price";
                worksheet.Cell(1, 5).Value = "Movie Rating";
                worksheet.Cell(1, 6).Value = "Genre";
                worksheet.Cell(1, 7).Value = "Date&Time";

                for (int i = 1; i <= result.Count(); i++)
                {
                    var item = result[i - 1];

                    worksheet.Cell(i + 1, 1).Value = i;
                    worksheet.Cell(i + 1, 2).Value = item.MovieName;
                    worksheet.Cell(i + 1, 3).Value = item.MovieDescription;
                    worksheet.Cell(i + 1, 4).Value = "$"+ item.TicketPrice;
                    worksheet.Cell(i + 1, 5).Value = item.MovieRating;
                    worksheet.Cell(i + 1, 6).Value = item.Genre;
                    worksheet.Cell(i + 1, 7).Value = item.ValidFor;

                 

                }

                using (var stream = new MemoryStream())
                {
                    workBook.SaveAs(stream);

                    var content = stream.ToArray();

                    return File(content, contentType, fileName);
                }
            }

        }
    }
}
