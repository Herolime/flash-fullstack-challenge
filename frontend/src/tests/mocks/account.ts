import type { Account, AccountFormData } from '@/interfaces/account'

export const mockAccount: Account = {
  id: 1,
  familyName: 'Smith',
  apartmentNumber: '101A',
  address: '123 Main Street',
}

export const mockFormData: AccountFormData = {
  familyName: 'Smith',
  apartmentNumber: '101A',
  address: '123 Main Street',
}
