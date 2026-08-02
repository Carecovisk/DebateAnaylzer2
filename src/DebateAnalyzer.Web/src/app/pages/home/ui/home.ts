import { Component, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { AnalysisApiService } from '../../../entities/analysis/api/analysis-api.service';
import { AnalysisDto } from '../../../entities/analysis/model/analysis';

const YOUTUBE_URL_PATTERN =
  /^(https?:\/\/)?(www\.)?(youtube\.com\/watch\?v=|youtu\.be\/)[\w-]{11}(\S*)?$/;

@Component({
  selector: 'app-home',
  imports: [FormsModule],
  templateUrl: './home.html',
})
export class Home {
  private readonly router = inject(Router);
  private readonly analysisApi = inject(AnalysisApiService);

  protected readonly videoUrl = signal('');
  protected readonly isSubmitting = signal(false);
  protected readonly submitError = signal<string | null>(null);

  protected readonly isValidUrl = () => YOUTUBE_URL_PATTERN.test(this.videoUrl().trim());

  protected onSubmit(): void {
    if (!this.isValidUrl() || this.isSubmitting()) {
      return;
    }
    this.submitAnalysis();
  }

  private submitAnalysis(): void {
    this.isSubmitting.set(true);
    this.submitError.set(null);

    this.analysisApi.submit(this.videoUrl().trim()).subscribe({
      next: (analysis) => this.navigateToProcessingPage(analysis.id),
      error: () => this.handleSubmitError(),
    });
  }

  private handleSubmitError(): void {
    this.isSubmitting.set(false);
    this.submitError.set('Could not start analysis. Please try again.');
  }

  private navigateToProcessingPage(jobId: AnalysisDto['id']): void {
    this.router.navigate(['/processing', jobId], {
      queryParams: { url: this.videoUrl().trim() },
    });
  }
}
