<template>
  <div class="col-12 wrapper">
    <div>
      <b-card
        tag="article"
        style="max-width: 20rem;"
        class="mb-2"
      >
        <b-card-title>{{accommodation.name}}</b-card-title>
        <b-card-text>
          {{accommodation.description}}
        </b-card-text>
        <b-card-text>
          {{accommodation.address}}
        </b-card-text>
        <b-card-text>
          {{accommodation.from | showTime}} - {{accommodation.to | showTime}}
        </b-card-text>
      </b-card>
    </div>
  </div>
</template>

<script>
import { mapGetters, mapMutations } from "vuex"

export default {
  props: {
    accommodationProp: {
      required: true
    }
  },
  data() {
    return {
      accommodationsLoaded: false,
      accommodationsOpen: false
    }
  },
  computed: {
    accommodation() {
      return this.accommodationProp
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