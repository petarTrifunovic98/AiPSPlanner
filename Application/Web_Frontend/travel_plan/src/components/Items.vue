<template>
  <div class="col-12 wrapper">
    <div style="margin-top:30px; font-weight: bold; font-size: 30px;">
      Items:
    </div>
    <div v-for="item in tripItems" :key="item.itemId">
      <ItemBox :itemProp="item"/>
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
      tripItems: 'getSpecificTripItems'
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
</style>