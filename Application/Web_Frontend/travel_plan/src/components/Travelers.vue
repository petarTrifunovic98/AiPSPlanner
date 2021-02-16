<template>
  <div  v-if="specificTrip" class="main-wrap">
    <div style="font-weight: bold; font-size: 25px;">
      Travelers:
    </div>
    <div class="travelers">
      <img src="../assets/left-arrow.svg" class="arrows" @click="rotateTravelers(-1)" v-if="canRotateLeft">
      <div v-for="traveler in travelersPortion" :key="traveler.userId">
        <TravelerBox :travelerProp="traveler"/>
      </div>
      <img src="../assets/right-arrow.svg" class="arrows" @click="rotateTravelers(1)" v-if="canRotateRight">
    </div>
  </div>
  <Spinner v-else />
</template>

<script>
import { mapGetters, mapMutations } from "vuex"
import TravelerBox from "@/components/TravelerBox.vue"
import Spinner from "@/components/Spinner.vue"

export default {
  components: {
    TravelerBox,
    Spinner
  },
  data() {
    return {
      travelersShown: 3,
      firstTravelerInd: 0
    }
  },
  computed: {
    ...mapGetters({
      isDataLoaded: 'getIsDataLoaded',
      tripTravelers: 'getSpecificTripTravelers',
      specificTrip: 'getSpecificTrip'
    }),
    travelersPortion() {
      if(!this.tripTravelers)
        return null
      else
        return this.tripTravelers.slice(this.firstTravelerInd, this.firstTravelerInd + this.travelersShown)
    },
    canRotateLeft() {
      if(this.tripTravelers.length <= this.travelersShown)
        return false
      return this.firstTravelerInd > 0
    },
    canRotateRight() {
      if(this.tripTravelers.length <= this.travelersShown)
        return false
      return (this.firstTravelerInd + this.travelersShown - 1) < this.tripTravelers.length - 1
    }
  },
  methods: {
    rotateTravelers(factor) {
      if(factor > 0 && (this.firstTravelerInd + this.travelersShown - 1 + factor) <= this.tripTravelers.length)
        this.firstTravelerInd ++
      else if((this.firstTravelerInd + factor) >= 0)
        this.firstTravelerInd --

    },
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

.main-wrap {
  margin: 20px;
  flex-grow: 1;
}

.travelers {
  display: flex;
  flex-direction: row;
  border: 2px solid lightskyblue;
  border-radius: 10px;
  padding: 10px 10px;
  width: fit-content;
}

.arrows {
  height: 30px;
  width: 30px;
  align-self: center;
  margin: 0px 5px;
  padding: 3px;
  cursor: pointer;
  border: 1px solid white;
}

.arrows:hover {
  border: 1px lightgray solid;
  border-radius: 8px;
}


</style>