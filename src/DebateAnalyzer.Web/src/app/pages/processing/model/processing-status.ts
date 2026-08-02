export type ProcessingStageId = 'downloading' | 'transcribing' | 'analyzing';

export interface ProcessingStage {
  readonly id: ProcessingStageId;
  readonly label: string;
}

export const PROCESSING_STAGES: readonly ProcessingStage[] = [
  { id: 'downloading', label: 'Downloading video' },
  { id: 'transcribing', label: 'Transcribing audio' },
  { id: 'analyzing', label: 'Analyzing debate' },
];

export type StageState = 'pending' | 'active' | 'done';

export interface ProcessingStatus {
  readonly jobId: string;
  readonly stageStates: Readonly<Record<ProcessingStageId, StageState>>;
  readonly progressPercent: number;
  readonly completed: boolean;
  readonly failed: boolean;
  readonly errorMessage: string | null;
}
