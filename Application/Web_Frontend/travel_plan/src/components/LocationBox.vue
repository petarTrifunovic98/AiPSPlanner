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
        <div class="common-header" style="justify-content:left;">
          <img src="../assets/placeholder.svg" style="height: 20px; width: 20px; margin-right: 10px">
          <a :href="mapURL" target="_blank" v-if="!modeAddNew">
            View location on Google Maps
          </a>
          <div v-else>
            <b-form-input :type="'text'" v-model="editingLocation.latitude" placeholder="Latitude..."></b-form-input>
            <b-form-input :type="'text'" v-model="editingLocation.longitude" placeholder="Longitude..."></b-form-input>
          </div>
        </div>
        <b-card-text class="common-header" style="margin-top: 20px; justify-content:left; max-width:100%;">
          <img src="../assets/calendar.svg" style="height: 20px; width: 20px; margin-right: 10px">
          <span v-if="!inEditMode && !modeAddNew">{{location.from | showTime}} - {{location.to | showTime}}</span>
          <div style="display:flex;" v-else>
            <b-form-datepicker 
              v-model="editingLocation.from" style="width:fit-content;" size="sm" class="mb-2"
              :date-format-options="{ year: 'numeric', month: 'short', day: '2-digit' }">
            </b-form-datepicker>
            <span style="text-align:center; margin: 3px 3px 0px 3px;"> - </span>
            <b-form-datepicker 
              v-model="editingLocation.to" style="width:fit-content;" size="sm" class="mb-2"
              :date-format-options="{ year: 'numeric', month: 'short', day: '2-digit' }">
            </b-form-datepicker>
          </div>
        </b-card-text>
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
        <button type="button" class="btn btn-primary dugme" @click="showAccommodations" v-if="!accommodationsOpen && !modeAddNew"> View accommodations </button>
        <button type="button" class="btn btn-primary dugme" @click="hideAccommodations" v-else-if="!modeAddNew"> Hide accommodations </button>
      </b-card-body>
    </div>
    <ModalAreYouSure 
      :naslov="'Delete location'"
      :tekst="'Are you sure you want to delete this location?'"
      @close="openModalDelete = false" @yes="deleteOne" v-if="openModalDelete"
    />
  </div>
</template>

<script>
import { mapGetters, mapMutations } from "vuex"
import AccommodationBox from "@/components/AccommodationBox.vue"
import ModalAreYouSure from "@/components/ModalAreYouSure.vue"

export default {
  components: {
    AccommodationBox,
    ModalAreYouSure
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
        { name: "", description: "", latitude: "", longitude: "", from: "", to: ""} : 
        JSON.parse(JSON.stringify(this.locationProp)),
      openModalDelete: false
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
      if(this.modeAddNew) {
        if(this.editingLocation.name == "" || this.editingLocation.description == "" || 
          this.editingLocation.latitude == "" || this.editingLocation.longitude == "" || 
          this.editingLocation.from == "" || this.editingLocation.to == "")
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
    ...mapGetters({
      hasEditRights: 'getHasEditRights',
      tripId: 'getSpecificTripId'
    })
  },
  methods: {
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
</style>