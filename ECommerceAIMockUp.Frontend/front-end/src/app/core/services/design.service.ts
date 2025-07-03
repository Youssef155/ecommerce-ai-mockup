import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Design } from '../models/design.model';

@Injectable({ providedIn: 'root' })

export class DesignService {
  private apiUrl = 'https://your-api-url/api/designs';
  constructor(private http: HttpClient) {}

  getDesigns(): Observable<Design[]> {
    return this.http.get<Design[]>(this.apiUrl);
  }

  uploadDesign(file: File): Observable<any> {
    const formData = new FormData();
    formData.append('file', file);
    return this.http.post(`${this.apiUrl}/upload`, formData);
  }

  generateDesign(prompt: string): Observable<any> {
    return this.http.post(`${this.apiUrl}/generate`, { prompt });
  }

  saveGeneratedDesign(design: Design): Observable<any> {
    return this.http.post(`${this.apiUrl}/savegenerateddesign`, design);
  }

  discardDesign(design: Design): Observable<any> {
    return this.http.post(`${this.apiUrl}/discardgenerateddesign`, design);
  }
}