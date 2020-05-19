import { Component, Inject, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { FormControl } from '@angular/forms';
import { HttpClient, HttpParams } from '@angular/common/http';
import { SimpleNews } from '../models/SimpleNews';
import { ActivatedRoute } from '@angular/router';
import { DomSanitizer } from '@angular/platform-browser';
import { MatIconRegistry } from '@angular/material/icon';
import { MatDialog } from '@angular/material';
import { EmailSubDialogComponent, EmailDialogData } from '../email-sub-dialog/email-sub-dialog.component';

@Component({
  selector: 'app-simple-search',
  templateUrl: './simple-search.component.html',
  styleUrls: ['./simple-search.component.css']
})
export class SimpleSearchComponent implements OnInit {
  displayedColumns = ['title', 'author', 'text', 'tags'];
  newses: SimpleNews[];
  broaderQuery: String[];
  narowerQuery: String[];
  relatedQuery: String[];
  searchQuery = new FormControl('', []);
  dataSource = new MatTableDataSource<SimpleNews>();
  page: number;
  showSemanticSearch: boolean;

  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  constructor(private route: ActivatedRoute,
    private http: HttpClient,
    iconRegistry: MatIconRegistry,
    sanitizer: DomSanitizer,
    public subDialog: MatDialog,
    @Inject('BASE_URL') private baseUrl: string) {
    iconRegistry.addSvgIcon(
      'subscriptions',
      sanitizer.bypassSecurityTrustResourceUrl('assets/icons/subscriptions.svg'));
  }

  ngOnInit() {
    this.showSemanticSearch = false;
    this.dataSource.paginator = this.paginator;
    const sq = this.route.snapshot.paramMap.get('searchQuery');
    if (sq != null) {
      this.searchQuery.setValue(sq);
      this.searchNews();
    }
  }

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

  submitSearch() {
    this.searchNews();
    this.searchBroaderQuery();
    this.searchNarrowerQuery();
    this.searchRelatedQuery();
    this.showSemanticSearch = true;
  }

  searchNews() {
    this.page = 1;
    this.http.get<SimpleNews[]>(this.baseUrl + 'news/simple/' + this.searchQuery.value).subscribe(result => {
      this.dataSource = new MatTableDataSource<SimpleNews>(result);
      this.dataSource.paginator = this.paginator;
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

  openSubscribeDialog() {
    if (this.searchQuery.value != null && this.searchQuery.value != "") {
      const dialogHandler = this.subDialog.open(EmailSubDialogComponent, {
        minWidth: '500px',
        minHeight: '200px',
        data: {
          searchQr: this.searchQuery.value,
        } as EmailDialogData
      });

      dialogHandler.afterClosed().subscribe(result => {
        console.log('The dialog was closed');
      });
    }
  }
}
