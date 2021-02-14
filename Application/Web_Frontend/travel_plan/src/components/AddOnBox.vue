<template>
  <div class="col-12 wrapper">
    <div>
      <b-card
        tag="article"
        style="max-width: 20rem;"
        class="mb-2"
      >
        <b-card-title>
          <span v-if="!inEditMode">{{addOn.description}}</span>
          <input type="text" v-model="editingAddOn.description" v-else>
        </b-card-title>
        <b-card-text>
          <span v-if="!inEditMode">{{addOn.price}}</span>
          <span v-else>
            <input type="number" v-model="editingAddOn.price">
          </span>
        </b-card-text>
        <b-card-text>
          {{addOn.type}}
        </b-card-text>
        <button type="button" class="btn btn-primary dugme" v-if="hasEditRights && !inEditMode" @click="toggleEditMode"> Edit </button>
        <button type="button" class="btn btn-primary dugme" v-if="inEditMode" @click="saveEdit"> Save </button>
        <button type="button" class="btn btn-primary dugme" v-if="inEditMode" @click="cancelEdit"> Cancel </button>
        <div v-for="lvl1AddOn in addOn.lvl1" :key="lvl1AddOn.addOnId">
          <AddOnBox :addOnProp="lvl1AddOn" :tripId="tripId"/>
        </div>
        <div v-for="lvl2AddOn in addOn.lvl2" :key="lvl2AddOn.addOnId">
          <AddOnBox :addOnProp="lvl2AddOn" :tripId="tripId"/>
        </div>
      </b-card>
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
    }
  },
  components: {
    AddOnBox
  },
  data() {
    return {
      inEditMode: false,
      editingAddOn: JSON.parse(JSON.stringify(this.addOnProp))
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
</style>