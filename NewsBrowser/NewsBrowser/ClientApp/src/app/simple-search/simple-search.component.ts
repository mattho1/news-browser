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
  broaderConceptsFreq = {};
  narrowerConceptsFreq: { [id: string] : number; } = {};
  relatedConceptsFreq: { [id: string] : number; } = {};
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
    //iconRegistry.addSvgIcon(
    //  'subscriptions',
    //  sanitizer.bypassSecurityTrustResourceUrl('assets/icons/subscriptions.svg'));
  }

  ngOnInit() {
    this.isUsingIndexFrequency();
    this.dataSource.paginator = this.paginator;
    const sq = this.route.snapshot.paramMap.get('searchQuery');
    if (sq != null) {
      this.searchQuery.setValue(sq);
      // this.searchNews();
      this.submitSearch();
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
  }

  searchNews() {
    this.page = 1;
    this.http.get<SimpleNews[]>(this.baseUrl + 'news/simple/' + this.searchQuery.value).subscribe(result => {
      this.dataSource = new MatTableDataSource<SimpleNews>(result);
      this.dataSource.paginator = this.paginator;
    }, error => console.error(error));
  }

  // used to force fetching data for new query
  newSearch(event, query: string)
  {
    this.searchQuery.setValue(query);
    this.submitSearch();
  }

  searchBroaderQuery() {
    this.broaderConcepts = [];
    this.broaderConceptsFreq = {};
    this.http.get<string[]>(this.baseUrl + 'news/broaderQuery/' + this.searchQuery.value).subscribe(result => {
      this.broaderConcepts = result;
      if (this.indexFreqEnabled) {        
        let loop = (concept: string) => {
          this.getConceptsFrequency(concept)
          .subscribe(result => {
            if (this.broaderConcepts.length) {
              this.broaderConceptsFreq[concept] = result;
              loop(this.broaderConcepts.shift());
            }
          });
        }
        loop(this.broaderConcepts.shift());
      } 
    }, error => console.error(error));
  }

  searchNarrowerQuery() {
    this.narowerConcepts = [];
    this.narrowerConceptsFreq = {};
    this.http.get<string[]>(this.baseUrl + 'news/narrowerQuery/' + this.searchQuery.value).subscribe(result => {
      this.narowerConcepts = result;
      if (this.indexFreqEnabled) {        
        let loop = (concept: string) => {
          this.getConceptsFrequency(concept)
          .subscribe(result => {
            if (this.narowerConcepts.length) {
              this.narrowerConceptsFreq[concept] = result;
              loop(this.narowerConcepts.shift());
            }
          });
        }
        loop(this.narowerConcepts.shift());
      } 
    }, error => console.error(error));
  }

  searchRelatedQuery() {
    this.relatedConcepts = [];
    this.relatedConceptsFreq = {};
    this.http.get<string[]>(this.baseUrl + 'news/relatedQuery/' + this.searchQuery.value).subscribe(result => {
      this.relatedConcepts = result;
      if (this.indexFreqEnabled) {        
        let loop = (concept: string) => {
          this.getConceptsFrequency(concept)
          .subscribe(result => {
            if (this.relatedConcepts.length) {
              this.relatedConceptsFreq[concept] = result;
              loop(this.relatedConcepts.shift());
            }
          });
        }
        loop(this.relatedConcepts.shift());
      } 
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
    this.http.get<boolean>(this.baseUrl + 'news/useIndexFreq').subscribe(result => {
      this.indexFreqEnabled = result;
      console.log(result);
    }, error => console.error(error));
  }


  getConceptsFrequency(c: string) {
      return this.http.get<number>(this.baseUrl + 'news/getIndexConceptFrequency/' + c);
  }

  shortenBadgeNumber(n: number)
  {
    if (n > 999) {
      return Math.floor(n / 1000) + "k";
    }
    return n;
  }

}
