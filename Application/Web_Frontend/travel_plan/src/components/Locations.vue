<template>
  <div class="wrapper" v-if="tripLocations">
    <div class="section-title">
      Locations:
    </div>
    <button style="margin-left: 20px;" v-if="hasEditRights" type="button" class="btn btn-primary dugme" @click="addFormOpen = !addFormOpen" v-text="addFormOpen ? 'Hide form' : 'Add a new location'"></button>
    <div style="width: fit-content;" v-if="addFormOpen">
      <LocationBox :modeAddNew="true"/>
    </div>
    <div style="display:flex; flex-wrap: wrap;">
      <div v-for="location in tripLocations" :key="location.locationId">
        <LocationBox :locationProp="location" :modeAddNew="false"/>
      </div>
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
  data() {
    return {
      addFormOpen: false
    }
  },
  computed: {
    ...mapGetters({
      isDataLoaded: 'getIsDataLoaded',
      tripLocations: 'getSpecificTripLocations',
      specificTrip: 'getSpecificTrip',
      hasEditRights: 'getHasEditRights'
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

.section-title {
  width:100%; 
  border:3px solid lightskyblue; 
  border-top-left-radius:20px; 
  border-top-right-radius:20px; 
  border-radius: 20px;
  margin-bottom:10px; 
  font-weight: bold; 
  font-size: 20px;
  padding: 0px 20px;
}
</style>