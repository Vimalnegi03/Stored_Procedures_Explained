import Axios from './AxiosServices'
import {InsertExcelRecord,InsertCsvRecord,GetRecord,DeleteRecord} from '../configuration/Configuration'
const axios=new Axios();

 class CrudOperation{
    UploadExcelFile(data){
    return axios.post(InsertExcelRecord,data,false);
    }
    UploadCsvFile(data){
    return axios.post(InsertCsvRecord,data,false);
    }
    ReadRecord(data){
        return axios.post(GetRecord,data,false);
    }
    DeleteRecord(data)
    {
        return axios.delete(DeleteRecord,data,false);
    }
}

const crudOperation=new CrudOperation()
export default crudOperation