export interface Product{
    id:number;
    name: string;
    descriptions : string;
    gender: number;
    season: number;
    price:number;
    categoryName:string;
    image:string;
}

export interface ApiResponse{
    data:{
        data: Product[];
        currentPage: number;
        totalPages: number;
        pageSize: number;
        hasPreviousPage: boolean;
        hasNextPage: boolean;
  };
    statusCode: number;
    isSucceeded: boolean;
}