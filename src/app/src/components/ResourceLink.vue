<template>
  <div class="card resource-item">
    <a
      class="resource-link"
      :target="noNewTab ? '_self' : '_blank'"
      :href="link.route"
    >
      <div v-html="link.text"></div>
      <hr v-if="link.text && hasSeparator">
      <p v-if="link.content">
        {{ link.content }}
      </p>
      <slot></slot>
    </a>
  </div>
</template>

<script lang="ts">
// Vue
import { Component, Prop, Vue } from 'vue-property-decorator';

// Models
import LinkModel from '../models/Link';

@Component
export default class ResourceLink extends Vue {
  @Prop() private link!: LinkModel;
  @Prop() private noNewTab!: boolean;
  @Prop() private hasSeparator!: boolean;
}
</script>

<style scoped lang="scss">
@import '@/styles/variables';
@import '@/styles/card';

.resource-item {
  // width: 100%;
  padding: 5px;
}

.resource-link {
  color: $link;
  font-weight: bold;
  text-decoration: none;
  height: 100%;
  // width: 100%;
  display: block;
  padding: 5px;

  hr {
    width: 98%;
    margin-left: 0;
  }
}
</style>
