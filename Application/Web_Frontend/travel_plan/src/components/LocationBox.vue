<template>
  <div class="wrapper">
    <div :class="['big-margins', isChosen ? 'chosen': '']">
      <b-card-header class="header-lvl-1 common-header">
        <span :class="canChoose ? 'choosable' : ''" @click="chooseLocation" v-if="!inEditMode && !modeAddNew">{{location.name}}</span>
        <b-form-input type="text" v-model="editingLocation.name" v-else style="width:fit-content;" placeholder="Enter location name..."></b-form-input>
        <div>
          <img src="../assets/edit_item.png" v-b-popover.hover.top="'Edit location'" class="action-img" v-if="hasEditRights && !inEditMode && !modeAddNew" @click="toggleEditMode">
          <button type="button" class="btn btn-primary dugme" v-if="inEditMode || modeAddNew" @click="modeAddNew ? addNew() : saveEdit()" :disabled="saveDisabled"  v-text="modeAddNew ? 'Add' : 'Save'"></button>
          <button type="button" class="btn btn-primary dugme" v-if="inEditMode" @click="cancelEdit"> Cancel </button>
          <img src="../assets/delete_item.png" v-b-popover.hover.top="'Delete location'" class="action-img" v-if="hasEditRights && !inEditMode && !modeAddNew" @click="openModalDelete = true">
        </div>
      </b-card-header>
      <b-card-body class="big-card">
        <b-card-text style="margin-bottom: 20px;">
          <span v-if="!inEditMode && !modeAddNew">{{location.description}}</span>
          <b-form-textarea v-else v-model="editingLocation.description" rows="3" no-resize placeholder="Enter location description..."></b-form-textarea>
        </b-card-text>
        <div class="common-header" style="justify-content:left;" v-if="!inEditMode">
          <img src="../assets/placeholder.svg" style="height: 20px; width: 20px; margin-right: 10px">
          <a :href="mapURL" target="_blank" v-if="!modeAddNew">
            View location on Google Maps
          </a>
          <div v-else style="width:100%;">
            <!-- <b-form-input 
              :type="'number'" :max="latitudeAbsMax" :min="-latitudeAbsMax" 
              :state="Math.abs(editingLocation.latitude) > latitudeAbsMax ? false : null"
              v-model="editingLocation.latitude" placeholder="Latitude...">
            </b-form-input>
            <b-form-input 
              :type="'number'" :max="longitudeAbsMax" :min="-longitudeAbsMax" 
              :state="Math.abs(editingLocation.longitude) > longitudeAbsMax ? false : null"
              v-model="editingLocation.longitude" placeholder="Longitude...">
            </b-form-input> -->
            <div class="map-toggle" @click="openModalMap=true">Open map to choose a location </div>
            <div> Currently chosen: {{mapLocationName}} </div>
            <ModalMap 
              :startingLatLng="startingLatLng" :startingLocationName="mapLocationName" v-if="openModalMap" 
              @close="openModalMap = false" @ok="getLocationFromMap"
            />
          </div>
        </div>
        <b-card-text class="common-header" style="margin-top: 20px; justify-content:left; max-width:100%;">
          <img src="../assets/calendar.svg" style="height: 20px; width: 20px; margin-right: 10px">
          <span v-if="!inEditMode && !modeAddNew">{{location.from | showTime}} - {{location.to | showTime}}</span>
          <div style="display:flex;" v-else>
            <b-form-datepicker 
              v-model="editingLocation.from" style="width:fit-content;" size="sm" class="mb-2"
              :date-format-options="{ year: 'numeric', month: 'short', day: '2-digit' }"
              :date-disabled-fn="isDateDisabled" :min="specificTrip.from" :max="specificTrip.to">
            </b-form-datepicker>
            <span style="text-align:center; margin: 3px 3px 0px 3px;"> - </span>
            <b-form-datepicker 
              v-model="editingLocation.to" style="width:fit-content;" size="sm" class="mb-2"
              :date-format-options="{ year: 'numeric', month: 'short', day: '2-digit' }"
              :date-disabled-fn="isDateDisabled" :min="specificTrip.from" :max="specificTrip.to">
            </b-form-datepicker>
          </div>
        </b-card-text>
        <b-card-text v-if="!modeAddNew && !inEditMode && myVotable" style="display:flex; flex-direction:column;">
          <div class="vote-div">
            <img src="../assets/like.png" class="vote-pic" >
            <span style="font-size: 20px; color:green;"> {{myVotable.positiveVotes}} </span>
          </div>
          <div class="vote-div">
            <img src="../assets/dislike.png" class="vote-pic">
            <span style="font-size: 20px; color:red;"> {{myVotable.negativeVotes}} </span>
          </div>
        </b-card-text>
        <button 
          type="button" class="btn btn-primary dugme" @click="openModalVotes = true" 
          v-if="!modeAddNew && !inEditMode" style="margin-bottom:10px;" v-text="hasEditRights ? 'Vote or view votes' : 'View votes'">
        </button>
        <div v-if="accommodationsOpen">
          <div style="margin: 30px 0px 10px 0px; font-weight: bold; font-size: 20px;" v-if="accommodations && accommodations.length > 0">
            Accommodations:
          </div>
          <div style="margin: 30px 0px 10px 0px; font-weight: bold; font-style:italic; font-size: 20px;" v-else>
            No accommodations
          </div>
          <div style="display:flex; flex-wrap:wrap;">
            <div v-for="accommodation in accommodations" :key="accommodation.accommodationId">
              <AccommodationBox :accommodationProp="accommodation" :tripId="location.tripId" :modeAddNew="false"/>
            </div>
          </div>
        </div>
        <div style="display:flex; flex-direction:column; width:fit-content;">
          <button type="button" class="btn btn-primary dugme" @click="showAccommodations" v-if="!accommodationsOpen && !modeAddNew"> View accommodations </button>
          <button type="button" class="btn btn-primary dugme" @click="hideAccommodations" v-else-if="!modeAddNew"> Hide accommodations </button>
        </div>
      </b-card-body>
    </div>
    <ModalAreYouSure 
      :naslov="'Delete location'"
      :tekst="'Are you sure you want to delete this location?'"
      @close="openModalDelete = false" @yes="deleteOne" v-if="openModalDelete"
    />
    <ModalVotes 
      :votableId="location.votable.votableId" :canVote="hasEditRights"
      @close="openModalVotes = false" v-if="openModalVotes"
    />
  </div>
