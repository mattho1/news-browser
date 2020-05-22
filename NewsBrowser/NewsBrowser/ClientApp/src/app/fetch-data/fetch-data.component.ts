import { Component, Inject, OnInit } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { FormControl, Validators, FormGroup, FormBuilder } from '@angular/forms';
import { Router } from '@angular/router';
import { NewNews } from '../models/newNews';

@Component({
  selector: 'app-fetch-data',
  templateUrl: './fetch-data.component.html',
  styleUrls: ['./fetch-data.component.css']
})
export class FetchDataComponent implements OnInit {
  form: FormGroup;
  textRequiredValue: String = 'You must enter a value';
  textTooLongText: String = 'Text is too long';

  constructor(private http: HttpClient,
    private fb: FormBuilder,
    private router: Router,
    @Inject('BASE_URL') private baseUrl: string) {
  }

  ngOnInit() {
    this.form = this.fb.group({
      title: new FormControl('', [Validators.required, Validators.maxLength(256)]),
      author: new FormControl('', [Validators.required, Validators.maxLength(256)]),
      site: new FormControl('', [Validators.required, Validators.maxLength(256)]),
      language: new FormControl('', [Validators.required, Validators.maxLength(256)]),
      imgUrl: new FormControl(''),
      contents: new FormControl('', [Validators.required])
    })
  }

  addNews() {
  //  let params = new HttpParams();
  //  params = params.append('title', this.form.get('title').value,);
  //  params = params.append('author', this.form.get('author').value,);
  //  params = params.append('site', this.form.get('site').value,);
  //  params = params.append('language', this.form.get('language').value);
  //  params = params.append('imgUrl', this.form.get('imgUrl').value);
  //  params = params.append('contents', this.form.get('contents').value);
    const newNews = {
      title: this.form.get('title').value,
      author: this.form.get('author').value,
      site: this.form.get('site').value,
      language: this.form.get('language').value,
      imgUrl: this.form.get('imgUrl').value,
      contents: this.form.get('contents').value
    } as NewNews;

    let formData = this.toFormData(newNews);
    this.http.post<NewNews>(this.baseUrl + 'news/addedNews', newNews).subscribe(result => {
      this.router.navigate(['news-details/' + result.id]);
    }, error => console.error(error));
  }

  getErrorMessageTitle() {
    return this.form.get('title').hasError('required') ? this.textRequiredValue :
      this.form.get('title').hasError('maxlength') ? this.textTooLongText :
        '';
  }

  getErrorMessageAuthor() {
    return this.form.get('author').hasError('required') ? this.textRequiredValue :
      this.form.get('author').hasError('maxlength') ? this.textTooLongText :
        '';
  }

  getErrorMessageSite() {
    return this.form.get('site').hasError('required') ? this.textRequiredValue :
      this.form.get('site').hasError('maxlength') ? this.textTooLongText :
        '';
  }

  getErrorMessageLanguage() {
    return this.form.get('language').hasError('required') ? this.textRequiredValue :
      this.form.get('language').hasError('maxlength') ? this.textTooLongText :
        '';
  }

  getErrorMessageContents() {
    return this.form.get('contents').hasError('required') ? this.textRequiredValue : '';
  }

  private toFormData<T>(formValue: T) {
    const formData = new FormData();
    for (const key of Object.keys(formValue)) {
      const value = formValue[key];
      if (typeof value === 'object' && value !== null) {
        for (const innerKey of Object.keys(value)) {
          const innerValue = value[innerKey];
          formData.append(innerKey, innerValue);
        }
      }
      else
        formData.append(key, value);
    }
    return formData;
  }
}
