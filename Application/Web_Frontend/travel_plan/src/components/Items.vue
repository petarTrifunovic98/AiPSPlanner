<template>
  <div class="wrapper" v-if="specificTrip">
    <div class="section-title">
      Items
    </div>
    <button 
      style="margin-left: 20px;" v-if="hasEditRights" type="button" 
      class="btn btn-primary custom-btn" @click="addFormOpen = !addFormOpen" >
      <img v-if="!addFormOpen" src="../assets/add.svg" class="action-img">
      <span v-text="addFormOpen ? 'Hide form' : 'Add a new item'"></span>
    </button>
    <div style="width: fit-content;" v-if="addFormOpen">
      <ItemBox :modeAddNew="true" @addedNew="addFormOpen = false"/>
    </div>
    <div style="display:flex; flex-wrap: wrap;">
      <div v-for="item in tripItems" :key="item.itemId">
        <ItemBox :itemProp="item" :modeAddNew="false"/>
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
  data() {
    return {
      addFormOpen: false
    }
  },
  computed: {
    ...mapGetters({
      isDataLoaded: 'getIsDataLoaded',
      tripItems: 'getSpecificTripItems',
      specificTrip: 'getSpecificTrip',
      hasEditRights: 'getHasEditRights'
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

.custom-btn {
  background-color: lightskyblue;
  border-color: lightskyblue;
  display: flex;
  flex-wrap: nowrap;
  align-items: center;
  color: black;
}

.custom-btn:hover {
  background-color: rgb(104, 185, 235);
  border-color: rgb(104, 185, 235);
  color: black;
}

.custom-btn:focus {
  background-color: rgb(104, 185, 235);
  border-color: rgb(104, 185, 235);
  color: black;
  outline: none;
  box-shadow: none;
}

.action-img {
  height:20px; 
  width:20px;
  cursor: pointer;
  margin-right: 10px;
}
</style>