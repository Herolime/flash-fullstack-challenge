import { createRouter, createWebHistory } from 'vue-router'
import ListView from '@/views/accounts/ListView.vue'
import FormView from '@/views/accounts/FormView.vue'
import DetailView from '@/views/accounts/DetailView.vue'
import ContactFormView from '@/views/contacts/ContactFormView.vue'
import VehicleFormView from '@/views/vehicles/VehicleFormView.vue'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'home',
      component: ListView,
    },
    {
      path: '/accounts/form/:id?',
      name: 'Account Form',
      component: FormView,
      props: true,
    },
    {
      path: '/accounts/:id',
      name: 'Account Details',
      component: DetailView,
      props: true,
    },
    {
      path: '/accounts/:accountId/contacts/form/:id?',
      name: 'Contact Form',
      component: ContactFormView,
      props: true,
    },
    {
      path: '/accounts/:accountId/vehicles/form/:id?',
      name: 'Vehicle Form',
      component: VehicleFormView,
      props: true,
    },
  ],
})

export default router
