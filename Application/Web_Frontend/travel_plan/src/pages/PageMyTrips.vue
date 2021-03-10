<template>
  <div class="wrapper" v-if="isDataLoaded">
    <div v-if="tripsPortion.length == 0" class="no-trips"> No trips yet </div>
    <div v-if="tripsPortion.length == 0" class="button-div">
      <button type="button" class="btn btn-success dugme" @click="goToNewTrip">
        <img src="../assets/add.svg">
          Create trip
      </button>
    </div>
    <div class="users" v-if="tripsPortion.length > 0">
      <TripBox
        :tripProp="trip"
        v-for="trip in tripsPortion" 
        :key="trip.tripId"
      />
    </div>
  </div>
  <Spinner v-else />
</template>

<script>
import { mapGetters } from "vuex"
import TripBox from "@/components/TripBox"
import Spinner from "@/components/Spinner.vue"

export default {
  props: {
    toastInfo: {
      required: false
    }
  },
  components: {
    TripBox,
    Spinner
  },
  data() {
    return {
      currentPage: 1,
      perPage: 10,
      lastPage: 1
    }
  },
  computed: {
    ...mapGetters({
      tripsPortion: 'getTripsPortion',
      isDataLoaded: 'getIsDataLoaded',
      authUserId: 'getAuthUserId'
    })
  },
  methods: {
    showToast(imgSrc, titleText, text) {
      const h = this.$createElement
      const toastTitle = h('div', {}, [
        h('img', { style: "width: 20px; height: 20px; margin-right:20px;", attrs: {
          src: require("../assets/" + imgSrc)
        } }),
        h('span', {}, titleText)
      ])

      this.$bvToast.toast(text, {
        title: [toastTitle],
        autHideDelay: 4000,
        variant: 'danger'
      })
    },
    onCreate() {
      this.$store.dispatch('fillTripsPortion', {
        userId: this.authUserId
      })
    },
    goToNewTrip()
    {
      this.$router.push("/new-trip")
    }
  },
  created() {
    this.onCreate()
    if(this.$store.state.notificationNumber == -1)
    {
        this.$store.dispatch("getNotificationNumber")
    }
    this.$store.dispatch('deleteSeenNotifications', false)
    if(this.toastInfo) {
      this.showToast(this.toastInfo.imgSrc, this.toastInfo.titleText, this.toastInfo.text)
    }
  }
}
</script>


<style scoped>
 
  .wrapper
  {
    display: flex;
    flex-direction: column;
    align-items: stretch;
    padding-top: 20px;
    padding-left:10px;
    padding-right:10px;
  }
  
  .body {
    background-color: #f8f9fa!important;
  }

  .users {
    display: flex; 
    flex-direction:column; 
    align-items:center;
  }

  .pag-top {
    margin-bottom: 40px;
    margin-top: 20px;
    z-index:0;
  }

  .pag-bottom {
    margin: 40px 0 0px 0;
    z-index:0;
    padding-bottom: 40px;
  }

  .filter-sticky {
    display:flex;
    flex-direction: column;
    border-radius: 5px;
    border: 1px solid black;
    background-color: white;
    margin: 0 8% 0 8%;
  }

  .filter-parameters {
    display: flex;
    flex-direction: row;
    flex-wrap: wrap;
    align-items:center;
    padding:10px;
    font-size: 14px;
    border-radius: 5px;
    justify-content: space-around;
  }

  .filter-with-warning {
    margin-right:3%;
    margin-top:5px;
    display: flex;
    align-items: center;
  }

  .filter-rating-wrap {
    display:flex;
    font-weight: 600;
    align-items: center;

  }

  .filter-name-wrap {
    display:flex;
    font-weight: 600;
    align-items: center;
    margin-right: 3%;
    margin-top: 5px;
  }


  .filter-sort-wrap {
    display:flex;
    font-weight: 600;
    align-items: center;
    margin-right: 3%;
    margin-top:5px;
  }

  .filter-rating {
    width: 40px;
    margin-left:10px;
    margin-right:10px;
    border: 1px solid grey;
    border-radius: 5px;
    padding-left: 2px;
  }

  .filter-name {
    max-width:200px;
    border: 1px solid grey;
    border-radius: 5px;
    padding-left: 2px;
    width:100%;
  }

  .filter-sort {
    margin-left: 10px;
  }

  .btn-div {
    margin-left:30px;
    margin-right:10px;
  }

  .button {
    height:30px;
    width:30px;
    padding:0px;
    margin-top: 5px;
  }

  .btn-img {
      height: 20px;
      width: 20px;
    }

  .filter-invalid-span {
    color: red;
    font-size: 12px;
    padding:5px;
  }

  .ne-valja
  {
    background-color: rgb(255, 212, 212);
  }

  .activeUsers
  {
    display: flex;
    flex-direction: row;
    justify-content: center;
    align-items: center;
    margin:5px;
  }

  .cekiranje
  {
    margin-left: 15px;
    margin-right: 15px;
    height:20px;
    width:20px;
  }

  .no-results {
    font-weight: bold;
    font-size: 18px;
    word-break: break-word;
    display: flex;
    flex-direction: column;
    align-items: center;
    text-align: center;
  }

  .no-trips
  {
    width: 100%;
    text-align: center;
    font-size: 26px;
    margin-top: 50px;
    font-weight: 600;
    font-style: italic;
    color: grey;
  }

  .button-div
  {
    width: 100%;
    margin-top: 30px;
    display: flex;
    flex-direction: column;
    align-items: center;
    justify-content: center;
  }

  @media only screen and (max-width:650px)
  {
    .filter-sticky {
      align-self: center;
    }

    .filter-parameters
    {
      padding: 10px;
      flex-direction: column;
      align-items: flex-start;
      align-self:stretch;
      align-items: stretch;
    }

    .btn-img {
      height: 20px;
      width: 20px;
    }

    .btn-div {
      margin-left: 5px;
      align-self:center;
    }

    .button {
      width:100px !important;
    }

    .filter-name-wrap {
      width:100%;
    }

    .filter-name {
      max-width: unset;
      width:100%;
    }
  }

  @media only screen and (max-width: 499px)
  {

    .filter-rating {
      width: 30px;
    }

    .filter-invalid-span {
      font-size:12px;
    }

  }

</style>