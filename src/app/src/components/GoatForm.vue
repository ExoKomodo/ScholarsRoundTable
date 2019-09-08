<template>
  <div class="form">
    <slot></slot>
  </div>
</template>

<script lang="ts">
import { Component, Prop, Vue } from 'vue-property-decorator';

@Component
export default class GoatForm extends Vue {}
</script>

<style scoped lang="scss">
@import '@/styles/variables';

.button-input,
.number-input,
.select-input,
.text-input {
  margin: 5px;
}
</style>
