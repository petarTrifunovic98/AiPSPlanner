<template>
    <Spinner v-if="!isDataLoaded" />
    <div class = "wrapper" v-else>
        <div class="novi-tim">
            <NewTeam @newTeam="joinNewTeam" />
        </div>
        <div class="timovi">
            <TeamList :editable="true"/>
        </div>
    </div>
</template>

<script>
import Spinner from "@/components/Spinner"
import NewTeam from "@/components/NewTeam"
import TeamList from "@/components/TeamList"
export default {
    components:
    {
        Spinner,
        NewTeam,
        TeamList
    },
    computed:
    {
        TeamList()
        {
            return this.$store.state.myTeams
        },
        isDataLoaded()
        {
            return this.$store.state.isDataLoaded
        }
    },
    watch:
    {
        TeamList(newValue, oldValue)
        {
            if(newValue && !oldValue) 
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
        joinNewTeam(newValue)
        {   
            console.log("Joined groip for team with id " + newValue)
            this.$travelPlanHub.JoinTeamGroup(newValue)
        },
        onPageLeave()
        {
            if(this.TeamList)
            {
                this.TeamList.forEach(team => 
                {
                    console.log("Left group for team " + team.name)
                    this.$travelPlanHub.LeaveTeamGroup(team.teamId)
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
        }
    },
    created()
    {   
        window.addEventListener('beforeunload', this.leavePage)
        this.$store.state.myTeams = null
        this.$store.dispatch('fillMyTeams')
    },
    beforeDestroy() {
        this.onPageLeave()
    }
}
</script>

<style scoped>
    .wrapper
    {
        padding-top:10px;
        padding-bottom: 10px;
        display:flex;
        flex-direction: row;
        justify-content: center;
        align-items: flex-start;
        width: 100%;
    }
    .timovi
    {
        width: 94%;
    }
    .novi-tim
    {
        width: 420px;
        margin-top: 100px;
        padding:15px
    }
  
  .body {
    background-color: #f8f9fa!important;
  }

  .items
  {
      background-color: white;
      border: 1px solid black;
      padding:10px;
      border-radius: 20px;
      min-width: 70%;
      min-height: 400px;
      text-align: center;
  }

  .no-items
  {
      font-size: 20px;
      font-style: italic;
      color: grey;
      margin-top: 100px;
  }

   .poravnanje
    {
        text-align: center;
    }

    .spisak
    {
        width:40px;
        height:40px;
    }

    .ikonica
    {
        width: 28px;
        height: 28px;
    }

    @media only screen and (max-width: 1100px)
    {
        .wrapper
        {
            flex-direction: column;
            align-content: center;
            align-items: center;
        }
        .novi-tim
        {
            width: 90%;
            margin-top:20px;
            padding:0px;
        }
    }
</style>