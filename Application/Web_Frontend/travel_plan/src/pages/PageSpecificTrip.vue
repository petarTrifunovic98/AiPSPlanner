<template>
  <div v-if="isDataLoaded && specificTrip" class="main-wrap">
    <div class="a-row primary-row">
      <div class="basic-info">
        <BasicInfo :tripInfo="tripInfo" v-if="tripInfo"/>
        <div class="leave-trip">
          <button 
            type="button" class="btn btn-danger leave-btn justify-btn" 
            @click="openModalLeave = true" v-if="hasEditRights"
            :disabled="!canLeave">
            <img src="../assets/failed.svg" class="action-img" >
            <span> Leave trip </span>
          </button>
          <img v-if="!canLeave" src="../assets/information.svg" class="info" v-b-popover.hover.top="leaveInfo">
        </div>
        <button type="button" class="btn btn-primary justify-btn back-btn" @click="backToTrips">
          <img src="../assets/back.png" class="action-img">
          <span> Back to trips </span>
        </button>
      </div>
      <Travelers @addMember="addMember"/>
    </div>
    <div class="a-row" >
      <AddOns :tripId="parseInt(tripId)"/>
    </div>
    <div class="a-row" >
      <Locations/>
    </div>
    <div class="a-row" >
      <Items/>
    </div>
    <ModalAreYouSure 
      :naslov="'Leave trip'"
      :tekst="'Are you sure you want to leave this trip?'"
      @close="openModalLeave = false" @yes="leaveTrip" v-if="openModalLeave"
    />
  </div>
  <Spinner v-else />
</template>

<script>
import * as signalR from '@aspnet/signalr'
import { mapGetters, mapMutations } from "vuex"
import BasicInfo from "@/components/BasicInfo.vue"
import AppendOnlyList from "@/components/AppendOnlyList.vue"
import Locations from "@/components/Locations.vue"
import Items from "@/components/Items.vue"
import AddOns from "@/components/AddOns.vue"
import Travelers from "@/components/Travelers.vue"
import Spinner from "@/components/Spinner.vue"
import ModalAreYouSure from "@/components/ModalAreYouSure.vue"

