<template>
  <div v-if="isDataLoaded && specificTrip" class="main-wrap">
    <div class="a-row primary-row">
      <BasicInfo :tripInfo="tripInfo" v-if="tripInfo"/>
      <Travelers/>
    </div>
    <div class="a-row" >
      <AddOns :tripId="parseInt(tripId)"/>
    </div>
    <div class="a-row" >
      <Locations/>
    </div>
    <div class="a-row" >
      <Items/>
    </div>
  </div>
  <Spinner v-else />
</template>

<script>
import * as signalR from '@aspnet/signalr'
import { mapGetters, mapMutations } from "vuex"
import BasicInfo from "@/components/BasicInfo.vue"
import AppendOnlyList from "@/components/AppendOnlyList.vue"
import Locations from "@/components/Locations.vue"
import Items from "@/components/Items.vue"
import AddOns from "@/components/AddOns.vue"
import Travelers from "@/components/Travelers.vue"
import Spinner from "@/components/Spinner.vue"

export default {
  components: {
    BasicInfo,
    AppendOnlyList,
    Locations,
    Items,
    AddOns,
    Travelers,
    Spinner
  },
  props: {
    tripProp: {
      required: false
    }
  },
  data() {
    return {
      trip: this.tripProp,
      tripId: this.$route.params.id
    }
  },
  computed: {
    ...mapGetters({
      tripAdditionalInfo: 'getTripAdditionalInfo',
      isDataLoaded: 'getIsDataLoaded',
      specificTrip: 'getSpecificTrip',
      getAuthUserId: 'getAuthUserId',
      accommodationTypes: 'getAccommodationTypes',
      hasEditRights: 'getHasEditRights'
    }),
    tripInfo() {
      if(!this.specificTrip) {
        return null
      }
      else {
        return {
          tripId: this.specificTrip.tripId,
          name: this.specificTrip.name,
          description: this.specificTrip.description,
          from: this.specificTrip.from,
          to: this.specificTrip.to
        }
      }
    }
    // tripId() {
    //   return this.$route.params.id
    // }
  },
  watch: {
    hasEditRights(newValue, oldValue) {
      if(newValue == false) {
        this.$travelPlanHub.JoinTripGroup(parseInt(this.tripId))
        console.log("joined")
      }
    }
  },
  methods: {
    onTripInfoEdited(trip) {
      this.trip.name = trip.name
      this.trip.description = trip.description
      this.trip.from = trip.from
      this.trip.to = trip.to
    },
    leavePage(event) {
      event.preventDefault()
      console.log("Leave page")
      if(this.hasEditRights) {
        this.$store.dispatch('releaseEditRights', {
          tripId: this.tripId
        })
      }
      else {
        this.$store.dispatch('cancelEditRequest', {
          tripId: this.tripId,
          userId: this.getAuthUserId
        })
      }
      event.returnValue = ''
      window.removeEventListener('beforeunload', this.leavePage)
    },
    ...mapMutations({
      setSpecificTrip: 'setSpecificTrip'
    })
  },
  created() {
    const that = this
    window.addEventListener('beforeunload', this.leavePage)

    if(this.tripProp) {
      this.setSpecificTrip(this.tripProp)
    }
    else 
      this.$store.dispatch('fillSpecificTrip', {tripId: this.tripId})

    this.$store.dispatch('requestTripEdit', {
      tripId: this.tripId,
      userId: this.getAuthUserId
    }).then(() => {
      this.$store.dispatch('fillTripLocations', {
        tripId: this.tripId
      }).then(() => {
        this.$store.dispatch('fillTripAddOns', {
          tripId: this.tripId
        }).then(() => {
          this.$store.dispatch('fillAvailableDecorations', {
            tripId: this.tripId
          })
        })
      })
    })

    if(!this.accommodationTypes)
      this.$store.dispatch('fillAccommodationTypes')
  },
  beforeDestroy() {
    if(this.hasEditRights) {
      this.$store.dispatch('releaseEditRights', {
        tripId: this.tripId
      })
    }
    else {
      this.$travelPlanHub.LeaveTripGroup(this.tripId)
      this.$store.dispatch('cancelEditRequest', {
        tripId: this.tripId,
        userId: this.getAuthUserId
      })
    }
    this.setSpecificTrip(null)
    window.removeEventListener('beforeunload', this.leavePage)
  }
}
</script>

<style scoped>

.main-wrap {
  display: flex;
  flex-direction: column;
}

.a-row {
  display: flex;
  flex-direction: row;
  justify-content: space-between;
  border-radius: 20px;
  border-radius: 20px;
  margin: 30px 5%;
  align-items: center;
  box-shadow: 5px 10px 10px lightgray;
}

.primary-row {
  border-top: 30px solid lightskyblue;
  border-bottom: 30px solid lightskyblue;
}

</style>