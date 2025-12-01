import axios from 'axios';

export class HttpClient {
  private client: ReturnType<typeof axios.create>;

  constructor(defaultHeaders?: Record<string, string>) {
    const baseURL = import.meta.env.VITE_API_BASE_URL; 

    this.client = axios.create({
      baseURL,
      headers: defaultHeaders,
    });
  }

  async get<T>(url: string, config?: object): Promise<T> {
    const response = await this.client.get<T>(url, config);
    return response.data;
  }

  async post<T, B = unknown>(url: string, data?: B, config?: object): Promise<T> {
    const response = await this.client.post<T>(url, data, config);
    return response.data;
  }

  async put<T, B = unknown>(url: string, data?: B, config?: object): Promise<T> {
    const response = await this.client.put<T>(url, data, config);
    return response.data;
  }

  async delete<T>(url: string, config?: object): Promise<T> {
    const response = await this.client.delete<T>(url, config);
    return response.data;
  }

  setHeader(key: string, value: string) {
    this.client.defaults.headers.common[key] = value;
  }

  removeHeader(key: string) {
    delete this.client.defaults.headers.common[key];
  }
}

export default HttpClient;
