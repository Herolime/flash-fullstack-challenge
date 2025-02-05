<script setup lang="ts">
import type { VehicleFormData } from '@/interfaces/vehicle'

interface Props {
  mode?: 'create' | 'update'
}

const vehicleTypes = [
  'Sedan',
  'SUV',
  'Truck',
  'Van',
  'Coupe',
  'Wagon',
  'Convertible',
  'Hatchback',
  'Motorcycle'
]

withDefaults(defineProps<Props>(), {
  mode: 'create',
})

const model = defineModel<VehicleFormData>('form-data', {
  default: {
    make: '',
    model: '',
    year: new Date().getFullYear(),
    licensePlate: '',
    color: '',
    type: '',
  },
})

const emit = defineEmits<{
  (e: 'submit', data: VehicleFormData): void
}>()

const handleSubmit = () => {
  emit('submit', model.value)
}
</script>

<template>
  <form @submit.prevent="handleSubmit" class="p-6 rounded-lg mx-auto">
    <div class="space-y-4 w-90 flex flex-col gap-4">
      <div class="flex flex-col">
        <label for="make" class="text-sm font-medium text-white-700 mb-1">Make</label>
        <input
          id="make"
          v-model="model.make"
          type="text"
          required
          class="px-3 py-2 border border-gray-700 text-gray-900 bg-white rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
        />
      </div>

      <div class="flex flex-col">
        <label for="model" class="text-sm font-medium text-white-700 mb-1">Model</label>
        <input
          id="model"
          v-model="model.model"
          type="text"
          required
          class="px-3 py-2 border border-gray-300 text-gray-900 rounded-md bg-white focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
        />
      </div>

      <div class="flex flex-col">
        <label for="year" class="text-sm font-medium text-white-700 mb-1">Year</label>
        <input
          id="year"
          v-model="model.year"
          type="number"
          required
          class="px-3 py-2 border border-gray-300 text-gray-900 rounded-md bg-white focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
        />
      </div>

      <div class="flex flex-col">
        <label for="licensePlate" class="text-sm font-medium text-white-700 mb-1"
          >License Plate</label
        >
        <input
          id="licensePlate"
          v-model="model.licensePlate"
          type="text"
          required
          class="px-3 py-2 border border-gray-300 text-gray-900 rounded-md bg-white focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
        />
      </div>

      <div class="flex flex-col">
        <label for="color" class="text-sm font-medium text-white-700 mb-1">Color</label>
        <input
          id="color"
          v-model="model.color"
          type="text"
          class="px-3 py-2 border border-gray-300 text-gray-900 rounded-md bg-white focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
        />
      </div>

      <div class="flex flex-col">
        <label for="type" class="text-sm font-medium text-white-700 mb-1">Type</label>
        <select
          id="type"
          v-model="model.type"
          class="px-3 py-2 border border-gray-300 text-gray-900 rounded-md bg-white focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
        >
          <option value="">Select a vehicle type</option>
          <option v-for="type in vehicleTypes" :key="type" :value="type">{{ type }}</option>
        </select>
      </div>

      <div class="flex justify-end">
        <button
          type="submit"
          class="px-4 py-2 text-gray-100 cursor-pointer border rounded-md hover:bg-blue-600 focus:outline-none focus:ring-2 focus:ring-blue-500 focus:ring-offset-2"
        >
          {{ mode === 'create' ? 'Create' : 'Update' }} Vehicle
        </button>
      </div>
    </div>
  </form>
</template>
