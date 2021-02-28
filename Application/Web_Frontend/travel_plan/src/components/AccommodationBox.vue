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
        <b-card-text>
          <span v-if="!inEditMode && !modeAddNew" style="font-weight:bold;"> {{accommodation.type}}</span>
          <b-form-select v-model="selectedType" size="sm" style="width:fit-content;" v-else>
            <b-form-select-option v-for="(type, ind) in accommodationTypesOptions" :key="ind" :value="ind">
              {{type}}
            </b-form-select-option>
          </b-form-select>
        </b-card-text>
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
              :date-format-options="{ year: 'numeric', month: 'short', day: '2-digit' }"
              :min="dateMin" :max="dateMax" :disabled="chosenLocationId == -1">
            </b-form-datepicker>
            <span style="text-align:center; margin: 3px 3px 0px 3px;"> - </span>
            <b-form-datepicker 
              v-model="editingAccommodation.to" style="width:fit-content;"
              :date-format-options="{ year: 'numeric', month: 'short', day: '2-digit' }"
              :min="dateMin" :max="dateMax" :disabled="chosenLocationId == -1">
            </b-form-datepicker>
          </div>
        </b-card-text>
        <b-card-text v-if="!modeAddNew && !inEditMode && myVotable" style="display:flex; flex-direction:row;">
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
          type="button" class="btn btn-primary custom-btn" @click="openModalVotes = true" 
          v-if="!modeAddNew && !inEditMode" style="margin-bottom:10px;">
          <img src="../assets/vote.svg" class="btn-img">
          <span v-text="hasEditRights ? 'Vote or view votes' : 'View votes'"></span>
        </button>
        <b-card-text v-if="picturesOpen && !modeAddNew">
          <div style="margin: 30px 0px 10px 0px; font-weight: bold; font-size: 20px;" v-if="pictures && pictures.length > 0">
            Pictures:
          </div>
          <div style="margin: 30px 0px 10px 0px; font-weight: bold; font-style:italic; font-size: 20px;" v-else>
            No pictures
          </div>
          <div v-if="pictures" class="img-container">
            <div v-for="(picture, ind) in pictures" :key="picture.accommodationPictureId" class="removable-img">
              <img 
                class="expandable-image" :src="'data:;base64,' + picture.picture" @click="expandPicture(ind)"/>
              <img v-if="hasEditRights" src="../assets/delete_item.png" class="action-img" style="margin: 0px;" v-b-popover.hover.top="'Delete image'" @click="beginDeletePicture(picture.accommodationPictureId)">
            </div>

            <img class="expandable-image" src='@/assets/add.svg' @click="$refs.file.click()" v-if="hasEditRights">
            <input type="file" ref="file" style="display: none" accept="image/*" @change="pictureSelected" v-if="hasEditRights"/>

            <LightBox :media="expandedPictures" v-if="pictureExpanded" :startAt="clickedPicture" @onClosed="pictureExpanded = false"></LightBox>
          </div>
        </b-card-text>
        <b-card-text v-if="modeAddNew" class="common-header" style="justify-content:center; max-width:none; width:100%;">
          <div class="icon-simulation" v-b-popover.hover.top="'Click on the name of the location you wish to add the accommodation to'"> ? </div>
        </b-card-text>
        <div style="display:flex; flex-direction:column; width:fit-content;">
          <button type="button" class="btn btn-primary custom-btn" @click="showPictures" v-if="!picturesOpen && !modeAddNew"> 
            <img src="../assets/image.png" class="btn-img">
            <span> View pictures </span> 
          </button>
          <button type="button" class="btn btn-primary custom-btn" @click="hidePictures" v-else-if="!modeAddNew"> 
            <img src="../assets/image.png" class="btn-img">
            <span> Hide pictures </span>
          </button>
        </div>
      </b-card-body>
    </div>
    <ModalAreYouSure 
      :naslov="'Delete accommodation'"
      :tekst="'Are you sure you want to delete this accommodation?'"
      @close="openModalDelete = false" @yes="deleteOne" v-if="openModalDelete"
    />
    <ModalAreYouSure 
      :naslov="'Delete picture'"
      :tekst="'Are you sure you want to delete this picture?'"
      @close="openModalDeletePicture = false" @yes="deletePicture" v-if="openModalDeletePicture"
    />
    <ModalVotes 
      :votableId="accommodation.votable.votableId" :canVote="hasEditRights"
      @close="openModalVotes = false" v-if="openModalVotes"
    />
  </div>
</template>

<script>
import { mapGetters, mapMutations } from "vuex"
import ModalAreYouSure from "@/components/ModalAreYouSure.vue"
import LightBox from "vue-image-lightbox"
import ModalVotes from "@/components/ModalVotes.vue"

