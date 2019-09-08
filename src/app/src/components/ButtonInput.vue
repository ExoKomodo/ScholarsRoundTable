<template>
  <div class="button-input">
    <input
      type="button"
      class="button"
      @click="handler(parameters)"
      :value="text"
      :disabled="isDisabled"
    >
  </div>
</template>

<script lang="ts">
import { Component, Prop, Vue } from 'vue-property-decorator';

@Component
export default class ButtonInput extends Vue {
  @Prop() private handler!: () => void;
  @Prop() private parameters: any;
  @Prop() private isDisabled!: boolean;
  @Prop() private text!: string;
}
</script>

<style scoped lang="scss">
@import '@/styles/variables';

/**
 * 1. Change the font styles in all browsers.
 * 2. Remove the margin in Firefox and Safari.
 */

.button {
  font-family: inherit; /* 1 */
  font-size: 100%; /* 1 */
  line-height: 1.15; /* 1 */
  margin: 0; /* 2 */

  /**
  * Show the overflow in IE.
  * 1. Show the overflow in Edge.
  */
  overflow: visible;

  /**
  * Remove the inheritance of text transform in Edge, Firefox, and IE.
  * 1. Remove the inheritance of text transform in Firefox.
  */
  text-transform: none;

  -webkit-appearance: button;
  display: inline-block;
  border: none;
  padding: 0.5rem 0.5rem;
  margin: 0;
  text-decoration: none;
  background-color: lighten($primary-dark, 20%);
  color: $accent-1;
  cursor: pointer;
  text-align: center;
  -webkit-appearance: none;
  -moz-appearance: none;

  &:focus {
    outline: 1px solid #fff;
    outline-offset: -4px;
  }

  &:disabled {
    background-color: #c7c7c7;
    color: lighten(black, 50%);
    cursor: default;
  }

  &:hover {
    box-shadow: 0 12px 12px 0 rgba(0, 0, 0, 0.24);
  }
}
</style>
