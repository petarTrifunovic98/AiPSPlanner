<template>
    <b-card class="col-6" border-variant="primary" no-body>
      <b-card-body>
        <div class="media-center">
          <p class="image is-96x96">
            <img class="rounded-image" :src="'data:;base64,' + trip.icon">
          </p>
        </div>
          
        <div class="info">
          <div class="info-element">
            <span class="full-name-span">{{trip.name}}</span>
          </div>
          <div class="info-element">
            <span>{{trip.description}}</span>
          </div>
          <div class="info-element">
            <span>{{trip.from | showTime}} - {{trip.to | showTime}}</span>
          </div>
        </div>
          <button type="button" class="btn btn-primary dugme col-3"> View trip page </button>
      </b-card-body>
    </b-card>
</template>

<script>
export default {
    props: {
      trip: {
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
    margin: 10px 15% 10px 15%;
    border-radius: 15px;
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
    word-break:break-all;
    margin: 5px 0 0 5px;
    align-items: center;
  }

  .grade {
    word-break:normal;
  }

  .full-name-span:hover {
    cursor: pointer !important;
    text-decoration: underline;
    color:lightseagreen;
  }

  @media only screen and (max-width:650px)
  {
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
      
    }

    .grade-label {
      margin: 5px 5px 0 0;
    }
    
    .info {
      margin-left: 0;
    }

    .info-element {
      flex-direction: column;
      align-items: flex-start;
      margin-top:15px;
    }

    .mt-2 {
      width:80%;
    }

  }

  /* @media only screen and (max-width:500px)
  {
    .card {
      margin: 40px 10px 40px 10px;
    }

  } */

</style>