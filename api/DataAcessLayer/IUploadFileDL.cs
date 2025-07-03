using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.CommonLayer.Model;

namespace api.DataAcessLayer
{
    public interface IUploadFileDL
    {
        public Task<UploadExcelFileResponse> UploadExcelFile(UploadExcelFileRequest request, string Path);
        public Task<ReadRecordResponse> ReadRecord(ReadRecordRequest request);

        public Task<DeleteRecordResponse> DeleteRecord(DeleteRecordRequest request);

        public Task<UploadCsvFileResponse> UploadCsvFile(UploadCsvFileRequest request, string Path);
    }
}