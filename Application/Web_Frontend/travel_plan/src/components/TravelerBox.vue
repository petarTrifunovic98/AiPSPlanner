<template>
  <div class="col-12 wrapper">
    <a @click="goToProfile" v-if="travelerProp.picture"><img :src="'data:;base64,' + travelerProp.picture" class="rounded-image" v-b-popover.hover.right="travelerProp.username"></a>
    <a @click="goToProfile" v-else><img :src="require('../assets/no-picture.png')" class="rounded-image" v-b-popover.hover.right="travelerProp.username"></a>
    <a @click="goToProfile"><span> {{travelerProp.name}} {{travelerProp.lastName}} </span></a>
  </div>
</template>

<script>
export default {
  props: {
    travelerProp: {
      required: true
    }
  },
  methods:
  {
    goToProfile()
    {
      this.$router.push({
        name: "PageViewProfile", 
        params: {
            id: this.travelerProp.userId
        }
      })
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