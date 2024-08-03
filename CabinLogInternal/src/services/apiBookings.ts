import { Cabins } from './apiCabins';
import { Guest } from './apiGuests';

export type Booking = {
  id: number;
  created_at: string;
  startDate: string;
  endDate: string;
  numberOfNights: number;
  numGuests: number;
  cabinPrice: number;
  extrasPrice: number;
  totalPrice: number;
  status: string;
  hasBreakfast: boolean;
  isPaid: boolean;
  observations: string;
  cabinId: number;
  guestId: number;
  cabin?: Cabins;
  guest?: Guest;
};

type GetBookingsProp = {
  filter?: {
    field: string;
    value: string;
  };
  sortBy?: {
    field: string;
    direction: string;
  };
};

export async function getBooking(id: number): Promise<Booking | void> {
  try {
    const response = await fetch(`http://localhost:5000/bookings/${id}`);
    if (!response.ok) {
      throw new Error('Error occurred while fetching booking.');
    }
    const bookingData: Booking = await response.json();

    return bookingData;
  } catch (e) {
    console.error(`Error occured: ${e}`);
  }
}

export async function getBookings({ filter, sortBy }: GetBookingsProp): Promise<Booking[]> {
  try {
    let url = 'http://localhost:5000/bookings';
    const query = new URLSearchParams();

    if (filter) {
      query.append(`${filter.field}`, `${filter.value}`);
    }
    if (sortBy) {
      query.append('options', sortBy.field);
      query.append('sortOrder', sortBy.direction);
    }
    if (query.toString()) {
      url += `?${query.toString()}`;
    }

    const response = await fetch(url);
    if (!response.ok) {
      throw new Error('Error occurred while fetching bookings.');
    }
    const bookings: Booking[] = await response.json();
    return bookings;
  } catch (e) {
    console.error(`Error occurred ${e}`);
    return [];
  }
}
