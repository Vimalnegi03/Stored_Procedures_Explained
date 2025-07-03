using System;

namespace api.CommonLayer.Model
{
    public class UploadExcelFileRequest
    {
        public IFormFile File { get; set; }
    }
    public class UploadExcelFileResponse
    {
        public bool IsSuccess { get; set; }
        //will inform successful if successfull
        public string Message { get; set; }

    }
    public class ExcelBulkUploadParamerter
    {
        public string UserName { get; set; }
        public string EmailId { get; set; }
        public string MobileNumber { get; set; }
        public int Age { get; set; }
        public int Salary { get; set; }
        public string Gender { get; set; }
        
    }
}