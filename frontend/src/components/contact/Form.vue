<script setup lang="ts">
import type { ContactFormData } from '@/interfaces/contact'

interface Props {
  mode?: 'create' | 'update'
}

withDefaults(defineProps<Props>(), {
  mode: 'create',
})

const model = defineModel<ContactFormData>('form-data', {
  default: {
    firstName: '',
    lastName: '',
    email: '',
    phoneNumber: ''
  },
})

const emit = defineEmits<{
  (e: 'submit', data: ContactFormData): void
}>()

const handleSubmit = () => {
  emit('submit', model.value)
}
</script>

<template>
  <form @submit.prevent="handleSubmit" class="p-6 rounded-lg mx-auto">
    <div class="space-y-4 w-90 flex flex-col gap-4">
      <div class="flex flex-col">
        <label for="firstName" class="text-sm font-medium text-white-700 mb-1">First Name</label>
        <input
          id="firstName"
          v-model="model.firstName"
          type="text"
          required
          class="px-3 py-2 border border-gray-700 text-gray-900 bg-white rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
        />
      </div>

      <div class="flex flex-col">
        <label for="lastName" class="text-sm font-medium text-white-700 mb-1">Last Name</label>
        <input
          id="lastName"
          v-model="model.lastName"
          type="text"
          required
          class="px-3 py-2 border border-gray-300 text-gray-900 rounded-md bg-white focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
        />
      </div>

      <div class="flex flex-col">
        <label for="email" class="text-sm font-medium text-white-700 mb-1">Email</label>
        <input
          id="email"
          v-model="model.email"
          type="email"
          required
          class="px-3 py-2 border border-gray-300 text-gray-900 rounded-md bg-white focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
        />
      </div>

      <div class="flex flex-col">
        <label for="phoneNumber" class="text-sm font-medium text-white-700 mb-1">Phone Number</label>
        <input
          id="phoneNumber"
          v-model="model.phoneNumber"
          type="tel"
          required
          class="px-3 py-2 border border-gray-300 text-gray-900 rounded-md bg-white focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
        />
      </div>

      <div class="flex justify-end">
        <button
          type="submit"
          class="px-4 py-2 text-gray-100 cursor-pointer border rounded-md hover:bg-blue-600 focus:outline-none focus:ring-2 focus:ring-blue-500 focus:ring-offset-2"
        >
          {{ mode === 'create' ? 'Create' : 'Update' }} Contact
        </button>
      </div>
    </div>
  </form>
</template>