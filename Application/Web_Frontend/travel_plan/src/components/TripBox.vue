<template>
    <b-card border-variant="primary" no-body>
      <b-card-body>
        <div class="media-center">
          <a @click="goToAdditionalInfo">
            <p class="image is-96x96">
              <img class="rounded-image" :src="'data:;base64,' + tripProp.icon" v-b-popover.hover.bottom="'Click to view trip tips and tricks'">
            </p>
          </a>
        </div>
          
        <div class="info">
          <div class="info-element">
            <a @click="onViewTripPage"><span class="full-name-span" v-b-popover.hover.top="'View trip page'">{{tripProp.name}}</span></a>
          </div>
          <div class="info-element">
            <span class="desc">{{tripProp.description}}</span>
          </div>
          <div class="info-element">
            <span>{{tripProp.from | showTime}} - {{tripProp.to | showTime}}</span>
          </div>
        </div>
          <button type="button" class="btn btn-primary dugme" @click="onViewTripPage"> View trip page </button>
      </b-card-body>
    </b-card>
</template>

<script>
export default {
    props: {
      tripProp: {
        required: true, 
        type: Object
      }
    },
    data()
    {
      return {}
    },
    methods: {
      // goToTripPage() {
      //   //prepraviti da se stranici kao prop prosledi user
      //   this.$router.push({name: "PageViewProfile", params: {id: this.user.id, user: this.user, RequestSelect:this.RequestSelect}})
      // },
      onViewTripPage() {
        this.$router.push({name: "PageSpecificTrip", params: {id: this.tripProp.tripId, noEditRequest: false}})
      },
      goToAdditionalInfo()
      {
        this.$router.push({name: "PageTripAdditionalInfo", params: {id: this.tripProp.tripId}})
      }
    }
}
</script>

<style scoped>
  .dugme
  {
    padding: 8px;
    margin-top: 7px;
    height: fit-content;
    align-self: center;
    max-width:100%;
    justify-self: stretch;
  }

  .dodaj-dugme
  {
    display: flex;
    flex-direction: row;
    justify-content: center;
  }

  .rounded-image {
    border-radius: 60px;
    border: 2px solid grey;
    height:96px;
    width:96px;
    object-fit:cover;
    cursor:pointer;
  }

  .card-body {
    display:flex;
    flex-direction: row;
  }

  .card-header {
    font-size: 15px;
    font-weight: bold;
    border-top-left-radius: 15px;
    border-top-right-radius: 15px;
  }

  .card-footer {
    font-size: 15px;
    font-weight: bold;
    border-bottom-left-radius: 15px;
    border-bottom-right-radius: 15px;
    display:flex; 
    flex-direction:row;
  }

  .card {
    margin: 10px 30% 10px 30%;
    border-radius: 15px;
    width: 60%;
  }

  .rating-grade {
    font-size:15px; 
    font-weight: 700; 
    color:black;
  }

  .rating-message {
    padding-top:5px;
    font-size:18px;
    font-weight: bold;
  }

  .mt-2 {
    width:60%; 
    min-width:70px;
  }

  .grade-label {
    margin: 5px 10px 0 0;
  }

  .info {
    display:flex;
    flex-direction: column;
    margin-left: 10px;
    width:100%;
  }

  .info-element {
    display:flex;
    flex-direction: row;
    margin: 5px 0 0 5px;
    align-items: center;
  }

  .grade {
    word-break:normal;
  }

  .full-name-span {
    font-weight: bold;
    font-size: 20px;
    text-overflow: ellipsis;

  }

  .full-name-span:hover {
    color: rgb(100, 100, 100);
  }

  .desc {
    margin-right: 20%;
    align-self: center;
  }

  @media only screen and (max-width:700px)
  {
    .dugme {
      margin-top: 20px;
    }

    .card-footer {
      flex-direction: column;
    }

    .card-body {
      flex-direction:column;
      align-items:center;
      padding:10px;
    }

    .card {
      margin:10px 10% 0 10%;
      font-size: 15px;
      width: 80%;
    }

    .grade-label {
      margin: 5px 5px 0 0;
    }
    
    .info {
      margin-left: 0;
      display: flex;
      flex-direction: column;
      justify-content: center;
      align-items: center;
    }

    .info-element {
      flex-direction: column;
      align-items: flex-start;
      margin-top:15px;
    }

    .mt-2 {
      width:80%;
    }

    .desc {
      margin-right: 0px;
    }
  }

  /* @media only screen and (max-width:500px)
  {
    .card {
      margin: 40px 10px 40px 10px;
    }

  } */

</style>