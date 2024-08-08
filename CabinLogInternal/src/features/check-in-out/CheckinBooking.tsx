import styled from 'styled-components';
import BookingDataBox from '../bookings/BookingDataBox';

import Row from '../../ui/Row';
import Heading from '../../ui/Heading';
import ButtonGroup from '../../ui/ButtonGroup';
import Button from '../../ui/Button';
import ButtonText from '../../ui/ButtonText';

import { useMoveBack } from '../../hooks/useMoveBack';
import { useBooking } from '../bookings/useBooking';
import Spinner from '../../ui/Spinner';
import { useEffect, useState } from 'react';
import Checkbox from '../../ui/Checkbox';
import { useCheckin } from './useCheckin';
import { useSettings } from '../settings/useSettings';

const Box = styled.div`
  /* Box */
  background-color: var(--color-grey-0);
  border: 1px solid var(--color-grey-100);
  border-radius: var(--border-radius-md);
  padding: 2.4rem 4rem;
`;

function CheckinBooking() {
  const [confirmPaid, setConfirmPaid] = useState(false);
  const [addBreakfast, setAddBreakfast] = useState(false);
  const { booking, isLoading } = useBooking();
  const { settings, isLoading: isLoadingSettings } = useSettings();
  const moveBack = useMoveBack();
  const { checkin, isCheckingIn } = useCheckin();

  useEffect(() => {
    setConfirmPaid(booking?.isPaid ?? false);
  }, [booking]);

  if (isLoading || isLoadingSettings) return <Spinner />;
  if (!booking) return <div>Failed to loadin bookings</div>;
  if (!settings) return <div>Failed to load settings</div>;

  const {
    id: bookingId,
    guest: guests,
    totalPrice,
    numGuests,
    hasBreakfast,
    numberOfNights: numNights,
  } = booking;

  const optionalBreakfastPrice = settings[0].breakfastPrice * numNights * numGuests;
  function handleCheckin() {
    if (!confirmPaid) return;
    const bookingIdStr = String(bookingId);
    if (addBreakfast) {
      checkin({
        bookingId: bookingIdStr,
        breakfast: {
          hasBreakfast: true,
          extrasPrice: optionalBreakfastPrice,
          totalPrice: totalPrice + optionalBreakfastPrice,
        },
      });
    } else {
      checkin({ bookingId: bookingIdStr });
    }
  }

  return (
    <>
      <Row type="horizontal">
        <Heading as="h1">Check in booking #{bookingId}</Heading>
        <ButtonText onClick={moveBack}>&larr; Back</ButtonText>
      </Row>

      <BookingDataBox booking={booking} />

      {!hasBreakfast && (
        <Box>
          <Checkbox
            checked={addBreakfast}
            onChange={() => {
              setAddBreakfast((add) => !add);
              setConfirmPaid(false);
            }}
            id="Breafkast"
          >
            Want to add breakfast for {optionalBreakfastPrice}?
          </Checkbox>
        </Box>
      )}

      <Box>
        <Checkbox
          checked={confirmPaid}
          onChange={() => setConfirmPaid((confirm) => !confirm)}
          id={String(bookingId)}
          disabled={confirmPaid || isCheckingIn}
        >
          I confirm that {guests?.fullName} has paid the total amount of{' '}
          {!addBreakfast
            ? totalPrice
            : `${totalPrice + optionalBreakfastPrice} (${totalPrice} + ${optionalBreakfastPrice})`}
        </Checkbox>
      </Box>

      <ButtonGroup>
        <Button onClick={handleCheckin} disabled={!confirmPaid || isCheckingIn}>
          Check in booking #{bookingId}
        </Button>
        <Button variation="secondary" onClick={moveBack}>
          Back
        </Button>
      </ButtonGroup>
    </>
  );
}

export default CheckinBooking;