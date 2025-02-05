import { describe, expect, beforeEach, test } from 'vitest'
import { mount, type VueWrapper } from '@vue/test-utils'
import Card from '@/components/account/Card.vue'
import { mockAccount } from './mocks/Account'

describe('Card.vue', () => {
  let wrapper: VueWrapper

  beforeEach(() => {
    wrapper = mount(Card, {
      props: {
        account: mockAccount,
      },
    })
  })

  test('should mount the component', () => {
    expect(wrapper.exists()).toBe(true)
  })

  test('should display account information correctly', () => {
    expect(wrapper.find('h3').text()).toBe('Smith')
    expect(wrapper.find('p').text()).toContain('101A')
    expect(wrapper.findAll('p')[1].text()).toBe('123 Main Street')
  })

  test('should emit click event when card is clicked', async () => {
    await wrapper.trigger('click')
    expect(wrapper.emitted()).toBeTruthy()
    expect(wrapper.emitted()).toHaveProperty('click')
  })

  test('should have correct base styling classes', () => {
    const cardElement = wrapper.find('.card')
    expect(cardElement.exists()).toBe(true)
    expect(cardElement.classes()).toContain('card')
  })
})
