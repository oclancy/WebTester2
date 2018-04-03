import { Component, OnInit } from '@angular/core';
import { SocketService } from '../socket-service.service'
import { Observable } from 'rxjs/Rx';

@Component({
  selector: 'app-message',
  templateUrl: './message.component.html',
  styleUrls: ['./message.component.css']
})
export class MessageComponent implements OnInit {

  constructor(private socketService: SocketService) { }

  public op: string;
  public data: string;

  get MessageTypes(): string[]
  {
    return [ "Shutdown", "RequestPrice" ];
  }

  sendMessage() {
    this.socketService
        .Send(this.op, this.data);
  }

  ngOnInit() {
  }

}
