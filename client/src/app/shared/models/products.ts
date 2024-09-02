//set up our type for our product for type safety in typescript
export type Product = {
    id: number;
    name: string;
    description: string;
    price: number;
    pictureUrl: string;
    type: string;
    brand: string;
    quantityInStock: number;
}