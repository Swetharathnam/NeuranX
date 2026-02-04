import { Component, Input, Output, EventEmitter, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { TaskItem, TaskStatus, TaskPriority, UpdateTaskRequest } from '../../models/task.model';
import { TaskService } from '../../services/task.service';

@Component({
  selector: 'app-task-item',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './task-item.component.html',
  styleUrls: ['./task-item.component.css']
})
export class TaskItemComponent implements OnInit {
  @Input() task!: TaskItem;
  @Output() taskUpdated = new EventEmitter<TaskItem>();
  @Output() taskDeleted = new EventEmitter<number>();

  isEditing: boolean = false;
  editingTask: UpdateTaskRequest = {
    title: '',
    description: '',
    status: TaskStatus.Todo,
    priority: TaskPriority.Medium,
    dueDate: undefined
  };

  TaskStatus = TaskStatus;
  TaskPriority = TaskPriority;
  priorityLabels = {
    [TaskPriority.Low]: 'Low',
    [TaskPriority.Medium]: 'Medium',
    [TaskPriority.High]: 'High'
  };
  statusLabels = {
    [TaskStatus.Todo]: 'To Do',
    [TaskStatus.InProgress]: 'In Progress',
    [TaskStatus.Done]: 'Done'
  };

  constructor(private taskService: TaskService) { }

  ngOnInit(): void {
  }

  startEdit(): void {
    this.isEditing = true;
    this.editingTask = {
      title: this.task.title,
      description: this.task.description,
      status: this.task.status,
      priority: this.task.priority,
      dueDate: this.task.dueDate
    };
  }

  saveTask(): void {
    if (!this.editingTask.title.trim()) {
      alert('Task title is required');
      return;
    }

    this.taskService.updateTask(this.task.id, this.editingTask).subscribe({
      next: (updatedTask: TaskItem) => {
        this.task = updatedTask;
        this.isEditing = false;
        this.taskUpdated.emit(updatedTask);
      },
      error: (error: any) => {
        console.error('Error updating task:', error);
        alert('Failed to update task');
      }
    });
  }

  cancelEdit(): void {
    this.isEditing = false;
  }

  deleteTask(): void {
    if (confirm('Are you sure you want to delete this task?')) {
      this.taskService.deleteTask(this.task.id).subscribe({
        next: () => {
          this.taskDeleted.emit(this.task.id);
        },
        error: (error: any) => {
          console.error('Error deleting task:', error);
          alert('Failed to delete task');
        }
      });
    }
  }

  changeStatus(): void {
    const statuses = [TaskStatus.Todo, TaskStatus.InProgress, TaskStatus.Done];
    const currentIndex = statuses.indexOf(this.task.status);
    const nextStatus = statuses[(currentIndex + 1) % statuses.length];

    const updateRequest: UpdateTaskRequest = {
      title: this.task.title,
      description: this.task.description,
      status: nextStatus,
      priority: this.task.priority,
      dueDate: this.task.dueDate
    };

    this.taskService.updateTask(this.task.id, updateRequest).subscribe({
      next: (updatedTask: TaskItem) => {
        this.task = updatedTask;
        this.taskUpdated.emit(updatedTask);
      },
      error: (error: any) => {
        console.error('Error updating task status:', error);
      }
    });
  }

  getPriorityClass(): string {
    switch (this.task.priority) {
      case TaskPriority.High:
        return 'priority-high';
      case TaskPriority.Medium:
        return 'priority-medium';
      case TaskPriority.Low:
        return 'priority-low';
      default:
        return '';
    }
  }

  getStatusClass(): string {
    switch (this.task.status) {
      case TaskStatus.Done:
        return 'status-done';
      case TaskStatus.InProgress:
        return 'status-in-progress';
      case TaskStatus.Todo:
        return 'status-todo';
      default:
        return '';
    }
  }

  formatDate(date: Date | undefined): string {
    if (!date) return 'No due date';
    return new Date(date).toLocaleDateString();
  }
}
