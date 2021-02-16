<template>
  <div class="wrapper">
    <div class="big-margins">
      <b-card-header class="header-lvl-1 common-header" style="justify-content:flex-end;">
        <img src="../assets/delete_item.png" v-b-popover.hover.top="'Delete item'" class="action-img" v-if="!inEditMode && !modeAddNew && hasEditRights" @click="openModalDelete = true">
        <button type="button" class="btn btn-primary dugme" v-if="modeAddNew" @click="addNew"> Add </button>
      </b-card-header>
      <b-card-body class="big-card">
        <div class="basic-info">
          <b-card-text class="header-lvl-1 common-header" style="background-color:white; max-width:none!important;">
            <span v-if="!inEditModeInfo && !modeAddNew">{{item.name}}</span>
            <b-form-input type="text" v-model="editingItem.name" v-else style="width:fit-content;" placeholder="Enter item name..."></b-form-input>
            <img src="../assets/edit_item.png" v-b-popover.hover.top="'Edit info'" class="action-img" v-if="hasEditRights && !inEditModeInfo && !modeAddNew" @click="toggleEditModeInfo">
            <button type="button" class="btn btn-primary dugme" v-if="inEditModeInfo" @click="saveEditInfo"> Save </button>
            <button type="button" class="btn btn-primary dugme" v-if="inEditModeInfo" @click="cancelEditInfo"> Cancel </button>
          </b-card-text>
          <b-card-text>
            <span v-if="!inEditModeInfo && !modeAddNew">{{item.description}}</span>
            <b-form-textarea v-else v-model="editingItem.description" rows="3" no-resize placeholder="Enter item description..."></b-form-textarea>
          </b-card-text>
          <b-card-text class="common-header" style="justify-content:left;">
            <img src="../assets/weight.svg" style="height: 20px; width: 20px; margin-right: 10px;">
            <span v-if="!inEditModeInfo && !modeAddNew">{{item.amount}} {{item.unit}}</span>
            <span v-else>
              <b-form-input :type="'number'" v-model="editingItem.amount" placeholder="Enter amount..."></b-form-input>
              <b-form-input :type="'text'" v-model="editingItem.unit" placeholder="Enter unit"></b-form-input>
            </span>
          </b-card-text>
        </div>
        <b-card-text class="common-header" v-if="!inEditModeUser && !modeAddNew">
          <div class="common-header" style="justify-content: left;">
            <img :src="'data:;base64,' + item.user.picture" v-if="item.user.picture" class="rounded-image">
            <img :src="require('../assets/no-picture.png')" v-else class="rounded-image">
            <div style="width:fit-content;">
              {{item.user.name}} 
              {{item.user.lastName}}
              ({{item.user.username}})
            </div>
          </div>
          <img src="../assets/edit_item.png" v-b-popover.hover.top="'Change user'" class="action-img" @click="toggleEditModeUser" v-if="hasEditRights">
        </b-card-text>
        <b-card-text class="common-header" style="justify-content:left; max-width:none; " v-if="modeAddNew || inEditModeUser">
          <img :src="'data:;base64,' + tripTravelers[selectedUser].picture" v-if="tripTravelers[selectedUser].picture" class="rounded-image">
          <img :src="require('../assets/no-picture.png')" v-else class="rounded-image">
          <b-form-select v-model="selectedUser" size="sm" style="width:fit-content;">
            <b-form-select-option v-for="(traveler, ind) in tripTravelers" :key="traveler.userId" :value="ind">
              {{traveler.name}} {{traveler.lastName}} ({{traveler.username}})
            </b-form-select-option>
          </b-form-select>
          <button type="button" class="btn btn-primary dugme" v-if="!modeAddNew" @click="saveEditUser" :disabled="selectedUser == currentUserIndex"> Save </button>
          <button type="button" class="btn btn-primary dugme" v-if="!modeAddNew" @click="cancelEditUser"> Cancel </button>
        </b-card-text>
        <b-card-text class="common-header" v-if="!modeAddNew">
          <div class="common-header" style="justify-content:left;">
            <img 
              :src="item.checked ? require('../assets/check.svg') : require('../assets/unchecked.svg')" 
              class="standard-img" :style="hasEditRights ? 'cursor:pointer;' : 'cursor:auto;'"
              @click="toggleChecked">
            <span v-text="item.checked ? 'Checked!' : 'Unchecked...'" class="standard-img" style="cursor:auto;"></span>
          </div>
        </b-card-text>
       </b-card-body>
    </div>
    <ModalAreYouSure 
      :naslov="'Delete item'"
      :tekst="'Are you sure you want to delete this item?'"
      @close="openModalDelete = false" @yes="deleteOne" v-if="openModalDelete"
    />
    <ModalAreYouSure 
      :naslov="itemProp.checked ? 'Uncheck item' : 'Check item'"
      :tekst="'Are you sure you want to ' + (itemProp.checked ? 'uncheck' : 'check') + ' this item?'"
      @close="openModalCheck = false" @yes="changeChecked" v-if="openModalCheck"
    />
  </div>
