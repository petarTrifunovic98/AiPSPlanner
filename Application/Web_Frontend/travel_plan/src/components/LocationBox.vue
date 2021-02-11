<template>
  <div class="col-12 wrapper">
    <div>
      <b-card
        tag="article"
        style="max-width: 20rem;"
        class="mb-2"
      >
        <b-card-title>
          <span v-if="!inEditMode">{{location.name}}</span>
          <input type="text" v-model="editingLocation.name" v-else>
        </b-card-title>
        <b-card-text>
          <span v-if="!inEditMode">{{location.description}}</span>
          <textarea v-else v-model="editingLocation.description"></textarea>
        </b-card-text>
        <b-card-text>
          Longitude: {{location.longitude}}
        </b-card-text>
        <b-card-text>
          Latitude: {{location.latitude}}
        </b-card-text>
        <b-card-text>
          <span v-if="!inEditMode">{{location.from | showTime}} - {{location.to | showTime}}</span>
          <input type="date" v-if="inEditMode" v-model="editingLocation.from">
          <input type="date" v-if="inEditMode" v-model="editingLocation.to"> 
        </b-card-text>
        <button type="button" class="btn btn-primary dugme" v-if="hasEditRights && !inEditMode" @click="toggleEditMode"> Edit </button>
        <button type="button" class="btn btn-primary dugme" v-if="inEditMode" @click="saveEdit"> Save </button>
        <button type="button" class="btn btn-primary dugme" v-if="inEditMode" @click="cancelEdit"> Cancel </button>
        <div v-if="accommodationsOpen">
          <div style="margin-top:30px; font-weight: bold; font-size: 30px;">
            Accommodations:
          </div>
          <div v-for="accommodation in accommodations" :key="accommodation.accommodationId">
            <AccommodationBox :accommodationProp="accommodation" :tripId="location.tripId"/>
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
      accommodationsOpen: false,
      inEditMode: false,
      editingLocation: JSON.parse(JSON.stringify(this.locationProp))
    }
  },
  computed: {
    location() {
      return this.locationProp
    },
    accommodations() {
      return this.locationProp.accommodations
    },
    ...mapGetters({
      hasEditRights: 'getHasEditRights'
    })
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
    toggleEditMode() {
      this.inEditMode = !this.inEditMode
    },
    cancelEdit() {
      this.editingLocation = JSON.parse(JSON.stringify(this.locationProp))
      this.toggleEditMode()
    },
    saveEdit() {
      this.$store.dispatch('putEditLocation', this.editingLocation)
      this.toggleEditMode()
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
</style>