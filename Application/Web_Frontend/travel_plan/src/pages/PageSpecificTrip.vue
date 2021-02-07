<template>
  <div>
    <h1> {{trip.name}} </h1>
    <h2> {{trip.description}} </h2>
  </div>
</template>

<script>
import * as signalR from '@aspnet/signalr'


export default {
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
  methods: {
    onTripInfoEdited(trip) {
      this.trip = trip
    }
  },
  created() {
    this.$travelPlanHub.JoinTripGroup(this.tripProp.tripId)
    this.$travelPlanHub.$on('EditTripInfo', (trip) => this.trip = trip)
  },
  destroyed() {
    this.$travelPlanHub.$off('EditTripInfo')
  }
}
</script>

<style scoped>

</style>