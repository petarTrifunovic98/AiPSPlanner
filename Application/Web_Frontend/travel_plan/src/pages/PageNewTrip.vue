<template>
    <div class="wrapper">
        <Spinner v-if="!isDataLoaded"/>
        <div v-else class="new-trip">
            <div class="title-div"> New trip </div>
            <span class="labela"> Name: </span>
            <input v-model="newTrip.name" class="polje"/>
            <span class="labela"> Description: </span>
            <textarea v-model="newTrip.description" class="polje"/>
            <span class="labela"> From - To: </span>
            <div style="display:flex;" class="polje">
                <b-form-datepicker 
                    v-model="newTrip.from" style="width:fit-content;" 
                    :date-format-options="{ year: 'numeric', month: 'short', day: '2-digit' }">
                </b-form-datepicker>
                <span style="text-align:center; margin: 3px 10px 0px 10px;"> - </span>
                <b-form-datepicker 
                    v-model="newTrip.to" style="width:fit-content;"
                    :date-format-options="{ year: 'numeric', month: 'short', day: '2-digit' }">
                </b-form-datepicker>
            </div>
            <span class="labela"> Trip category: </span>
            <select v-model="newTrip.tripCategory" class="polje selekt">
                <option v-for="option in options" :key="option" class="option"> 
                    {{option}}
                </option>
            </select>
            <div class="kreiraj">
                <button type="button" :disabled="tripInvalid" class="btn btn-success dugme" @click="showModal = true">
                    <img src="../assets/finished.svg">
                    Create trip
                </button>
            </div>
        </div>
        <ModalAreYouSure :naslov="'Create a trip'"
                         :tekst="'Are you sure you want to create this trip?'"
                         @close="showModal = false" @yes="createTrip" v-if="showModal"/>
    </div>
</template>
 
<script>
import ModalAreYouSure from "@/components/ModalAreYouSure"
import Spinner from "@/components/Spinner.vue"
export default {
    components:
    {
        Spinner,
        ModalAreYouSure
    },
    computed:
    {
        isDataLoaded()
        {
            return this.$store.state.isDataLoaded
        },
        options()
        {
            var niz = []
            niz.push("Sea")
            niz.push("Winter")
            niz.push("Spa")
            niz.push("Other")
            return niz
        },
        tripInvalid()
        {
            return !this.newTrip.name || !this.newTrip.description || (this.newTrip.to < this.newTrip.from)
        }
    },
    data(){
        return{
            newTrip :{
                name: "",
                description: "",
                from: this.convertDate(new Date(Date.now())),
                to: this.convertDate(new Date(Date.now())),
                tripCategory : "Other"
            },
            showModal: false
        }
    },
    methods:
    {
        createTrip()
        {
            this.$store.dispatch("createTrip", this.newTrip)
            this.showModal = false
        },
        convertDate(date)
        {
            var year = date.getFullYear()
            var month = date.getMonth()
            var day = date.getDate()
            month += 1;
            if(month < 10)
                month = "0" + month
            if(day < 10)
                day = "0" + day

            return year + "-" + month + "-" + day
        }
    }
}
</script>

<style scoped>
    .wrapper
    {
        padding: 30px;
        width: 100%;
        display: flex;
        flex-direction: row;
        align-items: center;
        justify-content: center;
    }

    .new-trip
    {
        width: 85%;
        padding:10px;
        background-color: white;
        border: 1px solid black;
        border-radius: 7px;
        display: flex;
        flex-direction: column;
        justify-content: center;
        align-items: flex-start;
    }

    .title-div
    {
        width: 100%;
        text-align: center;
        font-size: 22px;
        font-weight: 600;
        margin-bottom: 10px;
        font-style: italic;
    }

    .labela
    {
        font-size: 18px;
        font-weight: 600;
        margin-bottom: 5px;
        margin-left: 10px;
    }

    .polje
    {
        margin-bottom: 15px;
        margin-left: 5px;
        margin-right: 15px;
        width: 98%;
        padding: 5px;
    }

    .selekt, option
    {
        padding: 10px;
        font-size: 16px;
    }

    .kreiraj
    {
        width: 100%;
        padding-top:30px;
        padding-bottom:15px;
        padding-right:30px;
        display: flex;
        flex-direction: row;
        justify-content: flex-end;
    }
</style>