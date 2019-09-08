<template>
  <div id="page-content">
    <div v-if="commentary">
      <div>
        <div>
          <h2 v-if="!!bookInfo && !!bookInfo.title">{{ bookInfo.title }}</h2>
          <h2 v-if="!bookInfo || !bookInfo.title">{{ commentaryName }}</h2>
          <img
            class="book-cover-image"
            v-if="!!bookInfo && !!bookInfo.imageUrl"
            :src="bookInfo.imageUrl"
          >
          <img
            class="book-cover-image"
            v-else-if="!!bookInfo && !!bookInfo.isbn"
            :src="`/books/${bookInfo.isbn}.jpg`"
          >
          <img
            class="book-cover-image"
            v-else
            src="/img/no-book-cover.png"
          >
          <div>
            <h2>
              Topical Books
            </h2>
            <ul>
              <li
                v-for="book of booksForCommentary"
                v-bind:key="book.id"
              >
                {{ book.name }}
              </li>
            </ul>
          </div>
        </div>
        <div>
          <h2>
            <a
              v-if="!!bookInfo && bookInfo.urls"
              class="link"
              :href="bookInfo.urls[0]"
            >
              Goodreads
            </a>
          </h2>
          <div v-if="!bookInfo || !bookInfo.urls">
            No Goodreads entry for ISBN: {{commentary.isbn}}
          </div>
        </div>
        <div>
          <h2>
            <a
              v-if="!!bookInfo && bookInfo.openLibraryUrl"
              class="link"
              :href="bookInfo.openLibraryUrl"
            >
              OpenLibrary
            </a>
          </h2>
          <div v-if="!bookInfo || !bookInfo.openLibraryUrl">
            No OpenLibrary information for ISBN: {{commentary.isbn}}
          </div>
        </div>
      </div>
      <div v-if="isAdmin">
        <div id="update-commentary">
          <h2>Update Commentary</h2>
          <GoatForm>
            <span>Name:</span>
            <TextInput
              class="inline"
              v-model="commentary.name"
              placeholder="Commentary Name"
            />
            <span>ISBN:</span>
            <TextInput
              class="inline"
              v-model="commentary.isbn"
              placeholder="ISBN - 13 Character"
            />
            <ButtonInput
              text="Update Commentary"
              :handler="updateCommentary"
              :isDisabled="!canUpdateCommentary"
            />
          </GoatForm>
        </div>
        <div id="update-rankings">
          <ul>
            <li
              v-for="(ranking, index) in rankings"
              v-bind:key="index"
            >
              <GoatForm>
                <div>
                  <span>Authority:</span>
                  <SelectInput
                    class="inline"
                    v-model="ranking.authorityId"
                    v-bind:options="authorityOptions"
                  />
                </div>
                <div>
                  <span>Ranking:</span>
                  <NumberInput
                    class="inline"
                    v-model="ranking.rank"
                    placeholder="Ranking: 1 to 5"
                    :min="1"
                    :max="5"
                    :step="0.01"
                  />
                </div>
                <ButtonInput
                  text="Update Ranking"
                  :handler="updateRanking"
                  :parameters="{'oldRanking': rankingsCopy[index], 'updatedRanking': ranking}"
                  :isDisabled="!canUpdateRanking(ranking)"
                />
              </GoatForm>
            </li>
          </ul>
        </div>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
// Vue
import { Component, Vue, Prop } from 'vue-property-decorator';

// Components
import ButtonInput from '@/components/ButtonInput.vue';
import GoatForm from '@/components/GoatForm.vue';
import NumberInput from '@/components/NumberInput.vue';
import ResourceLink from '@/components/ResourceLink.vue';
import SelectInput from '@/components/SelectInput.vue';
import TextInput from '@/components/TextInput.vue';

// Models
import AuthorityModel from '../models/Authority';
import BookModel from '../models/Book';
import BookInfoModel from '../models/BookInfo';
import CommentaryModel from '../models/Commentary';
import OptionModel from '../models/Option';
import RawRankingModel from '../models/RawRanking';

// Services
import AuthService from '../services/AuthService';
import BookInfoService from '../services/BookInfoService';
import HttpService, { HttpResponse, HttpError } from '../services/HttpService';

@Component({
  components: {
    ButtonInput,
    GoatForm,
    NumberInput,
    ResourceLink,
    SelectInput,
    TextInput,
  },
})
export default class Commentary extends Vue {
  private bookInfo: BookInfoModel | null | undefined = null;
  private bookOptions: OptionModel[] = [];
  private booksForCommentary: BookModel[] = [];
  private commentary: CommentaryModel | null = null;
  private commentaryName: string | null | undefined = null;
  private rankings: RawRankingModel[] = [];
  private rankingsCopy: RawRankingModel[] = [];

  private authorityOptions: OptionModel[] = [];

  private isLoadingAuthorities: boolean = true;
  private isLoadingBooks: boolean = false;

