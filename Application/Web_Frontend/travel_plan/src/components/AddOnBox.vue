<template>
  <div class="wrapper">
    <div :class="[(level == 1 || modeAddNew) ? 'big-margins' : 'small-margins', isChosen ? 'chosen': 'not-chosen']">
      <b-card-header :class="[headerClasses[level - 1], 'common-header']">
        <span :class="canChoose ? 'choosable' : ''" v-if="!modeAddNew" @click="addOnChosen">{{addOn.type}}</span>
        <b-form-select v-model="selectedDecoration" size="sm" style="width:fit-content;" v-else-if="modeAddNew">
          <b-form-select-option v-for="(dec, ind) in availableDecorationsOptions" :key="ind" :value="ind">
            {{dec}}
          </b-form-select-option>
        </b-form-select>
        <div>
          <img src="../assets/edit_item.png" v-b-popover.hover.top="'Edit add-on'" class="action-img" v-if="hasEditRights && !inEditMode && !modeAddNew" @click="toggleEditMode">
          <button type="button" class="btn btn-primary custom-btn" v-if="inEditMode || modeAddNew" @click="modeAddNew ? addNew() : saveEdit()" :disabled="saveDisabled"  v-text="modeAddNew ? 'Add' : 'Save'"></button>
          <button type="button" class="btn btn-primary custom-btn" v-if="inEditMode" @click="cancelEdit"> Cancel </button>
          <img src="../assets/delete_item.png" v-b-popover.hover.top="'Delete location'" class="action-img" v-if="hasEditRights && !inEditMode && !modeAddNew" @click="openModalDelete = true">
        </div>
      </b-card-header>
      <b-card-body :class="level == 1 ? 'big-card' : 'small-card'">
        <b-card-text style="margin-bottom: 20px;">
          <span v-if="!inEditMode && !modeAddNew">{{addOn.description}}</span>
          <b-form-textarea v-model="editingAddOn.description" v-else rows="3" no-resize placeholder="Enter add-on description..."></b-form-textarea>
        </b-card-text>
        <b-card-text class="common-header" style="justify-content: left;">
          <img src="../assets/cash.svg" style="height: 20px; width:20px; margin-right: 10px;" >
          <span v-if="!inEditMode && !modeAddNew">{{addOn.price}}</span>
          <b-form-input :type="'number'" v-model="editingAddOn.price" v-else style="width: 80px;" placeholder="Enter price..."></b-form-input>
        </b-card-text>
        <b-card-text v-if="modeAddNew" class="common-header" style="justify-content:center; max-width:none; width:100%;">
          <div class="icon-simulation" v-b-popover.hover.top='"Click on the title of the add-on you wish to decorate. " + 
          "Click on \"Add-ons\" if you wish to decorate the trip itself."'> ? </div>
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
          type="button" class="btn btn-primary custom-btn" 
          @click="openModalVotes = true" v-if="!modeAddNew && !inEditMode" >
          <img src="../assets/vote.svg" class="btn-img">
          <span v-text="hasEditRights ? 'Vote or view votes' : 'View votes'"></span>
        </button>
        <div style="display:flex; flex-wrap:wrap;" v-if="!modeAddNew">
          <div v-for="lvl1AddOn in addOn.lvl1" :key="lvl1AddOn.addOnId">
            <AddOnBox :modeAddNew="false" :canChoose="canChoose" :chosenAddOn="chosenAddOn" :addOnProp="lvl1AddOn" :tripId="tripId" :level="2" @addOnChosen="propagateAddOnChosen"/>
          </div>
        </div>
        <div style="display:flex; flex-wrap:wrap" v-if="!modeAddNew">
          <div v-for="lvl2AddOn in addOn.lvl2" :key="lvl2AddOn.addOnId">
            <AddOnBox :modeAddNew="false" :canChoose="canChoose" :chosenAddOn="chosenAddOn" :addOnProp="lvl2AddOn" :tripId="tripId" :level="3" @addOnChosen="propagateAddOnChosen"/>
          </div>
        </div>
      </b-card-body>
    </div>
    <ModalAreYouSure 
      :naslov="'Delete add-on'"
      :tekst="'Are you sure you want to delete this add-on?'"
      @close="openModalDelete = false" @yes="deleteOne" v-if="openModalDelete"
    />
    <ModalVotes 
      :votableId="addOn.votable.votableId" :canVote="hasEditRights"
      @close="openModalVotes = false" v-if="openModalVotes"
    />
  </div>
