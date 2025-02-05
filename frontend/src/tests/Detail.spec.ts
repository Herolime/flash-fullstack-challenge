import { beforeEach, describe, expect, test } from 'vitest'
import { mount, type VueWrapper } from '@vue/test-utils'
import Detail from '@/components/account/Detail.vue'
import { mockAccount } from './mocks/Account'

describe('Detail.vue', () => {
  let wrapper: VueWrapper

  beforeEach(() => {
    wrapper = mount(Detail, {
      props: {
        account: mockAccount,
      },
    })
  })

  test('renders account details when account is provided', () => {
    expect(wrapper.find('h2').text()).toBe(mockAccount.familyName)
    expect(wrapper.findAll('p')[0].text()).toContain(mockAccount.apartmentNumber)
    expect(wrapper.findAll('p')[1].text()).toBe(mockAccount.address)
  })
})
