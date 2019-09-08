<template>
  <div>
    <div id="header">
      <div id="header-content">
        <div id="header-title">Scholars Roundtable</div>
        <div id="header-tag">Biblical commentaries ranked by premier theologians</div>
      </div>
    </div>
    <div id="page-content">
      <div
        id="loaded-content"
        v-if="!isLoading"
      >
        <GoatForm class="center-form">
          <div class="inline input-container">
            <span>I want the best commentaries for</span>
            <SelectInput
              class="inline"
              v-model="selectedBookId"
              v-bind:options="bookOptions"
            />
          </div>
          <div class="inline input-container">
            <span>from</span>
            <SelectInput
              class="inline"
              v-model="selectedSeminaryId"
              v-bind:options="seminaryOptions"
            />
          </div>
          <div class="inline input-container">
            <ButtonInput
              id="search-button"
              class="inline"
              text="Search"
              :handler="getRankings"
              :isDisabled="!canGetRankings"
            />
          </div>
        </GoatForm>
        <div
          id="rankings"
          v-if="!isLoadingRankings && rankings.length > 0"
        >
          <div id="top-rankings">
            <h2 class="centered-header">Our top five!</h2>
            <ol id="top-rankings-list">
              <li
                v-for="ranking in topRankings"
                v-bind:key="ranking.commentaryId"
              >
                <ResourceLink
                  v-if="ranking.bookInfo"
                  :link="generateRankingLink(ranking)"
                >
                  <div class="book-info">
                    <div class="book-info-cover">
                      <img
                        v-if="ranking.bookInfo.imageUrl"
                        :src="ranking.bookInfo.imageUrl"
                      >
                      <img
                        v-if="!ranking.bookInfo.imageUrl"
                        :src="`/books/${ranking.bookInfo.isbn}.jpg`"
                      >
                    </div>
                    <div class="book-info-grid">
                      <div class="book-info-header">Title:</div>
                      <div class="book-info-data book-info-title">{{ranking.bookInfo.title}}</div>
                      <div class="book-info-header">Author:</div>
                      <div class="book-info-data">{{ranking.bookInfo.author}}</div>
                      <div
                        v-if="isAdmin"
                        class="book-info-header"
                      >Score:</div>
                      <div
                        v-if="isAdmin"
                        class="book-info-data"
                      >{{ranking.rank}}/5</div>
                    </div>
                  </div>
                </ResourceLink>
              </li>
            </ol>
          </div>
          <div
            id="bottom-rankings"
            v-if="bottomRankings"
          >
            <h3 class="centered-header">If you want to investigate further...</h3>
            <ol id="bottom-rankings-list">
              <li
                v-for="ranking in bottomRankings"
                v-bind:key="ranking.commentaryId"
              >
                <ResourceLink
                  v-if="ranking.bookInfo"
                  :link="generateRankingLink(ranking)"
                >
                  <div class="book-info">
                    <div class="book-info-cover">
                      <img
                        v-if="ranking.bookInfo.imageUrl"
                        :src="ranking.bookInfo.imageUrl"
                      >
                      <img
                        v-if="!ranking.bookInfo.imageUrl"
                        :src="`/books/${ranking.bookInfo.isbn}.jpg`"
                      >
                    </div>
                    <div class="book-info-grid">
                      <div class="book-info-header">Title:</div>
                      <div class="book-info-data book-info-title">{{ranking.bookInfo.title}}</div>
                      <div class="book-info-header">Author:</div>
                      <div class="book-info-data">{{ranking.bookInfo.author}}</div>
                      <div
                        v-if="isAdmin"
                        class="book-info-header"
                      >Score:</div>
                      <div
                        v-if="isAdmin"
                        class="book-info-data"
                      >{{ranking.rank}}/5</div>
                    </div>
                  </div>
                </ResourceLink>
              </li>
            </ol>
          </div>
        </div>
      </div>
      <div v-if="isLoading">
        <Spinner />
      </div>
      <div
        v-if="isLoadingRankings"
        id="rankings-spinner"
      >
        <Spinner :label="loadingMessage" />
      </div>
    </div>
  </div>
</template>

<script lang="ts">
// Vue
import { Component, Vue } from 'vue-property-decorator';

// Components
import ButtonInput from '@/components/ButtonInput.vue';
import GoatForm from '@/components/GoatForm.vue';
import ResourceLink from '@/components/ResourceLink.vue';
import SelectInput from '@/components/SelectInput.vue';
import Spinner from '@/components/Spinner.vue';

