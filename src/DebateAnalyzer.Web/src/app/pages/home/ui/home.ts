import { Component, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';

const YOUTUBE_URL_PATTERN =
  /^(https?:\/\/)?(www\.)?(youtube\.com\/watch\?v=|youtu\.be\/)[\w-]{11}(\S*)?$/;

@Component({
  selector: 'app-home',
  imports: [FormsModule],
  templateUrl: './home.html',
})
export class Home {
  private readonly router = inject(Router);

  protected readonly videoUrl = signal('');

  protected readonly isValidUrl = () => YOUTUBE_URL_PATTERN.test(this.videoUrl().trim());

  protected onSubmit(): void {
    if (!this.isValidUrl()) {
      return;
    }
    const jobId = crypto.randomUUID();
    this.router.navigate(['/processing', jobId], {
      queryParams: { url: this.videoUrl().trim() },
    });
  }
}
