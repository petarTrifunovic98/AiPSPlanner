<template>
  <div class="col-12 wrapper" v-if="specificTrip">
    <div>
      <b-card
        tag="article"
        style="max-width: 20rem;"
        class="mb-2"
      >
        <b-card-title>
          <span v-if="!inEditMode">{{tripInfo.name}}</span>
          <input type="text" v-model="editingInfo.name" v-else>
        </b-card-title>
        <b-card-text>
          <span v-if="!inEditMode">{{tripInfo.description}}</span>
          <textarea v-else v-model="editingInfo.description"></textarea>
        </b-card-text>
        <b-card-text>
          <span v-if="!inEditMode">{{tripInfo.from | showTime}} - {{tripInfo.to | showTime}}</span>
          <input type="date" v-if="inEditMode" v-model="editingInfo.from">
          <input type="date" v-if="inEditMode" v-model="editingInfo.to"> 
        </b-card-text>
        <button type="button" class="btn btn-primary dugme" v-if="hasEditRights && !inEditMode" @click="toggleEditMode"> Edit </button>
        <button type="button" class="btn btn-primary dugme" v-if="inEditMode" @click="saveEdit"> Save </button>
        <button type="button" class="btn btn-primary dugme" v-if="inEditMode" @click="cancelEdit"> Cancel </button>
      </b-card>
    </div>
  </div>
  <Spinner v-else />
</template>

<script>
import { mapGetters, mapMutations } from "vuex"
import Spinner from "@/components/Spinner.vue"

export default {
  props: {
    tripInfo: {
      type: Object,
      required: true
    }
  },
  components: {
    Spinner
  },
  data() {
    return {
      inEditMode: false,
      editingInfo: JSON.parse(JSON.stringify(this.tripInfo))
    }
  },
  computed: {
    ...mapGetters({
      isDataLoaded: 'getIsDataLoaded',
      hasEditRights: 'getHasEditRights',
      specificTrip: 'getSpecificTrip'
    })
  },
  methods: {
    onTripInfoEdited(tripInfo) {
      this.setSpecificTripBasicInfo(tripInfo)
    },
    toggleEditMode() {
      this.inEditMode = !this.inEditMode
    },
    cancelEdit() {
      this.editingInfo = JSON.parse(JSON.stringify(this.$store.getters["getSpecificTripBasicInfo"]))
      this.toggleEditMode()
    },
    saveEdit() {
      this.$store.dispatch('putEditTripInfo', this.editingInfo)
      this.onTripInfoEdited(this.editingInfo)
      this.toggleEditMode()
    },
    ...mapMutations({
      setSpecificTripBasicInfo: 'setSpecificTripBasicInfo'
    })
  },
  created() {
    this.$travelPlanHub.$on('EditTripInfo', this.onTripInfoEdited)
  },
  destroyed() {
    this.$travelPlanHub.$off('EditTripInfo')
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
</style>