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
  broaderConcepts: string[];
  narowerConcepts: string[];
  relatedConcepts: string[];
  broaderConceptsFreq: { [id: string] : number; } = {};
  narrowerConceeptsFreq: { [id: string] : number; } = {};
  relatedQueryFreq: { [id: string] : number; } = {};
  searchQuery = new FormControl('', []);
  dataSource = new MatTableDataSource<SimpleNews>();
  page: number;
  indexFreqEnabled: boolean;

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
    this.isUsingIndexFrequency();
    this.indexFreqEnabled = true;  // todo: just for tests, remove it
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
    if (this.indexFreqEnabled) {
      this.broaderConcepts.forEach(( c: string ) => {
        console.log("foreach");
        console.log(c);
        this.getConceptsFrequency(c);
      }
      console.log(this.broaderConcepts);
      for (var c of this.broaderConcepts) {
        this.getConceptsFrequency(c);
      }
      console.log(this.broaderConcepts);
    } 
  }

  searchNews() {
    this.page = 1;
    this.http.get<SimpleNews[]>(this.baseUrl + 'news/simple/' + this.searchQuery.value).subscribe(result => {
      this.dataSource = new MatTableDataSource<SimpleNews>(result);
      this.dataSource.paginator = this.paginator;
    }, error => console.error(error));
  }


  searchBroaderQuery() {
    this.broaderConcepts = [];
    this.http.get<string[]>(this.baseUrl + 'news/broaderQuery/' + this.searchQuery.value).subscribe(result => {
      this.broaderConcepts = result;
    }, error => console.error(error));
  }

  searchNarrowerQuery() {
    this.narowerConcepts = [];
    this.http.get<string[]>(this.baseUrl + 'news/narrowerQuery/' + this.searchQuery.value).subscribe(result => {
      this.narowerConcepts = result;
    }, error => console.error(error));
  }

  searchRelatedQuery() {
    this.relatedConcepts = [];
    this.http.get<string[]>(this.baseUrl + 'news/relatedQuery/' + this.searchQuery.value).subscribe(result => {
      this.relatedConcepts = result;
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

  isUsingIndexFrequency() {
    this.broaderConcepts = [];
    this.http.get<boolean>(this.baseUrl + 'news/useIndexFreq').subscribe(result => {
      this.indexFreqEnabled = result;
      console.log(result);
    }, error => console.error(error));
  }

//   getConceptsFrequency() {
//     this.http.get<number>(this.baseUrl + 'news/getIndexConceptFrequency/' + 'nature').subscribe(result => {
//         this.broaderConceptsFreq['nature'] = result;
//         console.log(result);
//       }, error => console.error(error));
// }


getConceptsFrequency(c: string) {
    this.http.get<number>(this.baseUrl + 'news/getIndexConceptFrequency/' + c).subscribe(result => {
      this.broaderConceptsFreq[c] = result;
      console.log(c);
      console.log(result);
    }, error => console.error(error));
}

  // getConceptsFrequency() {
  //     this.broaderConcepts.forEach(( c: string ) => {
  //       this.http.get<number>(this.baseUrl + 'news/getIndexConceptFrequency/' + c).subscribe(result => {
  //         this.broaderConceptsFreq[c] = result;
  //         console.log(c);
  //         console.log(result);
  //       }, error => console.error(error));
  //     });
  //   console.log(this.broaderConceptsFreq);
  // }
}