// Models
import AuthorityModel from '../models/Authority';
import BookModel from '../models/Book';
import CommentaryBookModel from '../models/CommentaryBook';
import LinkModel from '../models/Link';
import OptionModel from '../models/Option';
import RankingModel from '../models/Ranking';
import SeminaryModel from '../models/Seminary';

// Services
import AuthService from '../services/AuthService';
import BookInfoService from '../services/BookInfoService';
import HttpService, { HttpResponse, HttpError } from '../services/HttpService';
import BookInfoModel from '../models/BookInfo';

@Component({
  components: {
    ButtonInput,
    GoatForm,
    ResourceLink,
    SelectInput,
    Spinner,
  },
})
export default class Home extends Vue {
  private authorityOptions: OptionModel[] = [];
  private bookOptions: OptionModel[] = [];
  private rankings: RankingModel[] = [];
  private seminaryOptions: OptionModel[] = [];

  private selectedAuthorityId: number = -1;
  private selectedBookId: number = -1;
  private selectedCommentaryBookId: string = '';
  private selectedSeminaryId: number = -1;

  private isLoadingAuthorities: boolean = false;
  private isLoadingBooks: boolean = false;
  private isLoadingRankings: boolean = false;
  private isLoadingSeminaries: boolean = false;

  private loadingMessage: string = '';
  private loadingMessages: string[] = [
    'Finding the best commentaries...',
    'Scouring the codexes...',
    'Asking our pastor...',
    'Collecting recommendations...',
  ];

  public get canCreateRanking(): boolean {
    return true;
  }

  public get canGetRankings(): boolean {
    return !!this.selectedBookId && !!this.selectedSeminaryId;
  }

  public get bottomRankings(): RankingModel[] {
    let rankings: RankingModel[] = [];
    if (this.rankings.length > 5) {
      rankings = this.rankings.slice(5);
    }
    return rankings;
  }

  public get topRankings(): RankingModel[] {
    return this.rankings.slice(0, 5);
  }

  public get isAdmin(): boolean {
    return (
      !!AuthService && !!AuthService.userInfo && AuthService.userInfo.isAdmin
    );
  }

  public get isLoading(): boolean {
    return (
      this.isLoadingAuthorities ||
      this.isLoadingBooks ||
      this.isLoadingSeminaries
    );
  }

  private generateRankingLink(ranking: RankingModel): LinkModel {
    return new LinkModel(`/commentaries/${ranking.commentaryId}`);
  }

  private getBooks(): void {
    HttpService.get('api/books')
      .then((response: HttpResponse<BookModel[]>) => {
        let books = response.data;
        books = books.sort((a, b) => {
          return a.id - b.id;
        });
        this.bookOptions = [];
        if (books && books.length > 0) {
          this.selectedBookId = books[0].id;
          for (const book of books) {
            this.bookOptions.push(new OptionModel(book.id, book.name));
          }
        }
      })
      .catch((error: HttpError) => {
        this.bookOptions = [];
        console.error(error);
      })
      .finally(() => {
        this.isLoadingBooks = false;
      });
  }

  private getRankings(): void {
    if (!this.isLoadingRankings) {
      this.isLoadingRankings = true;
      this.setLoadingMessage();
      const params: Map<string, string> = new Map();
      params.set('bookId', this.selectedBookId.toString());
      params.set('seminaryId', this.selectedSeminaryId.toString());
      HttpService.get('api/rankings', true, params)
        .then((response: HttpResponse<RankingModel[]>) => {
          const rankings = response.data;
          this.rankings = [];
          for (const ranking of rankings) {
            BookInfoService.getBookInfo(ranking.commentaryIsbn)
              .then((bookInfo: BookInfoModel | null | undefined) => {
                if (!bookInfo) {
                  const splitName = ranking.commentaryName.split('-');
                  const author = splitName[splitName.length - 1].trim();
                  bookInfo = new BookInfoModel(
                    '',
                    '',
                    ranking.commentaryName,
                    [],
                    author,
                    ranking.commentaryIsbn,
                  );
                }
                ranking.bookInfo = bookInfo;
                this.rankings.push(ranking);
                this.sortRankings();
              })
              .catch((error: HttpError) => {
                const splitName = ranking.commentaryName.split('-');
                const author = splitName[splitName.length - 1].trim();
                ranking.bookInfo = new BookInfoModel(
                  '',
                  '',
                  ranking.commentaryName,
                  [],
                  author,
                  ranking.commentaryIsbn,
                );
                this.rankings.push(ranking);
                this.sortRankings();
              });
          }
        })
        .catch((error: HttpError) => {
          this.rankings = [];
          console.error(error);
        })
        .finally(() => {
          this.isLoadingRankings = false;
        });
    }
  }

