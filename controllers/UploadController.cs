using Microsoft.AspNetCore.Mvc;

namespace CsvFileUpload.Controllers
{
    [Route("/")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        private readonly FileUploadService fileUploadService;

        public UploadController(FileUploadService fileUploadService)
        {
            this.fileUploadService = fileUploadService;
        }

        [HttpGet]
        public async Task Get()
        {
            var file = Path.Combine(Directory.GetCurrentDirectory(), "views", "index.html");
            Response.ContentType = "text/html";
            await Response.SendFileAsync(file);
        }

        [HttpGet("{path}")]
        public async Task<IActionResult> GetPath(string path)
        {
            string fileContent;
            string contentType;

            if (path == "main.js")
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "src", "js", "main.js");
                fileContent = await System.IO.File.ReadAllTextAsync(filePath);
                contentType = "application/javascript";
            }
            else if (path == "styles.css")
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "src", "css", "styles.css");
                fileContent = await System.IO.File.ReadAllTextAsync(filePath);
                contentType = "text/css";
            }
            else
            {
                return NotFound();
            }

            Response.ContentType = contentType;
            return Content(fileContent);
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload(List<IFormFile> files)
        {
            if (files == null || files.Count == 0)
            {
                return BadRequest("No files uploaded");
            }

            var results = await fileUploadService.UploadFilesAsync(files);
            var successResults = results.Where(r => r.IsSuccess).ToList();
            var failedResults = results.Where(r => !r.IsSuccess).ToList();

            if (successResults.Count == files.Count)
            {
                return Ok("All files uploaded successfully");
            }
            else
            {
                var errorMessage = $"Some files failed to upload. Total files: {files.Count}, Success: {successResults.Count}, Failed: {failedResults.Count}";
                return BadRequest(errorMessage);
            }
        }
    }
}
