import { Component, inject, signal } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-results',
  templateUrl: './results.html',
})
export class Results {
  private readonly route = inject(ActivatedRoute);

  protected readonly jobId = signal(this.route.snapshot.paramMap.get('jobId') ?? '');
}
