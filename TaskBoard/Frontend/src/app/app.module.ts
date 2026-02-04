import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';

import { AppComponent } from './app.component';
import { TaskBoardComponent } from './components/task-board/task-board.component';
import { FilterByStatusPipe } from './pipes/filter-by-status.pipe';

@NgModule({
  declarations: [
    FilterByStatusPipe
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    FormsModule,
    AppComponent,
    TaskBoardComponent
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
