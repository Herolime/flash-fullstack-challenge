import { describe, it, expect } from 'vitest'
import { mount } from '@vue/test-utils'
import type { AccountFormData } from '@/interfaces/account'
import Form from '@/components/account/Form.vue'
import { mockFormData } from './mocks/Account'

describe('Form.vue', () => {
  const defaultFormData: AccountFormData = {
    apartmentNumber: '',
    familyName: '',
    address: '',
  }

  it('renders form fields correctly', () => {
    const wrapper = mount(Form)

    expect(wrapper.find('input#apartmentNumber').exists()).toBe(true)
    expect(wrapper.find('input#familyName').exists()).toBe(true)
    expect(wrapper.find('input#address').exists()).toBe(true)
  })

  it('initializes with default empty values', () => {
    const wrapper = mount(Form)
    const formData = wrapper.props('formData')

    expect(formData).toEqual(defaultFormData)
  })

  it('updates model when input values change', async () => {
    const wrapper = mount(Form)

    const apartmentInput = wrapper.find('input#apartmentNumber')
    await apartmentInput.setValue(mockFormData.apartmentNumber)

    const familyNameInput = wrapper.find('input#familyName')
    await familyNameInput.setValue(mockFormData.familyName)

    const addressInput = wrapper.find('input#address')
    await addressInput.setValue(mockFormData.address)

    expect(wrapper.props('formData')).toEqual(mockFormData)
  })

  it('emits submit event with form data when submitted', async () => {
    const wrapper = mount(Form)

    await wrapper.find('input#apartmentNumber').setValue(mockFormData.apartmentNumber)
    await wrapper.find('input#familyName').setValue(mockFormData.familyName)
    await wrapper.find('input#address').setValue(mockFormData.address)

    await wrapper.find('form').trigger('submit')

    expect(wrapper.emitted()).toBeTruthy()
    expect(wrapper.emitted()).toHaveProperty('submit')
  })

  it('displays correct button text based on mode prop', () => {
    const createWrapper = mount(Form, { props: { mode: 'create' } })
    const updateWrapper = mount(Form, { props: { mode: 'update' } })

    expect(createWrapper.find('button[type="submit"]').text()).toBe('Create Account')
    expect(updateWrapper.find('button[type="submit"]').text()).toBe('Update Account')
  })

  it('accepts initial form data through v-model', () => {
    const wrapper = mount(Form, {
      props: {
        formData: mockFormData,
      },
    })

    expect(wrapper.props('formData')).toEqual(mockFormData)
  })
})
