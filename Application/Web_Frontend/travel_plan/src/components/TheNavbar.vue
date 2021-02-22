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
      },
      onAddItemNotification(itemNotifInfo) {
        console.log("add item notification: ")
        console.log(itemNotifInfo)
        this.decreseNotifNumber()
        this.addItemNofication(itemNotifInfo)
      },
      onEditItemNotification(itemNotifInfo) {
        console.log("edit item notification: ")
        console.log(itemNotifInfo)
        this.decreseNotifNumber()
        this.editItemNotification(itemNotifInfo)
      },
      onRemoveItemNotification(itemNotifInfo) {
        console.log("remove item notification: ")
        console.log(itemNotifInfo)
        this.decreseNotifNumber()
        this.removeItenNotification(itemNotifInfo)
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
      decreseNotifNumber() {
        if(this.$route.name == "PageNotifications") {
          this.$store.state.notificationNumber --
        }
      },
      ...mapMutations({
        addItemNofication: 'addItemNotification',
        editItemNotification: 'editItemNotification',
        removeItenNotification: 'removeItemNotification',
        addNewTeam: 'addNewTeam'
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