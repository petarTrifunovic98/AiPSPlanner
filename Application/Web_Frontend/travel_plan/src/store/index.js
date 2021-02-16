import Vue from 'vue'
import Vuex from 'vuex'
import router from "../router/index"
import TravelPlanHub from "../travel-plan-hub"

Vue.use(Vuex)
Vue.use(TravelPlanHub)

export default new Vuex.Store({
  state: {
    host: "localhost",
    token: null,
    authUser: null,
    isLogedIn: false,
    isDataLoaded: true,
    tripsPortion: [],
    tripAdditionalInfo: null,
    hasEditRights: null,
    specificTrip: null,
    tripLocations: null,
    tripAddOns: null,
    notificationNumber: -1,
    user: null,
    wrongOriginalPass: false,
    myItems: null,
    myTeams: null,
    notifications: null
  },
  getters: {
    getIsDataLoaded: state => {
      return state.isDataLoaded
    },
    getAuthUserId: state => {
      return state.authUser.userId
    },
    getIsLogedIn: state => {
      return state.isLogedIn
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
      return state.tripLocations
    },
    getSpecificTripItems: state => {
      return state.specificTrip.itemList
    },
    getSpecificTripTravelers: state => {
      return state.specificTrip.travelers
    },
    getTripAddOns: state => {
      return state.tripAddOns
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
    setIsLogedIn(state, isLogedIn)
    {
      state.isLogedIn = isLogedIn
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
      state.tripLocations = locations
    },
    setTripAddOns(state, addOns) {
      if(addOns == null) {
        state.tripAddOns = null
        return
      }
      state.tripAddOns = {}
      addOns.slice(0).reverse().map(addOn => {
        if(addOn.lvl1DependId == 0 && addOn.lvl2DependId == 0) {
          state.tripAddOns[String(addOn.addOnId)] = addOn
          state.tripAddOns[String(addOn.addOnId)]["lvl1"] = {}
        }
        else if(addOn.lvl2DependId == 0) {
          state.tripAddOns[String(addOn.lvl1DependId)]["lvl1"][String(addOn.addOnId)] = addOn
          state.tripAddOns[String(addOn.lvl1DependId)]["lvl1"][String(addOn.addOnId)]["lvl2"] = {}
        }
        else {
          state.tripAddOns[String(addOn.lvl1DependId)]["lvl1"][String(addOn.lvl2DependId)]["lvl2"][String(addOn.addOnId)] = addOn
        }
      })
    },
    setLocationAccommodations(state, {data, locationId}) {
      const locationIndex = state.tripLocations.findIndex(loc => loc.locationId == locationId)
      state.tripLocations[locationIndex].accommodations = data
    },
    setSpecificTripBasicInfo(state, tripInfo) {
      state.specificTrip.name = tripInfo.name
      state.specificTrip.description = tripInfo.description
      state.specificTrip.from = tripInfo.from
      state.specificTrip.to = tripInfo.to
    },
    addLocationToSpecificTrip(state, location) {
      state.tripLocations.push(location)
    },
    replaceEditedLocation(state, location) {
      const locationIndex = state.tripLocations.findIndex(loc => loc.locationId == location.locationId)
      if(locationIndex > -1) {
        state.tripLocations[locationIndex].name = location.name
        state.tripLocations[locationIndex].description = location.description
        state.tripLocations[locationIndex].from = location.from
        state.tripLocations[locationIndex].to = location.to
      }
    },
    removeLocationFromSpecificTrip(state, locationId) {
      const locationIndex = state.tripLocations.findIndex(loc => loc.locationId == locationId)
      if(locationIndex > -1)
        state.tripLocations.splice(locationIndex, 1)
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
      const locationIndex = state.tripLocations.findIndex(loc => loc.locationId == accommodation.locationId)
      state.tripLocations[locationIndex].accommodations.push(accommodation)
    },
    editAccommodationForLocation(state, accommodation) {
      const locationIndex = state.tripLocations.findIndex(loc => loc.locationId == accommodation.locationId)
      if(locationIndex > -1) {
        const accommodationIndex = state.tripLocations[locationIndex].accommodations.findIndex(acc => acc.accommodationId == accommodation.accommodationId)
        if(accommodationIndex > -1)
          state.tripLocations[locationIndex].accommodations[accommodationIndex].type = accommodation.type
          state.tripLocations[locationIndex].accommodations[accommodationIndex].name = accommodation.name
          state.tripLocations[locationIndex].accommodations[accommodationIndex].description = accommodation.description
          state.tripLocations[locationIndex].accommodations[accommodationIndex].from = accommodation.from
          state.tripLocations[locationIndex].accommodations[accommodationIndex].to = accommodation.to
          state.tripLocations[locationIndex].accommodations[accommodationIndex].address = accommodation.address
      }
    },
    removeAccommodationFromLocation(state, accommodation) {
      const locationIndex = state.tripLocations.findIndex(loc => loc.locationId == accommodation.locationId)
      if(locationIndex > -1) {
        const accommodationIndex = state.tripLocations[locationIndex].accommodations.findIndex(acc => acc.accommodationId == accommodation.accommodationId)
        if(accommodationIndex > -1)
          state.tripLocations[locationIndex].accommodations.splice(accommodationIndex, 1)
      }
    },
    addTravelerToSpecificTrip(state, addedTravelers) {
      addedTravelers = addedTravelers.filter(traveler => state.specificTrip.travelers.findIndex(t => t.userId == traveler.userId) < 0)
      state.specificTrip.travelers = state.specificTrip.travelers.concat(addedTravelers)
    },
    removeTravelerFromSpecificTrip(state, travelerId) {
      const travelerIndex = state.specificTrip.travelers.findIndex(traveler => traveler.userId == travelerId)
      if(travelerIndex > -1) {
        state.specificTrip.travelers.splice(travelerIndex, 1)
      }
    },
    addAddOnToTrip(state, addOn) {
      if(addOn.lvl1DependId == 0 && addOn.lvl2DependId == 0) {
        addOn["lvl1"] = {}
        Vue.set(state.tripAddOns, String(addOn.addOnId), addOn)
      }
      else if(addOn.lvl2DependId == 0) {
        const topLevelAddOn = JSON.parse(JSON.stringify(state.tripAddOns[String(addOn.lvl1DependId)]))
        topLevelAddOn["lvl1"][String(addOn.addOnId)] = addOn
        topLevelAddOn["lvl1"][String(addOn.addOnId)]["lvl2"] = {}
        Vue.set(state.tripAddOns, String(addOn.lvl1DependId), topLevelAddOn)
      }
      else {
        const topLevelAddOn = JSON.parse(JSON.stringify(state.tripAddOns[String(addOn.lvl1DependId)]))
        topLevelAddOn["lvl1"][String(addOn.lvl2DependId)]["lvl2"][String(addOn.addOnId)] = addOn
        Vue.set(state.tripAddOns, String(addOn.lvl1DependId), topLevelAddOn)
      }
    },
    editAddOnForTrip(state, addOn) {
      if(addOn.lvl1DependId == 0 && addOn.lvl2DependId == 0) {
        const topLevelAddOn = JSON.parse(JSON.stringify(state.tripAddOns[addOn.addOnId]))
        topLevelAddOn.description = addOn.description
        topLevelAddOn.price = addOn.price
        Vue.set(state.tripAddOns, String(addOn.addOnId), topLevelAddOn)
      }
      else if(addOn.lvl2DependId == 0) {
        const topLevelAddOn = JSON.parse(JSON.stringify(state.tripAddOns[String(addOn.lvl1DependId)]))
        topLevelAddOn["lvl1"][String(addOn.addOnId)].description = addOn.description
        topLevelAddOn["lvl1"][String(addOn.addOnId)].price = addOn.price
        Vue.set(state.tripAddOns, String(addOn.lvl1DependId), topLevelAddOn)
      }
      else {
        const topLevelAddOn = JSON.parse(JSON.stringify(state.tripAddOns[String(addOn.lvl1DependId)]))
        topLevelAddOn["lvl1"][String(addOn.lvl2DependId)]["lvl2"][String(addOn.addOnId)].description = addOn.description
        topLevelAddOn["lvl1"][String(addOn.lvl2DependId)]["lvl2"][String(addOn.addOnId)].price = addOn.price
        Vue.set(state.tripAddOns, String(addOn.lvl1DependId), topLevelAddOn)
      }
    },
    removeAddOnsFromTrip(state, addOn) {
      if(addOn.lvl1DependId == 0 && addOn.lvl2DependId == 0) {
        Vue.delete(state.tripAddOns, String(addOn.addOnId))
      }
      else if(addOn.lvl2DependId == 0) {
        const topLevelAddOn = JSON.parse(JSON.stringify(state.tripAddOns[String(addOn.lvl1DependId)]))
        Vue.delete(topLevelAddOn["lvl1"], String(addOn.addOnId))
        Vue.set(state.tripAddOns, String(addOn.lvl1DependId), topLevelAddOn)
      }
      else {
        const topLevelAddOn = JSON.parse(JSON.stringify(state.tripAddOns[String(addOn.lvl1DependId)]))
        Vue.delete(topLevelAddOn["lvl1"][String(addOn.lvl2DependId)]["lvl2"], String(addOn.addOnId))
        Vue.set(state.tripAddOns, String(addOn.lvl1DependId), topLevelAddOn)
      }
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
            commit("setIsLogedIn", true)
            Vue.cookie.set('token',data['token'], { expires: '2h' });
            Vue.cookie.set('id',this.state.authUser.userId, { expires: '2h' });
            Vue.cookie.set('username',this.state.authUser.username, { expires: '2h' });
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
            commit("setIsLogedIn", true)           
            Vue.cookie.set('token',data['token'], { expires: '2h' });
            Vue.cookie.set('id',this.state.authUser.userId, { expires: '2h' });
            Vue.cookie.set('username',this.state.authUser.username, { expires: '2h' });
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

    fillSpecificTrip({commit}, payload) {
      commit("setDataLoaded", false)
      fetch("https://" + this.state.host + ":44301/api/trip/get-trip/" + payload.tripId, {
        method: "GET",
        headers: {
          "Content-type" : "application/json",
          "Authorization" : this.state.token
        }
      }).then(response => {
        if(response.ok) {
          response.json().then(data => {
            commit("setSpecificTrip", data)
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
            if(data == false) {
              
            }
            commit("setDataLoaded", true)
          })
        }
        else {
          commit("setDataLoaded", true)
        }
      })
    },

    releaseEditRights({commit}, payload) {
      fetch("https://" + this.state.host + ":44301/api/edit-rights/release-edit/trip/" + payload.tripId, {
        method: "DELETE",
        headers: {
          "Content-type" : "application/json",
          "Authorization" : this.state.token
        }
      }).then(response => {
        if(response.ok) {
          commit("setHasEditRights", null)
        }
        else {
        }
      })
    },

    cancelEditRequest({commit}, payload) {
      fetch("https://" + this.state.host + ":44301/api/edit-rights/cancel-edit-request/trip/" + payload.tripId + "/user/" + payload.userId, {
        method: "DELETE",
        headers: {
          "Content-type" : "application/json",
          "Authorization" : this.state.token
        }
      }).then(response => {
        if(response.ok) {
          commit("setHasEditRights", null)
        }
        else {
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
    },

    fillTripAddOns({commit}, payload) {
      commit("setDataLoaded", false)
      fetch("https://" + this.state.host + ":44301/api/add-on/get-add-ons/" + payload.tripId, {
        method: "GET",
        headers: {
          "Content-type" : "application/json",
          "Authorization" : this.state.token
        }
      }).then(response => {
        if(response.ok) {
          response.json().then(data => {
            commit("setTripAddOns", data)
            commit("setDataLoaded", true)
          })
        }
        else {
          commit("setDataLoaded", true)
        }
      })
    },

    putEditLocation({commit}, payload) {
      fetch("https://" + this.state.host + ":44301/api/location/edit-location/" + payload.tripId, {
        method: "PUT",
        headers: {
          "Content-type" : "application/json",
          "Authorization" : this.state.token
        },
        body: JSON.stringify( {
          "locationId" : payload.locationId,
          "name" : payload.name,
          "description": payload.description,
          "from": payload.from,
          "to": payload.to
        })
      }).then(response => {
        if(response.ok) {
          response.json().then(data => {
            console.log("Location edited")
          })
        }
        else {
        }
      }).catch(err => console.log(err))
    },

    putEditAccommodation({commit}, payload) {
      fetch("https://" + this.state.host + ":44301/api/accommodation/edit-accommodation/" + payload.tripId, {
        method: "PUT",
        headers: {
          "Content-type" : "application/json",
          "Authorization" : this.state.token
        },
        body: JSON.stringify( {
          "accommodationId" : payload.accommodationId,
          "type" : payload.type,
          "name" : payload.name,
          "description": payload.description,
          "from": payload.from,
          "to": payload.to,
          "address" : payload.address,
          "locationId" : payload.locationId
        })
      }).then(response => {
        if(response.ok) {
          response.json().then(data => {
            console.log("Accommodation edited")
          })
        }
        else {
        }
      }).catch(err => console.log(err))
    },

    putEditItemInfo({commit}, payload) {
      fetch("https://" + this.state.host + ":44301/api/item/edit-item/" + payload.tripId, {
        method: "PUT",
        headers: {
          "Content-type" : "application/json",
          "Authorization" : this.state.token
        },
        body: JSON.stringify( {
          "itemId" : payload.itemId,
          "name" : payload.name,
          "description": payload.description,
          "amount" : payload.amount,
          "unit" : payload.unit
        })
      }).then(response => {
        if(response.ok) {
          response.json().then(data => {
            console.log("Item info edited")
          })
        }
        else {
        }
      }).catch(err => console.log(err))
    },

    putEditAddOn({commit}, payload) {
      fetch("https://" + this.state.host + ":44301/api/add-on/edit-add-on/" + payload.tripId, {
        method: "PUT",
        headers: {
          "Content-type" : "application/json",
          "Authorization" : this.state.token
        },
        body: JSON.stringify( {
          "addOnId" : payload.addOnId,
          "description": payload.description,
          "price" : payload.price
        })
      }).then(response => {
        if(response.ok) {
          response.json().then(data => {
            console.log("Add-on edited")
          })
        }
        else {
        }
      }).catch(err => console.log(err))
    },

    putEditTripInfo({commit}, payload) {
      fetch("https://" + this.state.host + ":44301/api/trip/edit-info", {
        method: "PUT",
        headers: {
          "Content-type" : "application/json",
          "Authorization" : this.state.token
        },
        body: JSON.stringify( {
          "tripId" : payload.tripId,
          "name": payload.name,
          "description": payload.description,
          "from": payload.from,
          "to": payload.to,
        })
      }).then(response => {
        if(response.ok) {
          response.json().then(data => {
            console.log("Trip info edited")
          })
        }
        else {
        }
      }).catch(err => console.log(err))
    },

    getUserById({commit}, payload){
      fetch("https://" + this.state.host + ":44301/api/user/get-user/" + payload.userId, {
        method: "GET",
        headers: {
          "Content-type" : "application/json",
          "Authorization" : this.state.token
        }
      }).then(response => {
        if(response.ok) {
          response.json().then(data => {
            this.state.authUser = data
            if(router.currentRoute["path"] == "/401")
              router.back();
            if(router.currentRoute["path"] == "/" || router.currentRoute["path"] == "/login" || router.currentRoute["path"] == "/register")
            {
              router.push("/trips");
            }
          })
        }
        else {
        }
      }).catch(err => console.log(err))
    },

    getUserInfo({commit}, payload)
    {
      fetch("https://" + this.state.host + ":44301/api/user/get-user/" + payload.userId, {
        method: "GET",
        headers: {
          "Content-type" : "application/json",
          "Authorization" : this.state.token
        }
      }).then(response => {
        if(response.ok) {
          response.json().then(data => {
            this.state.user = data
          })
        }
        else {
        }
      }).catch(err => console.log(err))
    },

    getNotificationNumber(){
      fetch("https://" + this.state.host + ":44301/api/notifications/get-notification-number/" + this.state.authUser.userId, {
        method: "GET",
        headers: {
          "Content-type" : "application/json",
          "Authorization" : this.state.token
        }
      }).then(response => {
        if(response.ok) {
          response.json().then(data => {
            this.state.notificationNumber = data
          })
        }
        else {
        }
      }).catch(err => console.log(err))
    },

    logoutUser()
    {
      fetch("https://" + this.state.host + ":44301/api/user/logout/" + this.state.authUser.userId, {
        method: "GET",
        headers: {
          "Content-type" : "application/json",
          "Authorization" : this.state.token
        }
      }).then(response => {
        if(response.ok) {
            this.state.authUser = null
            this.state.token = null
            this.state.notificationNumber = -1
            this.state.isLogedIn = false
            this.state.tripsPortion = []
            this.state.tripAdditionalInfo = null
            this.state.hasEditRights = false
            this.state.specificTrip = null
            this.state.tripAddOns = []
            this.state.user = null
            this.state.myItems = null
            this.state.myTeams = null
            this.state.notifications = null

            Vue.cookie.delete('id');
            Vue.cookie.delete('token');
            Vue.cookie.delete('username');         
        }
        else {
        }
      }).catch(err => console.log(err))
    },

    editUser({commit}, {pictureChanged}) {
      fetch("https://" + this.state.host + ":44301/api/user/edit-info/", {
          method: 'PUT',
          headers: {
              "Content-Type": "application/json",
              "Authorization": this.state.token
          },
          body: JSON.stringify({
              "UserId": this.state.authUser.userId,
              "Name": this.state.authUser.name,
              "LastName": this.state.authUser.lastName,
              "Picture": pictureChanged ? this.state.authUser.picture : null
          })
      }).then(p => {

          if(p.ok) {
              p.json().then(data=> {
                  //console.log(data)
                  //console.log(this.state.authUser)
              })
          }
          else {
              //console.log("error")
          }

      })
    },

    changePassword({commit}, {newPassword, oldPassword})
    {
      this.state.isDataLoaded = false
      fetch("https://" + this.state.host + ":44301/api/user/change-password", {
          method: 'PUT',
          headers: {
              "Content-Type": "application/json",
              "Authorization": this.state.token
          },
          body: JSON.stringify({
              "UserId": this.state.authUser.userId,
              "OldPassword": oldPassword,
              "NewPassword": newPassword
          })
      }).then(p => {

          if(p.ok) {
            this.state.isDataLoaded = true
            this.state.wrongOriginalPass = false
            router.push("/");
            router.push({name: "PageViewProfile", 
              params: {
                id: this.state.authUser.userId, 
                user: this.state.authUser
            }})
          }
          else {
              this.state.isDataLoaded = true
              this.state.wrongOriginalPass = true
          }

      })
    },

    fillMyItems({commit})
    {
      commit("setDataLoaded", false)
      fetch("https://" + this.state.host + ":44301/api/item/get-items/user/" + this.state.authUser.userId, {
        method: "GET",
        headers: {
          "Content-type" : "application/json",
          "Authorization" : this.state.token
        }
      }).then(response => {
        if(response.ok) {
          response.json().then(data => {
            commit("setDataLoaded", true)
            this.state.myItems = data
          })
        }
        else {
          commit("setDataLoaded", true)
        }
      })
    },

    fillMyTeams({commit}){
      commit("setDataLoaded", false)
      fetch("https://" + this.state.host + ":44301/api/team/get-teams/user/" + this.state.authUser.userId, {
        method: "GET",
        headers: {
          "Content-type" : "application/json",
          "Authorization" : this.state.token
        }
      }).then(response => {
        if(response.ok) {
          response.json().then(data => {
            commit("setDataLoaded", true)
            this.state.myTeams = data
          })
        }
        else {
          commit("setDataLoaded", true)
        }
      })
    },

    fillNotifications({commit})
    {
      commit("setDataLoaded", false)
      fetch("https://" + this.state.host + ":44301/api/notifications/get-user-notifications/" + this.state.authUser.userId, {
        method: "GET",
        headers: {
          "Content-type" : "application/json",
          "Authorization" : this.state.token
        }
      }).then(response => {
        if(response.ok) {
          response.json().then(data => {
            commit("setDataLoaded", true)
            this.state.notifications = data
          })
        }
        else {
          commit("setDataLoaded", true)
        }
      })
    },

    seenNotifications()
    {
      fetch("https://" + this.state.host + ":44301/api/notifications/seen-notifications/" + this.state.authUser.userId, {
        method: "PUT",
        headers: {
          "Content-type" : "application/json",
          "Authorization" : this.state.token
        }
      }).then(response => {
        if(response.ok) {
          response.json().then(data => {
            console.log("seen notifications")
          })
        }
        else {
         console.log("Error seeing notifications")
        }
      })
    },

    deleteSeenNotifications({commit}, itemRelated)
    {
      fetch("https://" + this.state.host + ":44301/api/notifications/delete-seen/" + this.state.authUser.userId + "/" + itemRelated, {
        method: "DELETE",
        headers: {
          "Content-type" : "application/json",
          "Authorization" : this.state.token
        }
      }).then(response => {
        if(response.ok) {
          response.json().then(data => {
            console.log("delete notifications")
          })
        }
        else {
         console.log("Error deleting notifications")
        }
      })
    },

    createTeam({commit}, payload)
    {
      fetch("https://" + this.state.host + ":44301/api/team/creator/" + this.state.authUser.userId + "/create-team", {
        method: 'POST',
        headers: {
          "Content-type" : "application/json",
          "Authorization" : this.state.token
        },
        body: JSON.stringify({
          "name": payload.name
        })
      }).then(response => {
        if(response.ok) {
          response.json().then(data => {
            if(this.state.myTeams != null)
              this.state.myTeams.push(data);
          })
        }
        else {
        }
      })
    },

    editTeamInfo({commit}, payload)
    {
      fetch("https://" + this.state.host + ":44301/api/team/edit-info", {
        method: 'PUT',
        headers: {
          "Content-type" : "application/json",
          "Authorization" : this.state.token
        },
        body: JSON.stringify({
          "name": payload.name,
          "teamId": payload.teamId
        })
      }).then(response => {
        if(response.ok) {
          response.json().then(data => {
            console.log("ok")
          })
        }
        else {
        }
      })
    },

    leaveTeam({commit}, payload)
    {
      fetch("https://" + this.state.host + ":44301/api/team/remove-user/" + payload.teamId + "/" + this.state.authUser.userId, {
        method: 'PUT',
        headers: {
          "Content-type" : "application/json",
          "Authorization" : this.state.token
        }
      }).then(response => {
        if(response.ok) {
          console.log("left team")
        }
        else {
        }
      })
    }
  },
  modules: {
  }
})
