<template>
  <Spinner v-if="savingChanges" />
  <div v-else>
    <div class="main-container">
      <div class="picture-side">
        <div class="media-center">
          <p class="image is-128x128">
            <img class="rounded-image" :src="user.picture ? 'data:;base64,' + user.picture : require('../assets/no-picture.png')">
          </p>
        </div>
          <div class="content ime">
          {{ fullUserName }}
        </div>
      <div class="personal-info">
        <b-list-group >
          <b-list-group-item class="l-group-title">
            <span class="info-title" > Personal info </span>

            <div class="l-group-btns">
              <b-button :disabled="isFormInvalid" class="button is-primary title-btn" @click="openModalSave = true">
                <strong>Save changes</strong>
              </b-button>
              <b-button class="button is-primary title-btn" @click="openModalCancel = true">
                <strong>Dismiss changes</strong>
              </b-button>
            </div>
          </b-list-group-item>
          <b-list-group-item>
            <span v-text="'Old password'"></span>
            <div class="pass-and-warning">
              <input 
                class="input is-medium input-full" type="password" :placeholder="'Old password'" v-model="oldPassword"
              >
            </div>
          </b-list-group-item>
          <b-list-group-item>
            <span v-text="'New password'"></span>
            <div class="pass-and-warning">
              <input 
                class="input is-medium input-full" type="password" :placeholder="'Password'" 
                autocomplete="new-password" v-model="newPassword" @blur="$v.newPassword.$touch()"
              >
              <span v-if = "!$v.newPassword.minLength" class = "help is-danger">
                <span v-text="'Password must be at least 6 characters long'">  
                </span>
              </span>
            </div>
          </b-list-group-item>
          <b-list-group-item v-if="newPassword.length > 0">
            <span v-text="'Password confirmation'"></span>
            <div class="pass-and-warning">
              <input 
                class="input is-medium input-full" type="password" :placeholder="'Password confirmation'" 
                autocomplete="off" v-model="confirmPassword" @blur="$v.confirmPassword.$touch()"
              >
              <span v-if = "!$v.confirmPassword.sameAs" class = "help is-danger">
                <span v-text="'Password conformation needs to be the same as the password'">  
                </span>
              </span>
            </div>
          </b-list-group-item>
        </b-list-group>
      </div>
      </div>
    </div>
    <ModalAreYouSure 
      :naslov="'Save changes'"
      :tekst="'Are you sure you want to save all the changes?'"
      @close="openModalSave = false" @yes="saveChanges" v-if="openModalSave"
    />
    <ModalAreYouSure 
      :naslov="'Dismiss changes'"
      :tekst="'Are you sure you want to dismiss all the changes?'"
      @close="openModalCancel = false" @yes="$emit('cancelChanges')" v-if="openModalCancel"
    />
    <ModalError 
      :naslov="'Wrong password'"
      :text="'You entered an incorrect original password.'"
      @close="setWrongFalse" v-if="wrongOriginalPass"
    />
  </div>
</template>

