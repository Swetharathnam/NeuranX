import { TaskItem } from './task.model';

export interface Board {
  id: number;
  name: string;
  description: string;
  createdAt: Date;
  updatedAt: Date;
  tasks: TaskItem[];
}

export interface CreateBoardRequest {
  name: string;
  description: string;
}

export interface UpdateBoardRequest {
  name: string;
  description: string;
}
