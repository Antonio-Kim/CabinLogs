import { useQuery } from '@tanstack/react-query';
import { getBookings } from '../../services/apiBookings';
import { useSearchParams } from 'react-router-dom';

export function useBookings() {
  const [searchParams] = useSearchParams();
  const filterValue = searchParams.get('status');
  const sortByRaw = searchParams.get('sortBy') || 'startDate-desc';

  const filter =
    filterValue === 'all'
      ? undefined
      : filterValue
        ? { field: 'status', value: filterValue }
        : undefined;
  const [field, direction] = sortByRaw.split('-');
  const sortBy = { field, direction };
  const page = !searchParams.get('pageIndex') ? 1 : Number(searchParams.get('pageIndex'));

  const { isPending, data, error } = useQuery({
    queryKey: ['bookings', filter, sortBy, page],
    queryFn: () => getBookings({ filter, sortBy, page }),
  });

  const bookings = data?.bookings || [];
  const totalCount = data?.totalCount || 0;
  return { isPending, error, bookings, totalCount };
}
