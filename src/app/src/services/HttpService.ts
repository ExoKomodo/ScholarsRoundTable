import axios, { AxiosResponse, AxiosError } from 'axios';

export default class HttpService {
    public static baseUrl: string = process.env.NODE_ENV === 'development' ? 'https://localhost:5000' : 'https://scholarsroundtable.exokomodo.com';
    public static baseAuthUrl: string = process.env.NODE_ENV === 'development' ? 'https://localhost:5001' : 'https://scholarsroundtable.exokomodo.com';

    public static get(url: string, useBaseUrl: boolean = true, queryParams?: Map<string, string>): Promise<any> {
        url = HttpService.trimUrl(url);

        let queryParamString: string = '';
        if (!!queryParams && queryParams.size > 0) {
            queryParamString = '?';
            let i: number = 0;
            queryParams.forEach((value: string, key: string) => {
                queryParamString += `${i === 0 ? '' : '&'}${key}=${value}`;
                i++;
            });
        }

        if (useBaseUrl) {
            return axios.get(`${this.baseUrl}/${url}${queryParamString}`);
        } else {
            return axios.get(`${url}${queryParamString}`);
        }
    }

    public static parseQueryParams(queryString: string): Map<string, string> {
        const queryParams: Map<string, string> = new Map<string, string>();
        const pairs: string[] = (queryString[0] === '?' ? queryString.substr(1) : queryString).split('&');
        for (const pair of pairs) {
            const splitPair: string[] = pair.split('=');
            queryParams.set(decodeURIComponent(splitPair[0]), decodeURIComponent(splitPair[1] || ''));
        }
        return queryParams;
    }

    public static post(url: string, data: any): Promise<any> {
        url = HttpService.trimUrl(url);
        return axios.post(`${this.baseUrl}/${url}`, data);
    }

    public static put(url: string, data: any): Promise<any> {
        url = HttpService.trimUrl(url);
        return axios.put(`${this.baseUrl}/${url}`, data);
    }

    private static trimUrl(url: string): string {
        while (url.startsWith('/')) {
            url = url.slice(1);
        }
        return url;
    }
}

export interface HttpResponse<T = any> extends AxiosResponse<T> { }
export interface HttpError extends AxiosError { }
