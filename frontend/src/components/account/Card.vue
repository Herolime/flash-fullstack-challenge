<script setup lang="ts">
import type { Account } from '../../interfaces/account'
defineProps<{
  account: Account
}>()
</script>

<template>
  <div class="card" @click="$emit('click')">
    <h3>{{ account.familyName }}</h3>
    <p>Apartment: {{ account.apartmentNumber }}</p>
    <p>{{ account.address }}</p>
  </div>
</template>

<style scoped>
.card {
  padding: 1rem;
  border: 1px solid #ddd;
  border-radius: 4px;
  cursor: pointer;
}

.card:hover {
  background-color: #f5f5f5;
  color: var(--vt-c-text-light-1);
}
</style>
