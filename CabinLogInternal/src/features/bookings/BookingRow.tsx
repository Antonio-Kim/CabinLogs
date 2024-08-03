import styled from 'styled-components';

import Tag from '../../ui/Tag';
import Table from '../../ui/Table';
import { Guest } from '../../services/apiGuests';
import { Cabins } from '../../services/apiCabins';

const Cabin = styled.div`
  font-size: 1.6rem;
  font-weight: 600;
  color: var(--color-grey-600);
  font-family: 'Sono';
`;

const Stacked = styled.div`
  display: flex;
  flex-direction: column;
  gap: 0.2rem;

  & span:first-child {
    font-weight: 500;
  }

  & span:last-child {
    color: var(--color-grey-500);
    font-size: 1.2rem;
  }
`;

const Amount = styled.div`
  font-family: 'Sono';
  font-weight: 500;
`;

export type Booking = {
  id: number;
  created_at: string;
  startDate: string;
  endDate: string;
  numNights: number;
  numGuests: number;
  totalPrice: number;
  status: string;
  guest: Guest;
  cabin: Cabins;
};

type BookingRowProps = {
  booking: Booking;
};

function BookingRow({
  booking: {
    // id: bookingId,
    // created_at,
    startDate,
    endDate,
    numNights,
    // numGuests,
    totalPrice,
    status,
    guest: { fullName: guestName, email },
    cabin: { name: cabinName },
  },
}: BookingRowProps) {
  const statusToTagName = {
    unconfirmed: 'blue',
    'checked-in': 'green',
    'checked-out': 'silver',
  };

  return (
    <Table.Row>
      <Cabin>{cabinName}</Cabin>

      <Stacked>
        <span>{guestName}</span>
        <span>{email}</span>
      </Stacked>

      <Stacked>
        <span>
          {startDate} &rarr; {numNights} night stay
        </span>
        <span>
          {startDate} &mdash; {endDate}
        </span>
      </Stacked>

      <Tag type={statusToTagName[status as keyof typeof statusToTagName]}>
        {status.replace('-', ' ')}
      </Tag>

      <Amount>{totalPrice}</Amount>
    </Table.Row>
  );
}

export default BookingRow;
