import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { AppComponent } from './app.component';
import { MessageComponent } from './message/message.component';
import { HomeComponent } from './home/home.component';


const routes: Routes = [
  //{ path: '', component: AppComponent },
  { path: 'message', component: MessageComponent },
  { path: '', component: HomeComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