  private getSeminaries(): void {
    this.isLoadingSeminaries = true;
    HttpService.get('api/seminaries')
      .then((response: HttpResponse<SeminaryModel[]>) => {
        const seminaries = response.data;
        this.seminaryOptions = [];
        if (seminaries && seminaries.length > 0) {
          this.selectedSeminaryId = seminaries[0].id;
          for (const seminary of seminaries) {
            if (!AuthService.userInfo || !AuthService.userInfo.isAdmin) {
              if (!seminary.name.includes('Mock')) {
                this.seminaryOptions.push(
                  new OptionModel(seminary.id, seminary.name),
                );
              }
            } else {
              this.seminaryOptions.push(
                new OptionModel(seminary.id, seminary.name),
              );
            }
          }
        }
      })
      .catch((error: HttpError) => {
        this.seminaryOptions = [];
        console.error(error);
      })
      .finally(() => {
        this.isLoadingSeminaries = false;
      });
  }

  private setLoadingMessage(): void {
    this.loadingMessage = this.loadingMessages[
      Math.floor(Math.random() * this.loadingMessages.length)
    ];
  }

  private sortRankings(): void {
    this.rankings.sort((x: RankingModel, y: RankingModel) => {
      return y.rank - x.rank;
    });
  }

  private mounted() {
    this.getBooks();
    this.getSeminaries();
  }
}
</script>

<style scoped lang="scss">
@import '@/styles/page-content';
@import '@/styles/utility';
@import '@/styles/variables';

$header-height: 20rem;
$small-header-height: 10rem;
$large-header-height: 30rem;
$image-tint: rgba(0, 0, 0, 0.45);

#header {
  text-align: center;
  height: $header-height;
  width: 100vw;
  background: linear-gradient($image-tint, $image-tint),
    url('../assets/library-small.png');
  background-position: 0% 15%;
}

#header-content {
  color: $primary-light;
  font-weight: 500;
  font-size: 2rem;
  margin-top: auto;
  margin-bottom: auto;
  line-height: $header-height / 2;
  vertical-align: middle;
}

#header-tag {
  color: $primary-light;
  font-size: 2rem;
}

#header-title {
  color: $primary-light;
  font-weight: 900;
  font-size: 4rem;
}

@media screen and (max-width: 900px) {
  #header {
    height: $small-header-height;
    background: linear-gradient($image-tint, $image-tint),
      url('../assets/library-small.png');
  }

  #header-content {
    line-height: $small-header-height / 2;
  }

  #header-tag {
    font-size: 1rem;
    line-height: 1rem;
  }

  #header-title {
    font-size: 2rem;
  }
}

@media screen and (min-width: 2000px) {
  #header {
    background: linear-gradient($image-tint, $image-tint),
      url('../assets/library-medium.png');
  }
}

@media screen and (min-width: 3000px) {
  #header {
    height: $large-header-height;
    background: linear-gradient($image-tint, $image-tint),
      url('../assets/library-large.png');
  }

  #header-content {
    line-height: $large-header-height / 2;
  }
}

// Search
li {
  max-width: 40rem;
  margin: auto;
}

ol {
  padding: 0;
  list-style-type: none;
}

#rankings-spinner {
  margin-top: 15px;
}

.book-info {
  display: grid;
  grid-column-gap: 1rem;
  grid-template-columns: 10rem auto;
}

.book-info-cover {
  img {
    width: 100%;
    height: auto;
  }
}

.book-info-data {
  max-width: 30rem;
}

.book-info-grid {
  display: grid;
  grid-template-columns: 5rem auto;
}

.book-info-header {
  text-decoration: underline;
}

.book-info-title {
  font-style: italic;
}

.centered-header {
  text-align: center;
}

.center-form {
  width: max-content;
  max-width: 100%;
  margin: auto;
}

@media screen and (max-width: 400px) {
  #rankings {
    max-width: 90vw;
  }

  .book-info {
    grid-template-columns: 5rem auto;
  }
}

@media screen and (max-width: 900px) {
  .center-form {
    width: max-content;
    line-height: 2rem;
    text-align: end;
    margin-left: auto;
    margin-right: 0;
  }

  .input-container {
    display: block;
    span {
      font-size: 0.9rem;
    }
  }

  #search-button {
    margin-left: 0;
  }
}
</style>
