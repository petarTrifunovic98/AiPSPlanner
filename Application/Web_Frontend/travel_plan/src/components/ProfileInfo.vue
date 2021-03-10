<template>
  <div>
    <div class="main-container">
      <div class="picture-side">
        <div class="media-center">
          <p class="image is-128x128">
            <img class="rounded-image" :src="user.picture ? 'data:;base64,' + user.picture : require('../assets/no-picture.png')">
          </p>
        </div>
        <div class="content">
          {{ fullUserName }}
        </div>
      </div>
      <div class="personal-info">
        <b-list-group>
          <b-list-group-item class="l-group-title">
            <span class="info-title"> Personal info </span>
            <div class="l-group-btns" v-if="isMyProfile">
              <b-button 
                class="button is-primary title-btn"
                @click="goToProfileEdit()"
              >
                <img class="slika-dugme" src="../assets/edit.png">
                <strong>Edit</strong>
              </b-button>
              <b-button 
                class="button is-primary title-btn"
                @click="goToChangePass()"
              >
                <img class="slika-dugme" src="../assets/password.png">
                <strong>Change password</strong>
              </b-button>
            </div>
          </b-list-group-item>
          <b-list-group-item>
            <img 
              src="@/assets/username.svg" 
              height = "20" 
              width = "20"
              style = "margin-right: 15px"
            />
            <span class="list-value">{{ user.username }}</span>
          </b-list-group-item>
        </b-list-group>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  props: {
    user: {
      type: Object,
      required: true
    },
    isMyProfile: {
      type: Boolean,
      required: true
    }
  },
  data()
  {
    return{
      idHelp: null
    }
  },
  computed: {
    fullUserName() {
      return this.user.name + " " +this.user.lastName
    }
  },
  methods: {
    goToProfileEdit() {
      this.$emit("editProfile");
    },
    goToChangePass(){
      this.$emit("changePass");
    }
  }
}
</script>

<style scoped>
  
  .list-group {
    background-color:inherit;
  }
  .list-group-item {
    display: flex;
    justify-content: space-between;
    border: hidden;
    border-bottom: 1px solid lightgray;
    border-radius: 0px;
    align-items: center;
    background-color:inherit;
  }
  .list-key {
    font-weight: bold;
    font-size: 14px;
    flex-grow: 1;
    flex-shrink: 1;
    text-align: left;
  }
  .list-value {
    flex-grow: 2;
    flex-shrink: 1;
    text-align: left;
    font-size: 20px;
    margin-left: 30px;
    word-break: break-all;
  }
  .main-container {
    padding-top: 30px;
    margin-left: 10%;
    margin-right: 10%;
    display: flex;
    flex-direction: row;
    align-items:flex-start;
    border-radius: 10px;
  }
  .rounded-image {
    border-radius: 60px;
    border: 2px solid grey;
    height: 250px;
    width:250px;
    object-fit:cover;
  }
  .content {
    background-color: black;
    font-size: 20px;
    color:white;
    text-align: center;
    margin-top: 15px;
    margin-bottom: 2px;
    border-radius: 5px;
    word-break: break-all;
  }
  .info-title {
    font-size: 25px;
    font-weight: bold;
    color: black;
    flex-grow:1;
  }
  .is-128x128 {
    width: 250px;
    height: 250px;
  }
  .personal-info {
    display:flex;
    flex-direction: column;
    margin-left: 15px;
    flex-grow:2;
    margin-bottom: 30px;
  }
  .picture-side {
    flex-grow:1;
    margin-bottom: 30px;
  }
  .nav-buttons {
    display:flex;
    width: 100%;
    justify-content: center;
  }
  .ocene-dostignuca {
    margin: 10% 0 20px 0; 
    width: 100%;
    font-size: 20px;
    font-weight: bold;
  }
  .services {
    width:100%;
    font-size: 20px;
    font-weight: bold;
  }
  .addr-or-phone-item {
    border-bottom:dotted 2px lightgray;
    margin-bottom: 7px;
    padding-bottom: 7px;
  }
  .l-group-title {
    display:flex;
    flex-direction:row;
  }
  .title-btn {
    margin:5px 5px 0 5px;
    padding-bottom: 28px;
    padding-right: 15px;
    text-decoration: none;
  }
  .media-center {
    display:flex;
    flex-direction:column;
    align-items:center;
  }
  .l-group-btns {
    display:flex;
    flex-direction: column;
    margin-left:30px;
  }
  .l-item-progress {
    width:85%;
  }
  .slika-dugme
  {
    width: 22px;
    height: 22px;
    margin-right: 5px;
  }
  .dugmeRequest
  {
    margin-top:25px;
  }
  @media only screen and (max-width: 750px)
  {
    .main-container {
      flex-direction: column;
      padding-top: 30px;
      margin-left: 1%;
      margin-right: 1%;
      align-items:center;
      border-radius: 10px;
      font-size: 10px;
    }
    .list-value {
      flex-grow: 2;
      flex-shrink: 1;
      text-align: left;
      font-size: 17px;
      margin-left: 2%;
    }
    .l-group-title {
      border-top:1px solid lightgray;
    }
    .l-group-btns {
      display:flex;
      flex-direction: column;
      flex-grow:1;
    }
    .list-group-item {
      padding-left:1%;
      padding-right:1%;
    }
    .l-item-progress {
      width:100%;
    }
  }
</style>