<template>
  <div class="text-input">
    <input
      type="text"
      :placeholder="placeholder"
      v-bind:value="value"
      v-on:input="$emit('input', $event.target.value)"
    >
  </div>
</template>

<script lang="ts">
import { Component, Prop, Vue } from 'vue-property-decorator';

@Component
export default class Textinput extends Vue {
  @Prop() private placeholder!: string;
  @Prop() private value!: string;
}
</script>

<style scoped lang="scss">
</style>
