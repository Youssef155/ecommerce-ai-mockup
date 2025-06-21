export interface TokenData {
  token: string;
}
export interface JwtUserData {
  data: TokenData;
  statusCode: number;
  isSucceeded: boolean;
}

export interface DecodedToken {
  nameid: string;
  given_name: string;
  family_name: string;
  email: string;
}
