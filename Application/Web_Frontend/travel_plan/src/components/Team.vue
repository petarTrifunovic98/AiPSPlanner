<template>
    <div class="userService">
        <div class="telo">
            <div class="ime-tima"> {{team.name}} </div>
            <div class="members"> 
                <div v-for="traveler in team.members" :key="traveler.userId">
                    <b-card-text class="common-header">
                        <img :src="'data:;base64,' + traveler.picture" v-if="traveler.picture" class="rounded-image">
                        <img :src="require('../assets/no-picture.png')" v-else class="rounded-image">
                        <div style="width:fit-content;">
                            {{traveler.name}} 
                            {{traveler.lastName}}
                            ({{traveler.username}})
                        </div>
                    </b-card-text>
                </div>
            </div>
        </div>
        <ModalAreYouSure :naslov="'Leaving the team'"
                         :tekst="'Are you sure you want to leave this team?'"
                         @close="showModal = false" @yes="leaveTeam" v-if="showModal"/>
    </div>
</template>

<script>
import ModalAreYouSure from "@/components/ModalAreYouSure"
import TravelerBox from "@/components/TravelerBox"
export default {
    props:
    {
        team:
        {
            type: Object,
            required:true
        }
    },
    components:
    {
        ModalAreYouSure,
        TravelerBox
    },
    data()
    {
        return{
            showModal: false
        }
    },
    methods:
    {
        removeService()
        {
            console.log(this.userService)
            this.showModal = false;
            this.$store.state.userServices.forEach((item, index) =>
            {
                if(item.id == this.userService.id)
                    this.$store.state.userServices.splice(index,1)
            })
            this.$store.state.services.unshift(this.userService.service)
            this.$store.dispatch("removeUserService", this.userService.id)
        },
        saveChanges()
        {
            this.userService.max_dist = this.maxDist
            this.userService.min_rating = this.minRating
            this.userService.payment_ammount = this.paymentAmount
            this.userService.payment_type = this.paymentType
            this.isEditing = false
            console.log(this.userService)
            this.$store.dispatch("updateUserService", this.userService)
        },
        cancel()
        {
            this.maxDist = this.userService.max_dist
            this.scale = (this.userService.min_rating - 1) * 25
            this.paymentAmount = this.userService.payment_ammount
            this.paymentType = this.userService.payment_type
            this.isEditing = false
        }
    }  
}
</script>

<style scoped>
    .userService
    {
        display: flex;
        flex-direction: row-reverse;
        align-items: flex-start;
        justify-content: center;
        width: 380px;
        min-height: 215px;
        background-color: white;
        border: 1px solid grey;
        border-radius: 5px;
        margin: 10px;
        padding: 10px;
    }
    .telo
    {
        width: 100%;
        display: flex;
        flex-direction: column;
        align-items: flex-start;
        height: 100%;
    }

    .rounded-image {
        border-radius: 8px;
        border: 2px solid grey;
        height:30px;
        width:30px;
        object-fit:cover;
        margin-bottom: 10px;
        margin-right: 10px;
    }

    .common-header {
        display: flex;
        justify-content: space-between;
        align-items: center;
        flex-wrap: nowrap ;
    }
    .editovanje
    {
        height: 100%;
        width: fit-content;
        display: flex;
        flex-direction: column;
        align-content: flex-start;
    }
    .link-editovanje
    {
        height: fit-content;
        z-index: 1;
    }
    .razdvoji
    {
        margin-bottom: 15px;
    }
    .ime
    {
        font-size: 22px;
        font-weight: 750;
        color: rgb(48, 89, 165);
    }
    .grid-div
    {
        margin-left: 15px;
        height: 150px;
        width: 295px;
        padding-top: 7px;
        display: flex;
        flex-direction: column;
    }
    .row-div
    {
        flex-grow: 1;
        flex-shrink: 1;
        width: 100%;
        margin-bottom: 7px;
        align-items: center;
        display: flex;
        flex-direction: row;
    }
    .left-div
    {
        text-align: center;
        width: 180px;
        font-weight: 550;
    }
    .right-div
    {
        text-align: center;
        width: 115px;
    }
    .ocena-span
    {
        display: flex;
        flex-direction: row;
        justify-content: center;
    }
    .dugmeS
    {
        height: 25px;
        padding-left: 10px;
        padding-right: 10px;
        padding-bottom: 25px;
        padding-top: 0px;
    }
    .input-polje
    {
        width:70px;
        margin-right: 7px;
        text-align: center;
        height: 25px;
    }
    .vece
    {
        width:80px;
    }
    
    .skala
    {
        width:100px;
        margin-right: 7px;
    }

    .ime-tima
    {
        text-align: center;
        font-size: 25px;
        font-weight: 700;
        padding: 7px;
        width: 100%;
    }

    .members
    {
        display: flex;
        flex-direction: column;
        align-items: flex-start;
        width: 100%;
    }
    @media only screen and (max-width: 450px)
    {
        .userService
        {
            width: 110%;
            height: 235px;
        }
        .grid-div
        {
            margin-left: 5px;
        }
        .telo{
            width: 92%;
        }
        .left-div
        {
            width: 50%;
        }  
        .skala
        {
            width:90px;
            margin-right: 5px;
        } 
    }
</style>