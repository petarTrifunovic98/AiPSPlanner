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
                <Spinner v-if="!user && !isDataLoaded" />
                <div class="team-div" v-if="!user && isDataLoaded">
                    <TeamViewOnly v-for="team in teamList" :key="team.teamId" :team="team" @clicked="selectMember"
                                  :class="selectedMember == team.teamId ? 'selektovano' : 'standard'" />
                </div>
                <div class="user-div" v-if="user">
                    <div class="search-div"> 
                        <input class="searchbar" v-model="searchString" />
                        <button type="button" class="btn btn-primary dugme" @click="search" :disabled="!searchString">
                            <img  class = "ikonica" src="../assets/search.png">
                            Search users
                        </button>
                    </div>
                    <Spinner v-if="!isDataLoaded" />
                    <span v-if="!searchedUsers" class="natpis"> Search for users </span>
                    <span v-if="searchedUsers && isDataLoaded && userList.length == 0" class="natpis"> No such users exist </span>
                    <div class="user-list-div" v-if="searchedUsers && isDataLoaded && userList.length > 0">
                        <div v-for="user in userList" :key="user.userId" @click="selectedMember = user.userId"
                             :class="'single-user' + (selectedMember == user.userId? ' selektovano': ' standard')">
                            <img :src="'data:;base64,' + user.picture" v-if="user.picture" class="rounded-image">
                            <img :src="require('../assets/no-picture.png')" v-else class="rounded-image">
                            <span class="user-info"> {{user.name}} {{user.lastName}} ({{user.username}})</span>
                        </div>
                    </div>
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
import {mapMutations} from "vuex"
import Spinner from "@/components/Spinner"
import TeamViewOnly from "@/components/TeamViewOnly"
import ModalAreYouSure from "@/components/ModalAreYouSure"
export default {
    data(){
        return{
            user: true,
            selectedMember: -1,
            showModal: false,
            searchedUsers: false,
            searchString: ""
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
        },
        isDataLoaded()
        {
            return this.$store.state.isDataLoaded
        }
    },
    watch:
    {
        teamList(newValue, oldValue)
        {
            if(newValue) 
            {
                newValue.forEach(team => 
                {
                    console.log("Joined group for team " + team.name)
                    this.$travelPlanHub.JoinTeamGroup(team.teamId)
                })
            }
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
        },
        backTo()
        {
            if(this.objectType == "teams")
                this.$router.push("/teams")
            else
                this.$router.push({name: "PageSpecificTrip", params: {id: this.objectId, noEditRequest: true}})
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
            this.selectedMember = -1
            this.searchedUsers = false
        },
        search()
        {
            this.selectedMember = -1
            this.searchedUsers = true
            this.$store.dispatch('searchUsers', this.searchString)
            this.searchString = ""
        },
         onEditTeamName(team)
        {
            this.editTeamName(team)
        },
        onRemoveUserFromTeam(teamInfo)
        {
            this.removeUserFromTeam(teamInfo)
        },
        onAddMemberToTeam(teamInfo)
        {
            this.addMemberToTeam(teamInfo)
        },
        onPageLeave()
        {
            this.$travelPlanHub.$off('EditTeamName')
            this.$travelPlanHub.$off('RemoveUserFromTeam')
            this.$travelPlanHub.$off('AddMemberToTeam')
            if(this.TeamList)
            {
                this.TeamList.forEach(team => 
                {
                    console.log("Left group for team " + team.name)
                    this.$travelPlanHub.LeaveTeamGroup(parseInt(team.teamId))
                })
            }
            window.removeEventListener('beforeunload', this.leavePage)
        },
        leavePage(event)
        {
            event.preventDefault()
            console.log("Leave page")
            this.onPageLeave()
            event.returnValue = ''
        },
        ...mapMutations({
            editTeamName: 'editTeamName',
            removeUserFromTeam: 'removeUserFromTeam',
            addMemberToTeam: 'addMemberToTeam'
        })
    },
    components:
    {
        Spinner,
        TeamViewOnly,
        ModalAreYouSure
    },
    created()
    {
        window.addEventListener('beforeunload', this.leavePage)
        this.$store.state.myTeams = null
        this.$store.dispatch('fillMyTeams')
        this.$travelPlanHub.$on('EditTeamName', this.onEditTeamName)
        this.$travelPlanHub.$on('RemoveUserFromTeam', this.onRemoveUserFromTeam)
        this.$travelPlanHub.$on('AddMemberToTeam', this.onAddMemberToTeam)
    },
    beforeDestroy()
    {
        this.onPageLeave()
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
        margin-bottom: 20px;
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

    .user-div
    {
        width: 100%;
        display: flex;
        flex-direction: column;
        align-items: center;
        justify-content: center;
    }

    .search-div
    {
        width: 100%;
        display: flex;
        flex-direction: row;
        align-items: center;
        justify-content: space-evenly;
    }

    .searchbar
    {
        width: 60%;
    }

    .natpis
    {
        width: 100%;
        text-align: center;
        font-size: 24px;
        font-style: italic;
        color: darkgrey;
        height: 100px;
        padding-top: 50px;
    }

    .user-list-div
    {
        width: 100%;
        display: flex;
        flex-direction: row;
        align-items: center;
        justify-content: center;     
        flex-wrap: wrap;
        padding: 20px;
    }

    .single-user
    {
        width: fit-content;
        padding: 15px;
        background-color: white;
        border-radius: 6px;
        display: flex;
        flex-direction: row;
        margin-left: 30px;
        align-items: center;
        margin-bottom: 30px;
    }

    .rounded-image 
    {
        border-radius: 15px;
        border: 2px solid grey;
        height:80px;
        width:80px;
        object-fit:cover;
        margin-right: 30px;
    }

    .user-info
    {
        font-size: 20px;
        font-weight: 600;
    }
</style>