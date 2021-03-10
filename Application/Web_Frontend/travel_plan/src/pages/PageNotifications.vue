<template>
    <Spinner v-if="Notifications == null" />
    <div class="page-wrapper" v-else>
        <div v-if="Notifications.length == 0" class="no-notifications">
            <i> There are currently no notifications to show </i>
        </div>
        <div v-for="notification in Notifications" :key="notification.notificationId" class="notification-div">
            <Notification :notification="notification" />
        </div>
    </div>
</template>

<script>
import Spinner from "@/components/Spinner"
import Notification from "@/components/Notification"
export default {
    components:
    {
        Spinner,
        Notification
    },
    computed:
    {
        Notifications()
        {
            return this.$store.state.notifications
        },
        isDataLoaded()
        {
            return this.$store.state.isDataLoaded
        }
    },
    created()
    {
        if(this.$store.state.notifications == null)
        {
            this.$store.dispatch("fillNotifications");
        }
        this.$store.dispatch("seenNotifications");
        this.$store.state.notificationNumber = 0
    }
}
</script>

<style scoped>
    .page-wrapper
    {
        width: 100%;
        display: flex;
        flex-direction: column;
        align-items: center;
        padding:20px;
    }
    .notification-div
    {
        width: 100%;
        display: flex;
        flex-direction: column;
        align-items: center;
        margin-bottom: 20px;
    }
    .button-div
    {
        width:100%;
        display:flex;
        flex-direction: column;
        align-items: center;
        margin-top:25px;
    }
    .no-notifications
    {
        margin-top:40px;
        display: flex;
        flex-direction: column;
        align-items: center;
        font-size: 20px;
    }
    @media only screen and (max-width: 450px)
    {
        .page-wrapper
        {
            padding-left: 0px;
            padding-right: 0px;
        }
    }
</style>