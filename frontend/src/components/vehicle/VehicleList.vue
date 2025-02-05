<script setup lang="ts">
import type { Vehicle } from '@/interfaces/vehicle'
import editIcon from '@/assets/edit.svg'
import trashIcon from '@/assets/trash.svg'

defineProps<{
  vehicles: Vehicle[]
  updateClick: (id: number) => unknown
  deleteClick: (id: number) => unknown
}>()
</script>

<template>
  <div class="divide-y divide-gray-200">
    <div v-for="vehicle in vehicles" :key="vehicle.id" class="py-4">
      <div class="flex flex-row">
        <div class="flex flex-col gap-1 px-4">
          <div class="flex items-center justify-between">
            <h3 class="text-lg font-medium text-gray-100">
              {{ vehicle.make }} {{ vehicle.model }} ({{ vehicle.year }})
            </h3>
          </div>
          <div class="text-sm text-gray-300">
            <p>License Plate: {{ vehicle.licensePlate }}</p>
            <p v-if="vehicle.color">Color: {{ vehicle.color }}</p>
            <p v-if="vehicle.type">Type: {{ vehicle.type }}</p>
          </div>
        </div>
        <div class="flex gap-2 pt-1">
          <img
            :src="editIcon"
            alt="Edit"
            @click="updateClick(vehicle.id)"
            class="icon w-5 h-5 cursor-pointer hover:opacity-80"
          />
          <img
            :src="trashIcon"
            @click="deleteClick(vehicle.id)"
            alt="Delete"
            class="w-5 h-5 cursor-pointer hover:opacity-80"
          />
        </div>
      </div>
    </div>
  </div>
</template>
