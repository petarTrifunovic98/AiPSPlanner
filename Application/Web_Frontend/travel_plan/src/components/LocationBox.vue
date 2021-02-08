<template>
  <div class="col-12 wrapper">
    <div>
      <b-card
        tag="article"
        style="max-width: 20rem;"
        class="mb-2"
      >
        <b-card-title>{{location.name}}</b-card-title>
        <b-card-text>
          {{location.description}}
        </b-card-text>
        <b-card-text>
          Longitude: {{location.longitude}}
        </b-card-text>
        <b-card-text>
          Latitude: {{location.latitude}}
        </b-card-text>
        <b-card-text>
          {{location.from | showTime}} - {{location.to | showTime}}
        </b-card-text>
        <div v-if="accommodationsOpen">
          <div v-for="accommodation in accommodations" :key="accommodation.accommodationId">
            <AccommodationBox :accommodationProp="accommodation"/>
          </div>
        </div>
        <button type="button" class="btn btn-primary dugme" @click="showAccommodations" v-if="!accommodationsOpen"> View accommodations </button>
        <button type="button" class="btn btn-primary dugme" @click="hideAccommodations" v-else> Hide accommodations </button>
      </b-card>
    </div>
  </div>
</template>

<script>
import { mapGetters, mapMutations } from "vuex"
import AccommodationBox from "@/components/AccommodationBox.vue"

export default {
  components: {
    AccommodationBox
  },
  props: {
    locationProp: {
      required: true
    }
  },
  data() {
    return {
      accommodationsLoaded: false,
      accommodationsOpen: false
    }
  },
  computed: {
    location() {
      return this.locationProp
    },
    accommodations() {
      return this.locationProp.accommodations
    }
  },
  methods: {
    showAccommodations() {
      if(!this.accommodationsLoaded) {
        this.$store.dispatch('fillLocationAccommodations', {
          locationId: this.location.locationId
        }).then(() => {
          this.accommodationsLoaded = true
          this.accommodationsOpen = true
        })
      }
      else {
        this.accommodationsOpen = true
      }
    },
    hideAccommodations() {
      this.accommodationsOpen = false
    },
    onAddAccommodation(accommodation) {
      this.addAccommodation(accommodation)
    },
    ...mapMutations({
      addAccommodation: 'addAccommodationToLocation'
    })
  },
  created() {
    this.$travelPlanHub.$on('AddAccommodation', this.onAddAccommodation)
  },
  destroyed() {
    this.$travelPlanHub.$off('AddAccommodation')
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
</style>