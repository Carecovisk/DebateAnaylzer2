import { AnalysisDto, AnalysisStatus } from '../../../entities/analysis/model/analysis';
import { ProcessingStageId, ProcessingStatus, StageState } from './processing-status';

const STAGE_ORDER: readonly ProcessingStageId[] = ['downloading', 'transcribing', 'analyzing'];

export function mapAnalysisToProcessingStatus(dto: AnalysisDto): ProcessingStatus {
  const activeIndex = stageIndexForStatus(dto.status);

  return {
    jobId: dto.id,
    stageStates: buildStageStates(activeIndex, dto.status),
    progressPercent: progressPercentFor(activeIndex, dto.status),
    completed: dto.status === 'Completed',
    failed: dto.status === 'Failed',
    errorMessage: dto.errorMessage,
  };
}

function stageIndexForStatus(status: AnalysisStatus): number {
  switch (status) {
    case 'Downloading':
      return 0;
    case 'Transcribing':
      return 1;
    case 'Analyzing':
      return 2;
    default:
      return -1;
  }
}

function buildStageStates(
  activeIndex: number,
  status: AnalysisStatus,
): Record<ProcessingStageId, StageState> {
  const allDone = status === 'Completed';

  return Object.fromEntries(
    STAGE_ORDER.map((id, index): [ProcessingStageId, StageState] => [
      id,
      allDone ? 'done' : index < activeIndex ? 'done' : index === activeIndex ? 'active' : 'pending',
    ]),
  ) as Record<ProcessingStageId, StageState>;
}

function progressPercentFor(activeIndex: number, status: AnalysisStatus): number {
  if (status === 'Completed') {
    return 100;
  }
  if (activeIndex < 0) {
    return 0;
  }
  return Math.round(((activeIndex + 1) / STAGE_ORDER.length) * 100);
}