export default {
  components: {
    ModalAreYouSure,
    LightBox,
    ModalVotes
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
      picturesOpen: false,
      picturesLoaded: false,
      clickedPicture: null,
      pictureExpanded: false,
      inEditMode: false,
      editingAccommodation: this.modeAddNew ? 
        { name: "", type: "", description: "", address: "", from: "", to: "" } :
        JSON.parse(JSON.stringify(this.accommodationProp)),
      openModalDelete: false,
      openModalDeletePicture: false,
      openModalVotes: false,
      pictureToDeleteId: -1,
      selectedType: 0
    }
  },
  computed: {
    dateMin() {
      if(this.modeAddNew && this.chosenLocationId != -1) 
        return this.tripLocations.find(loc => loc.locationId == this.chosenLocationId).from
      else if (!this.modeAddNew)
        return this.myLocation.from
      else
        return null
    },
    dateMax() {
      if(this.modeAddNew && this.chosenLocationId != -1) 
        return this.tripLocations.find(loc => loc.locationId == this.chosenLocationId).to
      else if (!this.modeAddNew)
        return this.myLocation.to
      else
        return null
    },
    myLocation() {
      return this.$store.getters.getLocation(this.accommodationProp.locationId)
    },
    accommodation() {
      return this.accommodationProp
    },
    pictures() {
      return this.accommodationProp.pictures
    },
    expandedPictures() {
      return this.pictures.map(pic => {
        return {
          thumb: 'data:;base64,' + pic.picture,
          src: 'data:;base64,' + pic.picture
        }
      })
    },
    saveDisabled() {
      if(this.modeAddNew || this.inEditMode) {
        if(this.editingAccommodation.name == "" || this.editingAccommodation.description == ""
          || this.editingAccommodation.address == "" || this.editingAccommodation.from == ""
          || this.editingAccommodation.to == "" || this.chosenLocationId == -1 || this.selectedType == 0 ||
          (new Date(this.editingAccommodation.to) < new Date(this.editingAccommodation.from)))
          return true
        else
          return false
      }
      else
        return false;
    },
    accommodationTypesOptions() {
      if(!this.accommodationTypes)
        return []
      else {
        let ret = ['Choose accommodation type']
        return ret.concat(this.accommodationTypes)
      }
    },
    myVotable() {
      return this.votables.find(v => v.votableId == this.accommodationProp.votable.votableId)
    },
    ...mapGetters({
      hasEditRights: 'getHasEditRights',
      tripId: 'getSpecificTripId',
      accommodationTypes: 'getAccommodationTypes',
      tripLocations: 'getSpecificTripLocations',
      votables: 'getVotables'
    })
  },
  methods: {
    showPictures() {
      if(!this.picturesLoaded) {
        this.$store.dispatch('fillAccommodationPictures', {
          accommodationId: this.accommodationProp.accommodationId,
          locationId: this.accommodationProp.locationId
        }).then(() => {
          this.picturesLoaded = true
          this.picturesOpen = true
        })
      }
      else {
        this.picturesOpen = true
      }
    },
    hidePictures() {
      this.picturesOpen = false
    },
    expandPicture(index) {
      this.clickedPicture = index
      this.pictureExpanded = true
    },
    pictureSelected(e) {
      const file = e.target.files[0];
      e.target.value = null
      this.addImage(file);
    },
    addImage(file) {
      if(!file.type.match('image*')) {
        console.log('not an image!');
      }
      else {
        const reader = new FileReader();
        reader.onload = (e) => this.addNewPicture(e.target.result);
        reader.readAsDataURL(file);
      }
    },
    toggleEditMode() {
      this.inEditMode = !this.inEditMode
    },
    cancelEdit() {
      this.editingAccommodation = JSON.parse(JSON.stringify(this.accommodationProp))
      this.selectedType = 0
      this.toggleEditMode()
    },
    saveEdit() {
      this.editingAccommodation.tripId = this.tripId
      this.editingAccommodation.type = this.accommodationTypesOptions[this.selectedType]
      this.$store.dispatch('putEditAccommodation', this.editingAccommodation)
      this.$travelPlanHub.$emit('EditAccommodation', this.editingAccommodation)
      this.toggleEditMode()
    },
    addNew() {
      this.editingAccommodation.locationId = this.chosenLocationId
      this.editingAccommodation.type = this.accommodationTypesOptions[this.selectedType]
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
      this.selectedType = 0
    },
    addNewPicture(picture) {
      const splitted = picture.split(',')
      let payload = {
        accommodationId: this.accommodationProp.accommodationId,
        tripId: this.tripId,
        locationId: this.accommodationProp.locaitonId,
        picture: splitted[1]
      }
      this.$store.dispatch('postAddAccommodationPicture', payload)
    },
    beginDeletePicture(pictureId) {
      this.pictureToDeleteId = pictureId
      this.openModalDeletePicture = true
    },
    deletePicture() {
      let payload = {
        tripId: this.tripId,
        deleteInfo: {
          accommodationPictureId: this.pictureToDeleteId,
          picture: null,
          accommodationId: this.accommodationProp.accommodationId,
          locationId: this.accommodationProp.locationId
        }
      }
      this.$store.dispatch('deleteAccommodationPicture', payload)
      this.$travelPlanHub.$emit('RemoveAccommodationPicture', payload.deleteInfo)
      this.openModalDeletePicture = false
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
  margin-bottom: 20px;
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

.expandable-image {
  margin: 5px;
  width: 60px;
  height: 60px;
  border: 1px solid grey;
  border-radius: 5px;
}

.expandable-image:hover {
  cursor: pointer;
  border-color: black;
}

.removable-img {
  display: flex;
  flex-direction: column;
  margin-right: 20px;
  align-items: center;
}

.img-container {
  display: flex;
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
  margin-right: 40px;
}

.custom-btn {
  background-color: lightskyblue;
  border-color: lightskyblue;
  display: flex;
  flex-wrap: nowrap;
  align-items: center;
  color: black;
  padding: 5px 10px;
  margin-left: 0px;
  margin-bottom: 20px;
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

.btn-img {
  height:20px; 
  width:20px;
  cursor: pointer;
  margin-right: 10px;
}
</style>