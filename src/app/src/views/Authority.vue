<template>
  <div id="page-content">
    <div v-if="authority">
      <h2>Authority: {{ authorityName }}</h2>
      <div v-if="seminary">
        <h2>Seminary</h2>
        <ResourceLink :link="generateSeminaryLink()" />
      </div>
      <div
        id="update-authority"
        v-if="isAdmin"
      >
        <h2>Update Authority</h2>
        <GoatForm>
          <span>Name:</span>
          <TextInput
            class="inline"
            v-model="authority.name"
            placeholder="Authority Name"
          />
          <span>Seminary:</span>
          <SelectInput
            class="inline"
            v-model="authority.seminaryId"
            v-bind:options="seminaryOptions"
          />
          <ButtonInput
            text="Update Authority"
            :handler="updateAuthority"
            :isDisabled="!canUpdateAuthority"
          />
        </GoatForm>
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
import ResourceLink from '@/components/ResourceLink.vue';
import SelectInput from '@/components/SelectInput.vue';
import TextInput from '@/components/TextInput.vue';

// Services
import AuthService from '../services/AuthService';
import HttpService, { HttpResponse, HttpError } from '../services/HttpService';

// Models
import AuthorityModel from '../models/Authority';
import LinkModel from '../models/Link';
import OptionModel from '../models/Option';
import SeminaryModel from '../models/Seminary';

@Component({
  components: {
    ButtonInput,
    GoatForm,
    ResourceLink,
    SelectInput,
    TextInput,
  },
})
export default class Authority extends Vue {
  private authority: AuthorityModel | null = null;
  private authorityName: string | null = null;
  private seminary: SeminaryModel | null = null;
  private seminaryOptions: OptionModel[] = [];

  private isLoadingSeminaries: boolean = true;

  public get canUpdateAuthority(): boolean {
    return !!this.authority && !!this.authority.id && !!this.authority.name;
  }

  public get isAdmin(): boolean {
    return (
      !!AuthService && !!AuthService.userInfo && AuthService.userInfo.isAdmin
    );
  }

  private generateSeminaryLink(): LinkModel | null {
    if (!!this.seminary) {
      return new LinkModel(
        `/seminaries/${this.seminary.id}`,
        this.seminary.name,
      );
    }
    return null;
  }

  private getAuthority(): void {
    HttpService.get(`api/authorities/${this.$route.params.id}`)
      .then((response: HttpResponse<AuthorityModel>) => {
        this.authority = response.data;
        this.authorityName = this.authority.name;
        this.getSeminary();
      })
      .catch((error: HttpError) => {
        console.error('No authority grabbed');
      });
  }

  private getSeminary(): void {
    if (!this.authority) {
      return;
    }
    HttpService.get(`api/seminaries/${this.authority.seminaryId}`)
      .then((response: HttpResponse<SeminaryModel>) => {
        this.seminary = response.data;
      })
      .catch((error: HttpError) => {
        console.error('No authority grabbed');
      });
  }

  private getSeminaries(): void {
    this.isLoadingSeminaries = true;
    HttpService.get('api/seminaries')
      .then((response: HttpResponse<SeminaryModel[]>) => {
        const seminaries = response.data;
        if (seminaries && seminaries.length > 0) {
          this.seminaryOptions = [];
          for (const seminary of seminaries) {
            this.seminaryOptions.push(
              new OptionModel(seminary.id, seminary.name),
            );
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

  private updateAuthority(): void {
    HttpService.put(
      `api/authorities/${this.authority != null ? this.authority.id : null}`,
      this.authority,
    )
      .then((response: HttpResponse<AuthorityModel>) => {
        if (!!this.authority) {
          this.authorityName = this.authority.name;
        }
        this.getSeminary();
      })
      .catch((error: HttpError) => {
        console.error(error);
      });
  }

  private mounted() {
    this.getAuthority();
    this.getSeminaries();
  }
}
</script>

<style scoped lang="scss">
@import '@/styles/page-content';
@import '@/styles/utility';
</style>
