type Setting = {
  id: number;
  created_at: string;
  minBookingLength: number;
  maxBookingLength: number;
  maxGuestsPerBOoking: number;
  breakfastPrice: number;
};

export async function getSettings(): Promise<Setting[]> {
  try {
    const response = await fetch('http://localhost:5000/settings');

    if (!response.ok) {
      throw new Error('Error occurred while fetching settings.');
    }
    const data: Setting[] = await response.json();
    return data;
  } catch (e) {
    console.error(`Error occurred: ${e}`);
    return [];
  }
}
