<template>
  <div class="wrapper">
    <div :class="modeAddNew ? 'big-margins' : 'small-margins'">
      <b-card-header class="header-lvl-2 common-header">
        <span v-if="!inEditMode && !modeAddNew">{{accommodation.name}}</span>
        <b-form-input type="text" v-model="editingAccommodation.name" v-else style="" placeholder="Enter accommodation name..."></b-form-input>
        <div style="display: flex;">
          <img src="../assets/edit_item.png" v-b-popover.hover.top="'Edit accommodation'" class="action-img" v-if="hasEditRights && !inEditMode && !modeAddNew" @click="toggleEditMode">
          <button type="button" class="btn btn-primary dugme" v-if="inEditMode || modeAddNew" @click="modeAddNew ? addNew() : saveEdit()" :disabled="saveDisabled" v-text="modeAddNew ? 'Add' : 'Save'"></button>
          <button type="button" class="btn btn-primary dugme" v-if="inEditMode" @click="cancelEdit"> Cancel </button>
          <img src="../assets/delete_item.png" v-b-popover.hover.top="'Delete accommodation'" class="action-img" v-if="hasEditRights && !inEditMode && !modeAddNew" @click="openModalDelete = true">
        </div>
      </b-card-header>
      <b-card-body class="small-card">
        <b-card-sub-title>
          <span v-if="!inEditMode && !modeAddNew">{{accommodation.type}}</span> <!-- promeniti na select -->
          <input type="text" v-model="editingAccommodation.type" v-else>
        </b-card-sub-title>
        <b-card-text>
          <span v-if="!inEditMode && !modeAddNew">{{accommodation.description}}</span>
          <b-form-textarea v-else v-model="editingAccommodation.description" rows="3" no-resize placeholder="Enter accommodation description..."></b-form-textarea>
        </b-card-text>
        <b-card-text class="common-header" style="justify-content:left; max-width:none; width: 100%;">
          <img src="../assets/house.svg" style="height:20px; width:20px; margin-right:10px;">
          <span v-if="!inEditMode && !modeAddNew">{{accommodation.address}}</span>
          <b-form-input type="text" v-model="editingAccommodation.address" v-else placeholder="Enter accommodation address.."></b-form-input>
        </b-card-text>
        <b-card-text class="common-header" style="margin-top: 20px; justify-content:left; max-width:100%;">
          <img src="../assets/calendar.svg" style="height: 20px; width: 20px; margin-right: 10px">
          <span v-if="!inEditMode && !modeAddNew">{{accommodation.from | showTime}} - {{accommodation.to | showTime}}</span>
          <div style="display:flex;" v-else>
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
        <b-card-text v-if="modeAddNew" class="common-header" style="justify-content:center; max-width:none; width:100%;">
          <div class="icon-simulation" v-b-popover.hover.top="'Click on the name of the location you wish to add the accommodation to'"> ? </div>
        </b-card-text>
      </b-card-body>
    </div>
    <ModalAreYouSure 
      :naslov="'Delete accommodation'"
      :tekst="'Are you sure you want to delete this accommodation?'"
      @close="openModalDelete = false" @yes="deleteOne" v-if="openModalDelete"
    />
  </div>
</template>

<script>
import { mapGetters, mapMutations } from "vuex"
import ModalAreYouSure from "@/components/ModalAreYouSure.vue"

export default {
  components: {
    ModalAreYouSure
  },
  props: {
    accommodationProp: {
      required: false
    },
    modeAddNew: {
      required: true,
      type: Boolean
    },
    chosenLocationId: {
      required: false,
      type: Number
    }
  },
  data() {
    return {
      inEditMode: false,
      editingAccommodation: this.modeAddNew ? 
        { name: "", type: "0", description: "", address: "", from: "", to: "" } :
        JSON.parse(JSON.stringify(this.accommodationProp)),
      openModalDelete: false
    }
  },
  computed: {
    accommodation() {
      return this.accommodationProp
    },
    saveDisabled() {
      if(this.modeAddNew) {
        if(this.editingAccommodation.name == "" || this.editingAccommodation.description == ""
          || this.editingAccommodation.address == "" || this.editingAccommodation.from == ""
          || this.editingAccommodation.to == "" || this.chosenLocationId == -1)
          return true
        else
          return false
      }
      else
        return false;
    },
    ...mapGetters({
      hasEditRights: 'getHasEditRights',
      tripId: 'getSpecificTripId'
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
    },
    addNew() {
      this.editingAccommodation.locationId = this.chosenLocationId
      this.$store.dispatch('postAddAccommodation', {
        tripId: this.tripId,
        newAccommodation: this.editingAccommodation
      })
      this.$emit("addedNew", "addedNew")
      this.restoreEditingAccommodation()
    },
    deleteOne() {
      let payload = {
        tripId: this.tripId,
        accRemovalInfo: {
          locationId: this.accommodationProp.locationId,
          accommodationId: this.accommodationProp.accommodationId
        }
      }
      this.$store.dispatch('deleteAccommodation', payload)
      this.$travelPlanHub.$emit('RemoveAccommodation', payload.accRemovalInfo)
    },
    restoreEditingAccommodation() {
      this.editingAccommodation = { name: "", type: "0", description: "", address: "", from: "", to: "" }
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
  background-color: white;
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

.icon-simulation {
  width: fit-content;
  font-size: 20px;
  margin: 0px 10px;
  border: 2px solid green;
  padding: 0px 9px;
  border-radius: 30px;
  cursor:context-menu;
  color: green;
}
</style>