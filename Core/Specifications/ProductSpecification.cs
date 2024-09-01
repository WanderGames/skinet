using Core.Entities;

namespace Core.Specifications;

public class ProductSpecification : BaseSpecification<Product>
{
    public ProductSpecification(ProductSpecParams specParams) : base(x => 
        //apply filters
        //filter by search (Product Name contains)
        (string.IsNullOrEmpty(specParams.Search) || x.Name.ToLower().Contains(specParams.Search)) &&
        //filter by brand
        (specParams.Brands.Count == 0 || specParams.Brands.Contains(x.Brand)) &&
        //filter by type
        (specParams.Types.Count == 0 || specParams.Types.Contains(x.Type))
    )
    {

        //apply pagination
        //ex page size of 5 and page index of 1, we want to skip the first 5 items and take the next 5
        //First Page: ApplyPaging(Skip: 5 * (1-1) = 0, Take: 5) so first page will be 0-5
        //Second Page: ApplyPaging(Skip: 5 * (2-1) = 5, Take: 5) so second page will be 5-10
        //Third Page: ApplyPaging(Skip: 5 * (3-1) = 10, Take: 5) so third page will be 10-15
        //each time we are taking PageSize(5 in this case) of products starting at the skip value
        ApplyPaging(specParams.PageSize * (specParams.PageIndex - 1), specParams.PageSize);

        //set up our sorting options
        switch (specParams.Sort)
        {
            case "priceAsc":
                AddOrderBy(x => x.Price);
                break;
            case "priceDesc":
                AddOrderByDescending(x => x.Price);
                break;
            default:
                AddOrderBy(x => x.Name);
                break;
        }
    }
}
