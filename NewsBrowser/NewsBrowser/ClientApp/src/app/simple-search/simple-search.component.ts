import { Component, Inject } from '@angular/core';
import { FormControl } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { SimpleNews } from '../models/SimpleNews';

@Component({
  selector: 'app-simple-search',
  templateUrl: './simple-search.component.html',
  styleUrls: ['./simple-search.component.css']
})
export class SimpleSearchComponent {
  displayedColumns = ['title', 'author', 'text', 'tags'];
  newses: SimpleNews[];
  broaderQuery: String[];
  narowerQuery: String[];
  relatedQuery: String[];
  searchQuery = new FormControl('', []);
  page: number;

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {
  }

  submitSearch() {
    this.searchNews();
    this.searchBroaderQuery();
    this.searchNarrowerQuery();
    this.searchRelatedQuery();
  }

  searchNews() {
    this.page = 1;
    this.newses = [];
    this.http.get<SimpleNews[]>(this.baseUrl + 'news/simple/' + this.searchQuery.value).subscribe(result => {
      this.newses = result;
    }, error => console.error(error));
  }

  searchBroaderQuery() {
    this.broaderQuery = [];
    this.http.get<String[]>(this.baseUrl + 'news/broaderQuery/' + this.searchQuery.value).subscribe(result => {
      this.broaderQuery = result;
    }, error => console.error(error));
  }

  searchNarrowerQuery() {
    this.narowerQuery = [];
    this.http.get<String[]>(this.baseUrl + 'news/narrowerQuery/' + this.searchQuery.value).subscribe(result => {
      this.narowerQuery = result;
    }, error => console.error(error));
  }

  searchRelatedQuery() {
    this.relatedQuery = [];
    this.http.get<String[]>(this.baseUrl + 'news/relatedQuery/' + this.searchQuery.value).subscribe(result => {
      this.relatedQuery = result;
    }, error => console.error(error));
  }
}
