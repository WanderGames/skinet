namespace Core.Specifications;

public class ProductSpecParams
{
    //imagine we have 1,000,000 products, we dont want the client to be able to request a million products at once
    //so we use pagination and set a maxmimum number of products per page
    private const int MaxPageSize = 50;

    public int PageIndex { get; set; } = 1;

    private int _pageSize = 6;
    public int PageSize
    {
        get => _pageSize;
        //check if the page size is greater than the max page size if it is we set it to the max page size or if it is less than 1 we set it to 1 (the value)
        set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
    }
    

    private List<string> _brands = [];
    public List<string> Brands
    {
        get => _brands; // example boards, gloves, so value would be type=boards,gloves
        set
        {   
            //split by comma
            _brands = value.SelectMany(x => x.Split(',', StringSplitOptions.RemoveEmptyEntries)).ToList();
        }
    }

    private List<string> _types = [];
    public List<string> Types
    {
        get => _types;
        set
        {   
            //split by comma
            _types = value.SelectMany(x => x.Split(',', StringSplitOptions.RemoveEmptyEntries)).ToList();
        }
    }

    public string? Sort { get; set; }

    private string? _search;
    public string Search
    {
        get => _search ?? "";
        set => _search = value.ToLower();
    }
    
    
}
