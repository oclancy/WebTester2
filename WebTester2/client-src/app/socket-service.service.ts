import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import { WebSocketSubject } from 'rxjs/observable/dom/WebSocketSubject'

@Injectable()
export class SocketService {

  private id: string;

  public Send(op:string, data:any): any  {
    this.Socket.next(JSON.stringify({ id: this.id, op: op, data:data }));
  }

  private Socket: any;

  public GetTime() {

    //return this.client.get<string>('/api/time');
    this.Send( 'GetTime', null );
    
  }

  private OnMessage(msg: any)
  {
    console.log('message received: ' + msg);

    if (this.id == null)
      this.id = msg;

  }

  constructor(private client: HttpClient)
  {
    this.Socket = WebSocketSubject.create('ws://localhost:4368/ws?id=ollie');

    this.Socket.subscribe(
      this.OnMessage,
      (err) => console.log(err),
      () => console.log('complete')
    );

  }

}
