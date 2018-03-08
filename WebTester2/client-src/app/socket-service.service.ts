import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import { WebSocketSubject } from 'rxjs/observable/dom/WebSocketSubject'

@Injectable()
export class SocketService {

  private Socket: any;

  public GetTime() {

    //return this.client.get<string>('/api/time');
    this.Socket.next(JSON.stringify({ op: 'GetTime' }));
    
  }

  constructor(private client: HttpClient)
  {
    this.Socket = WebSocketSubject.create('ws://localhost:4368/ws?id=ollie');

    this.Socket.subscribe(
      (msg) => console.log('message received: ' + msg),
      (err) => console.log(err),
      () => console.log('complete')
    );

  }

}
