using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.CommonLayer.Model
{
    public class ReadRecordRequest
    {
        public int PageNumber { get; set; }
        public int NumberOfRecordPerPage { get; set; }
    }
    public class ReadRecordResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }

        public List<ReadRecord> data { get; set; }
    }
    public class ReadRecord
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string EmailID { get; set; }
        public string MobileNumber { get; set; }
        public int Age { get; set; }
        public int Salary { get; set; }
    
   }
}