using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.CommonLayer.Model
{
    public class DeleteRecordRequest
    {
      public int UserId { get; set; }
    }
    public class DeleteRecordResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } 
    }
}