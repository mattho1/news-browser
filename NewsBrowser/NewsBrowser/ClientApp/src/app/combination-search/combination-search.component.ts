import { Component, Inject, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { FormControl } from '@angular/forms';
import { HttpClient, HttpParams } from '@angular/common/http';
import { SimpleNews } from '../models/SimpleNews';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-combination-search',
  templateUrl: './combination-search.component.html',
  styleUrls: ['./combination-search.component.css']
})
export class CombinationSearchComponent implements OnInit {
  displayedColumns = ['title', 'author', 'text', 'tags'];
  newses: SimpleNews[];
  searchQuery = new FormControl('', []);
  dataSource = new MatTableDataSource<SimpleNews>();
  page: number;
  selectedFieldType: string;
  selectedQueryType: string;

  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;

  constructor(private route: ActivatedRoute, private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {
  }

  ngOnInit() {
    this.dataSource.paginator = this.paginator;
    const ft = this.route.snapshot.paramMap.get('fieldType');
    const sq = this.route.snapshot.paramMap.get('searchQuery');
    if (ft != null && this.fieldTypes.includes(ft.toLowerCase())) {
        this.selectedFieldType = ft;
    }
    else {
      this.selectedFieldType = this.fieldTypes[0];
    }
    this.selectedQueryType = this.queryTypes[0];

    if (sq != null) {
      this.searchQuery.setValue(sq);
      this.searchNews();
    }
  }

  searchNews() {
    this.page = 1;
    let params = new HttpParams();
    params = params.append('queryType', this.selectedQueryType);
    params = params.append('fieldType', this.selectedFieldType);
    params = params.append('queryContent', this.searchQuery.value);

    this.http.get<SimpleNews[]>(this.baseUrl + 'news/combination', { params: params }).subscribe(result => {
      this.dataSource = new MatTableDataSource<SimpleNews>(result);
      this.dataSource.paginator = this.paginator;
    }, error => console.error(error));
  }

  newSearch(event, query: string) {
    this.selectedQueryType = 'exact';
    this.selectedFieldType = 'tag';
    this.searchQuery.setValue(query);
    this.searchNews();
  }

  fieldTypes: string[] = ['all', 'title', 'author', 'content', 'tag'];
  queryTypes: string[] = ['exact', 'fuzzy', 'synonyms'];

  chipColor(tagType: number) {
    if (tagType == 1) {
      return 'primary';
    } else if (tagType == 2) {
      return 'accent';
    } else if (tagType == 3) {
      return 'warn';
    } else if (tagType == 4) {
      return 'theme';
    }
    return 'basic';
  }
}

