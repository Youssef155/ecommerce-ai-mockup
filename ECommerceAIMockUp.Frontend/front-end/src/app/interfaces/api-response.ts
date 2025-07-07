export interface ApiResponse<T> {
  statusCode: number;
  data?: T;
  result?: T;
  message?: string;
  errors?: string[];
}
