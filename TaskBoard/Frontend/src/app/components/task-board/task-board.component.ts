import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Board, CreateBoardRequest, UpdateBoardRequest } from '../../models/board.model';
import { TaskItem } from '../../models/task.model';
import { BoardService } from '../../services/board.service';
import { TaskService } from '../../services/task.service';
import { TaskItemComponent } from '../task-item/task-item.component';
import { FilterByStatusPipe } from '../../pipes/filter-by-status.pipe';

@Component({
  selector: 'app-task-board',
  standalone: true,
  imports: [CommonModule, FormsModule, TaskItemComponent, FilterByStatusPipe],
  templateUrl: './task-board.component.html',
  styleUrls: ['./task-board.component.css']
})
export class TaskBoardComponent implements OnInit {
  boards: Board[] = [];
  selectedBoard: Board | null = null;
  newBoardName: string = '';
  newBoardDescription: string = '';
  editingBoardId: number | null = null;
  editingBoardName: string = '';
  editingBoardDescription: string = '';
  isLoadingBoards: boolean = false;
  isLoadingTasks: boolean = false;
  errorMessage: string = '';
  newTaskTitle: string = '';
  newTaskDescription: string = '';
  newTaskPriority: number = 1;
  showAddTaskForm: boolean = false;

  constructor(private boardService: BoardService, private taskService: TaskService) { }

  ngOnInit(): void {
    this.loadBoards();
  }

  loadBoards(): void {
    this.isLoadingBoards = true;
    this.errorMessage = '';
    this.boardService.getAllBoards().subscribe({
      next: (data: Board[]) => {
        this.boards = data;
        if (this.boards.length > 0) {
          this.selectBoard(this.boards[0]);
        }
        this.isLoadingBoards = false;
      },
      error: (error: any) => {
        this.errorMessage = 'Failed to load boards';
        console.error('Error loading boards:', error);
        this.isLoadingBoards = false;
      }
    });
  }

  selectBoard(board: Board): void {
    this.selectedBoard = board;
    this.loadBoardTasks();
  }

  loadBoardTasks(): void {
    if (!this.selectedBoard) return;
    this.isLoadingTasks = true;
    this.taskService.getTasksByBoardId(this.selectedBoard.id).subscribe({
      next: (tasks: TaskItem[]) => {
        if (this.selectedBoard) {
          this.selectedBoard.tasks = tasks;
        }
        this.isLoadingTasks = false;
      },
      error: (error: any) => {
        this.errorMessage = 'Failed to load tasks';
        console.error('Error loading tasks:', error);
        this.isLoadingTasks = false;
      }
    });
  }

  createBoard(): void {
    if (!this.newBoardName.trim()) {
      this.errorMessage = 'Board name is required';
      return;
    }

    const newBoard: CreateBoardRequest = {
      name: this.newBoardName,
      description: this.newBoardDescription
    };

    this.boardService.createBoard(newBoard).subscribe({
      next: () => {
        this.newBoardName = '';
        this.newBoardDescription = '';
        this.loadBoards();
      },
      error: (error: any) => {
        this.errorMessage = 'Failed to create board';
        console.error('Error creating board:', error);
      }
    });
  }

  updateBoard(): void {
    if (!this.editingBoardId || !this.editingBoardName.trim()) {
      this.errorMessage = 'Board name is required';
      return;
    }

    const updateBoard: UpdateBoardRequest = {
      name: this.editingBoardName,
      description: this.editingBoardDescription
    };

    this.boardService.updateBoard(this.editingBoardId, updateBoard).subscribe({
      next: () => {
        this.editingBoardId = null;
        this.editingBoardName = '';
        this.editingBoardDescription = '';
        this.loadBoards();
      },
      error: (error: any) => {
        this.errorMessage = 'Failed to update board';
        console.error('Error updating board:', error);
      }
    });
  }

  deleteBoard(boardId: number): void {
    if (confirm('Are you sure you want to delete this board? All tasks will be deleted as well.')) {
      this.boardService.deleteBoard(boardId).subscribe({
        next: () => {
          this.loadBoards();
        },
        error: (error: any) => {
          this.errorMessage = 'Failed to delete board';
          console.error('Error deleting board:', error);
        }
      });
    }
  }

  startEditBoard(board: Board): void {
    this.editingBoardId = board.id;
    this.editingBoardName = board.name;
    this.editingBoardDescription = board.description;
  }

  cancelEditBoard(): void {
    this.editingBoardId = null;
    this.editingBoardName = '';
    this.editingBoardDescription = '';
  }

  onTaskUpdated(updatedTask: TaskItem): void {
    if (this.selectedBoard) {
      const index = this.selectedBoard.tasks.findIndex(t => t.id === updatedTask.id);
      if (index > -1) {
        // Create a new array reference to trigger Angular change detection
        this.selectedBoard.tasks = [
          ...this.selectedBoard.tasks.slice(0, index),
          updatedTask,
          ...this.selectedBoard.tasks.slice(index + 1)
        ];
      }
    }
  }

  onTaskDeleted(taskId: number): void {
    if (this.selectedBoard) {
      this.selectedBoard.tasks = this.selectedBoard.tasks.filter(t => t.id !== taskId);
    }
  }

  createTask(): void {
    if (!this.selectedBoard) {
      this.errorMessage = 'Please select a board first';
      return;
    }

    if (!this.newTaskTitle.trim()) {
      this.errorMessage = 'Task title is required';
      return;
    }

    const newTask = {
      title: this.newTaskTitle,
      description: this.newTaskDescription,
      status: 0, // TaskStatus.Todo
      priority: this.newTaskPriority,
      boardId: this.selectedBoard.id
    };

    this.taskService.createTask(newTask).subscribe({
      next: (createdTask: any) => {
        if (this.selectedBoard) {
          // Create a new array reference to trigger Angular change detection
          this.selectedBoard.tasks = [...this.selectedBoard.tasks, createdTask];
        }
        this.newTaskTitle = '';
        this.newTaskDescription = '';
        this.newTaskPriority = 1;
        this.errorMessage = '';
      },
      error: (error: any) => {
        // Show more detailed error message from backend
        if (error.error && typeof error.error === 'string') {
          this.errorMessage = error.error;
        } else if (error.message) {
          this.errorMessage = error.message;
        } else {
          this.errorMessage = 'Failed to create task';
        }
        console.error('Error creating task:', error);
      }
    });
  }
}
