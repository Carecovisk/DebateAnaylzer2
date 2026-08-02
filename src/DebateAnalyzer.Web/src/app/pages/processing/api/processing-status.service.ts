import { Injectable, inject } from '@angular/core';
import { Observable, retry, switchMap, takeWhile, timer } from 'rxjs';
import { map } from 'rxjs/operators';
import { AnalysisApiService } from '../../../entities/analysis/api/analysis-api.service';
import { mapAnalysisToProcessingStatus } from '../model/map-analysis-to-processing-status';
import { ProcessingStatus } from '../model/processing-status';

const POLL_INTERVAL_MS = 2000;
const MAX_RETRIES = 3;

@Injectable({ providedIn: 'root' })
export class ProcessingStatusService {
  private readonly analysisApi = inject(AnalysisApiService);

  poll(jobId: string): Observable<ProcessingStatus> {
    return timer(0, POLL_INTERVAL_MS).pipe(
      switchMap(() =>
        this.analysisApi.getById(jobId).pipe(retry({ count: MAX_RETRIES, delay: POLL_INTERVAL_MS })),
      ),
      map(mapAnalysisToProcessingStatus),
      takeWhile((status) => !status.completed && !status.failed, true),
    );
  }
}
