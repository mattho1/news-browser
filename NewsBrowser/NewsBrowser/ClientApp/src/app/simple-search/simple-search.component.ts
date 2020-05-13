import { Component, Inject, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { SimpleNews } from '../models/SimpleNews';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-simple-search',
  templateUrl: './simple-search.component.html',
  styleUrls: ['./simple-search.component.css']
})
export class SimpleSearchComponent implements OnInit {
  displayedColumns = ['title', 'author', 'text', 'tags'];
  newses: SimpleNews[];
  searchQuery = new FormControl('', []);
  page: number;

  constructor(private route: ActivatedRoute, private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {
  }

  ngOnInit() {
    const sq = this.route.snapshot.paramMap.get('searchQuery');
    if (sq != null) {
      this.searchQuery.setValue(sq);
      this.searchNews();
    }
  }

  searchNews() {
    this.page = 1;
    this.newses = [];
    this.http.get<SimpleNews[]>(this.baseUrl + 'news/simple/' + this.searchQuery.value).subscribe(result => {
      this.newses = result;
    }, error => console.error(error));
  }
}

