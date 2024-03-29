import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { MatButtonModule, MatInputModule, MatTableModule, MatChipsModule, MatCardModule, MatPaginatorModule, MatSelectModule, MatIconModule, MatDialogModule, MatBadgeModule, MatListModule, MatGridListModule } from '@angular/material';
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
import { EmailSubDialogComponent } from './email-sub-dialog/email-sub-dialog.component';
import { UnsubscribeComponent } from './unsubscribe/unsubscribe.component';
import { router } from './router';

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
    EmailSubDialogComponent,
    UnsubscribeComponent,
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
    MatBadgeModule,
    MatPaginatorModule,
    MatSelectModule,
    MatIconModule,
    MatDialogModule,
    MatGridListModule,
    MatListModule,
    BrowserAnimationsModule,
    RouterModule.forRoot(router)
  ],
  providers: [],
  entryComponents: [
    EmailSubDialogComponent,
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
