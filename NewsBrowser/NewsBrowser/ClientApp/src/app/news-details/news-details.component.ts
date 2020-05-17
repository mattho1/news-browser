import { Component, OnInit, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { NewsDetails, EMPTY_NEWS_DETAILS } from '../models/newsDetails';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-news-details',
  templateUrl: './news-details.component.html',
  styleUrls: ['./news-details.component.css']
})
export class NewsDetailsComponent implements OnInit {

  news: NewsDetails;
  loading: boolean;

  constructor(private route: ActivatedRoute, private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {
    this.news = EMPTY_NEWS_DETAILS;
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

  ngOnInit() {
    const id = this.route.snapshot.paramMap.get('newsId');
    this.http.get<NewsDetails>(this.baseUrl + "news/" + id).subscribe(result => {
      this.news = result;
      this.news.imageUrl = (result.imageUrl == null || result.imageUrl == "") ? "assets/defaultPhoto/defaultNews.png" : result.imageUrl ;
    }, error => console.error(error));
  }
}
