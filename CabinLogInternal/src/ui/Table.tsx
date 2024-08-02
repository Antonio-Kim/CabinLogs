import { createContext, ReactNode, useContext } from 'react';
import styled from 'styled-components';
import { Cabins } from '../services/apiCabins';

const StyledTable = styled.div`
  border: 1px solid var(--color-grey-200);

  font-size: 1.4rem;
  background-color: var(--color-grey-0);
  border-radius: 7px;
  overflow: hidden;
`;

const CommonRow = styled.header<{ columns?: string }>`
  display: grid;
  grid-template-columns: ${(props) => props.columns || 'auto'};
  column-gap: 2.4rem;
  align-items: center;
  transition: none;
`;

const StyledHeader = styled(CommonRow)`
  padding: 1.6rem 2.4rem;

  background-color: var(--color-grey-50);
  border-bottom: 1px solid var(--color-grey-100);
  text-transform: uppercase;
  letter-spacing: 0.4px;
  font-weight: 600;
  color: var(--color-grey-600);
`;

const StyledBody = styled.section`
  margin: 0.4rem 0;
`;

const StyledRow = styled(CommonRow)`
  padding: 1.2rem 2.4rem;

  &:not(:last-child) {
    border-bottom: 1px solid var(--color-grey-100);
  }
`;

const Footer = styled.footer`
  background-color: var(--color-grey-50);
  display: flex;
  justify-content: center;
  padding: 1.2rem;

  &:not(:has(*)) {
    display: none;
  }
`;

const Empty = styled.p`
  font-size: 1.6rem;
  font-weight: 500;
  text-align: center;
  margin: 2.4rem;
`;

type TableProps = {
  columns: string;
  children: ReactNode;
};

type TableElementProps = {
  children: ReactNode;
};

type BodyElementProps = {
  data: Cabins[] | undefined;
  render: (item: Cabins) => ReactNode;
};

type TableContextType = {
  columns: string;
};

const TableContext = createContext<TableContextType | undefined>(undefined);

function Table({ columns, children }: TableProps) {
  return (
    <TableContext.Provider value={{ columns }}>
      <StyledTable role="table">{children}</StyledTable>
    </TableContext.Provider>
  );
}

function Header({ children }: TableElementProps) {
  const context = useContext(TableContext);
  if (!context) {
    throw new Error('Column must be used within a table');
  }
  const { columns } = context;
  return (
    <StyledHeader role="row" columns={columns}>
      {children}
    </StyledHeader>
  );
}
function Row({ children }: TableElementProps) {
  const context = useContext(TableContext);
  if (!context) {
    throw new Error('Row must be used within a Table');
  }
  const { columns } = context;
  return (
    <StyledRow role="row" columns={columns}>
      {children}
    </StyledRow>
  );
}
function Body({ data = [], render }: BodyElementProps) {
  if (!data.length) return <Empty>No data to show at the moment</Empty>;

  return <StyledBody>{data.map(render)}</StyledBody>;
}

Table.Header = Header;
Table.Row = Row;
Table.Body = Body;
Table.Footer = Footer;

export default Table;