using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using api.CommonLayer.Model;
using LumenWorks;
using ExcelDataReader;
using MySql.Data.MySqlClient;
using LumenWorks.Framework.IO.Csv;


namespace api.DataAcessLayer
{
    public class UploadFileDL : IUploadFileDL
    {
        public readonly IConfiguration _configuration;
        public readonly MySqlConnection _mySqlConnection;
        public UploadFileDL(IConfiguration configuration)
        {
            _configuration = configuration;
            _mySqlConnection = new MySqlConnection(_configuration["ConnectionStrings:MySqlDBConnectionString"]);
        }

        public async Task<DeleteRecordResponse> DeleteRecord(DeleteRecordRequest request)
        {
            DeleteRecordResponse response = new DeleteRecordResponse();
            response.IsSuccess = true;
            response.Message = "Successful";
            try
            {
                if (_mySqlConnection.State != ConnectionState.Open)
                {
                    await _mySqlConnection.OpenAsync();
                }
                string SqlQuery = @"DELETE FROM pathwaysdb.bulkuploadtable WHERE UserID=@UserID";
                using (MySqlCommand sqlCommand = new MySqlCommand(SqlQuery, _mySqlConnection))
                {
                    sqlCommand.CommandType = CommandType.Text;
                    sqlCommand.CommandTimeout = 180;
                    sqlCommand.Parameters.AddWithValue("@UserID", request.UserId);
                    int Status = await sqlCommand.ExecuteNonQueryAsync();
                    if (Status <= 0)
                    {
                        response.IsSuccess = false;
                        response.Message = "Query not executed";
                        return response;
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            finally
            {
                await _mySqlConnection.CloseAsync();
                await _mySqlConnection.DisposeAsync();
            }
            return response;
        }

        public  async Task<ReadRecordResponse> ReadRecord(ReadRecordRequest request)
        {
            ReadRecordResponse response = new ReadRecordResponse();
            response.IsSuccess = true;
            response.Message = "Successful";
            try
            {
                if (_mySqlConnection.State != ConnectionState.Open)
                {
                    await _mySqlConnection.OpenAsync();
                }
                string SqlQuery = @"SELECT * FROM pathwaysdb.bulkuploadtable LIMIT @Offset,@RecordPerPage";
                using (MySqlCommand sqlCommand = new MySqlCommand(SqlQuery, _mySqlConnection))
                {
                    int offset = (request.PageNumber - 1) * request.NumberOfRecordPerPage;
                    sqlCommand.CommandType = CommandType.Text;
                    sqlCommand.CommandTimeout = 180;
                    sqlCommand.Parameters.AddWithValue("@Offset",offset);
                    sqlCommand.Parameters.AddWithValue("@RecordPerPage", request.NumberOfRecordPerPage);

                    using (DbDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync())
                    {
                        if (sqlDataReader.HasRows)
                        {
                            response.data = new List<ReadRecord>();
                            while (await sqlDataReader.ReadAsync())
                            {
                                ReadRecord getdata = new ReadRecord();
                                getdata.UserId = sqlDataReader["UserId"] != DBNull.Value ? Convert.ToInt32(sqlDataReader["UserID"]) : -1;
                                getdata.UserName = sqlDataReader["UserName"] != DBNull.Value ? Convert.ToString(sqlDataReader["UserName"]) : "-1";
                                getdata.EmailID = sqlDataReader["EmailID"] != DBNull.Value ? Convert.ToString(sqlDataReader["EmailID"]) : "-1";
                                getdata.MobileNumber = sqlDataReader["MobileNumber"] != DBNull.Value ? Convert.ToString(sqlDataReader["MobileNumber"]) : "-1";
                                getdata.Age = sqlDataReader["Age"] != DBNull.Value ? Convert.ToInt32(sqlDataReader["Age"]) : -1;
                                getdata.Salary = sqlDataReader["Salary"] != DBNull.Value ? Convert.ToInt32(sqlDataReader["Salary"]) : -1;
                                response.data.Add(getdata);

                            }
                        }
                        else
                        {
                            response.Message = "Record Not Found";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            finally
            {
                await _mySqlConnection.CloseAsync();
               await _mySqlConnection.DisposeAsync();
            }
            return response;
        }

        public async Task<UploadCsvFileResponse> UploadCsvFile(UploadCsvFileRequest request, string Path)
        {
            UploadCsvFileResponse response = new UploadCsvFileResponse();
            List<ExcelBulkUploadParamerter>paramerters=new List<ExcelBulkUploadParamerter>();
            response.IsSuccess = true;
            response.Message = "Successful";
            try
            {
                if (request.File.FileName.ToLower().Contains(".csv"))
                {
                    DataTable value = new DataTable();
                    using (var csvReader = new CsvReader(new StreamReader(File.OpenRead(Path)), true))
                    {
                        value.Load(csvReader);
                    }
                    for (int i = 0; i < value.Rows.Count; i++)
                    {
                        ExcelBulkUploadParamerter rows = new ExcelBulkUploadParamerter();
                        rows.UserName = value.Rows[i][0] != null ? Convert.ToString(value.Rows[i][0]) : "-1";
                        rows.EmailId = value.Rows[i][1] != null ? Convert.ToString(value.Rows[i][1]) : "-1";
                        rows.MobileNumber = value.Rows[i][2] != null ? Convert.ToString(value.Rows[i][2]) : "-1";
                        rows.Age = value.Rows[i][3] != null ? Convert.ToInt32(value.Rows[i][3]) : -1;
                        rows.Salary = value.Rows[i][4] != null ? Convert.ToInt32(value.Rows[i][4]) : -1;
                        paramerters.Add(rows);
                    }
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = "Invalid File";
                    return response;
                }

                if (_mySqlConnection.State != ConnectionState.Open)
                {
                    await _mySqlConnection.OpenAsync();
                }
                string SqlQuery = @"INSERT INTO pathwaysdb.bulkuploadtable (UserName,EmailID,MobileNumber,Age,Salary) VALUES (@UserName,@EmailID,@MobileNumber,@Age,@Salary)
                ";
                foreach (ExcelBulkUploadParamerter rows in paramerters)
                {
                    using (MySqlCommand sqlCommand = new MySqlCommand(SqlQuery, _mySqlConnection))
                    {
                        sqlCommand.CommandType = CommandType.Text;
                        sqlCommand.CommandTimeout = 180;
                        sqlCommand.Parameters.AddWithValue("@UserName", rows.UserName);
                        sqlCommand.Parameters.AddWithValue("@EmailID", rows.EmailId);
                        sqlCommand.Parameters.AddWithValue("@MobileNumber", rows.MobileNumber);
                        sqlCommand.Parameters.AddWithValue("@Age", rows.Age);
                        sqlCommand.Parameters.AddWithValue("@Salary", rows.Salary);
                        int Status = await sqlCommand.ExecuteNonQueryAsync();
                        if (Status <= 0)
                        {
                            response.IsSuccess = false;
                            response.Message = "Query not executed";
                            return response;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            finally
            {
             await _mySqlConnection.CloseAsync();
             await _mySqlConnection.DisposeAsync();
            }
            return response;
        }

        public async Task<UploadExcelFileResponse> UploadExcelFile(UploadExcelFileRequest request, string Path)
        {
            //processing excess file
            UploadExcelFileResponse response = new UploadExcelFileResponse();
            List<ExcelBulkUploadParamerter> Parameters = new List<ExcelBulkUploadParamerter>();
            response.IsSuccess = true;
            response.Message = "Successful";
            try
            {
                if (_mySqlConnection.State != System.Data.ConnectionState.Open)
                {
                    await _mySqlConnection.OpenAsync();
                }
                if (request.File.FileName.ToLower().Contains(".xlsx"))
                {
                    FileStream stream = new FileStream(Path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
                    IExcelDataReader reader = ExcelReaderFactory.CreateReader(stream);
                    DataSet dataset = reader.AsDataSet(
                        new ExcelDataSetConfiguration()
                        {
                            UseColumnDataType = false,
                            ConfigureDataTable = (tableReader) => new ExcelDataTableConfiguration()
                            {
                                UseHeaderRow = true
                            }
                        }
                    );
                    for (int i = 0; i < dataset.Tables[0].Rows.Count; i++)
                    {
                        ExcelBulkUploadParamerter rows = new ExcelBulkUploadParamerter();
                        rows.UserName = dataset.Tables[0].Rows[i].ItemArray[0] != null ? Convert.ToString(dataset.Tables[0].Rows[i].ItemArray[0]) : "-1";
                        rows.EmailId = dataset.Tables[0].Rows[i].ItemArray[1] != null ? Convert.ToString(dataset.Tables[0].Rows[i].ItemArray[1]) : "-1";
                        rows.MobileNumber = dataset.Tables[0].Rows[i].ItemArray[2] != null ? Convert.ToString(dataset.Tables[0].Rows[i].ItemArray[2]) : "-1";
                        rows.Age = dataset.Tables[0].Rows[i].ItemArray[3] != null ? Convert.ToInt32(dataset.Tables[0].Rows[i].ItemArray[3]) : -1;
                        rows.Salary = dataset.Tables[0].Rows[i].ItemArray[4] != null ? Convert.ToInt32(dataset.Tables[0].Rows[i].ItemArray[4]) : -1;
                        
                        Parameters.Add(rows);

                    }
                    stream.Close();
                    if (Parameters.Count > 0)
                    {
                        string SqlQuery = @"INSERT INTO pathwaysdb.bulkuploadtable(UserName,EmailID,MobileNumber,Age,Salary) VALUES (@UserName,@EmailID,@MobileNumber,@Age,@Salary)";
                        foreach (ExcelBulkUploadParamerter rows in Parameters)
                        {
                            using (MySqlCommand sqlCommand = new MySqlCommand(SqlQuery, _mySqlConnection))
                            {
                                sqlCommand.CommandType = CommandType.Text;
                                sqlCommand.CommandTimeout = 180;
                                sqlCommand.Parameters.AddWithValue("@UserName", rows.UserName);
                                sqlCommand.Parameters.AddWithValue("@EmailID", rows.EmailId);
                                sqlCommand.Parameters.AddWithValue("@MobileNumber", rows.MobileNumber);
                                sqlCommand.Parameters.AddWithValue("@Age", rows.Age);
                                sqlCommand.Parameters.AddWithValue("@Salary", rows.Salary);
                                
                                int Status = await sqlCommand.ExecuteNonQueryAsync();
                                if (Status <= 0)
                                {
                                    response.IsSuccess = false;
                                    response.Message = "Query Not Executed";
                                    return response;
                                }
                            }
                        }
                    }
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = "Incorrect file format";
                    return response;
                }


            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            finally
            {
                await _mySqlConnection.CloseAsync();
                await _mySqlConnection.DisposeAsync();
            }
            return response;
        }
    }
}