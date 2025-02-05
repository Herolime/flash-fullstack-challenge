<template>
  <div class="form-view mr-36">
    <h1>{{ isUpdateMode ? 'Update' : 'Add' }} Parking Account</h1>
    <Form
      :mode="isUpdateMode ? 'update' : 'create'"
      v-model:form-data="accountData"
      @submit="handleSubmit"
    />
  </div>
</template>

<script setup lang="ts">
import Form from '@/components/account/Form.vue'
import type { AccountFormData } from '@/interfaces/account'
import { useRouter } from 'vue-router'
import { ref, onMounted } from 'vue'

const router = useRouter()
const props = defineProps<{ id?: string }>()

const accountData = ref<AccountFormData>({
  apartmentNumber: '',
  familyName: '',
  address: '',
})

const isUpdateMode = props.id && props.id !== undefined

const fetchAccount = async () => {
  if (isUpdateMode) {
    try {
      const response = await fetch(`http://localhost:5203/api/account/${props.id}`)
      if (response.ok) {
        const data = await response.json()
        accountData.value = {
          apartmentNumber: data.apartmentNumber,
          familyName: data.familyName,
          address: data.address,
        }
      }
    } catch (error) {
      console.error('Error fetching account:', error)
    }
  }
}

onMounted(() => fetchAccount())

const handleSubmit = async (formData: AccountFormData) => {
  try {
    const url = `http://localhost:5203/api/account${isUpdateMode ? `/${props.id}` : ''}`
    const method = isUpdateMode ? 'PUT' : 'POST'

    const response = await fetch(url, {
      method,
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(formData),
    })

    if (response.ok) {
      router.push('/')
    } else {
      console.error(`Failed to ${isUpdateMode ? 'update' : 'create'} account`)
    }
  } catch (error) {
    console.error(`Error ${isUpdateMode ? 'updating' : 'creating'} account:`, error)
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
