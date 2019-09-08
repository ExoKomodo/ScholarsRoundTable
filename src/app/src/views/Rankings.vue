<template>
  <div id="page-content">
    <h2>Rankings</h2>
    <div v-if="!isLoading">
      <GoatForm>
        <div>
          <span>Authority:</span>
          <SelectInput
            class="inline"
            v-model="selectedAuthorityId"
            v-bind:options="authorityOptions"
          />
        </div>
        <div>
          <span>Commentary Book:</span>
          <SelectInput
            class="inline"
            v-model="selectedCommentaryBookId"
            v-bind:options="commentaryBookOptions"
          />
        </div>
        <div>
          <span>Ranking:</span>
          <NumberInput
            class="inline"
            v-model="ranking"
            placeholder="Ranking: 1 to 5"
            :min="1"
            :max="5"
            :step="0.01"
          />
        </div>
        <ButtonInput
          text="Add Ranking"
          :handler="addRanking"
          :isDisabled="!canAddRanking"
        />
      </GoatForm>
      <span class="error-text">{{ errorText }}</span>
      <span class="success-text">{{ successText }}</span>
    </div>
    <div v-if="isLoading">
      <Spinner />
    </div>
  </div>
</template>

<script lang="ts">
// Vue
import { Component, Vue } from 'vue-property-decorator';

// Components
import ButtonInput from '@/components/ButtonInput.vue';
import GoatForm from '@/components/GoatForm.vue';
import NumberInput from '@/components/NumberInput.vue';
import Spinner from '@/components/Spinner.vue';
import SelectInput from '@/components/SelectInput.vue';
import TextInput from '@/components/TextInput.vue';

// Models
import AuthorityModel from '../models/Authority';
import CommentaryBookModel from '../models/CommentaryBook';
import OptionModel from '../models/Option';

// Services
import HttpService, { HttpResponse, HttpError } from '../services/HttpService';
import BookModel from '../models/Book';
import CommentaryModel from '../models/Commentary';

@Component({
  components: {
    ButtonInput,
    GoatForm,
    NumberInput,
    Spinner,
    SelectInput,
    TextInput,
  },
})
export default class Rankings extends Vue {
  public isLoadingAuthorities: boolean = true;
  public isLoadingCommentaryBooks: boolean = true;

  private authorityOptions: OptionModel[] = [];
  private commentaryBookOptions: OptionModel[] = [];

  private ranking: number = 1;
  private selectedAuthorityId: number = -1;
  private selectedCommentaryBookId: string = '';

  private errorText: string = '';
  private successText: string = '';

  public get canAddRanking(): boolean {
    return !!this.selectedAuthorityId && !!this.selectedCommentaryBookId;
  }

  public get isLoading(): boolean {
    return this.isLoadingAuthorities || this.isLoadingCommentaryBooks;
  }

  private addRanking() {
    if (this.canAddRanking) {
      const splitId = this.selectedCommentaryBookId.split(',');
      const newRanking: object = {
        authorityId: this.selectedAuthorityId,
        commentaryId: Number.parseInt(splitId[0], 10),
        bookId: Number.parseInt(splitId[1], 10),
        rank: Number.parseFloat(this.ranking.toString()),
      };
      HttpService.post('api/rankings', newRanking)
        .then(() => {
          this.errorText = '';
          this.successText = 'Successfully created ranking';
        })
        .catch((error: HttpError) => {
          console.error(error);
          this.successText = '';
          this.errorText =
            'Something went wrong creating this ranking. This can happen with a duplicate entry.';
        });
    }
  }

  private getAuthorities(): void {
    this.isLoadingAuthorities = true;
    HttpService.get('api/authorities')
      .then((response: HttpResponse<AuthorityModel[]>) => {
        const authorities = response.data;
        this.authorityOptions = [];
        if (authorities && authorities.length > 0) {
          this.selectedAuthorityId = authorities[0].id;
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

  private getCommentaryBooks(): void {
    this.isLoadingCommentaryBooks = true;
    HttpService.get('api/commentaryBooks')
      .then((response: HttpResponse<CommentaryBookModel[]>) => {
        const commentaryBooks = response.data;
        this.commentaryBookOptions = [];
        if (commentaryBooks && commentaryBooks.length > 0) {
          this.selectedCommentaryBookId = `${commentaryBooks[0].commentaryId},${commentaryBooks[0].bookId}`;

          HttpService.get(`api/books`)
            .then((bookResponse: HttpResponse<BookModel[]>) => {
              const books: Map<number, BookModel> = new Map<
                number,
                BookModel
              >();
              for (const book of bookResponse.data) {
                books.set(book.id, book);
              }

              const commentaryIds = commentaryBooks.map((commentaryBook) => {
                return commentaryBook.commentaryId;
              });
              HttpService.post(`api/commentaries/many`, commentaryIds)
                .then((commentaryResponse: HttpResponse<CommentaryModel[]>) => {
                  const commentaries = commentaryResponse.data;

                  for (const commentary of commentaries) {
                    for (const commentaryBook of commentaryBooks) {
                      if (commentaryBook.commentaryId === commentary.id) {
                        const book = books.get(commentaryBook.bookId);
                        this.commentaryBookOptions.push(
                          new OptionModel(
                            `${commentary.id},${commentaryBook.bookId}`,
                            `${commentary.name} - ${
                              !!book ? book.name : 'error'
                            }`,
                          ),
                        );
                      }
                    }
                  }
                })
                .catch((error: HttpError) => {
                  console.error(error);
                });
            })
            .catch((error: HttpError) => {
              console.error(error);
            });
        }
      })
      .catch((error: HttpError) => {
        this.commentaryBookOptions = [];
        console.error(error);
      })
      .finally(() => {
        this.isLoadingCommentaryBooks = false;
      });
  }

  private mounted() {
    this.getAuthorities();
    this.getCommentaryBooks();
  }
}
</script>

<style scoped lang="scss">
@import '@/styles/page-content';
@import '@/styles/utility';

li {
  list-style-type: none;
}

.error-text {
  color: red;
}

.success-text {
  color: green;
}
</style>
