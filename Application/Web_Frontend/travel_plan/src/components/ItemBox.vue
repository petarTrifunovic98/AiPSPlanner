<template>
  <div class="col-12 wrapper">
    <div>
      <b-card
        tag="article"
        style="max-width: 20rem;"
        class="mb-2"
      >
        <b-card-title>{{item.name}}</b-card-title>
        <b-card-text>
          {{item.description}}
        </b-card-text>
        <b-card-text>
          {{item.amount}} {{item.unit}}
        </b-card-text>
        <b-card-text>
          {{item.user.name}} {{item.user.lastName}} ({{item.user.username}})
        </b-card-text>
        <b-card-text>
        </b-card-text>
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
  computed: {
    item() {
      return this.itemProp
    }
  },
  methods: {
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