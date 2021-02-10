<template>
  <div v-if="isDataLoaded" class="row">
    <div class="col-4" >
      <BasicInfo/>
      <AddOns :tripId="trip.tripId"/>
    </div>
    <div class="col-4" >
      <Locations/>
    </div>
    <div class="col-4" >
      <Items/>
    </div>
  </div>
  <div v-else>
    <div>Loading...</div>
  </div>
</template>

<script>
import * as signalR from '@aspnet/signalr'
import BasicInfo from "@/components/BasicInfo.vue"
import AppendOnlyList from "@/components/AppendOnlyList.vue"
import Locations from "@/components/Locations.vue"
import Items from "@/components/Items.vue"
import AddOns from "@/components/AddOns.vue"
import { mapGetters, mapMutations } from "vuex"

export default {
  components: {
    BasicInfo,
    AppendOnlyList,
    Locations,
    Items,
    AddOns
  },
  props: {
    tripProp: {
      required: true
    }
  },
  data() {
    return {
      trip: this.tripProp
    }
  },
  computed: {
    ...mapGetters({
      tripAdditionalInfo: 'getTripAdditionalInfo',
      isDataLoaded: 'getIsDataLoaded',
      specificTrip: 'getSpecificTrip',
      getAuthUserId: 'getAuthUserId',
      hasEditRights: 'getHasEditRights'
    })
  },
  methods: {
    onTripInfoEdited(trip) {
      this.trip.name = trip.name
      this.trip.description = trip.description
      this.trip.from = trip.from
      this.trip.to = trip.to
    },
    leavePage(event) {
      event.preventDefault()
      console.log("Leave page")
      if(this.hasEditRights) {
        this.$store.dispatch('releaseEditRights', {
          tripId: this.tripProp.tripId
        })
      }
      else {
        this.$store.dispatch('cancelEditRequest', {
          tripId: this.tripProp.tripId,
          userId: this.getAuthUserId
        })
      }
      event.returnValue = ''
      window.removeEventListener('beforeunload', this.leavePage)
    },
    ...mapMutations({
      setSpecificTrip: 'setSpecificTrip'
    })
  },
  created() {
    const that = this
    window.addEventListener('beforeunload', this.leavePage)

    if(this.tripProp) {
      this.setSpecificTrip(this.tripProp)
    }

    this.$store.dispatch('requestTripEdit', {
      tripId: this.tripProp.tripId,
      userId: this.getAuthUserId
    }).then(() => {
      this.$store.dispatch('fillTripLocations', {
        tripId: this.tripProp.tripId
      }).then(() => {
        this.$store.dispatch('fillTripAddOns', {
          tripId: this.tripProp.tripId
        })
      })
    })
    
    this.$travelPlanHub.JoinTripGroup(this.tripProp.tripId)
  },
  destroyed() {
    if(this.hasEditRights) {
      this.$store.dispatch('releaseEditRights', {
        tripId: this.tripProp.tripId
      })
    }
    else {
      this.$store.dispatch('cancelEditRequest', {
        tripId: this.tripProp.tripId,
        userId: this.getAuthUserId
      })
    }
    window.removeEventListener('beforeunload', this.leavePage)
  }
}
</script>

<style scoped>

</style>