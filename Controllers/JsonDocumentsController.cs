#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JsonApi.Models;

namespace JSON_PG.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JsonDocumentsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private static readonly HttpClient client = new HttpClient();

        public JsonDocumentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // POST: api/JsonDocuments
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<JsonModel>> PostJsonModel(UrlModel url)
        {
            if (!IsUrlValid(url.Url))
            {
                return BadRequest("Invalid URL, consider adding scheme http or https");
            }

            var targetUrl = url.Url;
            var jsonResponse = await client.GetStringAsync(targetUrl);
            Console.WriteLine(jsonResponse);

            var filters = url.JsonDataToSavePointers;
            var filteredData = JsonDocumentsLogic.FilterJson(jsonResponse, filters);

            string filteredJson = JsonSerializer.Serialize(filteredData, new JsonSerializerOptions
            {
                WriteIndented = true // Optional: Make the JSON output indented for readability
            });
            var jsonEntiy = new JsonModel { BsonData = filteredJson };
            _context.JsonModels.Add(jsonEntiy);
            await _context.SaveChangesAsync();
            return Ok(jsonResponse);
        }

        private bool IsUrlValid(string url)
        {
            bool isValid = Uri.TryCreate(url, UriKind.Absolute, out Uri uriResult)
            && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
            return isValid;
        }
    }
}
