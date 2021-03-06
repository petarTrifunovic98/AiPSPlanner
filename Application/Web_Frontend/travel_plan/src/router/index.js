import Vue from 'vue'
import VueRouter from 'vue-router'
import PageRegister from "../pages/PageRegister.vue"
import PageLogin from "../pages/PageLogin.vue"
import PageMyTrips from "../pages/PageMyTrips.vue"
import PageSpecificTrip from "../pages/PageSpecificTrip.vue"
import PageWelcome from "../pages/PageWelcome.vue"
import PageNotAuthenticated from "../pages/PageNotAuthenticated.vue"
import PageNotFound from "../pages/PageNotFound.vue"
import PageViewProfile from "../pages/PageViewProfile.vue"
import PageItemList from "../pages/PageItemList.vue"
import PageNotifications from "../pages/PageNotifications.vue"
import PageTeamList from "../pages/PageTeamList.vue"
import PageAddMember from "../pages/PageAddMember.vue"
import PageTripAdditionalInfo from "../pages/PageTripAdditionalInfo.vue"
import PageNewTrip from "../pages/PageNewTrip.vue"
import store from "@/store"

Vue.use(VueRouter)

const routes = [
  {
    path: "/",
    name: "PageWelcome",
    component: PageWelcome,
    beforeEnter(to,from,next)
    {
        if(store.state.isLogedIn)
            next({name: 'PageMyTrips'})
        else
            next()
    }
  },
  {
    path: "/register",
    name: "PageRegister",
    component: PageRegister,
    beforeEnter(to,from,next)
    {
        if(store.state.isLogedIn)
            next({name: 'PageMyTrips'})
        else
            next()
    }
  },
  {
    path: "/login",
    name: "PageLogin",
    component: PageLogin,
    beforeEnter(to,from,next)
    {
        if(store.state.isLogedIn)
            next({name: 'PageMyTrips'})
        else
            next()
    }
  },
  {
    path: "/trips",
    name: "PageMyTrips",
    component: PageMyTrips,
    props: true,
    beforeEnter(to,from,next)
    {
        if(store.state.isLogedIn)
            next()
        else
            next({name: 'PageNotAuthenticated'})
    }
  },
  {
    path: "/items",
    name: "PageItemList",
    component: PageItemList,
    beforeEnter(to,from,next)
    {
        if(store.state.isLogedIn)
            next()
        else
            next({name: 'PageNotAuthenticated'})
    }
  },
  {
    path: "/teams",
    name: "PageTeamList",
    component: PageTeamList,
    beforeEnter(to,from,next)
    {
        if(store.state.isLogedIn)
            next()
        else
            next({name: 'PageNotAuthenticated'})
    }
  },
  {
    path: "/notifications",
    name: "PageNotifications",
    component: PageNotifications,
    beforeEnter(to,from,next)
    {
        if(store.state.isLogedIn)
            next()
        else
            next({name: 'PageNotAuthenticated'})
    }
  },
  {
    path: "/new-trip",
    name: "PageNewTrip",
    component: PageNewTrip,
    beforeEnter(to,from,next)
    {
        if(store.state.isLogedIn)
            next()
        else
            next({name: 'PageNotAuthenticated'})
    }
  },
  {
    path: "/specific-trip/:id",
    name: "PageSpecificTrip",
    component: PageSpecificTrip,
    props: true,
    beforeEnter(to,from,next)
    {
        if(store.state.isLogedIn)
            next()
        else
            next({name: 'PageNotAuthenticated'})
    }
  },
  {
    path: "/trip-additional-info/:id",
    name: "PageTripAdditionalInfo",
    component: PageTripAdditionalInfo,
    props: true,
    beforeEnter(to,from,next)
    {
        if(store.state.isLogedIn)
            next()
        else
            next({name: 'PageNotAuthenticated'})
    }
  },
  {
      path: "/profile/:id",
      name: "PageViewProfile",
      component: PageViewProfile,
      props: true,
      beforeEnter(to,from,next)
      {
          if(store.state.isLogedIn)
              next()
          else
              next({name: 'PageNotAuthenticated'})
      }
  },
  {
      path: "/add-member/:type/:id",
      name: "PageAddMember",
      component: PageAddMember,
      props: true,
      beforeEnter(to,from,next)
      {
          if(store.state.isLogedIn)
              next()
          else
              next({name: 'PageNotAuthenticated'})
      }
  },
  {
    path: '/401',
    name: 'PageNotAuthenticated',
    component: PageNotAuthenticated
  },
  {
    path: '*',
    name: 'PageNotFound',
    component: PageNotFound
  }
]

const router = new VueRouter({
  mode: 'history',
  scrollBehavior (to, from, savedPosition) {
    return { x: 0, y: 0 };
  },
  base: process.env.BASE_URL,
  routes
})

export default router