<script>
import {required, minLength, sameAs} from "vuelidate/lib/validators"
import ModalAreYouSure from "@/components/ModalAreYouSure"
import ModalError from "@/components/ModalError"
import Spinner from "@/components/Spinner"
export default {
  components: {
    ModalAreYouSure,
    ModalError,
    Spinner
  },
  props: {
    user: {
      required: true,
      type: Object
    }
  },
  created() {
    
  },
  data() {
    return {
      changedUser: JSON.parse(JSON.stringify(this.user)),
      openModalSave: false,
      openModalCancel: false,
      savingChanges: false,
      newPassword: "",
      confirmPassword: "",
      oldPassword: ""
    }
  },
  validations: {
    newPassword: { minLength: minLength(6) },
    confirmPassword: {sameAs: sameAs("newPassword")}
  },
  computed: {
    isFormInvalid(){
      if(this.newPassword.length > 0 && (this.$v.newPassword.$invalid || this.$v.confirmPassword.$invalid))
        return true
      return false
    },
    fullUserName() {
      return this.user.name + " " +this.user.lastName
    },
    wrongOriginalPass(){
        return this.$store.state.wrongOriginalPass
    }
  },
  methods: {
    saveChanges() {
      this.savingChanges = true
      this.openModalSave = false

      this.$store.dispatch('changePassword', {newPassword: this.newPassword, oldPassword: this.oldPassword})
      
      let vm = this
      function callback() {
          console.log("Called callback")
        if(vm.$store.state.isDataLoaded == true) {
            console.log("Data is loaded")
          if(!vm.$store.wrongOriginalPass)
          {             
            vm.$emit('saveEditChanges')
            console.log("Password is correct")
          }
        }
        else {
          setTimeout(callback, 200);
          console.log("Setting callback")
        }
      }
      
      callback()
    },
    setWrongFalse()
    {
        this.$store.state.wrongOriginalPass = false
    }
  }
}
</script>

<style scoped>
  .ime
  {
      padding-left: 20px;
      padding-right: 20px;
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
    margin-right:1%;
    margin-left:20px;
    width:100%;
  }
  .over-flow {
    width:100%;
    word-break: normal;
    overflow-wrap:break-word;
  }
  .main-container {
    padding-top: 30px;
    margin-left: 10%;
    margin-right: 10%;
    display: flex;
    flex-direction: row;
    border-radius: 10px;
  }
  .rounded-image {
    border-radius: 60px;
    border: 2px solid grey;
    height: 250px;
    width:250px;
    object-fit:cover;
    position: relative;
    top:0;
    left:0; 
    cursor:pointer;
  }
  .transparent {
    position: absolute;
    opacity: 0;
  }
  .semi-transparent {
    position: absolute;
    opacity: 0.5;
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
    border-radius: 60px;
  }
  .personal-info {
    display:flex;
    flex-direction: column;
    margin-left: 15px;
    flex-grow:2;
    margin-bottom:30px;
  }
  .picture-side {
    flex-grow:1;
    display:flex;
    flex-direction:column;
    align-items:center;
    margin-right:15px;
    margin-bottom:30px;
  }
  .remove-pic-div {
    width: fit-content;
    align-self: flex-start;
    margin-bottom: 5px;
    display: flex;
    align-items: center;
    font-weight: 600;
  }
  .remove-pic-div:hover {
    cursor: pointer;
    color: blue;
  }
  .remove-pic {
    margin-right: 5px;
    border-radius: 10px;
  }
  .flex-row-elements {
    display: flex;
  }
  .acc-or-remove-icon {
    margin: 0 10px 0 5px; 
    cursor:pointer;
  }
  .address {
    border-bottom:dotted 2px lightgray;
    margin-bottom: 7px;
    padding-bottom: 7px;
  }
  .phones {
    margin-bottom: 10px;
  }
  .title-btn {
    margin:5px 5px 0 5px;
  }
  .l-group-title {
    display:flex;
    flex-direction:row;
  }
  .shown {
    visibility:visible;
  }
  .hidden {
    visibility:hidden;
  }
  .l-group-btns {
    display:flex;
    flex-direction: column;
    margin-left:30px;
  }
  .picture-msg {
    width:250px;
    font: size 12px;
    text-align:center;
  }
  .visible {
    visibility: visible;
  }
  .invisible {
    visibility: hidden;
    height:0px;
    position: absolute;
    top: 0px;
  }
  .map-wrap {
    padding-bottom: 100px;
  }
  .pass-and-warning {
    margin-left: 20px;
    width: 100%;
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
    }
    .list-value {
      flex-grow: 2;
      flex-shrink: 1;
      text-align: left;
      font-size: 17px;
      margin-left: 3%;
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
    .picture-side {
      margin-right:0;
    }
  }
</style>