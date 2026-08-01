export type ProcessingStageId =
  | 'downloading'
  | 'transcribing'
  | 'detecting-fallacies'
  | 'fact-checking'
  | 'generating-report';

export interface ProcessingStage {
  readonly id: ProcessingStageId;
  readonly label: string;
}

export const PROCESSING_STAGES: readonly ProcessingStage[] = [
  { id: 'downloading', label: 'Downloading video' },
  { id: 'transcribing', label: 'Transcribing audio' },
  { id: 'detecting-fallacies', label: 'Detecting fallacies' },
  { id: 'fact-checking', label: 'Fact-checking claims' },
  { id: 'generating-report', label: 'Generating report' },
];

export type StageState = 'pending' | 'active' | 'done';

export interface ProcessingStatus {
  readonly jobId: string;
  readonly stageStates: Readonly<Record<ProcessingStageId, StageState>>;
  readonly progressPercent: number;
  readonly completed: boolean;
}
