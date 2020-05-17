import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { MatButtonModule, MatInputModule, MatTableModule, MatChipsModule, MatCardModule, MatPaginatorModule, MatSelectModule, MatIconModule } from '@angular/material';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { CounterComponent } from './counter/counter.component';
import { FetchDataComponent } from './fetch-data/fetch-data.component';
import { SimpleSearchComponent } from './simple-search/simple-search.component';
import { NewsDetailsComponent } from './news-details/news-details.component';
import { AdvancedSearchComponent } from './advanced-search/advanced-search.component';
import { CombinationSearchComponent } from './combination-search/combination-search.component';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    CounterComponent,
    FetchDataComponent,
    SimpleSearchComponent,
    NewsDetailsComponent,
    AdvancedSearchComponent,
    CombinationSearchComponent,
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    MatButtonModule,
    MatInputModule,
    MatTableModule,
    MatChipsModule,
    MatCardModule,
    MatPaginatorModule,
    MatSelectModule,
    MatIconModule,
    BrowserAnimationsModule,
    RouterModule.forRoot([
      { path: '', component: SimpleSearchComponent, pathMatch: 'full' },
      { path: 'advanced-search', component: AdvancedSearchComponent }, 
      { path: 'advanced-search/:fieldType/:searchQuery', component: AdvancedSearchComponent },
      { path: 'combination-search', component: CombinationSearchComponent },
      { path: 'combination-search/:fieldType/:searchQuery', component: CombinationSearchComponent },
      { path: 'counter', component: CounterComponent },
      { path: 'fetch-data', component: FetchDataComponent },
      { path: 'simple-search', component: SimpleSearchComponent },
      { path: 'simple-search/:searchQuery', component: SimpleSearchComponent },
      { path: 'news-details/:newsId', component: NewsDetailsComponent },
    ])
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
