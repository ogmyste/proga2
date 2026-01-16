using System.Threading.Tasks;
using KooliProjekt.Application.Features.Authors;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KooliProjekt.WebAPI.Controllers
{
    public class AuthorsController : ApiControllerBase
    {
        private readonly IMediator _mediator;
        public AuthorsController(IMediator mediator) 
        { 
            _mediator = mediator;
        }

        [HttpGet]
        [Route("List")]
        public async Task<IActionResult> List([FromQuery] ListAuthorsQuery query)
        {
            var response = await _mediator.Send(query);

            return Result(response);
        }

        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> Get(int id)
        {
            var query = new GetAuthorQuery { Id = id };
            var response = await _mediator.Send(query);

            return Result(response);
        }

        [HttpPost]
        [Route("Save")]
        public async Task<IActionResult> Save(SaveAuthorCommand command)
        {
            var response = await _mediator.Send(command);

            return Result(response);
        }

        [HttpDelete]
        [Route("Delete")]
        public async Task<IActionResult> Delete(DeleteAuthorCommand command)
        {
            var response = await _mediator.Send(command);

            return Result(response);
        }
    }
}
