type Booking = {
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
};

export async function getBooking(id: number): Promise<Booking[]> {
  try {
    const response = await fetch(`http://localhost:5000/bookings/${id}`);
    if (!response.ok) {
      throw new Error('Error occured while fetching booking.');
    }
    const data: Booking[] = await response.json();
    return data;
  } catch (e) {
    console.error(`Error occured: ${e}`);
    return [];
  }
}
