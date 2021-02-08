<template>
  <div class="col-12 wrapper">
    <div v-for="location in tripLocations" :key="location.locationId">
      <LocationBox :locationProp="location"/>
    </div>
  </div>
</template>

<script>
import { mapGetters, mapMutations } from "vuex"
import LocationBox from "@/components/LocationBox.vue"

export default {
  components: {
    LocationBox
  },
  computed: {
    ...mapGetters({
      isDataLoaded: 'getIsDataLoaded',
      tripLocations: 'getSpecificTripLocations'
    })
  },
  methods: {
    onAddLocation(location) {
      this.addLocation(location)
    },
    onEditLocation(location) {
      this.editLocation(location)
    },
    onRemoveLocation(locationId) {
      this.removeLocation(locationId)
    },
    ...mapMutations({
      addLocation: 'addLocationToSpecificTrip',
      editLocation: 'replaceEditedLocation',
      removeLocation: 'removeLocationFromSpecificTrip'
    })
  },
  created() {
    this.$travelPlanHub.$on('AddLocation', this.onAddLocation)
    this.$travelPlanHub.$on('EditLocation', this.onEditLocation)
    this.$travelPlanHub.$on('RemoveLocation', this.onRemoveLocation)
  },
  destroyed() {
    this.$travelPlanHub.$off('AddLocation')
    this.$travelPlanHub.$off('EditLocation')
    this.$travelPlanHub.$off('RemoveLocation')
  }
}
</script>

<style scoped>
.wrapper {
  width:100%;
}
</style>