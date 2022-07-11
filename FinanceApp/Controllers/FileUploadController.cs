using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FinanceApp.Data;
using FinanceApp.Model;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;


namespace FinanceApp.Controllers
{
    [EnableCors("AllowOrigin")]
    [Route("api/[controller]")]
    [ApiController]
    public class FileuploadController : ControllerBase
    {

        public readonly UserDbContext context;
        public FileuploadController(UserDbContext userdbcontext)
        {
            context = userdbcontext;
        }

        [HttpPost("Upload"), DisableRequestSizeLimit]
        public IActionResult Fileupload(IFormFile files)
        {
            try
            {
                var file = Request.Form.Files[0];
                var date = DateTime.Now.Date.Month.ToString() + " " + DateTime.Now.Date.Year.ToString() + " " + DateTime.Now.Day.ToString();
                var foldername = Path.Combine("Resource", "Images", date);
                var pathtosave = Path.Combine(Directory.GetCurrentDirectory(), foldername);
                if (file.Length > 0)
                {
                    Directory.CreateDirectory(pathtosave);
                    var filename = file.FileName.Trim('"');
                    var fulllpath = Path.Combine(pathtosave, filename).ToString();
                    var dbpath = Path.Combine(foldername, filename);
                    var filepathattachment = Path.Combine(foldername, filename).ToString();
                    using (var stream = new FileStream(fulllpath, FileMode.Append))
                    {
                        file.CopyTo(stream);
                    }
                    var filedetails = SaveFileToDB(filename, filepathattachment);
                    return Ok(filedetails);
                }
                return BadRequest();
            }

            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        private FileUploadModel SaveFileToDB(string filename, string filepathattachment)
        {
            var objfile = new FileUploadModel()
            {
                AttachmentId = 0,
                AttachmentName = filename,
                AttachmentPath = filepathattachment
            };
            context.FileAttachment.Add(objfile);
            context.SaveChanges();
            return (objfile);
        }

        [HttpGet("DownloadFile")]
        public IActionResult DownloadFileAttachment(int id)
        {
            var file = context.FileAttachment.Where(n => n.AttachmentId == id).FirstOrDefault();
            var filepath = Path.Combine(Directory.GetCurrentDirectory(), file.AttachmentPath);
            if (!System.IO.File.Exists(filepath))
                return NotFound();
            var memory = new MemoryStream();
            using (var stream = new FileStream(filepath, FileMode.Open))
            {
                stream.CopyTo(memory);
            }
            memory.Position = 0;
            return File(memory, GetContentType(filepath), filepath);
        }
        private string GetContentType(string path)
        {
            var provider = new FileExtensionContentTypeProvider();
            string contentType;
            if (!provider.TryGetContentType(path, out contentType))
            {

                contentType = "application/octet-stream";
            }
            return contentType;
        }

        [HttpGet("atttchmentFile")]
        public IActionResult GetAttachmentPath()
        {
            var user = context.FileAttachment.AsQueryable();
            return Ok(user);
        }
    }
}



