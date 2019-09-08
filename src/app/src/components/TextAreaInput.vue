<template>
  <div class="textarea-input">
    <textarea
      :placeholder="placeholder"
      v-bind:value="value"
      v-on:input="$emit('input', $event.target.value)"
    >
    </textarea>
  </div>
</template>

<script lang="ts">
import { Component, Prop, Vue } from 'vue-property-decorator';

@Component
export default class TextAreainput extends Vue {
  @Prop() private placeholder!: string;
  @Prop() private value!: string;
}
</script>

<style scoped lang="scss">
</style>