</template>

<script>
import { mapGetters, mapMutations } from "vuex"
import AddOnBox from "@/components/AddOnBox.vue"

export default {
  components: {
  },
  name: "AddOnBox",
  props: {
    addOnProp: {
      required: false
    },
    level: {
      required: true,
      type: Number
    },
    modeAddNew: {
      required: true,
      type: Boolean
    },
    chosenAddOn: {
      required: false
    },
    canChoose: {
      required: false,
      type: Boolean
    },
    availableDecorations: {
      required: false,
      type: Array
    }
  },
  components: {
    AddOnBox
  },
  data() {
    return {
      inEditMode: false,
      editingAddOn: this.modeAddNew ?
        { type: "", description: "", price: "" } :
        JSON.parse(JSON.stringify(this.addOnProp)),
      headerClasses: ['header-lvl-1', 'header-lvl-2', 'header-lvl-3'],
      openModalDelete: false,
      openModalVotes: false,
      selectedDecoration: 0
    }
  },
  computed: {
    addOn() {
      return this.addOnProp
    },
    availableDecorationsOptions() {
      let ret = ['Choose add-on type']
      return ret.concat(this.modeAddNew ? this.availableDecorations : 
        this.$store.getters.getAvailableDecorations(this.addOnProp))
    },
    saveDisabled() {
      if(this.modeAddNew) {
        if(this.editingAddOn.description == "" || this.editingAddOn.price == "" || this.selectedDecoration == 0)
          return true
        else
          return false
      }
      else
        return false
    },
    isChosen() {
      if(!this.modeAddNew && !this.inEditMode && this.chosenAddOn != null &&
      (this.chosenAddOn.addOnId == this.addOnProp.addOnId))
        return true
      return false
    },
    myVotable() {
      return this.votables.find(v => v.votableId == this.addOnProp.votable.votableId)
    },
    ...mapGetters({
      hasEditRights: 'getHasEditRights',
      tripId: 'getSpecificTripId',
      votables: 'getVotables'
    })
  },
  methods: {
    toggleEditMode() {
      this.inEditMode = !this.inEditMode
      this.$emit("addOnChosen", null)
    },
    propagateAddOnChosen(addOn) {
      this.$emit("addOnChosen", addOn)
    },
    cancelEdit() {
      this.editingAddOn = JSON.parse(JSON.stringify(this.addOnProp))
      this.toggleEditMode()
    },
    saveEdit() {
      this.editingAddOn.tripId = this.tripId
      this.$store.dispatch('putEditAddOn', this.editingAddOn)
      this.$travelPlanHub.$emit('EditAddOn', this.editingAddOn)
      this.toggleEditMode()
    },
    addNew() {
      this.editingAddOn.tripId = this.tripId
      this.editingAddOn.type = this.availableDecorationsOptions[this.selectedDecoration]
      if(!this.chosenAddOn) {
        this.editingAddOn.lvl1DependId = 0
        this.editingAddOn.lvl2DependId = 0
      }
      else if(this.chosenAddOn.lvl1DependId != 0) {
        this.editingAddOn.lvl1DependId = this.chosenAddOn.lvl1DependId
        this.editingAddOn.lvl2DependId = this.chosenAddOn.addOnId
      }
      else {
        this.editingAddOn.lvl1DependId = this.chosenAddOn.addOnId
        this.editingAddOn.lvl2DependId = 0
      }
      this.$store.dispatch('postAddAddOn', this.editingAddOn)
      this.$emit("addedNew", "addedNew")
      this.restoreEditingAccommodation()
      this.$forceUpdate()
    },
    deleteOne() {
      let payload = {
        tripId: this.tripId,
        addOnId: this.addOnProp.addOnId
      }
      this.$store.dispatch('deleteAddOn', payload)
      this.$travelPlanHub.$emit('RemoveAddOn', this.addOnProp)
      this.$emit("addOnChosen", null)
    },
    restoreEditingAccommodation() {
      this.editingAddOn = { type: "", description: "", price: "" }
    },
    addOnChosen() {
      if(this.canChoose)
       this.$emit("addOnChosen", this.addOnProp)
    }
  },
  beforeCreate: function() {
    this.$options.components.ModalAreYouSure = require("@/components/ModalAreYouSure.vue").default
    this.$options.components.ModalVotes = require("@/components/ModalVotes.vue").default
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

.not-chosen {
  background-color: white;
  border-radius: 10px;
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