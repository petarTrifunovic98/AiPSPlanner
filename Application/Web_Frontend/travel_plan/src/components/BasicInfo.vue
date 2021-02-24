<template>
  <div class="wrapper" v-if="specificTrip">
    <div>
      <div
        tag="article"
      >
        <b-card-title>
          <span v-if="!inEditMode" class="big-text">{{tripInfo.name}}</span>
          <b-form-input type="text" v-model="editingInfo.name" v-else style="width:fit-content;"></b-form-input>
        </b-card-title>
        <b-card-text>
          <span v-if="!inEditMode">{{tripInfo.description}}</span>
          <b-form-textarea v-else v-model="editingInfo.description" rows="3" no-resize style="width:fit-content;"></b-form-textarea>
        </b-card-text>
        <b-card-text style="display:flex; align-items:center;">
          <img src="../assets/calendar.svg" style="height:20px; width:20px; margin-right:10px;">
          <span v-if="!inEditMode">{{tripInfo.from | showTime}} - {{tripInfo.to | showTime}}</span>
          <div style="display:flex;" v-if="inEditMode">
            <b-form-datepicker 
              v-model="editingInfo.from" style="width:fit-content;" :date-disabled-fn="isDateDisabled"
              :date-format-options="{ year: 'numeric', month: 'short', day: '2-digit' }">
            </b-form-datepicker>
            <span style="text-align:center; margin: 3px 3px 0px 3px;"> - </span>
            <b-form-datepicker 
              v-model="editingInfo.to" style="width:fit-content;" :date-disabled-fn="isDateDisabled"
              :date-format-options="{ year: 'numeric', month: 'short', day: '2-digit' }">
            </b-form-datepicker>
          </div>
        </b-card-text>
        <button type="button" class="btn btn-primary dugme" v-if="hasEditRights && !inEditMode" @click="toggleEditMode"> Edit </button>
        <button type="button" class="btn btn-primary dugme" v-if="inEditMode" @click="saveEdit" :disabled="invalidDates"> Save </button>
        <button type="button" class="btn btn-primary dugme" v-if="inEditMode" @click="cancelEdit"> Cancel </button>
      </div>
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
    }),
    invalidDates() {
      return new Date(this.editingInfo.to) < new Date(this.editingInfo.from)
    }
  },
  methods: {
    isDateDisabled(string, date) {
      return !this.$store.getters.getIsTripDateAvailable(date)
    },
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

.wrapper {
  margin: 20px;
  flex-grow: 1;
}

.big-text {
  font-weight: bold;
  font-size: 30px;
}
</style>