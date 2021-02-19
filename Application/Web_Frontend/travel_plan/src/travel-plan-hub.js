import { HubConnectionBuilder, LogLevel } from '@aspnet/signalr'

export default {
  install (Vue) {
    // use a new Vue instance as the interface for Vue components to receive/send SignalR events
    // this way every component can listen to events or send new events using this.$travelPlanHub

    const travelPlanHub = new Vue()
    Vue.prototype.$travelPlanHub = travelPlanHub

    // Provide methods to connect/disconnect from the SignalR hub
    let connection = null
    let startedPromise = null
    let manuallyClosed = false

    Vue.prototype.startSignalR = () => {
      connection = new HubConnectionBuilder()
        .withUrl("https://localhost:44301/travel-plan-hub")
        .configureLogging(LogLevel.Debug)
        .build()

      
      addSignalREventListener('EditTripInfo')

      addSignalREventListener('AddAddOn')
      addSignalREventListener('RemoveAddOn')
      addSignalREventListener('EditAddOn')

      addSignalREventListener('AddLocation')
      addSignalREventListener('RemoveLocation')
      addSignalREventListener('EditLocation')

      addSignalREventListener('AddAccommodation')
      addSignalREventListener('RemoveAccommodation')
      addSignalREventListener('EditAccommodation')

      addSignalREventListener('AddAccommodationPicture')
      addSignalREventListener('RemoveAccommodationPicture')

      addSignalREventListener('AddItem')
      addSignalREventListener('EditItem')
      addSignalREventListener('RemoveItem')
      
      addSignalREventListener('AddMemberToTrip')
      addSignalREventListener('RemoveUserFromTrip')

      addSignalREventListener('ChangeVotable')

      addSignalREventListener('AddItemNotification')
      addSignalREventListener('EditItemNotification')
      addSignalREventListener('RemoveItemNotification')

      function addSignalREventListener(name) {
        connection.on(name, (payload) => {
          travelPlanHub.$emit(name, payload)
        })
      }

      function start () {
        startedPromise = connection.start()
          .catch(err => {
            console.error('Failed to connect with hub', err)
            return new Promise((resolve, reject) => setTimeout(() => start().then(resolve).catch(reject), 5000))
          })
        return startedPromise
      }

      connection.onclose(() => {
        if (!manuallyClosed) start()
      })

      // Start everything
      manuallyClosed = false
      start()
    }

    Vue.prototype.stopSignalR = () => {
      if (!startedPromise) return

      manuallyClosed = true
      return startedPromise
        .then(() => connection.stop())
        .then(() => { startedPromise = null })
    }

    travelPlanHub.JoinTripGroup = (tripId) => {
      if (!startedPromise) return

      return startedPromise
        .then(() => connection.invoke('JoinTripGroup', tripId))
        .catch(console.error)
    }

    travelPlanHub.JoinItemGroup = (userId) => {
      if (!startedPromise) return

      return startedPromise
        .then(() => connection.invoke('JoinItemGroup', userId))
        .catch(console.error)
    }

    travelPlanHub.LeaveTripGroup = (tripId) => {
      if(!startedPromise) return

      return startedPromise
      .then(() => connection.invoke('LeaveTripGroup', tripId))
      .catch(console.error)
    }

    travelPlanHub.LeaveItemGroup = (userId) => {
      if(!startedPromise) return

      return startedPromise
      .then(() => connection.invoke('LeaveItemGroup', userId))
      .catch(console.error)
    }
  }
}