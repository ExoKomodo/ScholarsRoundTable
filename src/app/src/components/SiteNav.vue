<template>
  <nav id="site-nav">
    <ul>
      <li id="website-banner">
        <div>
          <router-link
            id="search-link"
            :to="'/'"
          >
            <h1>
              <div>Scholars</div>
              <div>Roundtable</div>
            </h1>
          </router-link>
        </div>
      </li>
      <NavLink
        class="nav-link"
        v-for="link in links"
        v-bind:key="link.route"
        :link="link"
      />
      <li>
        <img
          v-if="!isLoggedIn"
          id="profile-picture"
          src="@/assets/btn_google_light_normal_ios.svg"
          @click="login()"
        >
        <img
          v-if="isLoggedIn"
          id="profile-picture"
          :src="userInfo.profilePic"
          @click="logout()"
        >
      </li>
    </ul>
  </nav>
</template>

<script lang="ts">
// Vue
import { Component, Prop, Vue } from 'vue-property-decorator';

// Components
import NavLink from '@/components/NavLink.vue';

// Models
import LinkModel from '../models/Link';
import UserInfoModel from '../models/UserInfo';

// Services
import AuthService from '../services/AuthService';
import HttpService, { HttpResponse, HttpError } from '../services/HttpService';

@Component({
  components: {
    NavLink,
  },
})
export default class SiteNav extends Vue {
  private isLoggedIn: boolean = false;
  private isLoading: boolean = false;
  private links: LinkModel[] = [];
  private userInfo: UserInfoModel | null = null;

  public login(): void {
    AuthService.login();
  }

  public logout(): void {
    AuthService.logout();
  }

  private mounted(): void {
    this.setLinks();
    let queryString: string = window.location.href;
    const splitQueryString: string[] = queryString.split('?');
    let userId: string | undefined | null = null;
    if (this.userInfo !== null) {
      this.userInfo = AuthService.userInfo;
      this.isLoggedIn = AuthService.isLoggedIn();
      AuthService.rerouteNonAdminUsers(this.$router);
      this.setLinks();
      return;
    }
    if (splitQueryString.length > 1) {
      queryString = splitQueryString[1];
      const queryParams: Map<string, string> = HttpService.parseQueryParams(
        queryString,
      );
      if (queryParams.has('userId')) {
        userId = queryParams.get('userId');
      }
    } else {
      const userInfo: UserInfoModel | null = AuthService.userInfo;
      if (userInfo !== null) {
        userId = userInfo.id;
      }
    }
    if (userId != null) {
      AuthService.setUserInfo(userId).then(() => {
        this.userInfo = AuthService.userInfo;
        this.isLoggedIn = AuthService.isLoggedIn();
        AuthService.rerouteNonAdminUsers(this.$router);
        this.setLinks();
      });
    } else {
      AuthService.rerouteNonAdminUsers(this.$router);
    }
  }

  private setLinks(): void {
    this.links = [];
    const userInfo: UserInfoModel | null = AuthService.userInfo;
    if (userInfo !== null && userInfo.isAdmin) {
      this.links.push(
        new LinkModel('/authorities', 'Authorities'),
        new LinkModel('/commentaries', 'Commentaries'),
        new LinkModel('/seminaries', 'Seminaries'),
        new LinkModel('/rankings', 'Rankings'),
      );
    }
  }
}
</script>

<style lang="scss">
@import '@/styles/variables';

#profile-picture {
  object-fit: contain;
  height: 100%;
  cursor: pointer;
}

#search-link {
  text-decoration: none;
}

#site-nav {
  top: 0;
  width: 100%;

  ul {
    height: 4rem;
    list-style-type: none;
    margin: 0;
    padding: 0;
    overflow-x: auto;
    overflow-y: hidden;
    white-space: nowrap;
    background-color: $primary-dark;

    li,
    .nav-link {
      height: 100%;
      vertical-align: top;
      display: inline-block;
      width: 10rem;

      a {
        display: flex;
        color: $primary-light;
        align-items: center;
        justify-content: center;
        text-decoration: none;
        width: 100%;
        height: 4rem;
      }

      a:hover {
        background-color: $link;
      }
    }

    li:last-child {
      float: right;
      height: 4rem;
      width: auto;
    }
  }
}

#website-banner {
  margin-right: 1rem;
  margin-left: 1rem;
  margin-top: auto;
  margin-bottom: auto;

  div {
    height: 100%;

    a {
      display: block;

      h1 {
        width: 9rem;
        color: $accent-1;
        margin: 0;
        font-size: 1.3rem;
      }
    }

    a:hover {
      background-color: $primary-dark !important;
    }
  }
}
</style>
