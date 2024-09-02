// set up our type for our pagination for type safety in typescript
export type Pagination<T> = {
    pageIndex: number;
    pageSize: number;
    count: number;
    data: T[]
}