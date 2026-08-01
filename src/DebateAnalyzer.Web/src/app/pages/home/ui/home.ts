import { Component, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';

const YOUTUBE_URL_PATTERN =
  /^(https?:\/\/)?(www\.)?(youtube\.com\/watch\?v=|youtu\.be\/)[\w-]{11}(\S*)?$/;

@Component({
  selector: 'app-home',
  imports: [FormsModule],
  templateUrl: './home.html',
})
export class Home {
  protected readonly videoUrl = signal('');
  protected readonly submitted = signal(false);

  protected readonly isValidUrl = () => YOUTUBE_URL_PATTERN.test(this.videoUrl().trim());

  protected onSubmit(): void {
    if (!this.isValidUrl()) {
      return;
    }
    this.submitted.set(true);
  }
}
