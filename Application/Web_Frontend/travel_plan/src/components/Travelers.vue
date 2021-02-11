<template>
  <div class="col-12 wrapper">
    <div style="margin-top:30px; font-weight: bold; font-size: 30px;">
      Travelers:
    </div>
    <div v-for="traveler in tripTravelers" :key="traveler.userId">
      <div>{{traveler.name}} {{traveler.lastName}}</div>
    </div>
  </div>
</template>

<script>
import { mapGetters, mapMutations } from "vuex"
import ItemBox from "@/components/ItemBox.vue"

export default {
  components: {
    ItemBox
  },
  computed: {
    ...mapGetters({
      isDataLoaded: 'getIsDataLoaded',
      tripTravelers: 'getSpecificTripTravelers'
    })
  },
  methods: {
    onAddTraveler(item) {
      this.addTraveler(item)
    },
    onRemoveTraveler(itemId) {
      this.removeTraveler(itemId)
    },
    ...mapMutations({
      addTraveler: 'addTravelerToSpecificTrip',
      removeTraveler: 'removeTravelerFromSpecificTrip'
    })
  },
  created() {
    this.$travelPlanHub.$on('AddMemberToTrip', this.onAddTraveler)
    this.$travelPlanHub.$on('RemoveUserFromTrip', this.onRemoveTraveler)
  },
  destroyed() {
    this.$travelPlanHub.$off('AddMemberToTrip')
    this.$travelPlanHub.$off('RemoveUserFromTrip')
  }
}
</script>

<style scoped>
.wrapper {
  width:100%;
}
</style>