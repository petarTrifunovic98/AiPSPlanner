<template>
    <div>
        <nav class="tabovi">
            <a v-if="user" class="navbar-item aktivan" href="#">
                <span>Users</span>
            </a>
            <a v-else class="navbar-item" href="#" @click="goToUserTab">
                <span>Users</span>
            </a>
            <a v-if="!user" class="navbar-item aktivan" href="#">
                <span>Teams</span>
            </a>
            <a v-else class="navbar-item" href="#" @click="goToTeamTab">
                <span>Teams</span>
            </a>
        </nav>
        <div class="page-body">
            <div class="content-div">
                <Spinner v-if="!user && teamList == null" />
                <div class="team-div" v-if="!user && teamList != null">
                    <TeamViewOnly v-for="team in teamList" :key="team.teamId" :team="team" @clicked="selectMember"
                                  :class="selectedMember == team.teamId ? 'selektovano' : 'standard'" />
                </div>
                <button type="button" class="btn btn-success dugmeAdd" @click="showModal = true" :disabled="selectedMember == -1">
                    <img  class = "ikonica" src="../assets/plus.png">
                    Add
                </button>
            </div>
            <button type="button" class="btn btn-primary dugme" @click="backTo">
                <img  class = "ikonica" src="../assets/back.png">
                Back to {{objectType}}
            </button>
        </div>
        <ModalAreYouSure :naslov="'Add users'"
                         :tekst="'Are you sure you want to add these users?'"
                         @close="showModal = false" @yes="addSelected" v-if="showModal"/>
    </div>
</template>

<script>
import Spinner from "@/components/Spinner"
import TeamViewOnly from "@/components/TeamViewOnly"
import ModalAreYouSure from "@/components/ModalAreYouSure"
export default {
    data(){
        return{
            user: true,
            selectedMember: -1,
            showModal: false,
            searchedUsers: false
        }
    },
    computed:
    {
        objectId()
        {
            return this.$route.params.id
        },
        objectType()
        {
            return this.$route.params.type
        },
        teamList()
        {
            return this.$store.state.myTeams
        },
        userList()
        {
            return this.$store.state.searchedUsers
        }
    },
    methods:
    {
        switchTab()
        {
            this.selectedMember = -1
            this.searchedUsers = false
        },
        goToUserTab()
        {
            if(this.user != true)
                this.switchTab();
            this.user = true
        },
        goToTeamTab()
        {
            this.$store.state.searchedUsers = null
            if(this.user != false)
                this.switchTab();
            this.user = false
            if(this.teamList == null)
                this.$store.dispatch('fillMyTeams')
        },
        backTo()
        {
            if(this.objectType == "teams")
                this.$router.push("/teams")
            else
                this.$router.push({name: "PageSpecificTrip", params: {id: this.objectId}})
        },
        selectMember(id)
        {
            this.selectedMember = id
        },
        addSelected()
        {
            console.log("Adding" + this.objectType + ", id: " + this.selectedMember)
            var endpoint = ""
            if(this.objectType == "teams")
            {
                if(this.user)
                    endpoint = "addUserToTeam"
                else
                    endpoint = "addTeamToTeam"
            }
            else
            {
                if(this.user)
                    endpoint = "addUserToTrip"
                else
                    endpoint = "addTeamToTrip"
            }
            this.$store.dispatch(endpoint, {objectId: this.objectId, memberId: this.selectedMember})
            this.showModal = false
        }
    },
    components:
    {
        Spinner,
        TeamViewOnly,
        ModalAreYouSure
    }
}
</script>

<style scoped>
    .tabovi
    {
        background-color: white;
        padding: 7px;
        min-height: 50px;
        width: 100%;
        display: flex;
        flex-direction: row;
        justify-content: center;
        align-items: center;
        margin-bottom: 30px;
    }

    .navbar-item
    {
        flex-grow: 1;
        flex-shrink: 1;
        justify-content: center;
        font-weight: 400;
        font-size: 20px;
        height: 100%;
        text-align: center;
    }

    .aktivan
    {
        background-color: rgb(233, 233, 233);
        text-decoration: underline;
    }

    .page-body
    {
        width: 100%;
        display: flex;
        flex-direction: column;
    }

    .ikonica
    {
        width: 20px;
        height: 20px;
        margin-right: 5px;
    }

    .dugme
    {
        width: fit-content;
        margin-left: 20px;
        margin-top: 20px;
    }

    .content-div
    {
        display: flex;
        flex-direction: column;
        width: 100%;
    }

    .dugmeAdd
    {
        width: fit-content;
        margin-right: 50px;
        margin-top: 20px;
        align-self: flex-end;
    }

    .team-div
    {
        padding: 20px;
        display: flex;
        flex-direction: row;
        justify-content: center;
        align-items: center;
        flex-wrap: wrap;
    }

    .selektovano
    {
        border:3px solid red;
    }
</style>