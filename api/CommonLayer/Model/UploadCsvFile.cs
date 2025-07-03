using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace api.CommonLayer.Model
{
    public class UploadCsvFileRequest
    {
     public IFormFile File { get; set; }
    }
    public class UploadCsvFileResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }
    
}