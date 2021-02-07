import Vue from 'vue'
import VueRouter from 'vue-router'
import PageRegister from "../pages/PageRegister.vue"
import PageLogin from "../pages/PageLogin.vue"
import PageMyTrips from "../pages/PageMyTrips.vue"
import PageSpecificTrip from "../pages/PageSpecificTrip"

Vue.use(VueRouter)

const routes = [
  {
    path: "/register",
    name: "PageRegister",
    component: PageRegister
  },
  {
    path: "/login",
    name: "PageLogin",
    component: PageLogin
  },
  {
    path: "/trips",
    name: "PageMyTrips",
    component: PageMyTrips
  },
  {
    path: "/specific-trip/:id",
    name: "PageSpecificTrip",
    component: PageSpecificTrip,
    props: true
  }
]

const router = new VueRouter({
  mode: 'history',
  base: process.env.BASE_URL,
  routes
})

export default router
