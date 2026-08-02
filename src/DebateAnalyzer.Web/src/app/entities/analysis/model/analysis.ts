export type AnalysisStatus = 'Queued' | 'Downloading' | 'Transcribing' | 'Analyzing' | 'Completed' | 'Failed';

export interface AnalysisDto {
  readonly id: string;
  readonly status: AnalysisStatus;
  readonly errorMessage: string | null;
}
