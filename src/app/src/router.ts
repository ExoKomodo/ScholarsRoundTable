import Vue from 'vue';
import Router from 'vue-router';

Vue.use(Router);

export default new Router({
  mode: 'history',
  base: process.env.BASE_URL,
  routes: [
    {
      path: '',
      name: 'home',
      component: () => import('./views/Home.vue'),
    },
    {
      path: '/search',
      name: 'search',
      component: () => import('./views/Home.vue'),
    },
    {
      path: '/faq',
      name: 'faq',
      component: () => import('./views/Faq.vue'),
    },
    {
      path: '/authorities',
      name: 'authorities',
      component: () => import('./views/Authorities.vue'),
    },
    {
      path: '/authorities/:id',
      name: 'authority',
      component: () => import('./views/Authority.vue'),
    },
    {
      path: '/commentaries',
      name: 'commentaries',
      component: () => import('./views/Commentaries.vue'),
    },
    {
      path: '/commentaries/:id',
      name: 'commentary',
      component: () => import('./views/Commentary.vue'),
    },
    {
      path: '/rankings',
      name: 'rankings',
      component: () => import('./views/Rankings.vue'),
    },
    {
      path: '/seminaries',
      name: 'seminaries',
      component: () => import('./views/Seminaries.vue'),
    },
    {
      path: '/seminaries/:id',
      name: 'seminary',
      component: () => import('./views/Seminary.vue'),
    },
    {
      path: '*',
      name: 'notFound',
      component: () => import('./views/NotFound.vue'),
    },
  ],
});
