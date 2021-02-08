<template>
  <div class="col-12 wrapper">
    <div>
      <b-card
        tag="article"
        style="max-width: 20rem;"
        class="mb-2"
      >
        <b-card-title>{{tripInfo.name}}</b-card-title>
        <b-card-text>
          {{tripInfo.description}}
        </b-card-text>
        <b-card-text>
          {{tripInfo.from | showTime}} - {{tripInfo.to | showTime}}
        </b-card-text>
      </b-card>
    </div>
  </div>
</template>

<script>
import { mapGetters, mapMutations } from "vuex"

export default {
  computed: {
    ...mapGetters({
      isDataLoaded: 'getIsDataLoaded',
      tripInfo: 'getSpecificTripBasicInfo'
    })
  },
  methods: {
    onTripInfoEdited(tripInfo) {
      this.setSpecificTripBasicInfo(tripInfo)
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