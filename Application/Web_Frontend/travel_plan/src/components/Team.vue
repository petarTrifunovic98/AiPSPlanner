<template>
    <div class="userService">
        <div class="telo">
            <div class="ime-tima"> 
                <span v-if="!isEditing"> {{team.name}} </span>
                <input v-model="newName" v-else class="unos">
                <a v-if="!isEditing" @click="isEditing = true"> <img  class = "slika" src="../assets/edit_black.svg"> </a>
                <button type="button" :disabled="invalidNewName" class="btn btn-success dugme" v-if="isEditing" @click="saveChanges">
                    <img src="../assets/finished.svg"> 
                </button>
                <button type="button" class="btn btn-danger dugme" v-if="isEditing" @click="cancel">
                    <img src="../assets/failed.svg"> 
                </button>
            </div>
            <div class="members"> 
                <div v-for="traveler in team.members" :key="traveler.userId">
                    <b-card-text class="common-header">
                        <a @click="goToTraveler(traveler.userId)" v-if="traveler.picture"><img :src="'data:;base64,' + traveler.picture" class="rounded-image"></a>
                        <a @click="goToTraveler(traveler.userId)" v-else><img :src="require('../assets/no-picture.png')" class="rounded-image"></a>
                        <a @click="goToTraveler(traveler.userId)">
                            <div style="width:fit-content;">
                                {{traveler.name}} 
                                {{traveler.lastName}}
                                ({{traveler.username}})
                         </div>
                        </a>
                    </b-card-text>
                </div>
            </div>
            <div class="dugmici">
                <button type="button" class="btn btn-primary dugmeS" @click="addMember">
                    <img  class = "ikonica" src="../assets/plus.png">
                    Add member
                </button>
                <button type="button" class="btn btn-danger dugmeS" @click="showModal = true">Leave team</button>
            </div>
        </div>
        <ModalAreYouSure :naslov="'Leave the team'"
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
            showModal: false,
            isEditing: false,
            newName: this.team.name
        }
    },
    computed:
    {
        invalidNewName()
        {
            return !this.newName
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
            this.isEditing = false
            this.team.name = this.newName
            this.$store.dispatch("editTeamInfo", {name: this.newName, teamId: this.team.teamId})
        },
        cancel()
        {
            this.isEditing = false
            this.newName = this.team.name
        },
        leaveTeam()
        {
            this.$store.state.myTeams.forEach((team, index) =>
            {
                if(team.teamId == this.team.teamId)
                    this.$store.state.myTeams.splice(index,1)
            })
            this.$store.dispatch("leaveTeam", {teamId: this.team.teamId})
        },
        addMember()
        {
            this.$router.push({
                name: "PageAddMember", 
                params: {
                    id: this.team.teamId, 
                    type: "teams"
                }
            })
        },
        goToTraveler(id)
        {
            this.$router.push({
                name: "PageViewProfile", 
                params: {
                    id: id
                }
            })
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
    .dugmici
    {
        margin-top: 15px;
        width: 100%;
        display: flex;
        flex-direction: row;
        justify-content: space-evenly;
        align-items: center;
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
    .dugme
    {
        margin-left: 10px;
        width: 40px;
        padding: 0px;
        padding-top: 3px;
        padding-bottom: 3px;
    }
    .input-polje
    {
        width:70px;
        margin-right: 7px;
        text-align: center;
        height: 25px;
    }
    .unos
    {
        width:80%;
    }
    
    .slika
    {
        width:18px;
        height:18px;
        margin-left: 20px;
    }

    .ime-tima
    {
        text-align: center;
        font-size: 25px;
        font-weight: 700;
        padding: 7px;
        width: 100%;
        display: flex;
        flex-direction: row;
        align-items: center;
        justify-content: center;
        margin-bottom: 5px;
    }

    .members
    {
        display: flex;
        flex-direction: column;
        align-items: flex-start;
        width: 100%;
    }

    .ikonica
    {
        width: 20px;
        height: 20px;
        margin-right: 5px;
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