<template>
  <section class="hero is-success is-fullheight">   
    <div class="hero-body">
      <div class="container has-text-centered">
        <div class="column is-4 is-offset-4">
          <h3 class="title has-text-grey">Login</h3>
          <p class="subtitle has-text-grey">Please login to proceed.</p>
          <div class="box">
            <form>
              <div class="field">
                <div class="control">
                  <label class = "login-label"> *Username: </label>
                  <input class="input is-large"
                         type="text"
                         placeholder="Username"
                         autofocus=""
                         v-model="form.username"
                         @blur="$v.form.username.$touch()">
                </div>
                <div v-if = "$v.form.username.$error" class = "form-error">
                    <span v-if="!$v.form.username.required"
                          class = "help is-danger">
                      <span> Username is required </span> 
                    </span>
                </div>
              </div>
              <div class="field">
                <div class="control">
                  <label class = "login-label"> *Password: </label>
                    <input class="input is-large"
                         type="password"
                         placeholder="Password"
                         autocomplete="current-password"
                         v-model="form.password"
                         @blur="$v.form.password.$touch()">
                </div>
                <div v-if = "$v.form.password.$error" class = "form-error">
                  <span v-if = "!$v.form.password.required"
                        class = "help is-danger">
                    <span> Password is required </span>
                  </span>    
                </div>
              </div>
              <button @click="loginFunc" 
                      class="button is-block is-info is-large is-fullwidth"
                      :disabled="isFormInvalid || !isDataLoaded">
                <div class="spinner-border ucitavanje" role="status" v-if="!isDataLoaded">
                  <span class="sr-only">Loading...</span>
                </div>
                <span v-if="isDataLoaded">Login</span>
              </button>
            </form>
            <p class = "text-danger upozorenje" v-if="TryLogIn && isDataLoaded && message">
              {{message}}
            </p>
            <p class = "text-danger upozorenje">
              Elements marked with * are required
            </p>
          </div>
          <p class="has-text-grey">
            <router-link :to = "'/'">
              <span>Home</span>
            </router-link>
            &nbsp;·&nbsp;
            <router-link :to = "'/register'">Sign Up</router-link>
          </p>
        </div>
      </div>
    </div>
  </section>
</template>

<script>
  import {required} from "vuelidate/lib/validators"
  import { mapGetters } from "vuex"
  export default {
    data(){
      return {
        form:
        {
          username : null,
          password : null
        },
        TryLogIn: false
      }
    },
    validations:
    {
      form:
      {
        username: { required },
        password: { required }
      }
    },
    computed:
    {
      ...mapGetters({
        isDataLoaded: 'getIsDataLoaded',
        message: 'getMessage'
      }),
      isFormInvalid() {
        return this.$v.form.$invalid
      },
      isLogedIn()
      {
        return this.$store.state.logedIn
      }
    },
    methods:
    {
      loginFunc()
      {
        this.$v.form.$touch()
        this.$store.dispatch("fillAuthUser", this.form)
        this.TryLogIn = true
      }
    }
  }
</script>

<style scoped>
  .ceoEkran
  {
    width: 100%;
    height:75vh;
  }

  .hero.is-success {
    background: #F2F6FA;
  }
  .hero .nav, .hero.is-success .nav {
    -webkit-box-shadow: none;
    box-shadow: none;
  }
  .box {
    margin-top: 1rem;
  }
  .avatar {
    margin-top: -70px;
    padding-bottom: 20px;
  }
  .avatar img {
    padding: 5px;
    background: #fff;
    border-radius: 50%;
    -webkit-box-shadow: 0 2px 3px rgba(10,10,10,.1), 0 0 0 1px rgba(10,10,10,.1);
    box-shadow: 0 2px 3px rgba(10,10,10,.1), 0 0 0 1px rgba(10,10,10,.1);
  }
  input {
    font-weight: 300;
  }
  p {
    font-weight: 700;
  }
  p.subtitle {
    padding-top: 1rem;
  }

  .login-label
  {
    margin-left: 5px;
    font-size: 18px;
  }

  .upozorenje
  {
    margin-top: 10px;
    font-weight: lighter;
  }

  .ucitavanje
  {
    margin-right:7px;
  }
</style>