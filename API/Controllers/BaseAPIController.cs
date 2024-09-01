using API.RequestHelpers;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

//The [ApiController] and [Route("api/[controller]")] attributes are used to configure the controller's behavior and routing
[ApiController]
[Route("api/[controller]")]
public class BaseAPIController : ControllerBase
{
    protected async Task<ActionResult> CreatePagedResult<T>(IGenericRepository<T> repository,
        ISpecification<T> spec, int pageIndex, int pageSize) where T : BaseEntity
    {
        var items = await repository.ListAsync(spec);
        //get the total count after our filters (ie. spec)
        var count = await repository.CountAsync(spec);

        //pass in or values to our pagination
        var pagination = new Pagination<T>(pageIndex, pageSize, count, items);

        return Ok(pagination);
    }
}
