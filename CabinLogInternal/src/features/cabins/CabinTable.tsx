import Spinner from '../../ui/Spinner';
import CabinRow from './CabinRow';
import { useCabins } from './useCabins';
import Table from '../../ui/Table';
import Menus from '../../ui/Menu';
import { useSearchParams } from 'react-router-dom';
import { Cabins } from '../../services/apiCabins';

function CabinTable() {
  const { isLoading, data } = useCabins();
  const [searchParams] = useSearchParams();

  if (isLoading) {
    <Spinner />;
  }

  const filterValue = searchParams.get('discount') || 'all';
  let filteredCabins;
  if (filterValue === 'all') filteredCabins = data;
  if (filterValue === 'no-discount') filteredCabins = data?.filter((cabin) => cabin.discount === 0);
  if (filterValue === 'with-discount') filteredCabins = data?.filter((cabin) => cabin.discount > 0);

  const sortBy = searchParams.get('sortBy') || 'startDate-asc';
  const [field, direction] = sortBy.split('-') as [keyof Cabins, 'asc' | 'desc'];
  const modifier = direction === 'asc' ? 1 : -1;
  const sortedCabins = filteredCabins?.sort(
    (a: Cabins, b: Cabins) => (a[field] - b[field]) * modifier,
  );

  return (
    <Menus>
      <Table columns="0.6fr 1.8fr 2.2fr 1fr 1fr 1fr">
        <Table.Header>
          <div></div>
          <div>Cabin</div>
          <div>Capacity</div>
          <div>Price</div>
          <div>Discount</div>
          <div></div>
        </Table.Header>
        <Table.Body
          data={sortedCabins}
          render={(cabin) => <CabinRow cabin={cabin} key={cabin.id} />}
        />
      </Table>
    </Menus>
  );
}

export default CabinTable;
