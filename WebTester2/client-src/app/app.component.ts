import { Component, OnInit } from '@angular/core';
import { SocketService } from './socket-service.service'
import { Observable } from 'rxjs/Rx';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
 export class AppComponent implements OnInit{

  public Time: string;

  constructor(private socketService: SocketService)
  {
  }

  ngOnInit(): void {
    this.Time = "Waiting..."

    Observable.interval(500)
              .subscribe((x) => {
                this.socketService.GetTime()
                    .subscribe( s => this.Time = s );
              })
  }

  title = 'app';
}
