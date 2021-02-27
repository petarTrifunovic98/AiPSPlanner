<template>
  <div id="map" class="map" ref="map"></div>
</template>

<script>
export default {
  props: {
    startingLocation: {
      required: true
    }
  },
  data() {
    return {
      map: null,
      marker: null
    }
  },
  methods: {
    mapClick(latLng) {
      const marker = new window.google.maps.Marker({
        position: latLng,
        map: this.map
      })
      if(this.marker)
        this.marker.setMap(null)
      this.marker = marker
      this.$emit('mapClick', {
        lat: latLng.lat(),
        lng: latLng.lng()
      })
    }
  },
  mounted() {
    this.map = new window.google.maps.Map(this.$refs["map"], {
      zoom: 17
    })
    const vm = this
    
    window.google.maps.event.addListenerOnce(this.map, 'idle', checkForMap)
    function checkForMap() {
      if(vm.map) {
        vm.map.setCenter({
          lat: vm.startingLocation.lat,
          lng: vm.startingLocation.lng
        })
        vm.map.setZoom(8)
        const marker = new window.google.maps.Marker({
          position: {lat: vm.startingLocation.lat, lng: vm.startingLocation.lng},
          map: vm.map
        })
        vm.marker = marker
      }
      else 
        setTimeout(checkForMap, 200)
    }
    // checkForMap()
    this.map.addListener('click', (event) => {
      this.mapClick(event.latLng)
    })
  }
}
</script>

<style scoped>
  .map {
    min-height: 400px;
    min-width: 500px;
    align-self: stretch;
  }
</style>