  public get canUpdateCommentary(): boolean {
    return (
      !!this.commentary &&
      !!this.commentary.name &&
      !!this.commentary.isbn &&
      this.commentary.isbn.length === 13
    );
  }

  public get isAdmin(): boolean {
    return (
      !!AuthService && !!AuthService.userInfo && AuthService.userInfo.isAdmin
    );
  }

  public canUpdateRanking(ranking: RawRankingModel | null): boolean {
    return (
      !!ranking &&
      !!ranking.authorityId &&
      !!ranking.bookId &&
      !!ranking.commentaryId
    );
  }

  private constructGoodreadsUrls(ids: string[]) {
    const urls: string[] = ids.map((id: string) => {
      return `https://www.goodreads.com/book/show/${id}`;
    });
    return urls;
  }

  private getAuthorities(): void {
    this.isLoadingAuthorities = true;
    HttpService.get('api/authorities')
      .then((response: HttpResponse<AuthorityModel[]>) => {
        const authorities = response.data;
        this.authorityOptions = [];
        if (authorities && authorities.length > 0) {
          for (const authority of authorities) {
            this.authorityOptions.push(
              new OptionModel(authority.id, authority.name),
            );
          }
        }
      })
      .catch((error: HttpError) => {
        this.authorityOptions = [];
        console.error(error);
      })
      .finally(() => {
        this.isLoadingAuthorities = false;
      });
  }

  private getBookInfo(): void {
    if (this.commentary) {
      BookInfoService.getBookInfo(this.commentary.isbn)
        .then((bookInfo: BookInfoModel | null | undefined) => {
          this.bookInfo = bookInfo;
          if (!!this.bookInfo && !!this.bookInfo.title) {
            this.commentaryName = this.bookInfo.title;
          } else {
            this.bookInfo = new BookInfoModel(
              '',
              '',
              !!this.commentaryName ? this.commentaryName : '',
              [],
              '',
              !!this.commentary ? this.commentary.isbn : '',
            );
          }
        })
        .catch((error: HttpError) => {
          console.error('Could not get commentary info', error);
        });
    }
  }

  private getBooksForCommentary(): void {
    HttpService.get(`api/commentaryBooks/${this.$route.params.id}`)
      .then((response: HttpResponse<BookModel[]>) => {
        this.booksForCommentary = response.data;
      })
      .catch((error: HttpError) => {
        console.error('No books for commentary');
      });
    this.getBookInfo();
  }

  private getCommentary(): void {
    HttpService.get(`api/commentaries/${this.$route.params.id}`)
      .then((response: HttpResponse<CommentaryModel>) => {
        this.commentary = response.data;
        this.commentaryName = this.commentary.name;
        this.getBooksForCommentary();
        this.getRankingsForCommentary();
      })
      .catch((error: HttpError) => {
        console.error('No commentary grabbed');
      });
  }

  private getRankingsForCommentary(): void {
    const params = new Map<string, string>();
    if (!!this.commentary && !!this.commentary.id) {
      params.set('commentaryId', this.commentary.id.toString());
    }
    HttpService.get('api/rankings/query', true, params)
      .then((response: HttpResponse<RawRankingModel[]>) => {
        this.rankings = response.data;
        this.rankingsCopy = JSON.parse(JSON.stringify(this.rankings));
      })
      .catch((error: HttpError) => {
        this.rankings = [];
        this.rankingsCopy = [];
        console.error(error);
      });
  }

  private updateCommentary(): void {
    HttpService.put(
      `api/commentaries?/${
        this.commentary != null ? this.commentary.id : null
      }`,
      this.commentary,
    )
      .then((response: HttpResponse<CommentaryModel>) => {
        if (!!this.commentary) {
          this.commentaryName = this.commentary.name;
        }
        this.getBooksForCommentary();
      })
      .catch((error: HttpError) => {
        console.error(error);
      });
  }

  private updateRanking(rankingModels: any): void {
    const oldRankingKey = 'oldRanking';
    const updatedRankingKey = 'updatedRanking';
    const oldRanking: RawRankingModel = rankingModels[
      oldRankingKey
    ] as RawRankingModel;
    const updatedRanking: RawRankingModel = rankingModels[
      updatedRankingKey
    ] as RawRankingModel;
    if (!!updatedRanking && !!updatedRanking.rank) {
      updatedRanking.rank = Number.parseFloat(updatedRanking.rank.toString());
    }
    HttpService.put('api/rankings', updatedRanking)
      .then((response: HttpResponse<RawRankingModel>) => {
        console.log('Successfully updated ranking');
      })
      .catch((error: HttpError) => {
        console.error(error);
      });
  }

  private mounted() {
    this.getAuthorities();
    this.getCommentary();
  }
}
</script>

<style scoped lang="scss">
@import '@/styles/page-content';
@import '@/styles/utility';

.book-cover-image {
  width: 10rem;
  height: auto;
}
</style>
