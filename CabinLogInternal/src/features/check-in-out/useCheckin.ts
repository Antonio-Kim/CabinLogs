import { useMutation, useQueryClient } from '@tanstack/react-query';
import { Booking, updateBooking } from '../../services/apiBookings';
import toast from 'react-hot-toast';
import { useNavigate } from 'react-router-dom';

export function useCheckin() {
  const queryClient = useQueryClient();
  const navigate = useNavigate();
  const { mutate: checkin, isPending: isCheckingIn } = useMutation({
    mutationFn: (bookingId: string) =>
      updateBooking(bookingId, {
        status: 'checked-in',
        isPaid: true,
      }),

    onSuccess: (data: Booking) => {
      toast.success(`Booking #${data.id} successfully checked in`);
      queryClient.invalidateQueries({ active: true });
      navigate('/');
    },
    onError: (error: Error) => {
      toast.error(`Failed to check in: ${error.message}`);
    },
  });

  return { checkin, isCheckingIn };
}
