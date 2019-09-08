<template>
  <div id="app">
    <SiteNav />
    <div id="site-content">
      <router-view />
    </div>
    <footer>
      <div id="footer-container">
        <a
          v-if="!!faqLink"
          :href="faqLink.route"
          class="link"
        >
          <span v-html="faqLink.text"></span>
        </a>
        <div>© Scholars Roundtable 2020</div>
      </div>
    </footer>
  </div>
</template>

<script lang="ts">
// Vue
import { Component, Vue } from 'vue-property-decorator';

// Components
import SiteNav from '@/components/SiteNav.vue';

// Models
import LinkModel from './models/Link';
import UserInfoModel from './models/UserInfo';

// Services
import AuthService from './services/AuthService';

@Component({
  components: {
    SiteNav,
  },
})
export default class App extends Vue {
  private faqLink: LinkModel | null = null;

  private mounted(): void {
    this.faqLink = new LinkModel('/faq', 'FAQ');
  }
}
</script>

<style lang="scss">
@import '@/styles/utility';
@import '@/styles/variables';
@import url('https://fonts.googleapis.com/css?family=Libre+Baskerville&display=swap');

$footer-height: 5rem;

html {
  font-size: 100%;
  min-height: 100%;
  position: relative;
}

body {
  margin: 0;
  background-color: $primary-light;
}

footer {
  bottom: 0;
  width: 100%;
  position: absolute;
  display: block;
  height: $footer-height;
  overflow: hidden;
  background-color: $accent-3;
}

#app {
  font-family: 'Libre Baskerville', serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
}

#footer-container {
  text-align: center;
  margin-top: 1rem;

  div {
    margin-top: 0.5rem;
  }
}

#site-content {
  margin-bottom: $footer-height;
}
</style>
