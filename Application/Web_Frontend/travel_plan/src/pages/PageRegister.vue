<template>
  <section class="hero is-success is-fullheight">
    <div class="hero-body" v-if="!tryRegister || !isDataLoaded || !isLogedIn">
      <div class="container has-text-centered">
        <div class="column is-4 is-offset-4">
          <h3 class="title has-text-grey">Register</h3>
          <p class="subtitle has-text-grey">Please register to proceed.</p>
          <div class="box">
            <form>
              <div class="field">                
                <div class="control">
                  <label class = "register-label"> *Name: </label>
                    <input class="input is-large"
                         type="text"
                         placeholder="Name"
                         v-model = "form.name"
                         @blur="$v.form.name.$touch()">
                </div>
                <div v-if = "$v.form.name.$error" class = "form-error">
                    <span v-if = "!$v.form.name.required"
                          class = "help is-danger"> 
                            <span>  Name is required </span> 
                          </span>
                </div>
              </div>
              <div class="field">
                <div class="control">
                  <label class = "register-label"> *Last name: </label>
                    <input class="input is-large"
                         type="text"
                         placeholder="Last name"
                         v-model = "form.lastName"
                         @blur="$v.form.lastName.$touch()">
                </div>
                <div v-if = "$v.form.lastName.$error" class = "form-error">
                    <span v-if = "!$v.form.lastName.required"
                        class = "help is-danger"> 
                        <span>  Last name is required </span>  
                    </span>
                </div>
              </div>
              <div class="field">
                <div class="control">
                  <label class = "register-label"> *Username: </label>
                  <input class="input is-large"
                         type="text"
                         placeholder="Username"
                         v-model = "form.username"
                         @blur="$v.form.username.$touch()">
                </div>
                <div v-if = "$v.form.username.$error" class = "form-error">
                    <span v-if = "!$v.form.username.required"
                          class = "help is-danger">
                            <span>Username is required </span>  
                        </span>
                </div>
              </div>
              <div class="field">
                <div class="control">
                  <label class = "register-label"> *Password: </label>
                    <input class="input is-large"
                         type="password"
                         placeholder="Password"
                         autocomplete="new-password"
                         v-model = "form.password"
                         @blur="$v.form.password.$touch()">
                </div>
                <div v-if = "$v.form.password.$error" class = "form-error">
                    <span v-if = "!$v.form.password.required"
                          class = "help is-danger">
                            <span>Password is required </span>  
                          </span>
                    <span v-if = "!$v.form.password.minLength"
                          class = "help is-danger">
                            <span>Password must be at least 6 characters long</span>  
                    </span>
                </div>
              </div>
              <div class="field">
                <div class="control">
                  <label class = "register-label"> *Password conformation: </label>
                    <input class="input is-large"
                         type="password"
                         placeholder="Password Confirmation"
                         autocomplete="off"
                         v-model = "form.passwordConformation"
                         @blur="$v.form.passwordConformation.$touch()">
                </div>
                <div v-if = "$v.form.passwordConformation.$error" class = "form-error">
                    <span v-if = "!$v.form.passwordConformation.required"
                          class = "help is-danger">
                            <span>Password conformation is required</span> 
                          </span>
                    <span v-if = "!$v.form.passwordConformation.sameAs"
                          class = "help is-danger">
                            <span>Password conformation needs to be the same as the password </span> 
                    </span>
                </div>
              </div>
              <button :disabled="isFormInvalid || !isDataLoaded"
                @click.prevent = "register" 
                type="submit" 
                class="button is-block is-info is-large is-fullwidth">
                  <div class="spinner-border ucitavanje" role="status" v-if="!isDataLoaded">
                    <span class="sr-only">Loading...</span>
                  </div>
                  <span v-if="isDataLoaded">Register</span>
              </button>
            </form>
            <p class = "text-danger upozorenje" v-if="tryRegister && isDataLoaded && message">
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
            <router-link :to = "'/login'">
                <span>Login</span>
            </router-link>
          </p>
          <div id="mapid"></div>
        </div>
      </div>
    </div>
  </section>
</template>

<script>
    import {required, minLength, sameAs} from "vuelidate/lib/validators"
    import { mapGetters } from "vuex"
    export default {
        data(){
          return{
            form:
            {
              name: null,
              lastName: null,
              username: null,
              password: null,
              passwordConformation: null
            },
            tryRegister: false
          }
        },
        computed:
        {
          ...mapGetters({
            isDataLoaded: 'getIsDataLoaded',
            message: 'getMessage'
          }),
          isFormInvalid(){
            return this.$v.form.$invalid
          },
          isLogedIn()
          {
            return this.$store.state.logedIn
          }
        },
        validations:
        {
            form:
            {
                name: { required },
                lastName: { required },
                username: { required },
                password:{ required, minLength: minLength(6)},
                passwordConformation: { required, sameAs: sameAs("password") }
            }
        },
        methods:
        {
            register()
            {
              this.$v.form.$touch()
              this.$store.dispatch("createUser", this.form)
              this.tryRegister = true
            },
            navigateTo()
            {
              this.$router.push('/profile/' + this.$store.state.authUser.id)
            }
        }
    } 
</script>

<style scoped>
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

  .register-label
  {
    margin-left: 5px;
    font-size: 18px;
  }

  .upozorenje
  {
    margin-top: 10px;
    font-weight: lighter;
  }

  #mapid { height: 180px; }
</style>