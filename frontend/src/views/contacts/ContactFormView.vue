<template>
  <div class="form-view mr-36">
    <h1>{{ isUpdateMode ? 'Update' : 'Add' }} Contact</h1>
    <Form
      :mode="isUpdateMode ? 'update' : 'create'"
      v-model:form-data="contactData"
      @submit="handleSubmit"
    />
  </div>
</template>

<script setup lang="ts">
import Form from '@/components/contact/Form.vue'
import type { ContactFormData } from '@/interfaces/contact'
import { useRouter } from 'vue-router'
import { ref, onMounted } from 'vue'

const router = useRouter()
const props = defineProps<{
  accountId: string
  id?: string
}>()

const contactData = ref<ContactFormData>({
  firstName: '',
  lastName: '',
  email: '',
  phoneNumber: '',
})

const isUpdateMode = props.id && props.id !== undefined

const fetchContact = async () => {
  if (isUpdateMode) {
    try {
      const response = await fetch(`http://localhost:5203/api/contact/${props.id}`)
      if (response.ok) {
        const data = await response.json()
        contactData.value = {
          firstName: data.firstName,
          lastName: data.lastName,
          email: data.email,
          phoneNumber: data.phoneNumber,
        }
      }
    } catch (error) {
      console.error('Error fetching contact:', error)
    }
  }
}

onMounted(() => fetchContact())

const handleSubmit = async (formData: ContactFormData) => {
  try {
    const url = `http://localhost:5203/api/contact${isUpdateMode ? `/${props.id}` : ''}`
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
      console.error(`Failed to ${isUpdateMode ? 'update' : 'create'} contact`)
    }
  } catch (error) {
    console.error(`Error ${isUpdateMode ? 'updating' : 'creating'} contact:`, error)
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
