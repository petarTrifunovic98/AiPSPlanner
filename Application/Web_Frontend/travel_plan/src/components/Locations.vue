<template>
  <div class="col-12 wrapper" v-if="tripLocations">
    <div style="margin-top:30px; font-weight: bold; font-size: 30px;">
      Locations:
    </div>
    <div v-for="location in tripLocations" :key="location.locationId">
      <LocationBox :locationProp="location"/>
    </div>
  </div>
  <Spinner v-else />
</template>

<script>
import { mapGetters, mapMutations } from "vuex"
import LocationBox from "@/components/LocationBox.vue"
import Spinner from "@/components/Spinner.vue"

export default {
  components: {
    LocationBox,
    Spinner
  },
  computed: {
    ...mapGetters({
      isDataLoaded: 'getIsDataLoaded',
      tripLocations: 'getSpecificTripLocations',
      specificTrip: 'getSpecificTrip'
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
    onAddAccommodation(accommodation) {
      this.addAccommodation(accommodation)
    },
    onEditAccommodation(accommodation) {
      this.editAccommodation(accommodation)
    },
    onRemoveAccommodation(accommodation) {
      this.removeAccommodation(accommodation)
    },
    ...mapMutations({
      addLocation: 'addLocationToSpecificTrip',
      editLocation: 'replaceEditedLocation',
      removeLocation: 'removeLocationFromSpecificTrip',
      addAccommodation: 'addAccommodationToLocation',
      editAccommodation: 'editAccommodationForLocation',
      removeAccommodation: 'removeAccommodationFromLocation',
      setLocations: 'setTripLocations'
    })
  },
  created() {
    this.$travelPlanHub.$on('AddLocation', this.onAddLocation)
    this.$travelPlanHub.$on('EditLocation', this.onEditLocation)
    this.$travelPlanHub.$on('RemoveLocation', this.onRemoveLocation)
    this.$travelPlanHub.$on('AddAccommodation', this.onAddAccommodation)
    this.$travelPlanHub.$on('EditAccommodation', this.onEditAccommodation)
    this.$travelPlanHub.$on('RemoveAccommodation', this.onRemoveAccommodation)
  },
  destroyed() {
    this.$travelPlanHub.$off('AddLocation')
    this.$travelPlanHub.$off('EditLocation')
    this.$travelPlanHub.$off('RemoveLocation')
    this.$travelPlanHub.$off('AddAccommodation')
    this.$travelPlanHub.$off('EditAccommodation')
    this.$travelPlanHub.$off('RemoveAccommodation')
    this.setLocations(null)
  }
}
</script>

<style scoped>
.wrapper {
  width:100%;
}
</style>