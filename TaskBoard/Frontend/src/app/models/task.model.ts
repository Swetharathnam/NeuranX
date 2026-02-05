export enum TaskStatus {
  Todo = 0,
  InProgress = 1,
  Done = 2
}

export enum TaskPriority {
  Low = 0,
  Medium = 1,
  High = 2
}

export interface TaskItem {
  id: number;
  title: string;
  description: string;
  status: TaskStatus;
  priority: TaskPriority;
  dueDate?: Date | string;
  boardId: number;
  createdAt: Date | string;
  updatedAt: Date | string;
}

export interface CreateTaskRequest {
  title: string;
  description: string;
  status: TaskStatus;
  priority: TaskPriority;
  dueDate?: Date | string;
  boardId: number;
}

export interface UpdateTaskRequest {
  title: string;
  description: string;
  status: TaskStatus;
  priority: TaskPriority;
  dueDate?: Date | string;
}