</template>

<script>
import { mapGetters, mapMutations } from "vuex"
import AccommodationBox from "@/components/AccommodationBox.vue"
import ModalAreYouSure from "@/components/ModalAreYouSure.vue"
import ModalVotes from "@/components/ModalVotes.vue"
import ModalMap from "@/components/ModalMap.vue"

export default {
  components: {
    AccommodationBox,
    ModalAreYouSure,
    ModalVotes,
    ModalMap
  },
  props: {
    locationProp: {
      required: false
    },
    modeAddNew: {
      required: true,
      type: Boolean
    },
    chosenLocationId: {
      required: false,
      type: Number
    },
    canChoose: {
      required: false,
      type: Boolean
    }
  },
  data() {
    return {
      accommodationsLoaded: false,
      accommodationsOpen: false,
      inEditMode: false,
      editingLocation: this.modeAddNew ? 
        { name: "", description: "", latitude: 43.32129607887601, longitude: 21.897650399832497, from: "", to: ""} : 
        JSON.parse(JSON.stringify(this.locationProp)),
      startingLatLng: { lat: 43.32129607887601, lng: 21.897650399832497 },
      mapLocationName: "Niš, Serbia",
      openModalDelete: false,
      openModalVotes: false,
      openModalMap: false,
      latitudeAbsMax: 90,
      longitudeAbsMax: 180
    }
  },
  computed: {
    location() {
      return this.locationProp
    },
    accommodations() {
      return this.locationProp.accommodations
    },
    mapURL() {
      return 'https://www.google.com/maps/search/?api=1&query=' + this.locationProp.latitude + "," + this.locationProp.longitude
    },
    saveDisabled() {
      if(this.modeAddNew || this.inEditMode) {
        if(this.editingLocation.name == "" || this.editingLocation.description == "" ||
          this.editingLocation.from == "" || this.editingLocation.to == "" || 
          Math.abs(this.editingLocation.latitude) > this.latitudeAbsMax || 
          Math.abs(this.editingLocation.longitude) > this.longitudeAbsMax || 
          (new Date(this.editingLocation.to) < new Date(this.editingLocation.from)))
          return true
        else 
          return false
      }
      else 
        return false
    },
    isChosen() {
      if(!this.modeAddNew && !this.inEditMode && (this.chosenLocationId == this.locationProp.locationId))
        return true
      return false
    },
    myVotable() {
      return this.votables.find(v => v.votableId == this.locationProp.votable.votableId)
    },
    ...mapGetters({
      hasEditRights: 'getHasEditRights',
      tripId: 'getSpecificTripId',
      votables: 'getVotables',
      specificTrip: 'getSpecificTrip'
    })
  },
  methods: {
    getLocationFromMap(location) {
      this.editingLocation.latitude = location.latLng.lat
      this.editingLocation.longitude = location.latLng.lng
      this.startingLatLng = {
        lat: this.editingLocation.latitude,
        lng: this.editingLocation.longitude
      }
      this.mapLocationName = location.name
      this.openModalMap = false
    },
    isDateDisabled(string, date) {
      if(this.modeAddNew)
        return false
      return !this.$store.getters.getIsLocationDateAvailable(date, this.location.locationId)
    },
    showAccommodations() {
      if(!this.accommodationsLoaded) {
        this.$store.dispatch('fillLocationAccommodations', {
          locationId: this.location.locationId
        }).then(() => {
          this.accommodationsLoaded = true
          this.accommodationsOpen = true
        })
      }
      else {
        this.accommodationsOpen = true
      }
    },
    hideAccommodations() {
      this.accommodationsOpen = false
    },
    toggleEditMode() {
      if(!this.inEditMode && !this.accommodationsLoaded) {
        this.$store.dispatch('fillLocationAccommodations', {
          locationId: this.location.locationId
        }).then(() => {
          this.accommodationsLoaded = true
        })
      }
      this.inEditMode = !this.inEditMode
      this.$emit("chosenLocation", -1)
    },
    cancelEdit() {
      this.editingLocation = JSON.parse(JSON.stringify(this.locationProp))
      this.toggleEditMode()
    },
    saveEdit() {
      this.$store.dispatch('putEditLocation', this.editingLocation)
      this.$travelPlanHub.$emit('EditLocation', this.editingLocation)
      this.toggleEditMode()
    },
    addNew() {
      this.editingLocation.latitude = parseFloat(this.editingLocation.latitude)
      this.editingLocation.longitude = parseFloat(this.editingLocation.longitude)
      this.editingLocation.tripId = this.tripId
      this.$store.dispatch('postAddLocation', this.editingLocation)
      this.$emit("addedNew", "addedNew")
      this.restoreEditingLocation()
    },
    deleteOne() {
      let payload = {
        tripId: this.tripId,
        locationId: this.locationProp.locationId
      }
      this.$store.dispatch('deleteLocation', payload)
      this.$travelPlanHub.$emit('RemoveLocation', payload.locationId)
      this.$emit("chosenLocation", -1)
    },
    restoreEditingLocation() {
      this.editingLocation = { name: "", description: "", latitude: "", longitude: "", from: "", to: ""}
    },
    chooseLocation() {
      if(this.canChoose)
        this.$emit("chosenLocation", this.locationProp.locationId)
    }
  }
}
</script>

