<template>
    <div>
        <Spinner v-if="!isDataLoaded"/>
        <div class="page-content" v-else>
            <div class="packing-list">
                <span class="div-title"> Packing List </span>
                <div v-for="element in additionalInfo.packingList" :key="element.item" class="one-item"> 
                    <input type="checkbox" v-model="element.checked"/>
                    <span class="item-name"> {{element.item}} </span>
                </div>
                <div class="add-item">
                    <button type="button" class="btn btn-primary dugmeAdd" @click="adding = true" v-if="!adding">
                        <img  class = "ikonica" src="../assets/plus.png">
                        Add item
                    </button>
                    <input v-model="itemToAdd" v-if="adding" class="unos"/>
                    <button type="button" :disabled="!itemToAdd" class="btn btn-success dugme" v-if="adding" @click="addItem">
                        <img src="../assets/finished.svg"> 
                    </button>
                    <button type="button" class="btn btn-danger dugme" v-if="adding" @click="cancel">
                        <img src="../assets/failed.svg"> 
                    </button>
                </div>
            </div>
            <div class="tips-and-tricks">
                <span class="div-title"> Tips & Tricks </span>
                <div class="tt-list">
                    <div v-for="tip in additionalInfo.tipsAndTricks" :key="tip" class="tip">
                        {{tip}}
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
import Spinner from "@/components/Spinner"
export default {
    data(){
        return{
            adding: false,
            itemToAdd: ""
        }
    },
    computed:
    {
        tripId()
        {
            return this.$route.params.id
        },
        isDataLoaded()
        {
            return this.$store.state.isDataLoaded
        },
        additionalInfo()
        {
            return this.$store.state.tripAdditionalInfo
        }
    },
    methods:
    {
      cancel()
      {
        this.adding = false
        this.itemToAdd = ""
      },
      addItem()
      {
        this.$store.state.tripAdditionalInfo.packingList.push({item: this.itemToAdd, checked: false})
        this.$store.dispatch("addPackingListItem", {tripId: this.tripId, item: this.itemToAdd})
        this.cancel()
      }
    },
    created()
    {
        this.$store.dispatch('fillTripAdditionalInfo', {tripId: this.tripId})
    },
    components:
    {
        Spinner
    }
}
</script>

<style scoped>
    .page-content
    {
        padding-top:30px;
        padding-bottom:30px;
        display: flex;
        flex-direction: row;
        align-items: flex-start;
        justify-content: space-evenly;
    }

    .packing-list
    {
        width: 35%;
        padding: 10px;
        border-radius: 10px;
        border: 1px solid black;
        display: flex;
        flex-direction: column;
        align-items: flex-start;
        justify-content: center;
        text-align: center;
        background-color: white;
    }

    .div-title
    {
        width: 100%;
        text-align: center;
        font-size: 20px;
        font-weight: 600;
        margin-bottom: 10px;font-style: italic;
    }

    .one-item
    {
        display: flex;
        flex-direction: row;
        margin-bottom: 7px;
        align-items: center;
    }

    .item-name
    {
        margin-left: 4px;
    }

    .add-item
    {
        margin-top: 10px;
        margin-bottom: 10px;
        display: flex;
        flex-direction: row;
        align-items: center;
        justify-content: space-evenly;
    }

    .dugmeAdd
    {
        width: fit-content;
        justify-self: center;
    }

    .ikonica
    {
        width: 20px;
        height: 20px;
        margin-right: 5px;
    }

    .dugme
    {
        margin-left: 10px;
        width: 40px;
        padding: 0px;
        padding-top: 3px;
        padding-bottom: 3px;
    }

    .unos
    {
        width: 85%;
    }

    .tips-and-tricks
    {
        width: 60%;
        padding: 10px;
        border-radius: 10px;
        border: 1px solid black;
        display: flex;
        flex-direction: column;
        align-items: flex-start;
        justify-content: center;
        background-color: white;
    }

    .tt-list
    {
        display: flex;
        flex-direction: row;
        flex-wrap: wrap;
        justify-content: space-evenly;
        align-items: center;
    }

    .tip
    {
        padding:5px;
        margin-bottom: 10px;
        text-align: justify;
    }
</style>