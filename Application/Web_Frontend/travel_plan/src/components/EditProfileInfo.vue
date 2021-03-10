<template>
  <Spinner v-if="savingChanges" />
  <div v-else>
    <div class="main-container">
      <div class="picture-side">
        <div class="media-center">
          <p class="image is-128x128"
            @drop.prevent="onDrop" @dragenter.prevent="onDragEnter" @dragleave.prevent="onDragLeave" 
            @dragover.prevent @mouseenter.prevent="onDragEnter" @mouseleave.prevent="onDragLeave"
            @click="$refs.file.click()"
          >
            <img class="rounded-image" :src="newPicture ? 'data:;base64,'+ picture : require('../assets/no-picture.png')">
            <img 
              v-show="isDragged" :class="['rounded-image', 'semi-transparent']" src="@/assets/camera.png"
            >
          </p>
        </div>
          <div :class="[isDragged ? 'shown' : 'hidden', 'picture-msg']">
            <span>Click the picture or drag and drop a new one</span>
          </div>
        <div style="display:flex; flex-direction:column; align-items:center">
            <input type="file" ref="file" style="display: none" accept="image/*" @change="pictureSelected"/>
          <div>
            <label style="margin-top: 15px;" class = "register-label"> Name: </label>
            <div class="field">
              <input
                class="input is-medium" type="text" placeholder="Name"
                @blur="$v.changedUser.name.$touch()" v-model="changedUser.name"
              >
            </div>
            <div v-if="$v.changedUser.name.$error" class = "form-error">
              <span v-if="!$v.changedUser.name.required" class = "help is-danger"> 
                <span>Name is required </span>  
              </span>
            </div>

            <label style="margin-top: 15px;" class = "register-label"> Last name: </label>
            <div class="field">
              <input 
                class="input is-medium" type="text" placeholder="Last name"
                @blur="$v.changedUser.lastName.$touch()" v-model="changedUser.lastName"
              >
            </div>
            <div v-if="$v.changedUser.lastName.$error" class = "form-error">
              <span v-if="!$v.changedUser.lastName.required" class = "help is-danger">
                <span>Last name is required </span>  
              </span>
            </div>
          </div>
        </div>
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
        </b-list-group>
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
  </div>
</template>

<script>
import {required} from "vuelidate/lib/validators"
import ModalAreYouSure from "@/components/ModalAreYouSure"
import Spinner from "@/components/Spinner"
export default {
  components: {
    ModalAreYouSure,
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
      newPicture: this.user.picture,
      isDragged: false,
      dragCounter: 0,
      pictureChanged: false,
      openModalSave: false,
      openModalCancel: false,
      savingChanges: false,
    }
  },
  validations: {
    changedUser: {
      lastName: { required },
      name: { required }
    }
  },
  computed: {
    picture() {
      if(this.newPicture)
      {
        const splitted = this.newPicture.split(',')
        if (splitted.length == 2)
          return splitted[1]
        else 
          return this.newPicture
      }
      return null
    },
    isFormInvalid(){
      if(this.$v.changedUser.$invalid)
        return true
      return false
    }
  },
  methods: {
    saveChanges() {
      this.savingChanges = true
      this.openModalSave = false
      this.changedUser.picture = this.picture

      this.$store.state.authUser = this.changedUser
      this.$store.dispatch('editUser', {pictureChanged: this.pictureChanged})
      this.$emit('saveEditChanges')
    },
    pictureSelected(e) {
      const file = e.target.files[0];
      e.target.value = null
      this.addImage(file);
    },
    onDrop(e) {
      e.stopPropagation();
      const file = e.dataTransfer.files[0];
      this.addImage(file);
    },
    addImage(file) {
      if(!file) {
        this.isDragged = false;
        this.dragCounter = 0;
        return 
      }
      if(!file.type.match('image*')) {
        console.log('not an image!');
      }
      else {
        const reader = new FileReader();
        reader.onload = (e) => this.newPicture = e.target.result;
        reader.readAsDataURL(file);
      }
      this.isDragged = false;
      this.dragCounter = 0;
      this.pictureChanged = true
    },
    onDragEnter() {
      this.dragCounter ++;
      this.isDragged = true;
    },
    onDragLeave() {
      if(this.dragCounter>0)
        this.dragCounter --;
      if(!this.dragCounter)
        this.isDragged = false;
    },
    removePicture() {
      this.newPicture = null
    }
  }
}
</script>

<style scoped>
  
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