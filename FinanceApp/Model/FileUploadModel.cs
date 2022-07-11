using System;
using System.ComponentModel.DataAnnotations;

namespace FinanceApp.Model
{
    public class FileUploadModel
    {
        [Key]
        public int AttachmentId { get; set; }
        public string AttachmentName { get; set; }
        public string AttachmentPath { get; set; }
    }
}
