<template>
  <div class="wrapper" v-if="tripAddOns">
    <div class="section-title">
      Add-ons
    </div>
    <div style="display:flex; flex-wrap: wrap;">
      <div v-for="addOn in tripAddOns" :key="addOn.addOnId">
        <AddOnBox :addOnProp="addOn" :tripId="tripId" :level="1" />
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
  computed: {
    ...mapGetters({
      isDataLoaded: 'getIsDataLoaded',
      tripAddOns: 'getTripAddOns'
    })
  },
  methods: {
    onAddAddOn(addOn) {
      this.addAddOn(addOn)
      this.$forceUpdate()
    },
    onEditAddOn(addOn) {
      this.editAddOn(addOn)
      this.$forceUpdate()
    },
    onRemoveAddOn(addOnList) {
      this.removeAddOn(addOnList)
      this.$forceUpdate()
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
</style>