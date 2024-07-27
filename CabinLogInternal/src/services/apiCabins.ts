export type Cabin = {
  id: number;
  created_at: string;
  startDate: string;
  endDate: string;
  numberOfNights: string;
  numGuests: string;
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

export async function getCabins(): Promise<Cabin[]> {
  try {
    const response = await fetch('http://localhost:5000/cabins', {
      method: 'GET',
      headers: {
        'Content-Type': 'application/json',
      },
    });
    if (!response.ok) {
      throw new Error(`Error occurred while fetching cabins`);
    }
    const data: Cabin[] = await response.json();
    return data;
  } catch (e) {
    console.error(`Error occurred: ${e}`);
    return [];
  }
}
