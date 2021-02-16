<template>
  <div class="wrapper" v-if="specificTrip">
    <div class="section-title">
      Items:
    </div>
    <div style="display:flex; flex-wrap: wrap;">
      <div v-for="item in tripItems" :key="item.itemId">
        <ItemBox :itemProp="item"/>
      </div>
    </div>
  </div>
  <Spinner v-else />
</template>

<script>
import { mapGetters, mapMutations } from "vuex"
import ItemBox from "@/components/ItemBox.vue"
import Spinner from "@/components/Spinner.vue"

export default {
  components: {
    ItemBox,
    Spinner
  },
  computed: {
    ...mapGetters({
      isDataLoaded: 'getIsDataLoaded',
      tripItems: 'getSpecificTripItems',
      specificTrip: 'getSpecificTrip'
    })
  },
  methods: {
    onAddItem(item) {
      this.addItem(item)
    },
    onEditItem(item) {
      this.editItem(item)
    },
    onRemoveItem(itemId) {
      this.removeItem(itemId)
    },
    ...mapMutations({
      addItem: 'addItemToSpecificTrip',
      editItem: 'replaceEditedItem',
      removeItem: 'removeItemFromSpecificTrip'
    })
  },
  created() {
    this.$travelPlanHub.$on('AddItem', this.onAddItem)
    this.$travelPlanHub.$on('EditItem', this.onEditItem)
    this.$travelPlanHub.$on('RemoveItem', this.onRemoveItem)
  },
  destroyed() {
    this.$travelPlanHub.$off('AddItem')
    this.$travelPlanHub.$off('EditItem')
    this.$travelPlanHub.$off('RemoveItem')
  }
}
</script>

<style scoped>
.wrapper {
  width:100%;
}

.section-title {
  width:100%; 
  border:3px solid lightskyblue; 
  border-top-left-radius:20px; 
  border-top-right-radius:20px; 
  border-radius: 20px;
  margin-bottom:10px; 
  font-weight: bold; 
  font-size: 20px;
  padding: 0px 20px;
}
</style>