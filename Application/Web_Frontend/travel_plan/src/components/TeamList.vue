<template>
    <div class="wrapper-component">
        <Team v-for="team in TeamList" :key="team.teamId" :team="team"/>
    </div>
</template>

<script>
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