export default {
  components: {
    BasicInfo,
    AppendOnlyList,
    Locations,
    Items,
    AddOns,
    Travelers,
    Spinner,
    ModalAreYouSure
  },
  props: {
    noEditRequest: {
      required: false,
      type: Boolean
    }
  },
  data() {
    return {
      trip: null,
      tripId: this.$route.params.id,
      releaseEdit: true,
      openModalLeave: false,
      leaveInfo: "To leave this trip, you must assign all the items in this trip which you are in charge of to someone else."
    }
  },
  computed: {
    canLeave() {
      const ind = this.tripItems.findIndex(item => item.userId == this.getAuthUserId)
      return ind == -1
    },
    tripInfo() {
      if(!this.specificTrip) {
        return null
      }
      else {
        return {
          tripId: this.specificTrip.tripId,
          name: this.specificTrip.name,
          description: this.specificTrip.description,
          from: this.specificTrip.from,
          to: this.specificTrip.to
        }
      }
    },
    ...mapGetters({
      tripAdditionalInfo: 'getTripAdditionalInfo',
      isDataLoaded: 'getIsDataLoaded',
      specificTrip: 'getSpecificTrip',
      getAuthUserId: 'getAuthUserId',
      accommodationTypes: 'getAccommodationTypes',
      hasEditRights: 'getHasEditRights',
      leftTrip: 'getLeftTrip',
      tripItems: 'getSpecificTripItems'
    })
  },
  watch: {
    hasEditRights(newValue, oldValue) {
      if(newValue == false) {
        this.$travelPlanHub.JoinTripGroup(parseInt(this.tripId))
        console.log("joined trip group")
      }
    },
    leftTrip(newValue, oldValue) {
      if(newValue) {
        this.setLeftTrip(false)
        this.$router.push("/trips")
      }
    }
  },
  methods: {
    showToast(imgSrc, titleText, text) {
      const h = this.$createElement
      const toastTitle = h('div', {}, [
        h('img', { style: "width: 20px; height: 20px; margin-right:20px;", attrs: {
          src: require("../assets/" + imgSrc)
        } }),
        h('span', {}, titleText)
      ])

      this.$bvToast.toast(text, {
        title: [toastTitle],
        autHideDelay: 4000,
        variant: 'info'
      })
    },
    backToTrips() {
      this.$router.push("/trips")
    },
    leaveTrip() {
      let payload = {
        tripId: this.tripId,
        userId: this.getAuthUserId
      }
      this.$store.dispatch("putRemoveTripMember", payload)
    },
    addMember() {
      this.releaseEdit = false
      this.$router.push({
        name: "PageAddMember", 
        params: {
          id: this.tripId, 
          type: "trip"
        }
      })
    },
    onGetEditRights() {
      this.$travelPlanHub.LeaveTripGroup(parseInt(this.tripId))
      this.setEditRights(true)
      this.showToast('edit_item.png', 'You are the editor!', 'You have just been given the edit rights for this trip.')
    },
    onLoseEditRights() {
      this.setEditRights(false)
      this.$router.push({
        name: "PageMyTrips",
        params: {
          toastInfo: {
            imgSrc: 'edit_item.png',
            titleText: 'No longer the editor...',
            text: 'You have just lost the edit rights for this trip, due to long inactivity.'
          }
        }
      })
    },
    onPageLeave() {
      if(this.hasEditRights) {
        if(this.releaseEdit) {
          this.$store.dispatch('releaseEditRights', {
            tripId: this.tripId
          })
        }
      }
      else {
        this.$travelPlanHub.LeaveTripGroup(parseInt(this.tripId))
        this.$store.dispatch('cancelEditRequest', {
          tripId: this.tripId,
          userId: this.getAuthUserId
        })
      }
      this.$travelPlanHub.$off('ChangeVotable')
      this.$travelPlanHub.$off('EditRightsNotification')
      this.$travelPlanHub.$off('LostEditRightsNotification')
      this.setSpecificTrip(null)
      window.removeEventListener('beforeunload', this.leavePage)
    },
    leavePage(event) {
      event.preventDefault()
      console.log("Leave page")
      this.onPageLeave()
      event.returnValue = ''
    },
    onVotableChanged(newVotable) {
      this.changeVotable(newVotable)
    },
    ...mapMutations({
      setSpecificTrip: 'setSpecificTrip',
      setVotables: 'setVotables',
      setLeftTrip: 'setLeftTrip',
      changeVotable: 'replaceVotable',
      setEditRights: 'setHasEditRights'
    })
  },
  created() {
    const that = this
    this.setEditRights(null)
    window.addEventListener('beforeunload', this.leavePage)
    this.setVotables([])
    
    this.$store.dispatch('fillSpecificTrip', {tripId: this.tripId})

    if(!this.noEditRequest) {
      this.$store.dispatch('requestTripEdit', {
        tripId: this.tripId,
        userId: this.getAuthUserId
      })
    }
    
    this.$store.dispatch('fillTripLocations', {
      tripId: this.tripId
    }).then(() => {
      this.$store.dispatch('fillTripAddOns', {
        tripId: this.tripId
      }).then(() => {
        this.$store.dispatch('fillAvailableDecorations', {
          tripId: this.tripId
        })
      })
    })

    this.$travelPlanHub.$on('ChangeVotable', this.onVotableChanged)
    this.$travelPlanHub.$on('EditRightsNotification', this.onGetEditRights)
    this.$travelPlanHub.$on('LostEditRightsNotification', this.onLoseEditRights)

    if(!this.accommodationTypes)
      this.$store.dispatch('fillAccommodationTypes')
  },
  beforeDestroy() {
    this.onPageLeave()
  }
}
</script>

<style scoped>

.main-wrap {
  display: flex;
  flex-direction: column;
}

.a-row {
  display: flex;
  flex-direction: row;
  justify-content: space-between;
  border-radius: 20px;
  border-radius: 20px;
  margin: 30px 5%;
  align-items: center;
  box-shadow: 5px 10px 10px lightgray;
}

.primary-row {
  border-top: 30px solid lightskyblue;
  border-bottom: 30px solid lightskyblue;
}

.back-btn {
  margin-left: 20px;
  margin-bottom: 10px;
  width: fit-content;
  background-color: lightskyblue;
  border-color: lightskyblue;
  color: black;
}

.back-btn:hover {
  background-color: rgb(104, 185, 235);
  border-color: rgb(104, 185, 235);
  color: black;
}

.info {
  height: 20px;
  width: 20px;
  margin-left: 10px;
}

.basic-info {
  flex-grow:1; 
  display:flex; 
  flex-direction:column; 
  width:fit-content;
}

.leave-trip {
  display: flex;
  align-items: center;
  margin-left: 19px;
  margin-bottom: 10px;
  width: fit-content;
}

.action-img {
  height:20px; 
  width:20px;
  cursor: pointer;
  margin-right: 10px;
}

.justify-btn {
  display: flex;
  flex-wrap: nowrap;
  align-items: center;
}
</style>