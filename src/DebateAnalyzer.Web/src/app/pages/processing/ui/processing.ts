import { Component, DestroyRef, inject, OnInit, signal } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { ActivatedRoute, Router } from '@angular/router';
import { ProcessingStatusService } from '../api/processing-status.service';
import { PROCESSING_STAGES, ProcessingStage, ProcessingStatus, StageState } from '../model/processing-status';

@Component({
  selector: 'app-processing',
  templateUrl: './processing.html',
})
export class Processing implements OnInit {
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  private readonly statusService = inject(ProcessingStatusService);
  private readonly destroyRef = inject(DestroyRef);

  protected readonly stages: readonly ProcessingStage[] = PROCESSING_STAGES;
  protected readonly videoUrl = signal(this.route.snapshot.queryParamMap.get('url') ?? '');
  protected readonly status = signal<ProcessingStatus | null>(null);

  protected stageState(stageId: ProcessingStage['id']): StageState {
    return this.status()?.stageStates[stageId] ?? 'pending';
  }

  ngOnInit(): void {
    const jobId = this.route.snapshot.paramMap.get('jobId');
    if (!jobId) {
      this.router.navigateByUrl('/');
      return;
    }

    this.statusService
      .poll(jobId)
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe({
        next: (status) => this.status.set(status),
        complete: () => this.router.navigate(['/results', jobId]),
      });
  }
}
