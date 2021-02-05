import Vue from 'vue'
import Vuex from 'vuex'
import router from "../router/index"

Vue.use(Vuex)

export default new Vuex.Store({
  state: {
    host: "localhost",
    token: null,
    authUser: null,
    isDataLoaded: true,
    tripsPortion: []
  },
  getters: {
    getIsDataLoaded: state => {
      return state.isDataLoaded
    },
    getAuthUserId: state => {
      return state.authUser.userId
    },
    getTripsPortion: state => {
      return state.tripsPortion
    }
  },
  mutations: {
    setDataLoaded(state, loaded) {
      state.isDataLoaded = loaded
    },
    setAuthUser(state, userInfo) {
      state.token = userInfo.token
      state.authUser = {
        name: userInfo.name,
        lastName: userInfo.lastName,
        userId: userInfo.userId,
        username: userInfo.username,
        picture: userInfo.picture
      }
    },
    setTripsPortion(state, trips) {
      state.tripsPortion = trips
    }
  },
  actions: {
    createUser({commit}, payload) {
      commit("setDataLoaded", false)
      fetch("https://" + this.state.host + ":44301/api/user/add-account/", {
        method: 'POST',
        headers: {
          "Content-type" : "application/json"
        },
        body: JSON.stringify({
          "username": payload.username,
          "password": payload.password,
          "name": payload.name,
          "lastName": payload.lastName
        })
      }).then(response => {
        if(response.ok) {
          response.json().then(data => {
            commit("setAuthUser", data)
            commit("setDataLoaded", true)
            router.push("/trips")
          })
        }
        else {
          commit("setDataLoaded", true)
        }
      })
    },

    fillAuthUser({commit}, payload) {
      commit("setDataLoaded", false)
      fetch("https://" + this.state.host + ":44301/api/user/login/", {
        method: 'POST',
        headers: {
          "Content-type" : "application/json"
        },
        body:  JSON.stringify( {
          "username" : payload.username,
          "password" : payload.password
        })
      }).then( response => {
        if(response.ok) {
          response.json().then(data => {
            commit("setAuthUser", data)
            commit("setDataLoaded", true)
            // Vue.cookie.set('token',data['token']);
            // Vue.cookie.set('id',this.state.authUser.id, { expires: '1h' });
            // Vue.cookie.set('ime',this.state.authUser.first_name, { expires: '1h' });
            // Vue.cookie.set('prezime',this.state.authUser.last_name, { expires: '1h' });
            // Vue.cookie.set('admin',this.state.isAdmin, { expires: '1h' });
            router.push('/trips')
          })
        }
        else {
          commit("setDataLoaded", true)
        }
      });
    },

    fillTripsPortion({commit}, payload) {
      commit("setDataLoaded", false)
      fetch("https://" + this.state.host + ":44301/api/trip/get-trips/user/" + payload.userId, {
        method: "GET",
        headers: {
          "Content-type" : "application/json",
          "Authorization" : this.state.token
        }
      }).then(response => {
        if(response.ok) {
          response.json().then(data => {
            commit("setTripsPortion", data)
            commit("setDataLoaded", true)
          })
        }
        else {
          commit("setDataLoaded", true)
        }
      })
    }
  },
  modules: {
  }
})
