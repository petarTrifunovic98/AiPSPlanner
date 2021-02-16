<template>
    <div class="wrapper-component">
        <div class="opcije"> 
            <img src="../assets/new-team.svg" class="ikonica">
            <label class="labela"> New team </label>
        </div>
        <div class="opcije">
            <label class="labela-stavka"> Team name: </label>
            <input v-model="name">
        </div>
        <div class="opcije">
            <button type="button" class="btn btn-primary dugme" :disabled="isInvalid" @click="dodaj">Create</button>
        </div>
    </div>
</template>

<script>
export default {
    data()
    {
        return{
            name: ""
        }
    },
    methods:
    {
        cancel()
        {
            this.name = ""
        },
        dodaj()
        {
            this.$store.dispatch("createTeam", {name : this.name})
            this.cancel();
        }
    },
    computed:
    {
        isInvalid()
        {
            return this.name == null || this.name == ""
        }
    }
}
</script>

<style scoped>
    .wrapper-component
    {
        background-color: white;
        width:100%;
        display: flex;
        flex-direction: column;
        align-items: center;
        padding-top: 15px;
    }
    .selekt
    {
        width: 80%;
        margin-right: 7px;
        background-color: white;
        word-break:break-all;
        background-color: rgb(236, 236, 236);
    }
    .labela
    {
        margin-left: 5px;
        font-size: 20px;
        font-weight: 700;
        margin-bottom: 0px;
    }
    .opcije
    {
        display: flex;
        flex-direction: row;
        width: 100%;
        justify-content: center;
        align-items: center;
        margin-bottom: 15px;
    }
    .input-polje
    {
        width:70px;
        margin-right: 7px;
        text-align: center;
        height: 25px;
    }
    .labela-stavka
    {
        margin-bottom: 0px;
        margin-right: 7px;
    }
    .ikonica
    {
        width: 30px;
        height: 30px;
        margin-right: 7px;
    }
    .vece
    {
        width:100px;
    }
    .skala
    {
        width:120px;
        margin-right: 7px;
    }
    .dugme
    {
        margin-right:15px
    }
</style>