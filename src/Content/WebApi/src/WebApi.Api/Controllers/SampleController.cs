using Elastic.Apm;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Net.Mime;
using WebApi.Api.Localization;
using WebApi.Api.Models.Requests;
using WebApi.Api.Models.Responses;
using WebApi.Domain.Repositories;
using WebApi.Domain.Services;

namespace WebApi.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    public class SampleController : ControllerBase
    {
        private readonly ISampleService _service;
        private readonly ISampleRepository _repository;
        private readonly IStringLocalizer<Resource> _stringLocalizer;

        public SampleController(
            ISampleService service,
            ISampleRepository repository,
            IStringLocalizer<Resource> stringLocalizer)
        {
            _service = service;
            _repository = repository;
            _stringLocalizer = stringLocalizer;
        }

        /// <summary>Describe briefly what your endpoint does.</summary>
        /// <remarks>Here you can add a full description of what your endpoint does.</remarks>
        /// <param name="id" example="1">You can comment your param, saying what its purpose is.</param>
        /// <response code="200">Specify all http status codes your endpoint should use.</response>
        /// <response code="404">And please, check out the status code RFC.</response>
        /// <response code="500">You can specify the most probable reason why a Server error may occurs.</response>
        [HttpGet("{id}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(SampleResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Get(int? id)
        {
            var span = Agent.Tracer.CurrentTransaction.StartSpan("Request Test", "Request");
            try
            {
                if (!id.HasValue)
                {
                    return BadRequest();
                }

                var sample = _service.GetSampleBy(id.Value);

                if (sample is null)
                {
                    return NotFound();
                }

                return Ok(SampleResponse.From(sample));
            }
            finally
            {
                span.End();
            }
        }

        /// <summary>This endpoint is just to show how to work with objects at querystring.</summary>
        /// <param name="queryString">I recommend you do not disable SA1611, because it helps you!</param>
        /// <param name="page">Paging.</param>
        /// <response code="200">Specify all http status codes your endpoint should use.</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetObjectFromQuery(
            [FromQuery] QueryStringSample queryString,
            [FromQuery] PaginationRequest page)
        {
            return Ok(new
            {
                queryString,
                page,
            });
        }

        [HttpGet("translate/{translate}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetObjectFromQuery(
            [FromRoute] string translate)
        {
            return Ok(new
            {
                Word = _stringLocalizer[translate],
            });
        }
    }
}
