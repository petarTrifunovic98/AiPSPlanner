<template>
  <Spinner v-if="!userLoaded || !isDataLoaded" />
  <div class = "wrapper" v-else>
    <ProfileInfo 
      v-if="componentToShow == 'Info'"
      :user="isMyProfile ? $store.state.authUser : computedUser" 
      :isMyProfile="isMyProfile"
      @editProfile="componentToShow = 'Edit'"
      @changePass="componentToShow = 'Pass'"
    />
    <EditProfileInfo
      v-if="componentToShow == 'Edit'"
      :user="$store.state.authUser"
      @saveEditChanges="editSaved"
      @cancelChanges="componentToShow = 'Info'"
    />
    <ChangePass
      v-if="componentToShow == 'Pass'"
      :user="$store.state.authUser"
      @saveEditChanges="editSaved"
      @cancelChanges="componentToShow = 'Info'"
    />
  </div>
</template>

<script>
import ProfileInfo from "@/components/ProfileInfo"
import EditProfileInfo from "@/components/EditProfileInfo"
import ChangePass from "@/components/ChangePass"
import Spinner from "@/components/Spinner"
export default {
  props: {
    user: {
      required: false
    }
  },
  components: {
    ProfileInfo,
    EditProfileInfo,
    ChangePass,
    Spinner
  },
  data() {
    return {
      componentToShow: "Info"
    }
  },
  computed: {
    isMyProfile() {
      return this.$route.params.id == this.$store.getters['getAuthUserId']
    },
    computedUser() {
      return this.$store.state.user
    },
    userLoaded() {
      if(!this.isMyProfile)
        return this.computedUser != null
      else
        return true
    },
    isDataLoaded(){
        return this.$store.state.isDataLoaded
    }
  },
  methods: {
    editSaved() {
      console.log("SAVED")
      componentToShow = 'Info'
    },
    changedRoute() {
      this.$store.state.user = null
      let vm = this
      
      const routeId = this.$route.params.id
      if(!this.isMyProfile)
      {
        if(this.user)
          this.$store.state.user = this.user
        const onlyRating = !this.user ? false : true
        this.$store.dispatch('getUserInfo', {userId : routeId})
      }
      this.componentToShow = 'Info'
    }
  },
  created() {
    this.changedRoute()
  },
  watch: {
    $route() {
      this.changedRoute()
    }
  }
}
</script>

<style scoped>
  
  /* @media only screen and (min-width: 499px)
  {
    .wrapper
    {
      padding-top: 10px;
    }
  } */
</style>