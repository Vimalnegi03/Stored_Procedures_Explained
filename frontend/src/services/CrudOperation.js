import Axios from './AxiosServices'
import Configuration from '../configuration/Configuration'
const axios=new Axios();

export default class CrudOperation{
    UploadExcelFile(data){
    return axios.post(Configuration.InsertExcelFile,data,false);
    }
    UploadCsvFile(data){
    return axios.post(Configuration.InsertCsvFile,data,false);
    }
    ReadRecord(data){
        return axios.post(Configuration.GetRecord,data,false);
    }
    DeleteRecord(data)
    {
        return axios.delete(Configuration.DeleteRecord,data,false);
    }
}
