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
    notifications: null,
    searchedUsers: null,
    allAvailableDecorations: null,
    accommodationTypes: null,
    addOnWatch: null,
    positiveVotes: null,
    negativeVotes: null,
    votables: null
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
    getSpecificTripId: state => {
      return state.specificTrip.tripId
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
    getAccommodationTypes: state => {
      return state.accommodationTypes
    },
    getSpecificTripItems: state => {
      return state.specificTrip.itemList
    },
    getSpecificTripTravelers: state => {
      return state.specificTrip.travelers
    },
    getAvailableDecorations: (state) => (addOn) => {
      if(!state.tripAddOns)
        return []
      if(!addOn && !state.allAvailableDecorations) {
        return []
      }
      else if(!addOn) {
        return state.allAvailableDecorations.map(dec => dec.typeName)
      }
      else if(addOn) {
        if(addOn.lvl1DependId == 0 && addOn.lvl2DependId == 0) {
          let topLevelDec = state.allAvailableDecorations.find(dec => dec.typeName == addOn.type)
          if(topLevelDec) {
            return topLevelDec.nextLvlDecorations.map(dec => dec.typeName)
          }
        }
        else if(addOn.lvl2DependId == 0) {
          let topLevelType = state.tripAddOns[addOn.lvl1DependId].type
          let topLevelDec = state.allAvailableDecorations.find(dec => dec.typeName == topLevelType) 
          if(topLevelDec) {
            let lvl2Dec = topLevelDec.nextLvlDecorations.find(dec => dec.typeName == addOn.type)
            if(lvl2Dec) {
              return lvl2Dec.nextLvlDecorations.map(dec => dec.typeName)
            }
          }
        }
      }
      return []
    },
    getTripAddOns: state => {
      return state.tripAddOns
    },
    getAddOnWatch: state => {
      return state.addOnWatch
    },
    getPositiveVotes: state => {
      return state.positiveVotes
    },
    getNegativeVotes: state => {
      return state.negativeVotes
    },
    getVotables: state => {
      return state.votables
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
      if(locations == null)
        return
      if(state.votables) {
        state.tripLocations.forEach(loc => {
          state.votables.push(loc.votable)
        })
      }
    },
    setAvailableDecorations(state, data) {
      state.allAvailableDecorations = data
    },
    setTripAddOns(state, addOns) {
      if(addOns == null) {
        state.tripAddOns = null
        return
      }
      if(state.votables)
        addOns.forEach(addOn => {
          state.votables.push(addOn.votable)
        })
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
    setAccommodationTypes(state, data) {
      state.accommodationTypes = data
    },
    setLocationAccommodations(state, {data, locationId}) {
      if(state.votables) {
        data.forEach(acc => {
          state.votables.push(acc.votable)
        })
      }
      const locationIndex = state.tripLocations.findIndex(loc => loc.locationId == locationId)
      state.tripLocations[locationIndex].accommodations = data
    },
    setAccommodationPictures(state, {data, accommodationId, locationId}) {
      const locationIndex = state.tripLocations.findIndex(loc => loc.locationId == locationId)
      const accommodationIndex = state.tripLocations[locationIndex].accommodations.findIndex(acc => acc.accommodationId == accommodationId)
      state.tripLocations[locationIndex].accommodations[accommodationIndex].pictures = data
    },
    setSpecificTripBasicInfo(state, tripInfo) {
      state.specificTrip.name = tripInfo.name
      state.specificTrip.description = tripInfo.description
      state.specificTrip.from = tripInfo.from
      state.specificTrip.to = tripInfo.to
    },
    setVotes(state, {positive, data}) {
      if(positive) 
        state.positiveVotes = data
      else
        state.negativeVotes = data
    },
    setVotables(state, data) {
      state.votables = data
    },
    addLocationToSpecificTrip(state, location) {
      state.tripLocations.push(location)
      state.votables.push(location.votable)
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
      let locationIndex = state.tripLocations.findIndex(loc => loc.locationId == locationId)
      if(locationIndex > -1) {
        let votableIndex = state.votables.findIndex(v => v.votableId == state.tripLocations[locationIndex].votableId)
        if(votableIndex > -1)
          state.votables.splice(votableIndex, 1)
        state.tripLocations[locationIndex].accommodations.forEach(acc => {
          votableIndex = state.votables.findIndex(v => v.votableId == acc.votableId)
          if(votableIndex > -1)
            state.votables.splice(votableIndex, 1)
        })
        state.tripLocations.splice(locationIndex, 1)
      }
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
        state.specificTrip.itemList[itemIndex].userId = item.userId
        state.specificTrip.itemList[itemIndex].checked = item.checked
        if(item.user)
          state.specificTrip.itemList[itemIndex].user = item.user
      }
    },
    removeItemFromSpecificTrip(state, itemId) {
      const itemIndex = state.specificTrip.itemList.findIndex(it => it.itemId == itemId)
      if(itemIndex > -1)
        state.specificTrip.itemList.splice(itemIndex, 1)
    },
    addAccommodationToLocation(state, accommodation) {
      if(state.votables)
        state.votables.push(accommodation.votable)
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
        if(accommodationIndex > -1) {
          const votableIndex = state.votables.findIndex(v => v.votableId == state.tripLocations[locationIndex].accommodations[accommodationIndex].votableId)
          state.tripLocations[locationIndex].accommodations.splice(accommodationIndex, 1)
          if(votableIndex > -1)
           state.votables.splice(votableIndex, 1)
        }
      }
    },
    addPictureToAccommodation(state, data) {
      const locationIndex = state.tripLocations.findIndex(loc => loc.locationId == data.locationId)
      const accommodationIndex = state.tripLocations[locationIndex].accommodations.findIndex(acc => acc.accommodationId == data.accommodationId)
      state.tripLocations[locationIndex].accommodations[accommodationIndex].pictures.push(data)
    },
    removePictureFromAccommodation(state, deleteInfo) {
      const locationIndex = state.tripLocations.findIndex(loc => loc.locationId == deleteInfo.locationId)
      const accommodationIndex = state.tripLocations[locationIndex].accommodations.findIndex(acc => acc.accommodationId == deleteInfo.accommodationId)
      const pictureIndex = state.tripLocations[locationIndex].accommodations[accommodationIndex].pictures.findIndex(p => p.accommodationPictureId == deleteInfo.accommodationPictureId)
      state.tripLocations[locationIndex].accommodations[accommodationIndex].pictures.splice(pictureIndex, 1)
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
      if(state.votables)
        state.votables.push(addOn.votable)

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
      state.addOnWatch = 1
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
      state.addOnWatch = 1
    },
    removeAddOnsFromTrip(state, addOn) {
      if(addOn.lvl1DependId == 0 && addOn.lvl2DependId == 0) {
        let votableIndex = state.votables.findIndex(v => v.votableId == addOn.votableId)
        if(votableIndex > -1)
          state.votables.splice(votableIndex, 1)
        for(const prop in addOn.lvl1) {
          votableIndex = state.votables.findIndex(v => v.votableId == addOn.lvl1[prop].votableId)
          if(votableIndex > -1)
            state.votables.splice(votableIndex, 1)
          for(const prop2 in addOn.lvl1[prop].lvl2) {
            votableIndex = state.votables.findIndex(v => v.votableId == addOn.lvl1[prop].lvl2[prop2].votableId)
            if(votableIndex > -1)
              state.votables.splice(votableIndex, 1)
          }
        }
        Vue.delete(state.tripAddOns, String(addOn.addOnId))
      }
      else if(addOn.lvl2DependId == 0) {
        const topLevelAddOn = JSON.parse(JSON.stringify(state.tripAddOns[String(addOn.lvl1DependId)]))
        let votableIndex = state.votables.findIndex(v => v.votableId == topLevelAddOn.lvl1[String(addOn.addOnId)].votableId)
        if(votableIndex > -1) {
          state.votables.splice(votableIndex, 1)
          for(const prop in topLevelAddOn.lvl1[addOn.addOnId].lvl2) {
            votableIndex = state.votables.findIndex(v => v.votableId == topLevelAddOn.lvl1[String(addOn.addOnId)].lvl2[prop].votableId)
            if(votableIndex > -1) 
              state.votables.splice(votableIndex, 1)
          }
        } 
        Vue.delete(topLevelAddOn["lvl1"], String(addOn.addOnId))
        Vue.set(state.tripAddOns, String(addOn.lvl1DependId), topLevelAddOn)
      }
      else {
        const topLevelAddOn = JSON.parse(JSON.stringify(state.tripAddOns[String(addOn.lvl1DependId)]))
        let votableIndex = state.votables.findIndex(v => v.votableId == topLevelAddOn.lvl1[String(addOn.lvl2DependId)].lvl2[String(addOn.addOnId)].votableId)
        if(votableIndex > -1) 
          state.votables.splice(votableIndex, 1)
        Vue.delete(topLevelAddOn["lvl1"][String(addOn.lvl2DependId)]["lvl2"], String(addOn.addOnId))
        Vue.set(state.tripAddOns, String(addOn.lvl1DependId), topLevelAddOn)
      }
      state.addOnWatch = 1
    },
    editTeamName(state, team){
      state.myTeams.find(element => element.teamId == team.teamId).name = team.name
    },
    removeUserFromTeam(state, teamInfo){
      var team = state.myTeams.find(element => element.teamId == teamInfo.teamId)
      team.members = team.members.filter(element => element.userId != teamInfo.removedUserId)
    },
    addMemberToTeam(state, teamInfo){
      state.myTeams.find(element => element.teamId == teamInfo.teamId).members = teamInfo.users
    },
    replaceVotable(state, newVotable) {
      let votableIndex = state.votables.findIndex(v => v.votableId == newVotable.votableId)
      state.votables.splice(votableIndex, 1, newVotable)
    },
    addVote(state, newVote) {
      if(newVote.positive) {
        state.positiveVotes.push(newVote)
      }
      else
        state.negativeVotes.push(newVote)
    },
    changeVote(state, newVote) {
      if(!newVote.positive) {
        let voteIndex = state.positiveVotes.findIndex(v => v.voteId == newVote.voteId)
        state.positiveVotes.splice(voteIndex, 1)
        state.negativeVotes.push(newVote)
      }
      else {
        let voteIndex = state.negativeVotes.findIndex(v => v.voteId == newVote.voteId)
        state.negativeVotes.splice(voteIndex, 1)
        state.positiveVotes.push(newVote)
      }
    },
    removeVote(state, {positive, voteId}) {
      if(positive) {
        let voteIndex = state.positiveVotes.findIndex(v => v.voteId == voteId)
        state.positiveVotes.splice(voteIndex, 1)
      }
      else {
        let voteIndex = state.negativeVotes.findIndex(v => v.voteId == voteId)
        state.negativeVotes.splice(voteIndex, 1)
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
            this.state.tripAdditionalInfo.packingList = this.state.tripAdditionalInfo.packingList.map(element =>
            {
              var retValue = new Object()
              retValue.item = element
              retValue.checked = false
              return retValue
            })
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
        method: 'PUT',
        headers: {
          "Content-type" : "application/json",
          "Authorization" : this.state.token
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

    fillAccommodationTypes({commit}) {
      commit("setDataLoaded", false)
      fetch("https://" + this.state.host + ":44301/api/accommodation/get-accommodation-types/", {
        method: "GET",
        headers: {
          "Content-type" : "application/json",
          "Authorization" : this.state.token
        }
      }).then(response => {
        if(response.ok) {
          response.json().then(data => {
            commit("setAccommodationTypes", data)
            commit("setDataLoaded", true)
          })
        }
        else {
          console.log(response)
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

    fillAccommodationPictures({commit}, payload) {
      fetch("https://" + this.state.host + ":44301/api/accommodation/get-pictures/accommodation/" + payload.accommodationId, {
        method: "GET",
        headers: {
          "Content-type" : "application/json",
          "Authorization" : this.state.token
        }
      }).then(response => {
        if(response.ok) {
          response.json().then(data => {
            commit("setAccommodationPictures", {'data': data, 'accommodationId': payload.accommodationId, 'locationId': payload.locationId})
            commit("setDataLoaded", true)
          })
        }
        else {
          commit("setDataLoaded", true)
        }
      })
    },

    fillAvailableDecorations({commit}, payload) {
      commit("setDataLoaded", false)
      fetch("https://" + this.state.host + ":44301/api/add-on/get-available-decorations/" + payload.tripId, {
        method: "GET",
        headers: {
          "Content-type" : "application/json",
          "Authorization" : this.state.token
        }
      }).then(response => {
        if(response.ok) {
          response.json().then(data => {
            commit("setAvailableDecorations", data)
            commit("setDataLoaded", true)
          })
        }
        else {
          console.log(response)
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
            this.state.searchedUsers = null

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

    postAddLocation({commit}, newLocation) {
      fetch("https://" + this.state.host + ":44301/api/location/create-location/", {
        method: 'POST',
        headers: {
          "Content-type" : "application/json",
          "Authorization" : this.state.token
        },
        body: JSON.stringify({
          "name": newLocation.name,
          "description": newLocation.description,
          "latitude": newLocation.latitude,
          "longitude": newLocation.longitude,
          "from": newLocation.from,
          "to": newLocation.to,
          "tripId": newLocation.tripId
        })
      }).then(response => {
        if(response.ok) {
          response.json().then(data => {
            console.log("Location added")
            commit("addLocationToSpecificTrip", data)
          })
        }
        else {
          console.log(response)
        }
      })
    },

    deleteLocation({commit}, payload) {
      fetch("https://" + this.state.host + ":44301/api/location/delete-location/" + payload.locationId + "/" + payload.tripId, {
        method: 'DELETE',
        headers: {
          "Content-type" : "application/json",
          "Authorization" : this.state.token
        }
      }).then(response => {
        if(response.ok) {
          console.log("Location deleted")
        }
        else {
          console.log(response)
        }
      })
    },

    postAddAccommodation({commit}, {tripId, newAccommodation}) {
      fetch("https://" + this.state.host + ":44301/api/accommodation/create-accommodation/" + tripId, {
        method: 'POST',
        headers: {
          "Content-type" : "application/json",
          "Authorization" : this.state.token
        },
        body: JSON.stringify({
          "type": newAccommodation.type,
          "name": newAccommodation.name,
          "description": newAccommodation.description,
          "from": newAccommodation.from,
          "to": newAccommodation.to,
          "address": newAccommodation.address,
          "locationId": newAccommodation.locationId
        })
      }).then(response => {
        if(response.ok) {
          response.json().then(data => {
            console.log("Accommodation added")
            commit("addAccommodationToLocation", data)
          })
        }
        else {
          console.log(response)
        }
      })
    },

    deleteAccommodation({commit}, {tripId, accRemovalInfo}) {
      fetch("https://" + this.state.host + ":44301/api/accommodation/delete-accommodation/" + accRemovalInfo.accommodationId + "/" + tripId, {
        method: 'DELETE',
        headers: {
          "Content-type" : "application/json",
          "Authorization" : this.state.token
        }
      }).then(response => {
        if(response.ok) {
          console.log("Accommodation deleted")
        }
        else {
          console.log(response)
        }
      })
    },

    postAddAccommodationPicture({commit}, newPicture) {
      fetch("https://" + this.state.host + ":44301/api/accommodation/add-picture/" + newPicture.tripId, {
        method: 'POST',
        headers: {
          "Content-type" : "application/json",
          "Authorization" : this.state.token
        },
        body: JSON.stringify({
          "accommodationId": newPicture.accommodationId,
          "picture": newPicture.picture
        })
      }).then(response => {
        if(response.ok) {
          response.json().then(data => {
            console.log("Picture added")
            commit("addPictureToAccommodation", data)
          })
        }
        else {
          console.log(response)
        }
      })
    },

    deleteAccommodationPicture({commit}, {tripId, deleteInfo}) {
      fetch("https://" + this.state.host + ":44301/api/accommodation/delete-picture/" + deleteInfo.accommodationPictureId + "/" + tripId, {
        method: 'DELETE',
        headers: {
          "Content-type" : "application/json",
          "Authorization" : this.state.token
        }
      }).then(response => {
        if(response.ok) {
          console.log("Picture deleted")
        }
        else {
          console.log(response)
        }
      })
    },
    
    postAddItem({commit}, newItem) {
      fetch("https://" + this.state.host + ":44301/api/item/create-item/", {
        method: 'POST',
        headers: {
          "Content-type" : "application/json",
          "Authorization" : this.state.token
        },
        body: JSON.stringify({
          "name": newItem.name,
          "description": newItem.description,
          "amount": newItem.amount,
          "unit": newItem.unit,
          "userId": newItem.userId,
          "tripId": newItem.tripId
        })
      }).then(response => {
        if(response.ok) {
          response.json().then(data => {
            console.log("Item added")
            commit("addItemToSpecificTrip", data)
          })
        }
        else {
          console.log(response)
        }
      })
    },

    deleteItem({commit}, payload) {
      fetch("https://" + this.state.host + ":44301/api/item/delete-item/" + payload.itemId + "/" + payload.tripId, {
        method: 'DELETE',
        headers: {
          "Content-type" : "application/json",
          "Authorization" : this.state.token
        }
      }).then(response => {
        if(response.ok) {
          console.log("Item deleted")
        }
        else {
          console.log(response)
        }
      })
    },

    putChangeItemUser({commit}, payload) {
      fetch("https://" + this.state.host + ":44301/api/item/change-user/" + payload.itemId + "/" + payload.newUserId + "/" + payload.tripId, {
        method: 'PUT',
        headers: {
          "Content-type" : "application/json",
          "Authorization" : this.state.token
        }
      }).then(response => {
        if(response.ok) {
          console.log("Item user changed")
        }
        else {
          console.log(response)
        }
      })
    },

    putChangeCheckedItem({commit}, payload) {
      fetch("https://" + this.state.host + ":44301/api/item/change-checked/" + payload.itemId + "/" + payload.tripId, {
        method: 'PUT',
        headers: {
          "Content-type" : "application/json",
          "Authorization" : this.state.token
        }
      }).then(response => {
        if(response.ok) {
          console.log("Item checked status changed")
        }
        else {
          console.log(response)
        }
      })
    },

    postAddAddOn({commit}, newAddOn) {
      fetch("https://" + this.state.host + ":44301/api/add-on/create-add-on/", {
        method: 'POST',
        headers: {
          "Content-type" : "application/json",
          "Authorization" : this.state.token
        },
        body: JSON.stringify({
          "tripId": newAddOn.tripId,
          "addOnType": newAddOn.type,
          "price": newAddOn.price,
          "description": newAddOn.description,
          "lvl1DependId": newAddOn.lvl1DependId,
          "lvl2DependId": newAddOn.lvl2DependId
        })
      }).then(response => {
        if(response.ok) {
          response.json().then(data => {
            console.log("Add-on added")
            commit("addAddOnToTrip", data)
          })
        }
        else {
          console.log(response)
        }
      })
    },

    deleteAddOn({commit}, payload) {
      fetch("https://" + this.state.host + ":44301/api/add-on/delete-add-on/" + payload.addOnId + "/" + payload.tripId, {
        method: 'DELETE',
        headers: {
          "Content-type" : "application/json",
          "Authorization" : this.state.token
        }
      }).then(response => {
        if(response.ok) {
          console.log("Add-on deleted")
        }
        else {
          console.log(response)
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
    },

    addUserToTeam({commit}, payload){
      fetch("https://" + this.state.host + ":44301/api/team/add-user/" + payload.objectId + "/" + payload.memberId, {
        method: 'PUT',
        headers: {
          "Content-type" : "application/json",
          "Authorization" : this.state.token
        }
      }).then(response => {
        if(response.ok) {
          response.json().then(data => {
            console.log("User added to team")
            console.log(data)
            this.state.myTeams.forEach((element, index) => {
              if(element.teamId == payload.objectId)
              {
                this.state.myTeams[index].members = data.members;
              }
            });
          })
        }
        else {
          console.log(response)
        }
      })
    },

    addTeamToTeam({commit}, payload){
      fetch("https://" + this.state.host + ":44301/api/team/add-team/" + payload.objectId + "/" + payload.memberId, {
        method: 'PUT',
        headers: {
          "Content-type" : "application/json",
          "Authorization" : this.state.token
        }
      }).then(response => {
        if(response.ok) {
          response.json().then(data => {
            console.log("Team added to team")
            console.log(data)
            this.state.myTeams.forEach((element, index) => {
              if(element.teamId == payload.objectId)
              {
                this.state.myTeams[index].members = data.members;
              }
            });
          })
        }
        else {
          console.log(response)
        }
      })
    },

    addUserToTrip({commit}, payload){
      fetch("https://" + this.state.host + ":44301/api/trip/add-user/" + payload.objectId + "/" + payload.memberId, {
        method: 'PUT',
        headers: {
          "Content-type" : "application/json",
          "Authorization" : this.state.token
        }
      }).then(response => {
        if(response.ok) {
          response.json().then(data => {
            console.log("User added to trip")
            console.log(data)
            // Add travelers to the right trip
          })
        }
        else {
          console.log(response)
        }
      })
    },

    addTeamToTrip({commit}, payload){
      fetch("https://" + this.state.host + ":44301/api/trip/add-team/" + payload.objectId + "/" + payload.memberId, {
        method: 'PUT',
        headers: {
          "Content-type" : "application/json",
          "Authorization" : this.state.token
        }
      }).then(response => {
        if(response.ok) {
          response.json().then(data => {
            console.log("User added to trip")
            console.log(data)
            // Add travelers to the right trip
          })
        }
        else {
          console.log(response)
        }
      })
    },

    searchUsers({commit}, payload){
      commit("setDataLoaded", false)
      fetch("https://" + this.state.host + ":44301/api/user/get-users-substring/" + payload, {
        method: "GET",
        headers: {
          "Content-type" : "application/json",
          "Authorization" : this.state.token
        }
      }).then(response => {
        if(response.ok) {
          response.json().then(data => {
            commit("setDataLoaded", true)
            this.state.searchedUsers = data
          })
        }
        else {
          commit("setDataLoaded", true)
        }
      })
    },

    createTrip({commit}, payload){
      commit("setDataLoaded", false)
      fetch("https://" + this.state.host + ":44301/api/trip/creator/" + this.state.authUser.userId + "/create-trip", {
        method: 'POST',
        headers: {
          "Content-type" : "application/json",
          "Authorization" : this.state.token
        },
        body: JSON.stringify({
          "name": payload.name,
          "description": payload.description,
          "from": payload.from,
          "to": payload.to,
          "tripCategory": payload.tripCategory
        })
      }).then(response => {
        if(response.ok) {
          response.json().then(data => {
            commit("setDataLoaded", true)
            router.push("/")
          })
        }
        else {
          commit("setDataLoaded", true)
        }
      })
    },

    fillVotes({commit}, payload) {
      commit("setVotes", {positive: payload.positive, data: null})
      fetch("https://" + this.state.host + ":44301/api/vote/get-votes/" + payload.votableId + "/" + payload.positive, {
        method: "GET",
        headers: {
          "Content-type" : "application/json",
          "Authorization" : this.state.token
        }
      }).then(response => {
        if(response.ok) {
          response.json().then(data => {
            commit("setVotes", {positive: payload.positive, data: data})
          })
        }
        else {
          commit("setVotes", {positive: payload.positive, data: []})
        }
      })
    },

    postAddVote({commit}, {tripId, voteInfo}) {
      fetch("https://" + this.state.host + ":44301/api/vote/vote/" + tripId, {
        method: 'POST',
        headers: {
          "Content-type" : "application/json",
          "Authorization" : this.state.token
        },
        body: JSON.stringify({
          "positive": voteInfo.positive,
          "userId": voteInfo.userId,
          "votableId": voteInfo.votableId
        })
      }).then(response => {
        if(response.ok) {
          response.json().then(data => {
            commit("addVote", data)
          })
        }
        else {
          console.log(response)
        }
      })
    },

    putChangeVote({commit}, {tripId, voteInfo}) {
      fetch("https://" + this.state.host + ":44301/api/vote/change-vote/" + tripId, {
        method: 'PUT',
        headers: {
          "Content-type" : "application/json",
          "Authorization" : this.state.token
        },
        body: JSON.stringify({
          "voteId": voteInfo.voteId,
          "positive": voteInfo.positive
        })
      }).then(response => {
        if(response.ok) {
          response.json().then(data => {
            commit("changeVote", data)
          })
        }
        else {
          console.log(response)
        }
      })
    },

    deleteVote({commit}, payload) {
      fetch("https://" + this.state.host + ":44301/api/vote/remove-vote/" + payload.voteId + "/" + payload.tripId, {
        method: 'DELETE',
        headers: {
          "Content-type" : "application/json",
          "Authorization" : this.state.token
        }
      }).then(response => {
        if(response.ok) {
          commit("removeVote", {positive: payload.positive, voteId: payload.voteId})
        }
        else {
          console.log(response)
        }
      })
    }
  },
  modules: {
    
  }
})
