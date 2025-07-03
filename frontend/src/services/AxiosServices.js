import axios from 'axios';

export default class AxiosServices {
    post(url, data, IsRequired = false, Header) {
        return axios.post(url, data, IsRequired ? Header : undefined);
    }

    get(url, IsRequired = false, Header) {
        return axios.get(url, IsRequired ? Header : undefined);
    }

    delete(url, IsRequired = false, Header) {
        return axios.delete(url, IsRequired ? { headers: Header } : undefined);
    }
}
