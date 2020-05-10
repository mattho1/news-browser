import { Component, Inject } from '@angular/core';
import { FormControl } from '@angular/forms';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-simple-search',
  templateUrl: './simple-search.component.html',
  styleUrls: ['./simple-search.component.css']
})
export class SimpleSearchComponent {
  newses: News[];
  searchQuery = new FormControl('', []);
  page: number;

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {
  }

  searchNews() {
    this.page = 1;
    this.newses = [];
    this.http.get<News[]>(this.baseUrl + 'news/simple/' + this.searchQuery.value).subscribe(result => {
      this.newses = result;
    }, error => console.error(error));
  }
}

interface News {
  title: string;
  text: number;
}
