<template>
  <li>
    <a :href="link.route">
      {{ link.text }}
    </a>
  </li>
</template>

<script lang="ts">
// Vue
import { Component, Prop, Vue } from 'vue-property-decorator';

// Models
import LinkModel from '../models/Link';

@Component
export default class NavLink extends Vue {
  @Prop() private link!: LinkModel;
}
</script>

<style scoped lang="scss">
</style>
