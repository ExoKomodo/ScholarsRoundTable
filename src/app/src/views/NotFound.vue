<template>
  <div
    id="page-content"
    class="404"
  >
    <h2>
      404 Not Found
    </h2>
  </div>
</template>

<script lang="ts">
// Vue
import { Component, Vue } from 'vue-property-decorator';

@Component({
  components: {},
})
export default class NotFound extends Vue {}
</script>

<style scoped lang="scss">
@import '@/styles/page-content';
</style>