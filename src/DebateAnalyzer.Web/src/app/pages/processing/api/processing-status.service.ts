import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { PROCESSING_STAGES, ProcessingStatus, StageState } from '../model/processing-status';

const STAGE_DURATION_MS = 1400;

/**
 * Mock implementation until the backend exposes a real status endpoint.
 * Simulates advancing through PROCESSING_STAGES one at a time.
 */
@Injectable({ providedIn: 'root' })
export class ProcessingStatusService {
  poll(jobId: string): Observable<ProcessingStatus> {
    return new Observable<ProcessingStatus>((subscriber) => {
      let currentStageIndex = 0;

      const emit = () => {
        const stageStates = Object.fromEntries(
          PROCESSING_STAGES.map((stage, index): [string, StageState] => [
            stage.id,
            index < currentStageIndex ? 'done' : index === currentStageIndex ? 'active' : 'pending',
          ]),
        ) as ProcessingStatus['stageStates'];

        const completed = currentStageIndex >= PROCESSING_STAGES.length;
        const progressPercent = Math.min(
          100,
          Math.round((currentStageIndex / PROCESSING_STAGES.length) * 100),
        );

        subscriber.next({
          jobId,
          stageStates: completed
            ? (Object.fromEntries(
                PROCESSING_STAGES.map((stage): [string, StageState] => [stage.id, 'done']),
              ) as ProcessingStatus['stageStates'])
            : stageStates,
          progressPercent: completed ? 100 : progressPercent,
          completed,
        });

        if (completed) {
          subscriber.complete();
          return;
        }

        currentStageIndex += 1;
      };

      emit();
      const intervalId = setInterval(emit, STAGE_DURATION_MS);

      return () => clearInterval(intervalId);
    });
  }
}
