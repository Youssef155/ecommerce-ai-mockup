import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { RegisterDto } from '../models/RegisterDto';
import { LoginDto } from '../models/LoginDto';
import { DecodedToken, JwtUserData, TokenData } from '../models/TokenData';
import { jwtDecode } from 'jwt-decode';
import { map, ReplaySubject } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private readonly APIUrl = 'https://localhost:7256/api/account';
  private readonly userKey = 'userkey';

  private userInfoSource = new ReplaySubject<DecodedToken | null>(1);
  user$ = this.userInfoSource.asObservable();

  private rawTokenSource = new ReplaySubject<string | null>(1);
  token$ = this.rawTokenSource.asObservable();

  constructor(private http: HttpClient) {
    this.loadUserFromStorage(); 
  }

  login(model: LoginDto) {
    return this.http.post<JwtUserData>(`${this.APIUrl}/Login`, model).pipe(
      map((response: JwtUserData) => {
        if (response && response.isSucceeded && response.data?.token) {
          const tokenData: TokenData = { token: response.data.token };
          this.setUser(tokenData);
          return tokenData;
        }
        return null;
      })
    );
  }

  register(model: RegisterDto) {
    return this.http.post(`${this.APIUrl}/Register`, model);
  }

  logout() {
    localStorage.removeItem(this.userKey);
    this.userInfoSource.next(null);
    this.rawTokenSource.next(null);
  }

  private setUser(user: TokenData) {
    localStorage.setItem(this.userKey, JSON.stringify(user));

    const decoded = this.decodeToken(user.token);
    this.userInfoSource.next(decoded);

    this.rawTokenSource.next(user.token);
  }

  private loadUserFromStorage() {
    const stored = localStorage.getItem(this.userKey);
    if (stored) {
      try {
        const parsed = JSON.parse(stored) as TokenData;
        const decoded = this.decodeToken(parsed.token);

        this.userInfoSource.next(decoded);
        this.rawTokenSource.next(parsed.token);
      } catch (e) {
        console.error('Error loading token from localStorage', e);
        this.userInfoSource.next(null);
        this.rawTokenSource.next(null);
      }
    } else {
      this.userInfoSource.next(null);
      this.rawTokenSource.next(null);
    }
  }

  private decodeToken(token: string): DecodedToken {
    try {
      return jwtDecode<DecodedToken>(token);
    } catch (error) {
      console.error('Invalid token');
      return null!;
    }
  }
}
