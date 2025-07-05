import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Design } from '../models/design.model';
import { GeneratedDesign } from '../models/generated-design.model';

@Injectable({ providedIn: 'root' })

export class DesignService {
  private apiUrl = 'https://localhost:7256/api/Design';
  constructor(private http: HttpClient) {}

  getDesigns(): Observable<any> {
    return this.http.get<any>(this.apiUrl);
  }

  uploadDesign(file: File): Observable<any> {
    const formData = new FormData();
    formData.append('imageFile', file);
    return this.http.post(`${this.apiUrl}/Upload`, formData);
  }

  generateDesign(promptText: string): Observable<any> {
    return this.http.post(`${this.apiUrl}/Generate`, { prompt: promptText });
  }

  saveGeneratedDesign(generatedDesign: GeneratedDesign): Observable<any> {
    return this.http.post(`${this.apiUrl}/Savegenerateddesign`, generatedDesign);
  }

  discardDesign(generatedDesign: GeneratedDesign): Observable<any> {
    return this.http.post(`${this.apiUrl}/discardgenerateddesign`, generatedDesign);
  }
}