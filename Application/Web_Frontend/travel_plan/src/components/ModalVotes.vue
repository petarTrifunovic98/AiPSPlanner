<template>
  <transition name="modal">
    <div class="modal-mask">
      <div class="modal-wrapper">
        <div class="modal-container">

          <div class="modal-header">
            <h3 name="header">
              Votes
            </h3>
          </div>

          <div class="modal-body">
            <slot name="body">
              <div v-if="positiveVotes && negativeVotes">
                <div v-if="myVote" class="my-vote">
                  <span class="my-vote-text"> You voted </span>
                  <img :src="require('../assets/' + (myVote.positive ? 'like.png' : 'dislike.png'))" class="vote-pic" >
                </div>
                <div :class="['my-vote', 'change-vote', myVote.positive ? 'vote-red' : 'vote-green']" v-if="myVote && canVote" @click="changeVote"> 
                  <span class="my-vote-text"> Change to </span>
                  <img :src="require('../assets/' + (myVote.positive ? 'dislike.png' : 'like.png'))" class="vote-pic" >
                </div>
                <button type="button" class="btn btn-primary dugme" style="margin: 0px 0px 10px 0px;" v-if="myVote && canVote" @click="removeVote"> 
                  <span class="my-vote-text"> Remove vote </span>
                  <img src="../assets/delete_item.png" class="vote-pic" >
                </button>
                <div v-else-if="canVote" class="my-vote">
                  <span class="my-vote-text"> Vote </span>
                  <img src="../assets/like.png" class="vote-pic clickable" @click="vote(true)">
                  <img src="../assets/dislike.png" class="vote-pic clickable" @click="vote(false)">
                </div>
                <div v-else class="no-votes">
                  You have not voted yet
                </div>
              </div>
              <b-tabs content-class="mt-3">
                <b-tab title="Positive" active>
                  <div v-if="positiveVotes && travelers">
                    <div v-for="user in positiveUsers" :key="user.userId" class="one-vote">
                      <div>
                        <img :src="'data:;base64,' + user.picture" class="rounded-image" v-if="user.picture">
                        <img :src="require('../assets/no-picture.png')" class="rounded-image" v-else>
                        <span class="name">{{user.name}} {{user.lastName}}</span>
                      </div>
                      <img src="../assets/like.png" class="vote-pic" >
                    </div>
                    <div v-if="positiveVotes && positiveVotes.length == 0" class="no-votes">
                      No positive votes
                    </div>
                  </div>
                  <Spinner v-else />
                </b-tab>
                <b-tab title="Negative" >
                  <div v-if="negativeVotes && travelers">
                    <div v-for="user in negativeUsers" :key="user.userId" class="one-vote">
                      <div>
                        <img :src="'data:;base64,' + user.picture" class="rounded-image" v-if="user.picture">
                        <img :src="require('../assets/no-picture.png')" class="rounded-image" v-else>
                        <span class="name">{{user.name}} {{user.lastName}}</span>
                      </div>
                      <img src="../assets/dislike.png" class="vote-pic">
                    </div>
                    <div v-if="negativeVotes && negativeVotes.length == 0" class="no-votes">
                      No negative votes
                    </div>
                  </div>
                  <Spinner v-else />
                </b-tab>
              </b-tabs>
            </slot>
          </div>

          <div class="modal-footer">
            <slot name="footer">
              <button type="button" class="btn btn-secondary" @click="$emit('close')">
                Close
              </button>
            </slot>
          </div>
        </div>
      </div>
    </div>
  </transition>
</template>

<script>
import { mapGetters, mapMutations } from "vuex"
import Spinner from "@/components/Spinner.vue"

