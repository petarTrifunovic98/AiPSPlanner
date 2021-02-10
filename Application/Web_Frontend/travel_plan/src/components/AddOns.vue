<template>
  <div class="col-12 wrapper">
    <div v-for="addOn in tripAddOns" :key="addOn.addOnId">
      <AddOnBox :addOnProp="addOn" :tripId="tripId" />
    </div>
  </div>
</template>

<script>
import { mapGetters, mapMutations } from "vuex"
import AddOnBox from "@/components/AddOnBox.vue"

export default {
  props: {
    tripId: {
      required: true,
      type: Number
    }
  },
  components: {
    AddOnBox
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
      removeAddOn: 'removeAddOnsFromTrip'
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
  }
}
</script>

<style scoped>
.wrapper {
  width:100%;
}
</style>