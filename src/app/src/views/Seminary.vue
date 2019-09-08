<template>
  <div id="page-content">
    <div v-if="seminary">
      <h2>Seminary: {{ seminaryName }}</h2>
      <div
        id="update-seminary"
        v-if="isAdmin"
      >
        <h2>Update Seminary</h2>
        <GoatForm>
          <span>Name:</span>
          <TextInput
            class="inline"
            v-model="seminary.name"
            placeholder="Seminary Name"
          />
          <ButtonInput
            text="Update Seminary"
            :handler="updateSeminary"
            :isDisabled="!canUpdateSeminary"
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
import TextInput from '@/components/TextInput.vue';

// Services
import AuthService from '../services/AuthService';
import HttpService, { HttpResponse, HttpError } from '../services/HttpService';

// Models
import SeminaryModel from '../models/Seminary';

@Component({
  components: {
    ButtonInput,
    GoatForm,
    TextInput,
  },
})
export default class Seminary extends Vue {
  private seminary: SeminaryModel | null = null;
  private seminaryName: string | null = null;

  public get canUpdateSeminary(): boolean {
    return !!this.seminary && !!this.seminary.id && !!this.seminary.name;
  }

  public get isAdmin(): boolean {
    return (
      !!AuthService && !!AuthService.userInfo && AuthService.userInfo.isAdmin
    );
  }

  private getSeminary(): void {
    HttpService.get(`api/seminaries/${this.$route.params.id}`)
      .then((response: HttpResponse<SeminaryModel>) => {
        this.seminary = response.data;
        this.seminaryName = this.seminary.name;
      })
      .catch((error: HttpError) => {
        console.error('No seminary grabbed');
      });
  }

  private updateSeminary(): void {
    HttpService.put(
      `api/seminaries/${this.seminary != null ? this.seminary.id : null}`,
      this.seminary,
    )
      .then((response: HttpResponse<SeminaryModel>) => {
        if (this.seminary) {
          this.seminaryName = this.seminary.name;
        }
      })
      .catch((error: HttpError) => {
        console.error(error);
      });
  }

  private mounted() {
    this.getSeminary();
  }
}
</script>

<style scoped lang="scss">
@import '@/styles/page-content';
@import '@/styles/utility';
</style>
