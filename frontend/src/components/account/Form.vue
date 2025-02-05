<script setup lang="ts">
import type { AccountFormData } from '@/interfaces/account'

interface Props {
  mode?: 'create' | 'update'
}

withDefaults(defineProps<Props>(), {
  mode: 'create',
})

const model = defineModel<AccountFormData>('form-data', {
  default: {
    apartmentNumber: '',
    familyName: '',
    address: '',
  },
})

const emit = defineEmits<{
  (e: 'submit', data: AccountFormData): void
}>()

const handleSubmit = () => {
  emit('submit', model.value)
}
</script>

<template>
  <form @submit.prevent="handleSubmit" class="p-6 rounded-lg mx-auto">
    <div class="space-y-4 w-90 flex flex-col gap-4">
      <div class="flex flex-col">
        <label for="apartmentNumber" class="text-sm font-medium text-white-700 mb-1"
          >Apartment Number</label
        >
        <input
          id="apartmentNumber"
          v-model="model.apartmentNumber"
          type="text"
          required
          class="px-3 py-2 border border-gray-700 text-gray-900 bg-white rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
        />
      </div>

      <div class="flex flex-col">
        <label for="familyName" class="text-sm font-medium text-white-700 mb-1">Family Name</label>
        <input
          id="familyName"
          v-model="model.familyName"
          type="text"
          required
          class="px-3 py-2 border border-gray-300 text-gray-900 rounded-md bg-white focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
        />
      </div>

      <div class="flex flex-col">
        <label for="address" class="text-sm font-medium text-white-700 mb-1">Address</label>
        <input
          id="address"
          v-model="model.address"
          type="text"
          required
          class="px-3 py-2 border border-gray-300 text-gray-900 rounded-md bg-white focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
        />
      </div>

      <div class="flex justify-end">
        <button
          type="submit"
          class="px-4 py-2 text-gray-100 cursor-pointer border rounded-md hover:bg-blue-600 focus:outline-none focus:ring-2 focus:ring-blue-500 focus:ring-offset-2"
        >
          {{ mode === 'create' ? 'Create' : 'Update' }} Account
        </button>
      </div>
    </div>
  </form>
</template>
