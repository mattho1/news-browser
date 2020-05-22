import { Component, Inject, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { HttpClient, HttpParams } from '@angular/common/http';

@Component({
  selector: 'app-unsubscribe',
  templateUrl: './unsubscribe.component.html',
  styleUrls: ['./unsubscribe.component.css']
})
export class UnsubscribeComponent implements OnInit {
  subscriberId: string;

  constructor(private route: ActivatedRoute,
    private http: HttpClient,
    @Inject('BASE_URL') private baseUrl: string) {
  }

  ngOnInit(): void {
    this.subscriberId = this.route.snapshot.paramMap.get('subscriberId');
    this.subscribe();
  }

  subscribe() {
    let params = new HttpParams();
    params = params.append('subscriberId', this.subscriberId);

    this.http.get(this.baseUrl + 'subscriber/unsubscribe', { params: params }).subscribe(_ => {}, error => console.error(error));
  }
}
