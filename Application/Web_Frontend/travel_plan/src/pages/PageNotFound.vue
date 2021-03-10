<template>
  <div class="container">
    <ErrorView :title="'Ooooops, the page you are trying to access doesn\'t exist'"
               :status="'404'"/>
  </div>
</template>

<script>
  import ErrorView from '@/components/ErrorView'
  export default {
    components: {
      ErrorView
    }
  }
</script>

<style scoped>
    .container
    {
        padding-top: 2px;
        padding-bottom: 50px;
    }
</style>