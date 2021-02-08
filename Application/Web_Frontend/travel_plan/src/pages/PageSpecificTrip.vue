<template>
  <div v-if="isDataLoaded" class="row">
    <div class="col-4" >
      <BasicInfo/>
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
import { mapGetters, mapMutations } from "vuex"

export default {
  components: {
    BasicInfo,
    AppendOnlyList,
    Locations,
    Items
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
      getAuthUserId: 'getAuthUserId'
    })
  },
  methods: {
    onTripInfoEdited(trip) {
      this.trip.name = trip.name
      this.trip.description = trip.description
      this.trip.from = trip.from
      this.trip.to = trip.to
    },
    ...mapMutations({
      setSpecificTrip: 'setSpecificTrip'
    })
  },
  created() {
    if(this.tripProp) {
      this.setSpecificTrip(this.tripProp)
    }

    this.$store.dispatch('requestTripEdit', {
      tripId: this.tripProp.tripId,
      userId: this.getAuthUserId
    }).then(() => {
      this.$store.dispatch('fillTripLocations', {
        tripId: this.tripProp.tripId
      })
    })
    
    this.$travelPlanHub.JoinTripGroup(this.tripProp.tripId)
  },
  destroyed() {
  }
}
</script>

<style scoped>

</style>