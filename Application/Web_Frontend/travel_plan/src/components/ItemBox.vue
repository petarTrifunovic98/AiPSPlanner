<template>
  <div class="col-12 wrapper">
    <div>
      <b-card
        tag="article"
        style="max-width: 20rem;"
        class="mb-2"
      >
        <b-card-title>
          <span v-if="!inEditMode">{{item.name}}</span>
          <input type="text" v-model="editingItem.name" v-else>
        </b-card-title>
        <b-card-text>
          <span v-if="!inEditMode">{{item.description}}</span>
          <textarea v-else v-model="editingItem.description"></textarea>
        </b-card-text>
        <b-card-text>
          <span v-if="!inEditMode">{{item.amount}} {{item.unit}}</span>
          <span v-else>
            <input type="number" v-model="item.amount">
            <input type="text" v-model="item.unit">
          </span>
        </b-card-text>
        <b-card-text>
          {{item.user.name}} {{item.user.lastName}} ({{item.user.username}})
        </b-card-text>
        <button type="button" class="btn btn-primary dugme" v-if="hasEditRights && !inEditMode" @click="toggleEditMode"> Edit </button>
        <button type="button" class="btn btn-primary dugme" v-if="inEditMode" @click="saveEdit"> Save </button>
        <button type="button" class="btn btn-primary dugme" v-if="inEditMode" @click="cancelEdit"> Cancel </button>
      </b-card>
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