<template>
  <div class="wrapper">
    <div class="big-margins">
      <b-card-header class="header-lvl-1 common-header">
        <span v-if="!inEditMode">{{item.name}}</span>
        <b-form-input type="text" v-model="editingItem.name" v-else style="width:fit-content;" placeholder="Enter item name..."></b-form-input>
        <div>
          <button type="button" class="btn btn-primary dugme" v-if="hasEditRights && !inEditMode" @click="toggleEditMode"> Edit </button>
          <button type="button" class="btn btn-primary dugme" v-if="inEditMode" @click="saveEdit"> Save </button>
          <button type="button" class="btn btn-primary dugme" v-if="inEditMode" @click="cancelEdit"> Cancel </button>
        </div>
      </b-card-header>
      <b-card-body class="big-card">
        <b-card-text>
          <span v-if="!inEditMode">{{item.description}}</span>
          <b-form-textarea v-else v-model="editingItem.description" rows="3" no-resize placeholder="Enter item description..."></b-form-textarea>
        </b-card-text>
        <b-card-text class="common-header" style="justify-content:left;">
          <img src="../assets/weight.svg" style="height: 20px; width: 20px; margin-right: 10px;">
          <span v-if="!inEditMode">{{item.amount}} {{item.unit}}</span>
          <span v-else>
            <b-form-input :type="'number'" v-model="editingItem.amount" placeholder="Enter amount..."></b-form-input>
            <b-form-input :type="'text'" v-model="editingItem.unit" placeholder="Enter unit"></b-form-input>
          </span>
        </b-card-text>
        <b-card-text class="common-header" style="justify-content: left;">
          <img :src="'data:;base64,' + item.user.picture" v-if="item.user.picture" class="rounded-image">
          <img :src="require('../assets/no-picture.png')" v-else class="rounded-image">
          <div style="width:fit-content;">
            {{item.user.name}} 
            {{item.user.lastName}}
            ({{item.user.username}})
          </div>
        </b-card-text>
       </b-card-body>
    </div>
  </div>
</template>

<script>
import { mapGetters, mapMutations } from "vuex"

export default {
  props: {
    itemProp: {
      required: true
    }
  },
  data() {
    return {
      inEditMode: false,
      editingItem: JSON.parse(JSON.stringify(this.itemProp))
    }
  },
  computed: {
    item() {
      return this.itemProp
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
      this.editingItem = JSON.parse(JSON.stringify(this.itemProp))
      this.toggleEditMode()
    },
    saveEdit() {
      this.$store.dispatch('putEditItemInfo', this.editingItem)
      this.$travelPlanHub.$emit('EditItem', this.editingItem)
      this.toggleEditMode()
    }
  }
}
</script>

<style scoped>
.rounded-image {
  border-radius: 8px;
  border: 2px solid grey;
  height:30px;
  width:30px;
  object-fit:cover;
  margin-bottom: 10px;
  margin-right: 10px;
}

.wrapper {
  width: 100%;
}

.header-lvl-1 {
  font-size: 20px;
  font-weight: bold;
  background-color: lightskyblue;
}

.header-lvl-2 {
  font-size: 15px;
  padding: 5px;
  font-weight: bold;
  background-color: lightgreen;
}

.header-lvl-3 {
  font-size: 15px;
  padding: 5px;
  font-weight: bold;
  background-color: lightgoldenrodyellow;
}

.common-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  flex-wrap: nowrap ;
}

.card-body {
  border: 2px solid lightskyblue;
  border-top: hidden;
  border-bottom-left-radius: 10px;
  border-bottom-right-radius: 10px;
}

.card-header {
  border: 2px lightskyblue solid;
  border-bottom: hidden;
  border-top-left-radius: 10px!important;
  border-top-right-radius: 10px!important;
}

.btn {
  margin: 0px;
  margin-left: 8px;
  padding: 2px 5px;
}

.small-card {
  padding: 5px;
}

.big-margins {
  margin: 10px 20px;
}

.small-margins {
  margin-bottom: 5px;
  margin-right: 10px;
}

.card-text {
  max-width: 250px;
}
</style>