</template>

<script>
import { mapGetters, mapMutations } from "vuex"
import ModalAreYouSure from "@/components/ModalAreYouSure.vue"

export default {
  components: {
    ModalAreYouSure
  },
  props: {
    itemProp: {
      required: false
    },
    modeAddNew: {
      required: true,
      type: Boolean
    }
  },
  data() {
    return {
      inEditModeInfo: false,
      inEditModeUser: false,
      editingItem: this.modeAddNew ?
        { name: "", description: "", amount: 1, unit: "", userId: -1 } :
        JSON.parse(JSON.stringify(this.itemProp)),
      openModalDelete: false,
      openModalCheck: false,
      selectedUser: 0
    }
  },
  computed: {
    item() {
      return this.itemProp
    },
    inEditMode() {
      return this.inEditModeInfo || this.inEditModeUser
    },
    currentUserIndex() {
      return this.tripTravelers.findIndex(t => t.userId == this.itemProp.userId)
    },
    ...mapGetters({
      hasEditRights: 'getHasEditRights',
      tripId: 'getSpecificTripId',
      userId: 'getAuthUserId',
      tripTravelers: 'getSpecificTripTravelers'
    })
  },
  methods: {
    toggleEditModeInfo() {
      this.inEditModeInfo = !this.inEditModeInfo
    },
    toggleEditModeUser() {
      if(!this.modeAddNew) {
        this.selectedUser = this.tripTravelers.findIndex(t => t.userId == this.itemProp.userId)
      }
      this.inEditModeUser = !this.inEditModeUser
    },
    toggleChecked() {
      if(this.hasEditRights)
        this.openModalCheck = true
    },
    cancelEditInfo() {
      this.editingItem = JSON.parse(JSON.stringify(this.itemProp))
      this.toggleEditModeInfo()
    },
    saveEditInfo() {
      this.$store.dispatch('putEditItemInfo', this.editingItem)
      this.$travelPlanHub.$emit('EditItem', this.editingItem)
      this.toggleEditModeInfo()
    },
    cancelEditUser() {
      this.toggleEditModeUser()
    },
    saveEditUser() {
      this.$store.dispatch('putChangeItemUser', {
        itemId: this.itemProp.itemId,
        newUserId: this.tripTravelers[this.selectedUser].userId,
        tripId: this.tripId
      })
      this.editingItem.user = this.tripTravelers[this.selectedUser]
      this.editingItem.userId = this.tripTravelers[this.selectedUser].userId
      this.$travelPlanHub.$emit('EditItem', this.editingItem)
      this.toggleEditModeUser()
    },
    addNew() {
      this.editingItem.userId = this.tripTravelers[this.selectedUser].userId
      this.editingItem.tripId = this.tripId
      this.$store.dispatch('postAddItem', this.editingItem)
      this.editingItem.user = this.tripTravelers[this.selectedUser]
      this.$emit("addedNew", "addedNew")
      this.restoreEditingItem()
    },
    deleteOne() {
      let payload = {
        tripId: this.tripId,
        itemId: this.itemProp.itemId
      }
      this.$store.dispatch('deleteItem', payload)
      this.$travelPlanHub.$emit('RemoveItem', payload.itemId)
    },
    changeChecked() {
      this.$store.dispatch('putChangeCheckedItem', {
        itemId: this.itemProp.itemId,
        tripId: this.tripId
      })
      this.openModalCheck = false
      this.editingItem.checked = !this.editingItem.checked
      this.$travelPlanHub.$emit('EditItem', this.editingItem)
    },
    restoreEditingItem() {
      this.editingItem = { name: "", description: "", amount: 1, unit: "", userId: -1, user: {} }
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

.action-img {
  height:20px; 
  width:20px; 
  margin-left:10px; 
  cursor: pointer;
}

.standard-img {
  height: 20px;
  width: 20px;
  margin-right: 10px;
}

.basic-info {
  padding: 10px;
  border: 1px solid lightskyblue;
  border-radius: 10px;
  margin: -5px -5px 10px -5px;
}
</style>