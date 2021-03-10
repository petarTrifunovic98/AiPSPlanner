<template>
  <div class="notFoundContainer">
    <h2 class="subtitle">{{status}}<span> :(</span></h2>
    <div class="m-b-xxl">
      <h1 class="title">{{title}}</h1>
    </div>
  </div>
</template>

<script>
  export default {
    props: {
      title: {
        required: true,
        type: String
      },
      status: {
        required: true,
        type: String
      }
    }
  }
</script>

<style scoped>
  .title {
    font-size: 40px;
  }
  .subtitle {
    font-size: 140px;
    text-align: center;
  }
  .notFoundContainer {
    margin-top: 80px;
  }
  @media only screen and (max-width: 499px)
  {
    .subtitle 
    {
        font-size: 120px;
        text-align: center;
    }
    .title 
    {
        font-size: 30px;
    }
  }
</style>