export default {
  components: {
    Spinner
  },
  props: {
    votableId: {
      required: true,
      type: Number
    },
    canVote: {
      required: true,
      type: Boolean
    }
  },
  data() {
    return {
      viewPositive: true
    }
  },
  computed: {
    positiveUsers() {
      return this.positiveVotes.map(vote => this.travelers.find(t => t.userId == vote.userId))
    },
    negativeUsers() {
      return this.negativeVotes.map(vote => this.travelers.find(t => t.userId == vote.userId))
    },
    myVote() {
      let myVote = this.positiveVotes.find(vote => vote.userId == this.userId)
      if(!myVote) {
        myVote = this.negativeVotes.find(vote => vote.userId == this.userId)
      }
      if(myVote)
        return myVote
      else
        return null
    },
    ...mapGetters({
      hasEditRights: 'getHasEditRights',
      tripId: 'getSpecificTripId',
      positiveVotes: 'getPositiveVotes',
      negativeVotes: 'getNegativeVotes',
      userId: 'getAuthUserId',
      travelers: 'getSpecificTripTravelers',
      votables: 'getVotables'
    })
  },
  methods: {
    vote(positive) {
      let payload = {
        tripId: this.tripId,
        voteInfo: {
          positive: positive,
          userId: this.userId,
          votableId: this.votableId
        }
      }
      this.$store.dispatch('postAddVote', payload)
      let oldVotable = this.votables.find(v => v.votableId == this.votableId)
      this.$travelPlanHub.$emit('ChangeVotable', {
        votableId: this.votableId,
        positiveVotes: positive ? oldVotable.positiveVotes + 1 : oldVotable.positiveVotes,
        negativeVotes: positive ? oldVotable.negativeVotes : oldVotable.negativeVotes + 1
      })
    },
    changeVote() {
      let oldVotable = this.votables.find(v => v.votableId == this.votableId)
      let newPositive = !this.myVote.positive
      let payload = {
        tripId: this.tripId,
        voteInfo: {
          voteId: this.myVote.voteId,
          positive: newPositive
        }
      }
      this.$store.dispatch('putChangeVote', payload)
      this.$travelPlanHub.$emit('ChangeVotable', {
        votableId: this.votableId,
        positiveVotes: newPositive ? oldVotable.positiveVotes + 1 : oldVotable.positiveVotes - 1,
        negativeVotes: newPositive ? oldVotable.negativeVotes - 1 : oldVotable.negativeVotes + 1
      })
    },
    removeVote() {
      let oldVotable = this.votables.find(v => v.votableId == this.votableId)
      let positive = this.myVote.positive
      let payload = {
        tripId: this.tripId,
        voteId: this.myVote.voteId,
        positive: positive
      }
      this.$store.dispatch('deleteVote', payload)
      this.$travelPlanHub.$emit('ChangeVotable', {
        votableId: this.votableId,
        positiveVotes: positive ? oldVotable.positiveVotes - 1 : oldVotable.positiveVotes,
        negativeVotes: positive ? oldVotable.negativeVotes : oldVotable.negativeVotes - 1
      })
    },
    ...mapMutations({
      setVotes: 'setVotes'
    })
  }, 
  created() {
    this.$store.dispatch('fillVotes', {
      votableId: this.votableId,
      positive: true
    })
    this.$store.dispatch('fillVotes', {
      votableId: this.votableId,
      positive: false
    })
  },
  destroyed() {
    this.setVotes({
      positive: true,
      data: null
    })
    this.setVotes({
      positive: false,
      data: null
    })
  }
}
</script>

<style scoped>
    .modal-mask {
    position: fixed;
    z-index: 9998;
    top: 0;
    left: 0;
    width: 100%;
    height: 100%;
    background-color: rgba(0, 0, 0, 0.5);
    display: table;
    transition: opacity 0.3s ease;
    }
    @media only screen and (max-width: 600px)
    {
        .modal-mask 
        {
            height: 110%;
        }
    }
    .modal-wrapper {
    display: table-cell;
    vertical-align: middle;
    }
    .modal-container {
    width: 50%;
    margin: 0px auto;
    padding: 20px 30px;
    background-color: #fff;
    border-radius: 2px;
    box-shadow: 0 2px 8px rgba(0, 0, 0, 0.33);
    transition: all 0.3s ease;
    font-family: Helvetica, Arial, sans-serif;
    }
    .modal-header{
    margin-top: 0;
    color: red;
    font-size: 18px;
    }
    .modal-body {
    margin: 20px 0;
    }
    .modal-default-button {
    float: right;
    }
    .modal-enter {
    opacity: 0;
    }
    .modal-leave-active {
    opacity: 0;
    }
    .modal-enter .modal-container,
    .modal-leave-active .modal-container {
    -webkit-transform: scale(1.1);
    transform: scale(1.1);
    }
    .polje
    {
        width: 600px;
        height: 120px;
        border: 3px solid #cccccc;
        padding: 5px;
        font-family: Tahoma, sans-serif;
        background-position: bottom right;
        background-repeat: no-repeat;
    }
    
    .tabs {
      flex-direction: column;
    }

    .rounded-image {
      border-radius: 15px;
      border: 2px solid grey;
      height:48px;
      width:48px;
      object-fit:cover;
      margin-bottom: 10px;
    }

    .name {
      font-size: 20px;
      margin-left: 10px;
    }

    .vote-pic {
      height: 30px; 
      width: 30px; 
      margin-right: 15px;
    }

    .one-vote {
      display: flex;
      justify-content: space-between;
      align-items: center;
    }

    .tab-content > .active {
      max-height: 300px;
      overflow: hidden;
      overflow-y: scroll;
    }

    .my-vote {
      margin-bottom: 20px;
      display: flex;
      align-items: center;
    }
    
    .my-vote-text {
      font-size: 20px;
      margin-right: 20px;
    }

    .change-vote {
      width: fit-content;
      padding: 5px;
      border-radius: 5px;
      cursor: pointer;
      color: black;
      font-weight: 500;
    }

    .vote-red {
      background-color: #ff6b6b;
      border: 2px solid red;
    }

    .vote-green {
      background-color: #5fd35f;
      border: 2px solid green;
    }

    .no-votes {
      font-size: 30px;
      font-weight: bold;
      font-style: italic;
      margin-bottom: 10px;
    }

    .clickable {
      cursor: pointer;
    }

    .clickable:hover {
      width: 34px;
      height: 34px;
      margin: -2px 13px -2px -2px;
    }
</style>