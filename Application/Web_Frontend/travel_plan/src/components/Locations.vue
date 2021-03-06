<template>
  <div class="wrapper" v-if="tripLocations">
    <div class="section-title">
      Locations
    </div>
    <div class="btn-container">
      <button 
        style="margin-left: 20px;" v-if="hasEditRights" type="button" 
        class="btn btn-primary custom-btn" @click="toggleLocationForm" >
        <img v-if="!addLocationFormOpen" src="../assets/add.svg" class="action-img">
        <span v-text="addLocationFormOpen ? 'Hide new location form' : 'Add a new location'"></span>
      </button>
      <button 
        style="margin-left: 20px;" v-if="hasEditRights" type="button" 
        class="btn btn-primary custom-btn" @click="toggleAccommodationForm">
        <img v-if="!addAccommodationFormOpen" src="../assets/add.svg" class="action-img">
        <span v-text="addAccommodationFormOpen ? 'Hide new accommodaiton form' : 'Add a new accommodation'"></span>
      </button>
    </div>
    <div style="width: fit-content;" v-if="addLocationFormOpen">
      <LocationBox :modeAddNew="true"/>
    </div>
    <div style="width: fit-content;" v-if="addAccommodationFormOpen">
      <AccommodationBox :modeAddNew="true" :chosenLocationId="chosenLocId"/>
    </div>
    <div style="display:flex; flex-wrap: wrap;">
      <div v-for="location in tripLocations" :key="location.locationId">
        <LocationBox 
          :locationProp="location" :modeAddNew="false" :canChoose="addAccommodationFormOpen"
          :chosenLocationId="chosenLocId" @chosenLocation="setChosenLocId"/>
      </div>
    </div>
  </div>
  <Spinner v-else />
</template>

<script>
import { mapGetters, mapMutations } from "vuex"
import LocationBox from "@/components/LocationBox.vue"
import AccommodationBox from "@/components/AccommodationBox.vue"
import Spinner from "@/components/Spinner.vue"

export default {
  components: {
    LocationBox,
    AccommodationBox,
    Spinner
  },
  data() {
    return {
      addLocationFormOpen: false,
      addAccommodationFormOpen: false,
      chosenLocId: -1
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
    toggleLocationForm() {
      if(!this.addLocationFormOpen) {
        this.addAccommodationFormOpen = false
        this.chosenLocId = -1
      }
      this.addLocationFormOpen = !this.addLocationFormOpen
    },
    toggleAccommodationForm() {
      if(!this.addAccommodationFormOpen)
        this.addLocationFormOpen = false
      else
        this.chosenLocId = -1
      this.addAccommodationFormOpen = !this.addAccommodationFormOpen
    },
    setChosenLocId(id) {
      this.chosenLocId = id
    },
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
    onAddAccommodationPicture(picture) {
      this.addAccommodationPicture(picture)
    },
    onRemoveaccommodationPicture(pictureInfo) {
      this.removeAccommodationPicture(pictureInfo)
    },
    ...mapMutations({
      addLocation: 'addLocationToSpecificTrip',
      editLocation: 'replaceEditedLocation',
      removeLocation: 'removeLocationFromSpecificTrip',
      addAccommodation: 'addAccommodationToLocation',
      editAccommodation: 'editAccommodationForLocation',
      removeAccommodation: 'removeAccommodationFromLocation',
      addAccommodationPicture: 'addPictureToAccommodation',
      removeAccommodationPicture: 'removePictureFromAccommodation',
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
    this.$travelPlanHub.$on('AddAccommodationPicture', this.onAddAccommodationPicture)
    this.$travelPlanHub.$on('RemoveAccommodationPicture', this.onRemoveaccommodationPicture)
  },
  destroyed() {
    this.$travelPlanHub.$off('AddLocation')
    this.$travelPlanHub.$off('EditLocation')
    this.$travelPlanHub.$off('RemoveLocation')
    this.$travelPlanHub.$off('AddAccommodation')
    this.$travelPlanHub.$off('EditAccommodation')
    this.$travelPlanHub.$off('RemoveAccommodation')
    this.$travelPlanHub.$off('AddAccommodationPicture')
    this.$travelPlanHub.$off('RemoveAccommodationPicture')
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

.custom-btn {
  background-color: lightskyblue;
  border-color: lightskyblue;
  display: flex;
  flex-wrap: nowrap;
  align-items: center;
  color: black;
}

.custom-btn:hover {
  background-color: rgb(104, 185, 235);
  border-color: rgb(104, 185, 235);
  color: black;
}

.custom-btn:focus {
  background-color: rgb(104, 185, 235);
  border-color: rgb(104, 185, 235);
  color: black;
  outline: none;
  box-shadow: none;
}

.action-img {
  height:20px; 
  width:20px;
  cursor: pointer;
  margin-right: 10px;
}

.btn-container {
  display: flex;
}
</style>