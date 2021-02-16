<template>
  <div class="col-12 wrapper">
    <img :src="'data:;base64,' + travelerProp.picture" v-if="travelerProp.picture" class="rounded-image">
    <img :src="require('../assets/no-picture.png')" v-else class="rounded-image">
     {{travelerProp.name}} {{travelerProp.lastName}}
  </div>
</template>

<script>
export default {
  props: {
    travelerProp: {
      required: true
    }
  }
}
</script>

<style scoped>
.rounded-image {
  border-radius: 15px;
  border: 2px solid grey;
  height:48px;
  width:48px;
  object-fit:cover;
  margin-bottom: 10px;
}

.wrapper {
  width: 100%;
  display: flex;
  flex-direction: column;
}
</style>