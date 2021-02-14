<template>
  <div class="col-12 wrapper">
    <div>
      <b-card
        tag="article"
        style="max-width: 20rem;"
        class="mb-2"
      >
        <b-card-title>
          <span v-if="!inEditMode">{{accommodation.name}}</span>
          <input type="text" v-model="editingAccommodation.name" v-else>
        </b-card-title>
        <b-card-sub-title>
          <span v-if="!inEditMode">{{accommodation.type}}</span> <!-- promeniti na select -->
          <input type="text" v-model="editingAccommodation.type" v-else>
        </b-card-sub-title>
        <b-card-text>
          <span v-if="!inEditMode">{{accommodation.description}}</span>
          <textarea v-else v-model="editingAccommodation.description"></textarea>
        </b-card-text>
        <b-card-text>
          <span v-if="!inEditMode">{{accommodation.address}}</span>
          <input type="text" v-model="editingAccommodation.address" v-else>
        </b-card-text>
        <b-card-text>
          <span v-if="!inEditMode">{{accommodation.from | showTime}} - {{accommodation.to | showTime}}</span>
          <input type="date" v-if="inEditMode" v-model="editingAccommodation.from">
          <input type="date" v-if="inEditMode" v-model="editingAccommodation.to"> 
        </b-card-text>
        <button type="button" class="btn btn-primary dugme" v-if="hasEditRights && !inEditMode" @click="toggleEditMode"> Edit </button>
        <button type="button" class="btn btn-primary dugme" v-if="inEditMode" @click="saveEdit"> Save </button>
        <button type="button" class="btn btn-primary dugme" v-if="inEditMode" @click="cancelEdit"> Cancel </button>
      </b-card>
    </div>
  </div>
</template>

<script>
import { mapGetters, mapMutations } from "vuex"

export default {
  props: {
    accommodationProp: {
      required: true
    },
    tripId: {
      required: true,
      type: Number
    }
  },
  data() {
    return {
      accommodationsLoaded: false,
      accommodationsOpen: false,
      inEditMode: false,
      editingAccommodation: JSON.parse(JSON.stringify(this.accommodationProp))
    }
  },
  computed: {
    accommodation() {
      return this.accommodationProp
    },
    ...mapGetters({
      hasEditRights: 'getHasEditRights'
    })
  },
  methods: {
    toggleEditMode() {
      this.inEditMode = !this.inEditMode
    },
    cancelEdit() {
      this.editingAccommodation = JSON.parse(JSON.stringify(this.accommodationProp))
      this.toggleEditMode()
    },
    saveEdit() {
      this.editingAccommodation.tripId = this.tripId
      this.$store.dispatch('putEditAccommodation', this.editingAccommodation)
      this.$travelPlanHub.$emit('EditAccommodation', this.editingAccommodation)
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