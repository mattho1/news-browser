import { Component, Inject, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { FormControl, Validators } from '@angular/forms';
import { HttpClient, HttpParams } from '@angular/common/http';
import { SimpleNews } from '../models/SimpleNews';
import { ActivatedRoute } from '@angular/router';
import { DomSanitizer } from '@angular/platform-browser';
import { MatIconRegistry } from '@angular/material/icon';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { SimpleSearchComponent } from '../simple-search/simple-search.component';

export interface EmailDialogData {
  searchQr: string
}

@Component({
  selector: 'app-email-sub-dialog',
  templateUrl: './email-sub-dialog.component.html',
  styleUrls: ['./email-sub-dialog.component.css']
})
export class EmailSubDialogComponent implements OnInit {

  email = new FormControl('', [Validators.required, Validators.email]);

  ngOnInit(): void { }

  constructor(public dialogRef: MatDialogRef<SimpleSearchComponent>,
    private http: HttpClient,
    @Inject(MAT_DIALOG_DATA) private data: string,
    @Inject('BASE_URL') private baseUrl: string) {
  }

  cancelDialog(): void {
    this.dialogRef.close();
  }

  subscribeQuery() {
    let params = new HttpParams();
    params = params.append('email', this.email.value);
    params = params.append('subscribeQuery', this.data);

    this.http.get<SimpleNews[]>(this.baseUrl + 'subscriber/subscribe', { params: params }).subscribe(_ => {
      this.dialogRef.close();
    }, error => console.error(error));
  }

  getErrorMessage() {
    return this.email.hasError('required') ? 'You must enter a value' :
      this.email.hasError('email') ? 'Not a valid email' :
        '';
  }
}
