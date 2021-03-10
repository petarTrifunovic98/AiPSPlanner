<template>
    <nav class="navigacija">
        <div class = "top-navigacija"> 
            <div class="top-navigacija-beginning">
                <router-link :to = "'/'" class="navbar-item" v-if="!isLogedIn">
                    <h1 class="title is-4">TravelPan</h1>
                </router-link>
                <router-link :to = "'/trips'" class="navbar-item" v-else>
                    <h1 class="title is-4">TravelPlan</h1>
                </router-link>
                <div class="navbar-start" v-if="isLogedIn">
                    <li class="nav-item dropdown">
                        <a class="nav-link" href="#" id="navbarDropdownMenuLink" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                            <img  class = "slika" src = "../assets/menu.svg">
                        </a>
                        <span class = "kruzic" v-if="notificationNumber > 0"></span>
                        <div class="dropdown-menu" aria-labelledby="navbarDropdownMenuLink">
                            <router-link :to="'/notifications'" class="dropdown-item">
                                <img src = "../assets/notifications.svg">
                                <span class = "brojka" v-if="notificationNumber > 0"> {{notificationNumber}} </span>
                                <span class = "ikonica"> Notifications </span>
                            </router-link>
                            <router-link :to = "goToProfile()" class="dropdown-item">
                                <img src = "../assets/profile.svg">
                                <span class = "ikonica"> Profile </span>
                            </router-link>
                            <router-link :to="'/new-trip'" class="dropdown-item">
                                <img src = "../assets/add.svg">
                                <span class = "ikonica"> New trip </span>
                            </router-link>
                            <router-link :to="'/'" class="dropdown-item">
                                <img src = "../assets/map.svg" class="slicshka">
                                <span class = "ikonica"> My trips </span>
                            </router-link>
                            <router-link :to="'/items'" class="dropdown-item">
                                <img src = "../assets/checklist.svg" class="slicshka">
                                <span class = "ikonica"> My items </span>
                            </router-link>
                            <router-link :to="'/teams'" class="dropdown-item">
                                <img src = "../assets/team.svg" class="slicshka">
                                <span class = "ikonica"> My teams </span>
                            </router-link>
                            <div class="dropdown-divider"></div>
                            <a @click="odjaviSe" class="dropdown-item">
                                <img src = "../assets/logout.svg">
                                <span class = "ikonica"> Log out </span>
                            </a>
                        </div>
                    </li>
                </div>
            </div>
        </div>
        <div class = "bottom-navigacija" v-if="!isLogedIn"> 
            <router-link :to = "'/register'" class="button is-primary dugme">
              <strong>Sign up</strong>
            </router-link>
            <router-link :to = "'/login'" class="button is-light dugme">
              <span> Log in </span>
            </router-link>
        </div>
    </nav>
</template>

<script>
export default {
    data() {
        return {
            waitingLogOut: false
        }
    },
    computed:
    {
      isLogedIn()
      {
          return this.$store.state.isLogedIn
      },
      notificationNumber()
      {
          return this.$store.state.notificationNumber
      },
      hasEditRights()
      {
          return this.$store.state.hasEditRights
      }
    },
    watch: {
        hasEditRights(newValue, oldValue) 
        {
            if(oldValue == true && newValue == null && this.waitingLogOut)
            {
                this.waitingLogOut = false
                this.$store.dispatch("logoutUser")
            }
        }
    },
    methods:
    {
      odjaviSe()
      {
        this.$toasted.clear()
        this.$store.state.logedIn = false
        if(this.hasEditRights)
        {
            this.waitingLogOut = true
        }
        else
        {
            this.$store.dispatch("logoutUser")  
        }
        this.$router.push('/')
      },
      goToProfile()
      {
        return {
            name: "PageViewProfile", 
            params: {
                id: this.$store.state.authUser.userId, 
                user: this.$store.state.authUser
            }
        }
      }
    }
}
</script>

<style scoped>
    .navigacija
    {
        padding: 5px;
        min-height: 60px;
        width: 100%;
        display: flex;
        flex-direction: column;
        justify-content: space-around;
    }
    .bottom-navigacija
    {
        display: flex;
        flex-direction: row;
        justify-content: center;
        width: 100%;
    }
    .dugme
    {
        margin-right: 20px;
        margin-left: 20px;
        margin-bottom: 5px;
        margin-top: 0px;
    }
    .top-navigacija
    {
        display: flex;
        flex-direction: row;
        width:100%;
        justify-content: flex-start;
    }
    .top-navigacija-beginning
    {
        display: flex;
        flex-direction: row;
        flex-grow: 1;
        flex-shrink: 1;
    }
    .top-navigacija-end
    {
        justify-self: flex-end;
        flex-grow: 1;
        flex-shrink: 1;
    }
    .fleksa
    {
        display: flex;
        flex-direction: row;
        justify-content: flex-end;
        padding-right: 7px;
    }
    .zastava
    {
        height: 20px;
        margin-top: 7px;
        margin-left: 5px;
    }
    .ikonica
    {
        margin-left: 10px
    }
    .ikonica-uvucena
    {
        margin-left: 20px
    }
    .language
    {
        margin:5px;
    }
    .crtka
    {
        margin-top:5px
    }
    .slichka
    {
        width:24px;
        height:24px;
    }
    .brojka
    {
        background-color: red;
        width:fit-content;
        height:fit-content;
        color:white;
        border-radius:10px;
        padding: 1px;
        font-weight: 600;
        font-size: 12px;
        margin-bottom: 15px;
        margin-left: -10px;
        margin-top: -5px;
    }
    .kruzic
    {
        background-color: red;
        border-radius:10px;
        margin-left: -23px;
        margin-top: 9px;
        height: 12px;
        width: 12px;
    }
    .slicshka
    {
        width: 23px;
        height: 23px;
    }
</style>