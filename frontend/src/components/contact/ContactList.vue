<script setup lang="ts">
import type { Contact } from '@/interfaces/contact'
import editIcon from '@/assets/edit.svg'
import trashIcon from '@/assets/trash.svg'

defineProps<{
  contacts: Contact[]
  updateClick: (id: number) => unknown
  deleteClick: (id: number) => unknown
}>()
</script>

<template>
  <div class="divide-y divide-gray-200">
    <div v-for="contact in contacts" :key="contact.id" class="py-4">
      <div class="flex flex-row">
        <div class="flex flex-col gap-1 px-4">
          <div class="flex items-center justify-between">
            <h3 class="text-lg font-medium text-gray-400">
              {{ contact.firstName }} {{ contact.lastName }}
            </h3>
          </div>
          <div class="text-sm text-gray-500">
            <p>Email: {{ contact.email }}</p>
            <p>Phone: {{ contact.phoneNumber }}</p>
          </div>
        </div>
        <div class="flex gap-2 pt-1">
          <img
            :src="editIcon"
            alt="Edit"
            @click="updateClick(contact.id)"
            class="icon w-5 h-5 cursor-pointer hover:opacity-80"
          />
          <img
            :src="trashIcon"
            alt="Delete"
            @click="deleteClick(contact.id)"
            class="w-5 h-5 cursor-pointer hover:opacity-80"
          />
        </div>
      </div>
    </div>
  </div>
</template>
