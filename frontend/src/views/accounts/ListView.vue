<template>
  <div class="accounts-list">
    <h1>Parking Accounts</h1>
    <div class="accounts-grid">
      <Card
        v-for="account in accounts"
        :key="account.id"
        :account="account"
        @click="viewDetails(account.id)"
      />
      <button class="p-1 border rounded-2xl cursor-pointer" @click="router.push('/accounts/form')">
        Add Account
      </button>
    </div>
  </div>
</template>

<script setup lang="ts">
import Card from '@/components/account/Card.vue'
import type { Account } from '@/interfaces/account'
import { ref, onMounted } from 'vue'
import { useRouter } from 'vue-router'

const router = useRouter()
const accounts = ref<Account[]>([])

const fetchAccounts = async () => {
  try {
    const response = await fetch('http://localhost:5203/api/account')
    accounts.value = (await response.json()) as Account[]
  } catch (error) {
    console.error('Error fetching accounts:', error)
  }
}

const viewDetails = (id: number) => {
  router.push(`/accounts/${id}`)
}

onMounted(() => {
  fetchAccounts()
})
</script>

<style scoped>
.accounts-list {
  padding: 2rem;
  max-width: 1200px;
  margin: 0 auto;
}

.accounts-grid {
  display: grid;
  grid-template-columns: repeat(3, minmax(300px, 1fr));
  gap: 1.5rem;
  margin-top: 2rem;
}
</style>
