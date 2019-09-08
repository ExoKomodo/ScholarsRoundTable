<template>
  <div id="page-content">
    <h2>Commentaries</h2>
    <div v-if="!isLoading">
      <GoatForm>
        <span>Commentary:</span>
        <TextInput
          class="inline"
          v-model="commentaryName"
          placeholder="Commentary Name"
        />
        <span>ISBN:</span>
        <TextInput
          class="inline"
          v-model="commentaryIsbn"
          placeholder="ISBN - 13 Character"
        />
        <span>Books:</span>
        <SelectInput
          class="inline"
          v-model="selectedBookIds"
          v-bind:options="bookOptions"
          v-bind:isMultiple="true"
        />
        <ButtonInput
          text="Create Commentary"
          :handler="createCommentary"
          :isDisabled="!canCreateCommentary"
        />
      </GoatForm>
      <ul>
        <li
          v-for="commentary in commentaries"
          v-bind:key="commentary.id"
        >
          <ResourceLink :link="generateLink(commentary)" />
        </li>
      </ul>
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
import Commentary from '@/components/Commentary.vue';
import GoatForm from '@/components/GoatForm.vue';
import ResourceLink from '@/components/ResourceLink.vue';
import SelectInput from '@/components/SelectInput.vue';
import Spinner from '@/components/Spinner.vue';
import TextInput from '@/components/TextInput.vue';

// Models
import BookModel from '../models/Book';
import CommentaryModel from '../models/Commentary';
import LinkModel from '../models/Link';
import OptionModel from '../models/Option';

// Services
import HttpService, { HttpResponse, HttpError } from '../services/HttpService';

@Component({
  components: {
    ButtonInput,
    GoatForm,
    ResourceLink,
    SelectInput,
    Spinner,
    TextInput,
  },
})
export default class Commentaries extends Vue {
  private bookOptions: OptionModel[] = [];
  private commentaries: CommentaryModel[] = [];
  private commentaryName: string = '';
  private commentaryIsbn: string = '';
  private selectedBookIds: BookModel[] = [];

  private isLoadingBooks: boolean = false;
  private isLoadingCommentaries: boolean = false;

  public get canCreateCommentary(): boolean {
    return (
      !!this.commentaryName &&
      !!this.commentaryIsbn &&
      this.commentaryIsbn.length === 13 &&
      !!this.selectedBookIds &&
      this.selectedBookIds.length > 0
    );
  }

  public get isLoading(): boolean {
    return this.isLoadingBooks || this.isLoadingCommentaries;
  }

  private createCommentary() {
    if (this.canCreateCommentary) {
      const newCommentary: object = {
        name: this.commentaryName,
        bookIds: this.selectedBookIds,
        isbn: this.commentaryIsbn,
      };
      HttpService.post('api/commentaries', newCommentary)
        .then(() => {
          this.getCommentaries();
        })
        .catch((error: HttpError) => {
          console.error(error);
        });
    }
  }

  private generateLink(commentary: CommentaryModel): LinkModel {
    return new LinkModel(`/commentaries/${commentary.id}`, commentary.name);
  }

  private getBooks(): void {
    this.isLoadingBooks = true;
    HttpService.get('api/books')
      .then((response: HttpResponse<BookModel[]>) => {
        const books = response.data;
        if (books && books.length > 0) {
          this.selectedBookIds = [];
          for (const book of books) {
            this.bookOptions.push(new OptionModel(book.id, book.name));
          }
        }
      })
      .catch((error: HttpError) => {
        this.selectedBookIds = [];
        this.bookOptions = [];
        console.error(error);
      })
      .finally(() => {
        this.isLoadingBooks = false;
      });
  }

  private getCommentaries(): void {
    this.isLoadingCommentaries = true;
    HttpService.get('api/commentaries')
      .then((response: HttpResponse<CommentaryModel[]>) => {
        this.commentaries = response.data;
      })
      .catch((error: HttpError) => {
        this.commentaries = [];
        console.error(error);
      })
      .finally(() => {
        this.isLoadingCommentaries = false;
      });
  }

  private mounted() {
    this.getBooks();
    this.getCommentaries();
  }
}
</script>

<style scoped lang="scss">
@import '@/styles/page-content';
@import '@/styles/utility';

li {
  list-style-type: none;
}
</style>
