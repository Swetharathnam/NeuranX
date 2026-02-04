import { Pipe, PipeTransform } from '@angular/core';
import { TaskItem } from '../models/task.model';

@Pipe({
  name: 'filter',
  standalone: true
})
export class FilterByStatusPipe implements PipeTransform {
  transform(tasks: TaskItem[], status: number): TaskItem[] {
    if (!tasks) {
      return [];
    }
    return tasks.filter(task => task.status === status);
  }
}
