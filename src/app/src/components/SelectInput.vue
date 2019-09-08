<template>
  <div class="select-input">
    <select
      v-model="computedValue"
      v-bind:multiple="isMultiple"
    >
      <option
        v-for="option in options"
        v-bind:key="option.id"
        :value="option.id"
      >
        {{option.text}}
      </option>
    </select>
  </div>
</template>

<script lang="ts">
// Vue
import { Component, Prop, Vue } from 'vue-property-decorator';

// Models
import OptionModel from '../models/Option';

@Component
export default class SelectInput extends Vue {
  @Prop() private value!: string;
  @Prop() private options!: OptionModel[];
  @Prop() private isMultiple!: boolean;

  private get computedValue(): any {
    return this.value;
  }

  private set computedValue(newValue: any) {
    this.$emit('input', newValue);
  }
}
</script>

<style scoped lang="scss">
</style>
