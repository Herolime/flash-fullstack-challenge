<template>
  <div class="form-view mr-36">
    <h1>{{ isUpdateMode ? 'Update' : 'Add' }} Vehicle</h1>
    <Form
      :mode="isUpdateMode ? 'update' : 'create'"
      v-model:form-data="vehicleData"
      @submit="handleSubmit"
    />
  </div>
</template>

<script setup lang="ts">
import Form from '@/components/vehicle/Form.vue'
import type { VehicleFormData } from '@/interfaces/vehicle'
import { useRouter } from 'vue-router'
import { ref, onMounted } from 'vue'

const router = useRouter()
const props = defineProps<{
  accountId: string
  id?: string
}>()

const vehicleData = ref<VehicleFormData>({
  make: '',
  model: '',
  year: new Date().getFullYear(),
  licensePlate: '',
  color: '',
  type: '',
})

const isUpdateMode = props.id && props.id !== undefined

const fetchVehicle = async () => {
  if (isUpdateMode) {
    try {
      const response = await fetch(`http://localhost:5203/api/vehicle/${props.id}`)
      if (response.ok) {
        const data = await response.json()
        vehicleData.value = {
          make: data.make,
          model: data.model,
          year: data.year,
          licensePlate: data.licensePlate,
          color: data.color,
          type: data.type,
        }
      }
    } catch (error) {
      console.error('Error fetching vehicle:', error)
    }
  }
}

onMounted(() => fetchVehicle())

const handleSubmit = async (formData: VehicleFormData) => {
  try {
    const url = `http://localhost:5203/api/vehicle${isUpdateMode ? `/${props.id}` : ''}`
    const method = isUpdateMode ? 'PUT' : 'POST'

    const response = await fetch(url, {
      method,
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify({
        ...formData,
        accountId: props.accountId,
      }),
    })

    if (response.ok) {
      router.back()
    } else {
      console.error(`Failed to ${isUpdateMode ? 'update' : 'create'} vehicle`)
    }
  } catch (error) {
    console.error(`Error ${isUpdateMode ? 'updating' : 'creating'} vehicle:`, error)
  }
}
</script>

<style scoped>
.form-view {
  padding: 2rem;
  max-width: 800px;
  margin-left: calc(var(--spacing) * 84);
  margin-right: calc(var(--spacing) * 16);
}
</style>
