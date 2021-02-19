<template>
    <div class="wrapper-component">
        <Team v-for="team in TeamList" :key="team.teamId" :team="team"/>
    </div>
</template>

<script>
import { mapMutations } from "vuex"
import Team from "@/components/Team"
export default {
    components:
    {
        Team
    },
    computed:
    {
        TeamList()
        {
            return this.$store.state.myTeams
        }
    },
    methods:
    {
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
        ...mapMutations({
            editTeamName: 'editTeamName',
            removeUserFromTeam: 'removeUserFromTeam',
            addMemberToTeam: 'addMemberToTeam'
        })
    },
    created()
    {
        this.$travelPlanHub.$on('EditTeamName', this.onEditTeamName)
        this.$travelPlanHub.$on('RemoveUserFromTeam', this.onRemoveUserFromTeam)
        this.$travelPlanHub.$on('AddMemberToTeam', this.onAddMemberToTeam)
    },
    destroyed()
    {
        this.$travelPlanHub.$off('EditTeamName')
        this.$travelPlanHub.$off('RemoveUserFromTeam')
        this.$travelPlanHub.$off('AddMemberToTeam')
    }
}
</script>

<style scoped>
    .wrapper-component
    {
        padding: 20px;
        display: flex;
        flex-direction: row;
        justify-content: center;
        align-items: center;
        flex-wrap: wrap;
    }
</style>