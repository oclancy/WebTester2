import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';

@Injectable()
export class SocketService {

  public GetTime() : Observable<string>{

    return this.client.get<string>('/api/time');
  }

  constructor(private client: HttpClient) { }

}
