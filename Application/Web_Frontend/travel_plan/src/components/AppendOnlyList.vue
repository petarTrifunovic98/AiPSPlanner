<template>
  <b-list-group>
    <b-list-group-item>
      <b-form-input type="text" placeholder="Add to packing list..." v-model="newItem"></b-form-input>
      <b-button pill variant="primary" :disabled="(newItem == null) || newItem.length < 1" @click="onAddToPackingList">Add</b-button>
    </b-list-group-item>
    <b-list-group-item 
      v-for="ind in numOfShownItems"
      :key="ind">
      {{items[ind-1]}}
    </b-list-group-item>
    <b-list-group-item v-if="expandable" button @click="toggleExpandList">
      <strong v-text="listExpanded ? 'Collapse list' : 'Expand list'"></strong>
    </b-list-group-item>
  </b-list-group> 
</template>

<script>
export default {
  props: {
    itemsProp: {
      required: true,
      type: Array
    }
  },
  data() {
    return {
      listExpanded: false,
      newItem: "",
      items: this.itemsProp
    }
  },
  computed: {
    numOfShownItems() {
      if(this.items.length > 5 && !this.listExpanded)
        return 5
      else
        return this.items.length
    },
    expandable() {
      return this.items.length > 5
    }
  },
  methods: {
    toggleExpandList() {
      this.listExpanded = !this.listExpanded
    },
    onAddToPackingList() {
      this.items.unshift(this.newItem)
      this.newItem = ""
      this.$store.dispatch()
    }
  },
  created() {
    //this.$travelPlanHub.$on('EditTripInfo', this.onTripInfoEdited)
  }
}
</script>

<style>
.btn {
  margin: 3px;
  font-size: 14px!important;
}

.list-group-item {
  position: relative;
  display: block;
  padding: 10px;
  background-color: #fff;
  border: 1px solid rgba(0, 0, 0, 0.125);
  font-size: 14px;
}

.form-control {
  font-size: 14px;
}
</style>