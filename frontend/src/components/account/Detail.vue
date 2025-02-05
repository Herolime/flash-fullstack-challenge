<script setup lang="ts">
import type { Account } from '@/interfaces/account'

defineProps<{
  account: Account | undefined
}>()
</script>

<template>
  <div class="flex gap-1.5 flex-col">
    <h2 class="text-6xl text-gray-200">{{ account?.familyName }}</h2>
    <p class="text-4xl text-gray-400">Apartment: {{ account?.apartmentNumber }}</p>
    <p class="text-3xl text-gray-500">{{ account?.address }}</p>
  </div>
</template>

<style></style>
