import { TestBed } from '@angular/core/testing';
import { ActivatedRoute, convertToParamMap } from '@angular/router';
import { provideRouter } from '@angular/router';
import { Processing } from './processing';

describe('Processing', () => {
  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [Processing],
      providers: [
        provideRouter([]),
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
  });

  it('should render every processing stage', () => {
    const fixture = TestBed.createComponent(Processing);
    fixture.detectChanges();
    const compiled = fixture.nativeElement as HTMLElement;
    expect(compiled.textContent).toContain('Downloading video');
    expect(compiled.textContent).toContain('Generating report');
  });

  it('should show the submitted video url', () => {
    const fixture = TestBed.createComponent(Processing);
    fixture.detectChanges();
    const compiled = fixture.nativeElement as HTMLElement;
    expect(compiled.textContent).toContain('https://youtu.be/dQw4w9WgXcQ');
  });
});