<style scoped>
.rounded-image {
  border-radius: 60px;
  border: 2px solid grey;
  height:48px;
  width:48px;
  object-fit:cover;
  margin-bottom: 10px;
}

.wrapper {
  width: 100%;
}

.header-lvl-1 {
  font-size: 20px;
  font-weight: bold;
  background-color: lightskyblue;
}

.header-lvl-2 {
  font-size: 15px;
  padding: 5px;
  font-weight: bold;
  background-color: lightgreen;
}

.header-lvl-3 {
  font-size: 15px;
  padding: 5px;
  font-weight: bold;
  background-color: lightgoldenrodyellow;
}

.common-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  flex-wrap: nowrap ;
}

.card-body {
  border: 2px solid lightskyblue;
  border-top: hidden;
  border-bottom-left-radius: 10px;
  border-bottom-right-radius: 10px;
}

.card-header {
  border: 2px lightskyblue solid;
  border-bottom: hidden;
  border-top-left-radius: 10px!important;
  border-top-right-radius: 10px!important;
}

.btn {
  margin: 0px;
  margin-left: 8px;
  padding: 2px 5px;
}

.small-card {
  padding: 5px;
}

.big-margins {
  margin: 10px 20px;
}

.small-margins {
  margin-bottom: 5px;
  margin-right: 10px;
}

.card-text {
  max-width: 250px;
}

.action-img {
  height:20px; 
  width:20px; 
  margin-left:10px; 
  cursor: pointer;
}

.choosable:hover {
  cursor: pointer;
  color: blue;
}

.chosen {
  background-color: rgb(185, 255, 185);
  border-radius: 10px;
}

.vote-pic {
  height: 30px; 
  width: 30px; 
  margin-right: 15px;
}

.vote-div {
  display:flex;
  align-items: center;
  margin-bottom: 10px;
}

.map-toggle {
  color: #17a2b8;
  cursor: pointer;
}

.map-toggle:hover {
  text-decoration: underline;
}
</style>