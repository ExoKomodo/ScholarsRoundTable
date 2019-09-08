<template>
  <div class="number-input">
    <input
      v-bind:value="value"
      v-on:input="$emit('input', $event.target.value)"
      type="number"
      :placeholder="placeholder"
      :min="min"
      :max="max"
      :step="step"
    >
  </div>
</template>

<script lang="ts">
import { Component, Prop, Vue } from 'vue-property-decorator';

@Component
export default class NumberInput extends Vue {
  @Prop() private placeholder!: string;
  @Prop() private min!: number;
  @Prop() private max!: number;
  @Prop() private step!: number;
  @Prop() private value!: string;
}
</script>

<style scoped lang="scss">
</style>
