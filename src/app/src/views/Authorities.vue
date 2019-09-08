<template>
  <div id="page-content">
    <h2>Authorities</h2>
    <div v-if="!isLoading">
      <GoatForm>
        <span>Authority:</span>
        <TextInput
          class="inline"
          v-model="authorityName"
          placeholder="Authority Name"
        />
        <span>Seminary:</span>
        <SelectInput
          class="inline"
          v-model="selectedSeminaryId"
          v-bind:options="seminaryOptions"
        />
        <ButtonInput
          text="Create Authority"
          :handler="createAuthority"
          :isDisabled="!canCreateAuthority"
        />
      </GoatForm>
      <ul>
        <li
          v-for="authority in authorities"
          v-bind:key="authority.id"
        >
          <ResourceLink :link="generateLink(authority)" />
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
import SelectInput from '@/components/SelectInput.vue';
import Spinner from '@/components/Spinner.vue';
import TextInput from '@/components/TextInput.vue';

// Services
import HttpService, { HttpResponse, HttpError } from '../services/HttpService';

// Models
import AuthorityModel from '../models/Authority';
import OptionModel from '../models/Option';
import SeminaryModel from '../models/Seminary';
import LinkModel from '../models/Link';

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
export default class Authorities extends Vue {
  private authorities: AuthorityModel[] = [];
  private authorityName: string = '';
  private selectedSeminaryId: number = -1;
  private seminaries: SeminaryModel[] = [];
  private seminaryOptions: OptionModel[] = [];

  private isLoadingAuthorities: boolean = true;
  private isLoadingSeminaries: boolean = true;

  public get canCreateAuthority(): boolean {
    return !!this.authorityName && !!this.selectedSeminaryId;
  }

  public get isLoading(): boolean {
    return this.isLoadingAuthorities || this.isLoadingSeminaries;
  }

  private createAuthority() {
    if (this.canCreateAuthority) {
      const newAuthority: object = {
        name: this.authorityName,
        seminaryId: this.selectedSeminaryId,
      };
      HttpService.post('api/authorities', newAuthority)
        .then(() => {
          this.getAuthorities();
        })
        .catch((error: HttpError) => {
          console.error(error);
        });
    }
  }

  private generateLink(authority: AuthorityModel): LinkModel {
    return new LinkModel(`/authorities/${authority.id}`, authority.name);
  }

  private getAuthorities(): void {
    this.isLoadingAuthorities = true;
    HttpService.get('api/authorities')
      .then((response: HttpResponse<AuthorityModel[]>) => {
        this.authorities = response.data;
      })
      .catch((error: HttpError) => {
        this.authorities = [];
        console.error(error);
      })
      .finally(() => {
        this.isLoadingAuthorities = false;
      });
  }

  private getSeminaries(): void {
    this.isLoadingSeminaries = true;
    HttpService.get('api/seminaries')
      .then((response: HttpResponse<SeminaryModel[]>) => {
        this.seminaries = response.data;
        if (this.seminaries && this.seminaries.length > 0) {
          this.selectedSeminaryId = this.seminaries[0].id;
          for (const seminary of this.seminaries) {
            this.seminaryOptions.push(
              new OptionModel(seminary.id, seminary.name),
            );
          }
        }
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
    this.getAuthorities();
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
