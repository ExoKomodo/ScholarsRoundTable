<template>
  <div id="page-content">
    <h2>Seminaries</h2>
    <div v-if="!isLoading">
      <GoatForm>
        <span>Seminary:</span>
        <TextInput
          class="inline"
          v-model="seminaryName"
          placeholder="Seminary Name"
        />
        <ButtonInput
          text="Create Seminary"
          :handler="createSeminary"
          :isDisabled="!canCreateSeminary"
        />
      </GoatForm>
      <ul>
        <li
          v-for="seminary in seminaries"
          v-bind:key="seminary.id"
        >
          <ResourceLink :link="generateLink(seminary)" />
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
import GoatForm from '@/components/GoatForm.vue';
import ResourceLink from '@/components/ResourceLink.vue';
import Seminary from '@/components/Seminary.vue';
import Spinner from '@/components/Spinner.vue';
import TextInput from '@/components/TextInput.vue';

// Services
import HttpService, { HttpResponse, HttpError } from '../services/HttpService';

// Models
import LinkModel from '../models/Link';
import SeminaryModel from '../models/Seminary';

@Component({
  components: {
    ButtonInput,
    GoatForm,
    ResourceLink,
    Spinner,
    TextInput,
  },
})
export default class Seminaries extends Vue {
  private seminaries: SeminaryModel[] = [];
  private seminaryName: string = '';

  private isLoadingSeminaries: boolean = false;

  public get canCreateSeminary(): boolean {
    return !!this.seminaryName;
  }

  public get isLoading(): boolean {
    return this.isLoadingSeminaries;
  }

  private createSeminary() {
    if (this.canCreateSeminary) {
      const newSeminary: object = {
        name: this.seminaryName,
      };
      HttpService.post('api/seminaries', newSeminary)
        .then(() => {
          this.getSeminaries();
        })
        .catch((error: HttpError) => {
          console.error(error);
        });
    }
  }

  private generateLink(seminary: SeminaryModel): LinkModel {
    return new LinkModel(`/seminaries/${seminary.id}`, seminary.name);
  }

  private getSeminaries(): void {
    this.isLoadingSeminaries = true;
    HttpService.get('api/seminaries')
      .then((response: HttpResponse<SeminaryModel[]>) => {
        this.seminaries = response.data;
      })
      .catch((error: HttpError) => {
        this.seminaries = [];
        console.error(error);
      })
      .finally(() => {
        this.isLoadingSeminaries = false;
      });
  }

  private mounted() {
    this.getSeminaries();
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
