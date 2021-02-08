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
    tripsPortion: [],
    tripAdditionalInfo: null,
    hasEditRights: false,
    specificTrip: null
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
    },
    getTripAdditionalInfo: state => {
      return state.tripAdditionalInfo
    },
    getHasEditRights: state => {
      return state.hasEditRights
    },
    getSpecificTrip: state => {
      return state.specificTrip
    },
    getSpecificTripBasicInfo: state => {
      return {
        tripId: state.specificTrip.tripId,
        name: state.specificTrip.name,
        description: state.specificTrip.description,
        from: state.specificTrip.from,
        to: state.specificTrip.to
      }
    },
    getSpecificTripLocations: state => {
      return state.specificTrip.locations
    },
    getSpecificTripItems: state => {
      return state.specificTrip.itemList
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
    },
    setTripAdditionalInfo(state, info) {
      state.tripAdditionalInfo = info
    },
    setHasEditRights(state, value) {
      state.hasEditRights = value
    },
    setSpecificTrip(state, trip) {
      state.specificTrip = trip
    },
    setTripLocations(state, locations) {
      state.specificTrip.locations = locations
    },
    setLocationAccommodations(state, {data, locationId}) {
      const locationIndex = state.specificTrip.locations.findIndex(loc => loc.locationId == locationId)
      state.specificTrip.locations[locationIndex].accommodations = data
    },
    setSpecificTripBasicInfo(state, tripInfo) {
      state.specificTrip.name = tripInfo.name
      state.specificTrip.description = tripInfo.description
      state.specificTrip.from = tripInfo.from
      state.specificTrip.to = tripInfo.to
    },
    addLocationToSpecificTrip(state, location) {
      state.specificTrip.locations.push(location)
    },
    replaceEditedLocation(state, location) {
      const locationIndex = state.specificTrip.locations.findIndex(loc => loc.locationId == location.locationId)
      if(locationIndex > -1)
        state.specificTrip.locations.splice(locationIndex, 1, location)
    },
    removeLocationFromSpecificTrip(state, locationId) {
      const locationIndex = state.specificTrip.locations.findIndex(loc => loc.locationId == locationId)
      if(locationIndex > -1)
        state.specificTrip.locations.splice(locationIndex, 1)
    },
    addItemToSpecificTrip(state, item) {
      state.specificTrip.itemList.push(item)
    },
    replaceEditedItem(state, item) {
      const itemIndex = state.specificTrip.itemList.findIndex(it => it.itemId == item.itemId)
      if(itemIndex > -1) {
        state.specificTrip.itemList[itemIndex].name = item.name
        state.specificTrip.itemList[itemIndex].description = item.description
        state.specificTrip.itemList[itemIndex].amount = item.amount
        state.specificTrip.itemList[itemIndex].unit = item.unit
      }
    },
    removeItemFromSpecificTrip(state, itemId) {
      const itemIndex = state.specificTrip.itemList.findIndex(it => it.itemId == itemId)
      if(itemIndex > -1)
        state.specificTrip.itemList.splice(itemIndex, 1)
    },
    addAccommodationToLocation(state, accommodation) {
      const locationIndex = state.specificTrip.locations.findIndex(loc => loc.locationId == accommodation.locationId)
      state.specificTrip.locations[locationIndex].accommodations.push(accommodation)
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
    },

    requestTripEdit({commit}, payload) {
      commit("setDataLoaded", false)
      fetch("https://" + this.state.host + ":44301/api/edit-rights/request-edit/trip/" + payload.tripId + "/user/" + payload.userId, {
        method: "POST",
        headers: {
          "Content-type" : "application/json",
          "Authorization" : this.state.token
        }
      }).then(response => {
        if(response.ok) {
          response.json().then(data => {
            commit("setHasEditRights", data)
            commit("setDataLoaded", true)
          })
        }
        else {
          commit("setDataLoaded", true)
        }
      })
    },

    fillTripAdditionalInfo({commit}, payload) {
      commit("setDataLoaded", false)
      fetch("https://" + this.state.host + ":44301/api/trip/get-trip-additional-info/" + payload.tripId, {
        method: "GET",
        headers: {
          "Content-type" : "application/json",
          "Authorization" : this.state.token
        }
      }).then(response => {
        if(response.ok) {
          response.json().then(data => {
            commit("setTripAdditionalInfo", data)
            commit("setDataLoaded", true)
          })
        }
        else {
          commit("setDataLoaded", true)
        }
      })
    },

    addPackingListItem({commit}, payload) {
      fetch("https://" + this.state.host + ":44301/api/trip/add-to-trip-packing-list/" + payload.tripId, {
        method: 'POST',
        headers: {
          "Content-type" : "application/json"
        },
        body: JSON.stringify({
          "item": payload.item
        })
      }).then(response => {
        if(response.ok) {
          response.json().then(data => {
            console.log(data)
          })
        }
        else {
        }
      })
    },

    fillTripLocations({commit}, payload) {
      commit("setDataLoaded", false)
      fetch("https://" + this.state.host + ":44301/api/trip/get-trip-with-locations/" + payload.tripId, {
        method: "GET",
        headers: {
          "Content-type" : "application/json",
          "Authorization" : this.state.token
        }
      }).then(response => {
        if(response.ok) {
          response.json().then(data => {
            commit("setTripLocations", data.locations)
            commit("setDataLoaded", true)
          })
        }
        else {
          commit("setDataLoaded", true)
        }
      })
    },

    fillLocationAccommodations({commit}, payload) {
      fetch("https://" + this.state.host + ":44301/api/accommodation/get-location-accommodations/" + payload.locationId, {
        method: "GET",
        headers: {
          "Content-type" : "application/json",
          "Authorization" : this.state.token
        }
      }).then(response => {
        if(response.ok) {
          response.json().then(data => {
            commit("setLocationAccommodations", {'data': data, 'locationId': payload.locationId})
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
