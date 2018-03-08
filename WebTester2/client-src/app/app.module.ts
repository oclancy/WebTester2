import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';

import { AppComponent } from './app.component';
import { MessageComponent } from './message/message.component';
import { SocketService } from './socket-service.service';
import { AppRoutingModule } from './/app-routing.module';
import { TestDirective } from './test.directive';
import { HomeComponent } from './home/home.component';


@NgModule({
  declarations: [
    AppComponent,
    MessageComponent,
    TestDirective,
    HomeComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    AppRoutingModule
  ],
  providers: [
    SocketService,
    
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
