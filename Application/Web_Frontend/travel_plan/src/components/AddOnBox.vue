<template>
  <div class="wrapper">
    <div :class="level == 1 ? 'big-margins' : 'small-margins'">
      <b-card-header :class="[headerClasses[level - 1], 'common-header']">
        <span>{{addOn.type}}</span>
        <div>
          <button type="button" class="btn btn-primary dugme" v-if="hasEditRights && !inEditMode" @click="toggleEditMode"> Edit </button>
          <button type="button" class="btn btn-primary dugme" v-if="inEditMode" @click="saveEdit"> Save </button>
          <button type="button" class="btn btn-primary dugme" v-if="inEditMode" @click="cancelEdit"> Cancel </button>
        </div>
      </b-card-header>
      <b-card-body :class="level == 1 ? 'big-card' : 'small-card'">
        <b-card-text>
          <span v-if="!inEditMode">{{addOn.description}}</span>
          <b-form-textarea v-model="editingAddOn.description" v-else rows="3" no-resize placeholder="Enter add-on description..."></b-form-textarea>
        </b-card-text>
        <b-card-text class="common-header" style="justify-content: left;">
          <img src="../assets/cash.svg" style="height: 20px; width:20px; margin-right: 10px;" >
          <span v-if="!inEditMode">{{addOn.price}}</span>
          <b-form-input :type="'number'" v-model="editingAddOn.price" v-else style="width: 80px;" placeholder="Enter price..."></b-form-input>
        </b-card-text>
        <div style="display:flex; flex-wrap:wrap;">
          <div v-for="lvl1AddOn in addOn.lvl1" :key="lvl1AddOn.addOnId">
            <AddOnBox :addOnProp="lvl1AddOn" :tripId="tripId" :level="2"/>
          </div>
        </div>
        <div style="display:flex; flex-wrap:wrap">
          <div v-for="lvl2AddOn in addOn.lvl2" :key="lvl2AddOn.addOnId">
            <AddOnBox :addOnProp="lvl2AddOn" :tripId="tripId" :level="3"/>
          </div>
        </div>
      </b-card-body>
      <!-- </b-card> -->
    </div>
  </div>
</template>

<script>
import { mapGetters, mapMutations } from "vuex"
import AddOnBox from "@/components/AddOnBox.vue"

export default {
  name: "AddOnBox",
  props: {
    addOnProp: {
      required: true
    },
    tripId: {
      required: true,
      type: Number
    },
    level: {
      required: true,
      type: Number
    }
  },
  components: {
    AddOnBox
  },
  data() {
    return {
      inEditMode: false,
      editingAddOn: JSON.parse(JSON.stringify(this.addOnProp)),
      headerClasses: ['header-lvl-1', 'header-lvl-2', 'header-lvl-3']
    }
  },
  computed: {
    addOn() {
      return this.addOnProp
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
      this.editingAddOn = JSON.parse(JSON.stringify(this.addOnProp))
      this.toggleEditMode()
    },
    saveEdit() {
      this.editingAddOn.tripId = this.tripId
      this.$store.dispatch('putEditAddOn', this.editingAddOn)
      this.$travelPlanHub.$emit('EditAddOn', this.editingAddOn)
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