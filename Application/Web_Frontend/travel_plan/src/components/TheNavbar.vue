<template>
  <div class = "navigate">
    <TheNavbarSmall class = "mali"/>
    <TheNavbarLarge class = "veliki"/>
  </div>
</template>

<script>
  import { mapGetters, mapMutations } from "vuex"
  import TheNavbarSmall from "@/components/TheNavbarSmall"
  import TheNavbarLarge from "@/components/TheNavbarLarge"
  export default {
    components:
    {
      TheNavbarSmall,
      TheNavbarLarge
    },
    computed: {
      ...mapGetters({
        logedIn: 'getIsLogedIn',
        userId: 'getAuthUserId'
      })
    },
    watch: {
      logedIn(newValue, oldValue) {
        if(newValue == true) {
          this.subscribeToEvents()
        }
        else {
          this.$travelPlanHub.LeaveItemGroup(parseInt(this.userId))
          console.log("left item group")
          this.$travelPlanHub.$off('AddItemNotification')
          this.$travelPlanHub.$off('EditItemNotification')
          this.$travelPlanHub.$off('RemoveItemNotification')
          this.$travelPlanHub.$off('AddToTeamNotification')
          this.$travelPlanHub.$off('AddToTripNotification')
        }
      }
    },
    methods: {
      subscribeToEvents() {
        this.$travelPlanHub.JoinItemGroup(parseInt(this.userId))
        console.log("joined item group")
        this.$travelPlanHub.$on('AddItemNotification', this.onAddItemNotification)
        this.$travelPlanHub.$on('EditItemNotification', this.onEditItemNotification)
        this.$travelPlanHub.$on('RemoveItemNotification', this.onRemoveItemNotification)
        this.$travelPlanHub.$on('AddToTeamNotification', this.onAddedToTeam)
        this.$travelPlanHub.$on('AddToTripNotification', this.onAddedToTrip)
      },
      showToast(imgSrc, titleText, text) {
        const h = this.$createElement
        const toastTitle = h('div', {}, [
          h('img', { style: "width: 20px; height: 20px; margin-right:20px;", attrs: {
            src: require("../assets/" + imgSrc)
          } }),
          h('span', {}, titleText)
        ])

        this.$bvToast.toast(text, {
          title: [toastTitle],
          autHideDelay: 4000,
          variant: 'info'
        })
      },
      onAddItemNotification(itemNotifInfo) {
        console.log("add item notification: ")
        console.log(itemNotifInfo)
        if(!this.decreseNotifNumber()) {
          this.showToast('checklist.svg', 'New item assigned', 'You were assigned a new item named "' + itemNotifInfo.notification.relatedObjectName + '".')
        }
        this.addItemNofication(itemNotifInfo)
      },
      onEditItemNotification(itemNotifInfo) {
        console.log("edit item notification: ")
        console.log(itemNotifInfo)
        if(!this.decreseNotifNumber()) {
          this.showToast('checklist.svg', 'Item edited', 'The item "' + itemNotifInfo.notification.relatedObjectName + '" you are in charge of has been edited.')
        }
        this.editItemNotification(itemNotifInfo)
      },
      onRemoveItemNotification(itemNotifInfo) {
        console.log("remove item notification: ")
        console.log(itemNotifInfo)
        if(!this.decreseNotifNumber()) {
          this.showToast('checklist.svg', 'Item removed', 'The item "' + itemNotifInfo.notification.relatedObjectName + '" has been deleted. You are no longer in chage of it.')
        }
        this.removeItemNotification(itemNotifInfo)
      },
      onAddedToTeam(teamInfo) {
        console.log("added to team: ")
        console.log(teamInfo)
        if(this.$route.name == "PageTeamList" || this.$route.name == "PageAddMember") {
          console.log("Joined group for team " + teamInfo.name)
          this.$travelPlanHub.JoinTeamGroup(teamInfo.teamId)
          this.addNewTeam(teamInfo)
        }
      },
      onAddedToTrip(tripNotifInfo) {
        console.log("added to trip notification: ")
        console.log(tripNotifInfo)
        if(!this.decreseNotifNumber()) {
          this.showToast('map.svg', 'Added to trip', 'You were added to a new trip named "' + tripNotifInfo.notification.relatedObjectName + '".')
        }
        this.addTripNotification(tripNotifInfo)
      },
      decreseNotifNumber() {
        if(this.$route.name == "PageNotifications") {
          this.$store.state.notificationNumber --
          return true
        }
        return false
      },
      ...mapMutations({
        addItemNofication: 'addItemNotification',
        editItemNotification: 'editItemNotification',
        removeItemNotification: 'removeItemNotification',
        addNewTeam: 'addNewTeam',
        addTripNotification: 'addTripNotification'
      })
    },
    created() {
      if(this.logedIn) {
        this.subscribeToEvents()
      }
    }
  }
</script>

<style scoped>
  @media only screen and (min-width: 500px)
  {
    .mali
    {
      width:0px;
      visibility: hidden;
      min-height: 0px;
      margin: 0px;
    }
    
    .navigate
    {
      width: 100%;
      height: 70px;
      display: flex;
      flex-direction: column-reverse;
    }
    .veliki
    {
      padding-top:15px;
    }
  }
  @media only screen and (max-width: 499px)
  {
    .veliki
    {
      width:0px;
      visibility: hidden;
      min-height: 0px;
      margin: 0px;
    }
    .navigate
    {
      height: 85px;
      display: flex;
      flex-direction: column;
      padding-bottom:0px;
      margin-bottom:0px;
      margin-top: 0px;
      padding-top: 0px;
    }
    .mali
    {
      padding-bottom:0px;
      margin-bottom:0px;
      margin-top: 10px;
    }
  }
</style>