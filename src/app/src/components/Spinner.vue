<template>
  <div class="spinner-container">
    <div class="spinner"></div>
    <label
      class="spinner-label"
      v-if="label && label.length > 0"
    >
      {{ label }}
    </label>
  </div>
</template>

<script lang="ts">
import { Component, Prop, Vue } from 'vue-property-decorator';

@Component
export default class Spinner extends Vue {
  @Prop() private label!: string;
}
</script>

<style scoped lang="scss">
@import '@/styles/variables';

.spinner-container {
  margin: auto;
}

.spinner {
  border: 1rem solid lighten($accent-1, 30%);
  border-top: 1rem solid $accent-1;
  border-radius: 50%;
  width: 2rem;
  height: 2rem;
  animation: spin 2s linear infinite;

  margin: auto;
}

.spinner-label {
  margin: auto;
  display: table;
}

@keyframes spin {
  0% {
    transform: rotate(0deg);
  }
  100% {
    transform: rotate(360deg);
  }
}
</style>