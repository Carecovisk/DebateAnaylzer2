import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    loadComponent: () => import('./pages/home/ui/home').then((m) => m.Home),
  },
  {
    path: 'processing/:jobId',
    loadComponent: () => import('./pages/processing/ui/processing').then((m) => m.Processing),
  },
  {
    path: 'results/:jobId',
    loadComponent: () => import('./pages/results/ui/results').then((m) => m.Results),
  },
];
