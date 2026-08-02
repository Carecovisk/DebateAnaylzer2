import { provideHttpClient } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';
import { ActivatedRoute, convertToParamMap } from '@angular/router';
import { provideRouter } from '@angular/router';
import { AnalysisDto } from '../../../entities/analysis/model/analysis';
import { Processing } from './processing';

describe('Processing', () => {
  let httpMock: HttpTestingController;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [Processing],
      providers: [
        provideRouter([]),
        provideHttpClient(),
        provideHttpClientTesting(),
        {
          provide: ActivatedRoute,
          useValue: {
            snapshot: {
              paramMap: convertToParamMap({ jobId: 'job-123' }),
              queryParamMap: convertToParamMap({ url: 'https://youtu.be/dQw4w9WgXcQ' }),
            },
          },
        },
      ],
    }).compileComponents();

    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  function flushStatus(status: AnalysisDto['status']): void {
    httpMock
      .expectOne('http://localhost:5051/api/v1/analyses/job-123')
      .flush({ id: 'job-123', status, errorMessage: null } satisfies AnalysisDto);
  }

  it('should render every processing stage', async () => {
    const fixture = TestBed.createComponent(Processing);
    fixture.detectChanges();
    await new Promise((resolve) => setTimeout(resolve));
    flushStatus('Downloading');
    fixture.detectChanges();

    const compiled = fixture.nativeElement as HTMLElement;
    expect(compiled.textContent).toContain('Downloading video');
    expect(compiled.textContent).toContain('Analyzing debate');
  });

  it('should show the submitted video url', async () => {
    const fixture = TestBed.createComponent(Processing);
    fixture.detectChanges();
    await new Promise((resolve) => setTimeout(resolve));
    flushStatus('Downloading');
    fixture.detectChanges();

    const compiled = fixture.nativeElement as HTMLElement;
    expect(compiled.textContent).toContain('https://youtu.be/dQw4w9WgXcQ');
  });
});
