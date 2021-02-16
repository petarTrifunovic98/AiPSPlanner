<template>
  <div class="wrapper">
    <div class='big-margins'>
      <b-card-header class="header-lvl-1 common-header">
        <span v-if="!inEditMode">{{location.name}}</span>
        <b-form-input type="text" v-model="editingLocation.name" v-else style="width:fit-content;" placeholder="Enter location name..."></b-form-input>
        <div>
          <button type="button" class="btn btn-primary dugme" v-if="hasEditRights && !inEditMode" @click="toggleEditMode"> Edit </button>
          <button type="button" class="btn btn-primary dugme" v-if="inEditMode" @click="saveEdit"> Save </button>
          <button type="button" class="btn btn-primary dugme" v-if="inEditMode" @click="cancelEdit"> Cancel </button>
        </div>
      </b-card-header>
      <b-card-body class="big-card">
        <b-card-text style="margin-bottom: 20px;">
          <span v-if="!inEditMode">{{location.description}}</span>
          <b-form-textarea v-else v-model="editingLocation.description" rows="3" no-resize placeholder="Enter location description..."></b-form-textarea>
        </b-card-text>
        <div class="common-header" style="justify-content:left;">
          <img src="../assets/placeholder.svg" style="height: 20px; width: 20px; margin-right: 10px">
          <a :href="mapURL" target="_blank">
            View location on Google Maps
          </a>
        </div>
        <b-card-text class="common-header" style="margin-top: 20px; justify-content:left; max-width:100%;">
          <img src="../assets/calendar.svg" style="height: 20px; width: 20px; margin-right: 10px">
          <span v-if="!inEditMode">{{location.from | showTime}} - {{location.to | showTime}}</span>
          <div style="display:flex;" v-if="inEditMode">
            <b-form-datepicker 
              v-model="editingLocation.from" style="width:fit-content;" 
              :date-format-options="{ year: 'numeric', month: 'short', day: '2-digit' }">
            </b-form-datepicker>
            <span style="text-align:center; margin: 3px 3px 0px 3px;"> - </span>
            <b-form-datepicker 
              v-model="editingLocation.to" style="width:fit-content;"
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
          <div v-for="accommodation in accommodations" :key="accommodation.accommodationId">
            <AccommodationBox :accommodationProp="accommodation" :tripId="location.tripId"/>
          </div>
        </div>
        <button type="button" class="btn btn-primary dugme" @click="showAccommodations" v-if="!accommodationsOpen"> View accommodations </button>
        <button type="button" class="btn btn-primary dugme" @click="hideAccommodations" v-else> Hide accommodations </button>
      </b-card-body>
    </div>
  </div>
</template>

<script>
import { mapGetters, mapMutations } from "vuex"
import AccommodationBox from "@/components/AccommodationBox.vue"

export default {
  components: {
    AccommodationBox
  },
  props: {
    locationProp: {
      required: true
    }
  },
  data() {
    return {
      accommodationsLoaded: false,
      accommodationsOpen: false,
      inEditMode: false,
      editingLocation: JSON.parse(JSON.stringify(this.locationProp))
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
    ...mapGetters({
      hasEditRights: 'getHasEditRights'
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
    },
    cancelEdit() {
      this.editingLocation = JSON.parse(JSON.stringify(this.locationProp))
      this.toggleEditMode()
    },
    saveEdit() {
      this.$store.dispatch('putEditLocation', this.editingLocation)
      this.$travelPlanHub.$emit('EditLocation', this.editingLocation)
      this.toggleEditMode()
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


</style>