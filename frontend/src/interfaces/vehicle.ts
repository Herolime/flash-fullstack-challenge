export interface Vehicle {
  id: number
  accountId: number
  make: string
  model: string
  year: number
  licensePlate: string
  color?: string
  type?: string
}

export interface VehicleFormData {
  make: string
  model: string
  year: number
  licensePlate: string
  color?: string
  type?: string
}
