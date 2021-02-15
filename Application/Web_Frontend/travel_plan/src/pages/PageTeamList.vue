<template>
    <Spinner v-if="!isDataLoaded" />
    <div class = "wrapper" v-else>
        <div class = "items">
            <span v-if="TeamList.length == 0" class="no-items"> You currently have no assigned items </span>
            <table class="table smanji" v-else>
                <thead>
                    <tr>
                        <th  class="poravnanje" scope="col"><span>Name</span></th>
                        <th  class="poravnanje" scope="col"><span>Amount</span></th>
                        <th  class="poravnanje" scope="col"><span>Unit</span></th>
                        <th  class="poravnanje" scope="col"><span>Checked</span></th>
                        <th  class="poravnanje" scope="col"><span></span></th>
                    </tr>
                </thead>
                <tbody>
                    <tr v-for="item in ItemList" :key="item.itemId">
                        <td class="poravnanje"> <span  v-b-popover.hover.bottom="item.description"> {{item.name}} </span> </td>
                        <td class="poravnanje">
                            <span> {{item.amount}} </span>
                        </td>
                        <td class="poravnanje">
                            <span> {{item.unit}} </span>
                        </td>
                        <td class="poravnanje">
                            <img src="../assets/finished.svg" v-if="item.checked"/>
                            <img src="../assets/failed.svg" v-else/>
                        </td>
                        <td class="poravnanje">
                            <a @click="goToTrip(item.tripId)"><img src="../assets/trip.svg" v-b-popover.hover.top="'Trip: ' + item.trip.name + '. Click to go to trip page for more info.'" class="ikonica"/></a>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>
</template>

<script>
import Spinner from "@/components/Spinner"
export default {
    components:
    {
        Spinner
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
    methods:
    {
        goToTrip(tripId)
        {

        }
    },
    created()
    {
        if(this.TeamList == null)
        {
            this.$store.dispatch('fillMyTeams')
        }
    }
}
</script>

<style scoped>
.wrapper
  {
    display: flex;
    flex-direction: row;
    align-items: center;
    justify-content: center;
    padding-top: 20px;
    padding-left:10px;
    padding-right:10px;
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

    @media only screen and (max-width: 440px)
    {
        .smanji
        {
            font-size: 12px;
        }
        .labelDiv
        {
            font-size:16px; 
        }
    }
</style>