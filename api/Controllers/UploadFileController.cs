using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DataAcessLayer;
using api.CommonLayer.Model;
using Mysqlx.Crud;

namespace api.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class UploadFileController : ControllerBase
    {
        //this is imported from data access layer 
        public readonly IUploadFileDL _uploadFileDL;
        //Dependency injection
        public UploadFileController(IUploadFileDL uploadFileDL)
        {
            _uploadFileDL = uploadFileDL;
        }
        [HttpPost]
        public async Task<IActionResult> UploadExcelFile([FromForm] UploadExcelFileRequest request)
        {
            UploadExcelFileResponse response = new UploadExcelFileResponse();
            string Path = "UploadFileFolder/" + request.File.FileName;
            try
            {
                using (FileStream stream = new FileStream(Path, FileMode.CreateNew))
                {
                    await request.File.CopyToAsync(stream);
                }
                response = await _uploadFileDL.UploadExcelFile(request, Path);

                string[] Files = Directory.GetFiles("UploadFileFolder/");
                foreach (string file in Files)
                {
                    System.IO.File.Delete(file);
                    Console.WriteLine($"{file} is deleted");
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> ReadRecord(ReadRecordRequest request)
        {
            ReadRecordResponse response = new ReadRecordResponse();

            try
            {
                response = await _uploadFileDL.ReadRecord(request);

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;

            }
            return Ok(response);
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteRecord(DeleteRecordRequest request)
        {
            DeleteRecordResponse response = new DeleteRecordResponse();
            try
            {
                response = await _uploadFileDL.DeleteRecord(request);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            return Ok(response);
        }
         [HttpPost]
        public async Task<IActionResult> UploadCsvFile([FromForm] UploadCsvFileRequest request)
        {
            UploadCsvFileResponse response = new UploadCsvFileResponse();
            string Path = "UploadFileFolder/" + request.File.FileName;
            try
            {
                using (FileStream stream = new FileStream(Path, FileMode.CreateNew))
                {
                    await request.File.CopyToAsync(stream);
                }
                response = await _uploadFileDL.UploadCsvFile(request, Path);

                string[] Files = Directory.GetFiles("UploadFileFolder/");
                foreach (string file in Files)
                {
                    System.IO.File.Delete(file);
                    Console.WriteLine($"{file} is deleted");
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            return Ok(response);
        }
    }
}
