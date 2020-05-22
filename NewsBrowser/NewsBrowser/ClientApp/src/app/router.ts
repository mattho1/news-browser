import { HomeComponent } from "./home/home.component";
import { SimpleSearchComponent } from "./simple-search/simple-search.component";
import { AdvancedSearchComponent } from "./advanced-search/advanced-search.component";
import { CombinationSearchComponent } from "./combination-search/combination-search.component";
import { FetchDataComponent } from "./fetch-data/fetch-data.component";
import { NewsDetailsComponent } from "./news-details/news-details.component";
import { UnsubscribeComponent } from "./unsubscribe/unsubscribe.component";

export const router = [
  { path: '', component: SimpleSearchComponent, pathMatch: 'full' },
  { path: 'combination-search', component: CombinationSearchComponent },
  { path: 'combination-search/:fieldType/:searchQuery', component: CombinationSearchComponent },
  { path: 'fetch-data', component: FetchDataComponent },
  { path: 'simple-search', component: SimpleSearchComponent },
  { path: 'simple-search/:searchQuery', component: SimpleSearchComponent },
  { path: 'news-details/:newsId', component: NewsDetailsComponent },
  { path: 'unsubscribe/:unsubscribeId', component: UnsubscribeComponent },
]
