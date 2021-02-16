<template>
  <div class="wrapper">
    <div class="small-margins">
      <b-card-header class="header-lvl-2 common-header">
        <span v-if="!inEditMode">{{accommodation.name}}</span>
        <b-form-input type="text" v-model="editingAccommodation.name" v-else style="width:fit-content;" placeholder="Enter accommodation name..."></b-form-input>
        <div>
          <button type="button" class="btn btn-primary dugme" v-if="hasEditRights && !inEditMode" @click="toggleEditMode"> Edit </button>
          <button type="button" class="btn btn-primary dugme" v-if="inEditMode" @click="saveEdit"> Save </button>
          <button type="button" class="btn btn-primary dugme" v-if="inEditMode" @click="cancelEdit"> Cancel </button>
        </div>
      </b-card-header>
      <b-card-body class="small-card">
        <b-card-sub-title>
          <span v-if="!inEditMode">{{accommodation.type}}</span> <!-- promeniti na select -->
          <input type="text" v-model="editingAccommodation.type" v-else>
        </b-card-sub-title>
        <b-card-text>
          <span v-if="!inEditMode">{{accommodation.description}}</span>
          <b-form-textarea v-else v-model="editingAccommodation.description" rows="3" no-resize placeholder="Enter accommodation description..."></b-form-textarea>
        </b-card-text>
        <b-card-text class="common-header" style="justify-content:left;">
          <img src="../assets/house.svg" style="height:20px; width:20px; margin-right:10px;">
          <span v-if="!inEditMode">{{accommodation.address}}</span>
          <b-form-input type="text" v-model="editingAccommodation.address" v-else ></b-form-input>
        </b-card-text>
        <b-card-text class="common-header" style="margin-top: 20px; justify-content:left; max-width:100%;">
          <img src="../assets/calendar.svg" style="height: 20px; width: 20px; margin-right: 10px">
          <span v-if="!inEditMode">{{accommodation.from | showTime}} - {{accommodation.to | showTime}}</span>
          <div style="display:flex;" v-if="inEditMode">
            <b-form-datepicker 
              v-model="editingAccommodation.from" style="width:fit-content;" 
              :date-format-options="{ year: 'numeric', month: 'short', day: '2-digit' }">
            </b-form-datepicker>
            <span style="text-align:center; margin: 3px 3px 0px 3px;"> - </span>
            <b-form-datepicker 
              v-model="editingAccommodation.to" style="width:fit-content;"
              :date-format-options="{ year: 'numeric', month: 'short', day: '2-digit' }">
            </b-form-datepicker>
          </div>
        </b-card-text>
      </b-card-body>
    </div>
  </div>
</template>

<script>
import { mapGetters, mapMutations } from "vuex"

export default {
  props: {
    accommodationProp: {
      required: true
    },
    tripId: {
      required: true,
      type: Number
    }
  },
  data() {
    return {
      accommodationsLoaded: false,
      accommodationsOpen: false,
      inEditMode: false,
      editingAccommodation: JSON.parse(JSON.stringify(this.accommodationProp))
    }
  },
  computed: {
    accommodation() {
      return this.accommodationProp
    },
    ...mapGetters({
      hasEditRights: 'getHasEditRights'
    })
  },
  methods: {
    toggleEditMode() {
      this.inEditMode = !this.inEditMode
    },
    cancelEdit() {
      this.editingAccommodation = JSON.parse(JSON.stringify(this.accommodationProp))
      this.toggleEditMode()
    },
    saveEdit() {
      this.editingAccommodation.tripId = this.tripId
      this.$store.dispatch('putEditAccommodation', this.editingAccommodation)
      this.$travelPlanHub.$emit('EditAccommodation', this.editingAccommodation)
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