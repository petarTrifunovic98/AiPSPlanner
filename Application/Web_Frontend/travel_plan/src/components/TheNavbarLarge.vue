<template>
    <nav class="navigacija">
        <div class = "navigacija-beginning">
            <router-link :to = "'/'" class="navbar-item" v-if="!isLogedIn">
                <h1 class="title is-4 naziv">TravelPlan</h1>
            </router-link>
            <router-link :to = "'/trips'" class="navbar-item" v-else>
                <h1 class="title is-4 naziv">TravelPlan</h1>
            </router-link>
            <div class="navbar-start" v-if="isLogedIn">
                <li class="nav-item dropdown">
                    <a class="nav-link svetli" href="#" id="navbarDropdownMenuLink" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                        <img  class = "slika" src = "../assets/menu.svg">
                    </a>
                    <span class = "kruzic" v-if="notificationNumber > 0"></span>
                    <div class="dropdown-menu" aria-labelledby="navbarDropdownMenuLink">
                        <router-link :to="'/notifications'" class="dropdown-item">
                            <img src = "../assets/notifications.svg">
                            <span class = "brojka" v-if="notificationNumber > 0"> {{notificationNumber}} </span>
                            <span class = "ikonica"> Notifications </span>
                        </router-link>
                    </div>
                </li>
            </div>
        </div>
        <div class="navigacija-end">
            <div class = "fleksa">
                <div v-if="!isLogedIn"> 
                    <router-link :to = "'/register'" class="button is-primary dugme">
                        <strong>Sign up</strong> 
                    </router-link>
                    <router-link :to = "'/login'" class="button is-light dugme">
                        <span> Log in </span>
                    </router-link>
                </div>
                <div class="navbar-item has-dropdown is-hoverable" v-else>
                    <span class="nav-item dropdown">
                        <button class="ime nav-link dropdown-toggle title is-6 btn btn-link svetli" href="#" id="navbarDropdown" role="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                            {{authUserUsername}}
                        </button>
                        <div class="dropdown-menu profil-meni" aria-labelledby="navbarDropdownMenuLink">
                            <router-link :to = "goToProfile()" class="dropdown-item">
                                <img src = "../assets/profile.svg">
                                <span class = "ikonica"> Profile </span>
                            </router-link>
                            <div class="dropdown-divider"></div>
                            <a @click="odjaviSe" class="dropdown-item">
                                <img src = "../assets/logout.svg">
                                <span  class = "ikonica"> Log out </span>
                            </a>
                        </div>
                    </span>
                </div>
            </div>
        </div>
    </nav>
</template>

<script>
export default {
    computed:
    {
      isLogedIn()
      {
          return this.$store.state.isLogedIn
      },
      authUserUsername()
      {
          return this.$store.state.authUser.username
      },
      notificationNumber()
      {
          return this.$store.state.notificationNumber
      }
    },
    methods:
    {
      odjaviSe()
      {
        this.$store.state.isLogedIn = false
        this.$router.push('/')
        this.$store.dispatch("logoutUser")    
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
        min-height: 70px;
        width: 100%;
        display: flex;
        flex-direction: row;
        justify-content: flex-start;
        position:sticky;
    }
    .navigacija-beginning
    {
        display: flex;
        flex-direction: row;
        flex-grow: 1;
        flex-shrink: 1;
    }
    .navigacija-end
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
        padding-top: 12px;
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
    .meni
    {   
        margin-top: 17px;
        margin-left: 10px;
        padding-bottom:0px;
        height: 28px;
    }
    .lista
    {
        margin-top: 0px;
    }
    .dugme
    {
        margin-left: 5px;
        margin-right: 5px;
    }
    .ime
    {
        color: black;
        font-size: 18px;
        padding-top: 5px;
        font-weight: 600;
    }
    .profil-meni
    {
        width: 10px;
        margin-right: 30px;
    }
    .slika
    {
        margin-top: 7px;
    }
    .svetli:hover 
    {
        background-color:rgb(243, 242, 242);
    }
    .naslov
    {
        padding-left: 10px;
        font-weight: bolder;
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
        margin-top: 15px;
        height: 12px;
        width: 12px;
    }
    .naziv
    {
        margin-top: 7px;
    }
    @media only screen and (max-width: 1088px)
    {
        a.navbar-item.router-link-active
        {
            padding-top: 12px;
        }
    }
</style>