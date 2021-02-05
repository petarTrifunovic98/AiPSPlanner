<template>
  <div id="app">
    <div class = "page-wrapper bg-light">
      <router-view />
    </div>
  </div>
</template>

<style>
#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
}

#nav {
  padding: 30px;
}

#nav a {
  font-weight: bold;
  color: #2c3e50;
}

#nav a.router-link-exact-active {
  color: #42b983;
}

.bold {
  font-weight: bold;
}

.cover {
  background-size: cover;
  background-repeat: no-repeat;
  background-attachment: fixed;
}

.hero {
  position: relative;
}

.hero-body {
  padding: 3rem 1.5rem;
}

.hero-bg {
  background-image: linear-gradient(rgba(0, 0, 0, 0.4), rgba(0, 0, 0, 0.4)), url('https://images.unsplash.com/photo-1531263060782-b024de9b9793?ixlib=rb-1.2.1&ixid=eyJhcHBfaWQiOjEyMDd9&auto=format&fit=crop&w=1650&q=80');
  background-size: cover;
  background-repeat: no-repeat;
  background-attachment: fixed;
}

.page-wrapper
{
  min-height: 93vh;
}

.top-div
{
  background-color: rgba(138, 141, 145, 0.767);
  height: 12vh;
  color: white;
  padding: 27px;
  padding-left:100px;
  font-size: 35px;
  font-weight: bold;
}

.please
{
  position: sticky;
  top: 0;
  width: 100%;
  background-color: white;
  z-index: 1000000000;
}

.input-padding
{
  padding:5px;
}
</style>
