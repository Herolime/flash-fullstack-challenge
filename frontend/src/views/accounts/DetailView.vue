<script setup lang="ts">
import type { Account } from '@/interfaces/account'
import type { Contact } from '@/interfaces/contact'
import type { Vehicle } from '@/interfaces/vehicle'
import Detail from '@/components/account/Detail.vue'
import { onMounted, ref } from 'vue'
import VehicleList from '@/components/vehicle/VehicleList.vue'
import ContactList from '@/components/contact/ContactList.vue'
import router from '@/router'

const props = defineProps<{ id: string }>()
const account = ref<Account>()
const vehicles = ref<Vehicle[]>([])
const contacts = ref<Contact[]>([])
const deleteModalClicked = ref(false)

const fetchAccountData = async () => {
  try {
    const response = await fetch(`http://localhost:5203/api/account/${props.id}`)
    account.value = (await response.json()) as Account
  } catch (error) {
    console.error('Error fetching account:', error)
  }
}

const fetchVehicles = async () => {
  try {
    const response = await fetch(`http://localhost:5203/api/account/${props.id}/vehicles`)
    vehicles.value = (await response.json()) as Vehicle[]
  } catch (error) {
    console.error('Error fetching account vehicles:', error)
  }
}

const fetchContacts = async () => {
  try {
    const response = await fetch(`http://localhost:5203/api/account/${props.id}/contacts`)
    contacts.value = (await response.json()) as Contact[]
  } catch (error) {
    console.error('Error fetching account vehicles:', error)
  }
}

const deleteContact = async (contactId: number) => {
  try {
    const response = await fetch(`http://localhost:5203/api/contact/${contactId}`, {
      method: 'DELETE',
    })
    if (response.ok) {
      await fetchContacts()
    }
  } catch (error) {
    console.error('Error fetching account vehicles:', error)
  }
}

const deleteVehicle = async (vehicleId: number) => {
  try {
    const response = await fetch(`http://localhost:5203/api/vehicle/${vehicleId}`, {
      method: 'DELETE',
    })
    if (response.ok) {
      await fetchVehicles()
    }
  } catch (error) {
    console.error('Error fetching account vehicles:', error)
  }
}

const deleteAccount = async () => {
  try {
    const response = await fetch(`http://localhost:5203/api/account/${props.id}`, {
      method: 'DELETE',
    })
    if (response.ok) {
      router.replace('/')
    }
  } catch (error) {
    console.error('Error deleting account:', error)
  }
}

const updateAccount = () => router.push(`/accounts/form/${props.id}`)
const goToContactForm = (contactId?: number) =>
  router.push(`/accounts/${props.id}/contacts/form/${contactId ?? ''}`)
const goToVehicleForm = (vehicleId?: number) =>
  router.push(`/accounts/${props.id}/vehicles/form/${vehicleId ?? ''}`)

const onDeleteClick = () => (deleteModalClicked.value = !deleteModalClicked.value)

onMounted(() => {
  fetchAccountData().then(() => {
    fetchVehicles()
    fetchContacts()
  })
})
</script>

<template>
  <div class="flex flex-col gap-3">
    <div class="flex justify-between">
      <Detail :account="account" />
      <div class="flex flex-col justify-around">
        <button
          class="border w-24 py-3 px-2 rounded-xl text-gray-100 hover:underline cursor-pointer"
          @click="updateAccount()"
        >
          Update
        </button>
        <button
          class="border w-24 py-3 px-2 rounded-xl text-red-600 hover:underline cursor-pointer"
          @click="onDeleteClick()"
        >
          Delete
        </button>
      </div>
    </div>
    <div class="flex gap-5">
      <div>
        <h3 class="text-2xl">List of Registered Vehicles</h3>
        <VehicleList
          :vehicles="vehicles"
          :update-click="(id) => goToVehicleForm(id)"
          :delete-click="(id) => deleteVehicle(id)"
        />
        <button
          class="border w-full py-3 px-2 rounded-xl text-gray-100 hover:underline cursor-pointer"
          @click="goToVehicleForm()"
        >
          Add new vehicle
        </button>
      </div>
      <div>
        <h3 class="text-2xl">List of Registered Contacts</h3>
        <ContactList
          :contacts="contacts"
          :update-click="(id) => goToContactForm(id)"
          :delete-click="(id) => deleteContact(id)"
        />
        <button
          class="border w-full py-3 px-2 rounded-xl text-gray-100 hover:underline cursor-pointer"
          @click="goToContactForm()"
        >
          Add new contact
        </button>
      </div>
    </div>
  </div>
  <div
    v-show="deleteModalClicked"
    class="relative z-10"
    aria-labelledby="modal-title"
    role="dialog"
    aria-modal="true"
  >
    <div class="fixed inset-0 bg-gray-500/75 transition-opacity" aria-hidden="true"></div>

    <div class="fixed inset-0 z-10 w-screen overflow-y-auto">
      <div class="flex min-h-full items-end justify-center p-4 text-center sm:items-center sm:p-0">
        <div
          class="relative transform overflow-hidden rounded-lg text-left shadow-xl transition-all sm:my-8 sm:w-full sm:max-w-lg"
        >
          <div class="bg-gray-800 px-4 pt-5 pb-4 sm:p-6 sm:pb-4">
            <div class="sm:flex sm:gap-2 sm:items-start">
              <div
                class="mx-auto flex size-12 shrink-0 items-center justify-center rounded-full bg-red-100 sm:mx-0 sm:size-10"
              >
                <svg
                  class="size-6 text-red-600"
                  fill="none"
                  viewBox="0 0 24 24"
                  stroke-width="1.5"
                  stroke="currentColor"
                  aria-hidden="true"
                  data-slot="icon"
                >
                  <path
                    stroke-linecap="round"
                    stroke-linejoin="round"
                    d="M12 9v3.75m-9.303 3.376c-.866 1.5.217 3.374 1.948 3.374h14.71c1.73 0 2.813-1.874 1.948-3.374L13.949 3.378c-.866-1.5-3.032-1.5-3.898 0L2.697 16.126ZM12 15.75h.007v.008H12v-.008Z"
                  />
                </svg>
              </div>
              <div class="mt-3 text-center sm:mt-0 sm:ml-4 sm:text-left">
                <h3 class="text-base font-semibold text-gray-100" id="modal-title">
                  Delete account
                </h3>
                <div class="mt-2">
                  <p class="text-sm text-gray-200">
                    Are you sure you want to delete this account? All of the data relating to it
                    will be removed.
                  </p>
                </div>
              </div>
            </div>
          </div>
          <div class="bg-gray-700 px-4 py-3 sm:flex sm:flex-row-reverse sm:gap-3 sm:px-6">
            <button
              type="button"
              class="inline-flex w-full justify-center rounded-md px-3 py-2 text-sm font-semibold text-red-600 border border-red-600 shadow-xs hover:underline sm:ml-3 sm:w-auto"
              @click="deleteAccount()"
            >
              Delete
            </button>
            <button
              type="button"
              class="mt-3 inline-flex w-full justify-center rounded-md px-3 py-2 text-sm font-semibold text-gray-50 ring-1 shadow-xs ring-gray-300 ring-inset border border-gray-50 hover:underline sm:mt-0 sm:w-auto"
              @click="onDeleteClick()"
            >
              Cancel
            </button>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<style></style>
