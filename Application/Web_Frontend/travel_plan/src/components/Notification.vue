<template>
    <a class="notification-wrapper" @click="kliknuto">
        <div class="title-div">
            <img  class = "slika" src="../assets/delete_item.png" v-if="notification.type == 0">
            <img  class = "slika" src="../assets/new_item.png" v-if="notification.type == 1">
            <img  class = "slika" src="../assets/edit_item.png" v-if="notification.type == 2">
            <img  class = "slika" src="../assets/new_trip.png" v-if="notification.type == 3">
            <span> {{title}} </span>
        </div>
        <div class="linka"></div>
        <div class="body-div">
            <span> {{text}} </span>
        </div>
    </a>
</template>

<script>
export default {
    props:
    {
        notification:
        {
            required: true,
            type: Object
        }
    },
    computed:
    {
        title()
        {
            if(this.notification.type == 0)
                return "Removed item"
            else if(this.notification.type == 1)
                return "New item"
            else if(this.notification.type == 2)
                return "Item changed"
            else
                return "New trip"
        },
        text()
        {
            if(this.notification.type == 0)
                return "Item " + this.notification.relatedObjectName + " was removed from its trip, or assigned to someone else. You are no longer in charge of it."
            else if(this.notification.type == 1)
                return "You were assigned a new item named '" + this.notification.relatedObjectName + "'. Click on this notification to see its details in the item list."
            else if(this.notification.type == 2)
                return "The item '" + this.notification.relatedObjectName + "' you are in charge of has been edited. Click on this notification to see the details in the item list."
            else
                return "You were added to a new trip named '"  + this.notification.relatedObjectName + "'!"
        }
    },
    methods:
    {
        kliknuto()
        {
            if(this.notification.type == 3)
                this.$router.push("/trips")
            else
                this.$router.push("/items")
        }
    }
}
</script>

<style scoped>
    .notification-wrapper
    {
        width:80%;
        padding:20px;
        background-color: rgb(245, 245, 182);
        display:flex;
        flex-direction: column;
        align-items: flex-start;
        border-radius:10px;
    }
    .title-div
    {
        display: flex;
        flex-direction: row;
        align-items: center;
        justify-content: flex-start;
        width:100%;
        font-weight: 700;
        font-size: 22px;
    }
    .body-div
    {
        margin-top: 20px;
        display: flex;
        flex-direction: row;
        align-items: center;
        justify-content: flex-start;
        width:100%;
    }
    .linka
    {
        height:0px;
        width:300px;
        border-bottom: 1px solid rgb(46, 45, 45);
    }
    .footer-div
    {
        display: flex;
        flex-direction: row-reverse;
        align-items: center;
        justify-content: flex-start;
        margin-top:7px;
        width:100%;
    }
    .is-128x128 
    {
        width: 50px;
        height: 50px;
        margin-right:10px;
    }
    .rounded-image 
    {
        border-radius: 10px;
        height: 50px;
        width:50px;
        object-fit:cover;
        cursor: pointer;
    }
    .slika
    {
        width:25px;
        height:25px;
        margin-right: 10px;
        margin-bottom: 5px;
    }
    @media only screen and (max-width: 800px)
    {
        .notification-wrapper
        {
            width:98%;
        }
    }
     @media only screen and (max-width: 550px)
    {
        .linka
        {
            width:94%;
        }
    }
</style>