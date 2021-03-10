<template>
  <div class="wrapper" v-if="tripAddOns">
    <div :class="['section-title', addAddOnFormOpen ? 'choosable' : '']" @click="chooseTrip">
      Add-ons
    </div>
    <button 
      style="margin-left: 20px;" v-if="hasEditRights" type="button" 
      class="btn btn-primary custom-btn" @click="toggleAddOnForm">
      <img v-if="!addAddOnFormOpen" src="../assets/add.svg" class="action-img">
      <span v-text="addAddOnFormOpen ? 'Hide new add-on form' : 'Add a new add-on'"></span>
    </button>
    <div style="width: fit-content;" v-if="addAddOnFormOpen && canShowForm">
      <AddOnBox 
        :modeAddNew="true" :level="newAddOnLevel" :chosenAddOn="chosenAddOn"
        :availableDecorations="availableDecorations"/>
    </div>
    <div style="width: fit-content;" v-else-if="addAddOnFormOpen">
      <div class="message"> Choose another add-on! </div>
    </div>
    <div style="display:flex; flex-wrap: wrap;">
      <div v-for="addOn in tripAddOns" :key="addOn.addOnId">
        <AddOnBox 
          :addOnProp="addOn" :modeAddNew="false" :level="1" :canChoose="addAddOnFormOpen"
          :chosenAddOn="chosenAddOn" @addOnChosen="setChosenAddOn"/>
      </div>
    </div>
  </div>
  <Spinner v-else />
</template>

<script>
import { mapGetters, mapMutations } from "vuex"
import AddOnBox from "@/components/AddOnBox.vue"
import Spinner from "@/components/Spinner.vue"

export default {
  props: {
    tripId: {
      required: true,
      type: Number
    }
  },
  components: {
    AddOnBox,
    Spinner
  },
  data() {
    return {
      addAddOnFormOpen: false,
      chosenAddOn: null
    }
  },
  computed: {
    availableDecorations() {
      return this.$store.getters.getAvailableDecorations(this.chosenAddOn)
    },
    newAddOnLevel() {
      if(!this.chosenAddOn)
        return 1
      else if(this.chosenAddOn.lvl1DependId == 0 && this.chosenAddOn.lvl2DependId ==0)
        return 2
      else
        return 3
    },
    canShowForm() {
      if(this.availableDecorations && this.availableDecorations.length > 0)
        return true
      return false
    },
    ...mapGetters({
      isDataLoaded: 'getIsDataLoaded',
      tripAddOns: 'getTripAddOns',
      hasEditRights: 'getHasEditRights',
      allAvailableDecorations: 'getAvailableDecorations',
      addOnWatch: 'getAddOnWatch'
    })
  },
  watch: {
    addOnWatch(newValue, oldValue) {
      if(newValue) {
        this.$forceUpdate()
        this.$store.state.addOnWatch = null
      }
    }
  },
  methods: {
    toggleAddOnForm() {
      this.addAddOnFormOpen = !this.addAddOnFormOpen
      this.chosenAddOn = null
    },
    setChosenAddOn(addOn) {
      this.chosenAddOn = addOn
    },
    chooseTrip() {
      if(this.addAddOnFormOpen)
        this.chosenAddOn = null
    },
    onAddAddOn(addOn) {
      this.addAddOn(addOn)
      //this.$forceUpdate()
    },
    onEditAddOn(addOn) {
      this.editAddOn(addOn)
      //this.$forceUpdate()
    },
    onRemoveAddOn(addOnList) {
      this.removeAddOn(addOnList)
      //this.$forceUpdate()
    },
    ...mapMutations({
      addAddOn: 'addAddOnToTrip',
      editAddOn: 'editAddOnForTrip',
      removeAddOn: 'removeAddOnsFromTrip',
      setAddOns: 'setTripAddOns'
    })
  },
  created() {
    this.$travelPlanHub.$on('AddAddOn', this.onAddAddOn)
    this.$travelPlanHub.$on('EditAddOn', this.onEditAddOn)
    this.$travelPlanHub.$on('RemoveAddOn', this.onRemoveAddOn)
  },
  destroyed() {
    this.$travelPlanHub.$off('AddAddOn')
    this.$travelPlanHub.$off('EditAddOn')
    this.$travelPlanHub.$off('RemoveAddOn')
    this.setAddOns(null)
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

.message {
  font-size: 20px;
  font-weight: bold;
  font-style: italic;
  margin: 20px;
  padding: 10px;
  border: 2px lightskyblue solid;
  border-radius: 10px;
}

.choosable:hover {
  cursor: pointer;
  color: blue;
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
</style>