import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';
import { AnalysisDto } from '../model/analysis';

@Injectable({ providedIn: 'root' })
export class AnalysisApiService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiBaseUrl}/analyses`;

  submit(youTubeUrl: string): Observable<AnalysisDto> {
    return this.http.post<AnalysisDto>(this.baseUrl, { youTubeUrl });
  }

  getById(id: string): Observable<AnalysisDto> {
    return this.http.get<AnalysisDto>(`${this.baseUrl}/${id}`);
  }
}
