import { Component, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';

const YOUTUBE_URL_PATTERN =
  /^(https?:\/\/)?(www\.)?(youtube\.com\/watch\?v=|youtu\.be\/)[\w-]{11}(\S*)?$/;

interface SampleDebate {
  readonly title: string;
  readonly url: string;
}

interface CoreFeature {
  readonly title: string;
  readonly description: string;
}

const SAMPLE_DEBATES: readonly SampleDebate[] = [
  { title: 'Obama vs. Romney — 2012 Debate', url: 'https://www.youtube.com/watch?v=oGVDbsTV6h0' },
  { title: 'Biden vs. Trump — 2020 Debate', url: 'https://www.youtube.com/watch?v=1DYaqoyLexY' },
  {
    title: 'Kennedy vs. Nixon — 1960 Debate',
    url: 'https://www.youtube.com/watch?v=BicKuZQNqAI',
  },
];

const CORE_FEATURES: readonly CoreFeature[] = [
  {
    title: 'Fallacy detection',
    description: 'Automatically flags logical fallacies as they happen in the debate.',
  },
  {
    title: 'Fact-checking',
    description: 'Cross-references claims against trusted sources in real time.',
  },
  {
    title: 'Argument explanation',
    description: 'Breaks down each argument in plain language so you see the reasoning.',
  },
];

@Component({
  selector: 'app-home',
  imports: [FormsModule],
  templateUrl: './home.html',
})
export class Home {
  private readonly router = inject(Router);

  protected readonly videoUrl = signal('');
  protected readonly sampleDebates = SAMPLE_DEBATES;
  protected readonly coreFeatures = CORE_FEATURES;

  protected readonly isValidUrl = () => YOUTUBE_URL_PATTERN.test(this.videoUrl().trim());

  protected onSubmit(): void {
    if (!this.isValidUrl()) {
      return;
    }
    this.navigateToProcessingPage();
  }

  protected selectSampleDebate(sample: SampleDebate): void {
    this.videoUrl.set(sample.url);
  }

  private navigateToProcessingPage(): void {
    const jobId = crypto.randomUUID();
    this.router.navigate(['/processing', jobId], {
      queryParams: { url: this.videoUrl().trim() },
    });
  }
}
