import { TestBed } from '@angular/core/testing';
import { provideRouter } from '@angular/router';
import { Home } from './home';

describe('Home', () => {
  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [Home],
      providers: [provideRouter([])],
    }).compileComponents();
  });

  it('should render the hero headline', () => {
    const fixture = TestBed.createComponent(Home);
    fixture.detectChanges();
    const compiled = fixture.nativeElement as HTMLElement;
    expect(compiled.querySelector('h1')?.textContent).toContain('argument');
  });

  it('should disable the submit button for an invalid url', () => {
    const fixture = TestBed.createComponent(Home);
    fixture.componentInstance['videoUrl'].set('not-a-youtube-link');
    fixture.detectChanges();
    const button = fixture.nativeElement.querySelector('button[type="submit"]') as HTMLButtonElement;
    expect(button.disabled).toBeTruthy();
  });

  it('should enable the submit button for a valid youtube url', () => {
    const fixture = TestBed.createComponent(Home);
    fixture.componentInstance['videoUrl'].set('https://www.youtube.com/watch?v=dQw4w9WgXcQ');
    fixture.detectChanges();
    const button = fixture.nativeElement.querySelector('button[type="submit"]') as HTMLButtonElement;
    expect(button.disabled).toBeFalsy();
  });
});
