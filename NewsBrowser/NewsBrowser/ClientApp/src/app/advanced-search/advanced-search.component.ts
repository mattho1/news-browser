import { Component, Inject, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { FormControl } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { SimpleNews } from '../models/SimpleNews';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-advanced-search',
  templateUrl: './advanced-search.component.html',
  styleUrls: ['./advanced-search.component.css']
})
export class AdvancedSearchComponent implements OnInit {
  displayedColumns = ['title', 'author', 'text', 'tags'];
  newses: SimpleNews[];
  searchQuery = new FormControl('', []);
  dataSource = new MatTableDataSource<SimpleNews>();
  page: number;
  selectedType: string;

  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;

  constructor(private route: ActivatedRoute, private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {
  }

  ngOnInit() {
    this.dataSource.paginator = this.paginator;
    const ft = this.route.snapshot.paramMap.get('fieldType');
    const sq = this.route.snapshot.paramMap.get('searchQuery');
    if (ft != null && this.fieldTypes.includes(ft.toLowerCase())) {
        this.selectedType = ft;
    }
    else {
      this.selectedType = this.fieldTypes[0];
    }

    if (sq != null) {
      this.searchQuery.setValue(sq);
      this.searchNews();
    }
  }

  searchNews() {
    this.page = 1;
    this.http.get<SimpleNews[]>(this.baseUrl + 'news/advanced/' + this.selectedType + '/' + this.searchQuery.value).subscribe(result => {
      this.dataSource = new MatTableDataSource<SimpleNews>(result);
      this.dataSource.paginator = this.paginator;
    }, error => console.error(error));
  }

  fieldTypes: string[] = ['all', 'title', 'author', 'content', 'tag'];

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

