using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sr.ToDo.Core.Contracts;

namespace Sr.ToDo.WebApi.Controllers
{
	/// <summary>
	/// Represents a collection of functions to interact with the ToDo API endpoints
	/// </summary>
	[Produces("application/json")]
	[ApiConventionType(typeof(DefaultApiConventions))]
	[Route("api/[controller]")]
	[ApiController]
	public class ToDoController : ControllerBase
	{
		private readonly IUnitOfWork _unitOfWork;
		protected IMapper _Mapper = null;

		public ToDoController(IUnitOfWork uow)
		{
			_unitOfWork = uow;

			// ToDo: SR:SR Maybe use DI to inject the mapper
			var config = new MapperConfiguration(cfg =>
			{
				cfg.CreateMap<Core.Dal.ToDo, Models.ToDo>();
				cfg.CreateMap<Models.ToDo, Core.Dal.ToDo>();
			});
			_Mapper = config.CreateMapper();
		}

		// GET: api/ToDo
		/// <summary>
		/// Fetch all ToDos
		/// </summary>
		/// <remarks>Returns all ToDo entities</remarks>
		/// <returns>List&lt;ToDo?&gt;</returns>
		/// <exception cref="Sr.ToDo.WebApi.ApiException">Thrown when fails to make API call</exception>
		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesDefaultResponseType]
		public async Task<ActionResult<IEnumerable<Models.ToDo>>> Get()
		{
			var ToDos = await _unitOfWork.ToDos.GetAll();

			var result = _Mapper.Map<List<Models.ToDo>>(ToDos);
			return this.Ok(result);
		}

		// GET: api/ToDo/5
		/// <summary>
		/// Find ToDo by ID
		/// </summary>
		/// <param name="id">The id of ToDo that needs to be fetched</param>
		/// <remarks>Returns a single ToDo</remarks>
		/// <returns>ToDo</returns>
		/// <response code="200">Returned ToDo</response>
		/// <response code="404">If the ToDo with the supplied id is not found</response>
		[HttpGet("{id}", Name = "GetToDo")]
		[ProducesResponseType(200)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesDefaultResponseType]
		public async Task<ActionResult<Models.ToDo>> Get([Required] int id)
		{
			var toDo = await _unitOfWork.ToDos.Get(id);

			if (toDo == null)
			{
				Response.Headers.Add("x-status-reason", $"ToDo with id '{id}' is not found.");
				return NotFound();
			}

			var result = _Mapper.Map<Models.ToDo>(toDo);
			return this.Ok(result);
		}

		// POST: api/ToDo
		/// <summary>
		/// Add a new ToDo
		/// </summary>
		/// <remarks></remarks>
		/// <param name="value">ToDo object that needs to be added</param>
		/// <returns>A newly created ToDo</returns>
		/// <response code="201">Returns the newly created ToDo</response>
		/// <response code="400">If the ToDo is null or when the id is non zero.</response>
		[HttpPost]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesDefaultResponseType]
		public async Task<ActionResult<Models.ToDo>> Post([FromBody, Required] Models.ToDo value)
		{
			if (value.Id != 0)
			{
				Response.Headers.Add("x-status-reason", $"ToDo should not have an non positive id '{value.Id}' ");
				return BadRequest();
			}
			var ToDo = _Mapper.Map<Core.Dal.ToDo>(value);

			_unitOfWork.ToDos.Add(ToDo);

			await _unitOfWork.CommitAsync();

			//var inserted = await _ToDoRepository.Add(ToDo);
			//if (inserted == null || inserted.Id <= 0)
			//{
			//	Response.Headers.Add("x-status-reason", $"ToDo '{value}' is not saved.");
			//	this.BadRequest();
			//}

			//var result = _Mapper.Map<Models.ToDo>(inserted);
			//return this.Ok(result);

			return CreatedAtAction(nameof(Get), new { id = ToDo.Id }, ToDo);
		}

		// PUT: api/ToDo/5
		/// <summary>
		/// Updates a specific ToDo
		/// </summary>
		/// <param name="id">The id of ToDo that needs to be updated</param>
		/// <param name="value">ToDo object that needs to be updated</param>
		/// <returns>NoContent</returns>
		/// <response code="204">Returns the updated item</response>
		/// <response code="400">
		/// If the item is null or when the id does not matches the id of the item.
		/// </response>
		/// <response code="404">If the ToDo with the supplied id is not found</response>
		[HttpPut("{id}")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesDefaultResponseType]
		public async Task<ActionResult<Models.ToDo>> Put(int id, [FromBody, Required] Models.ToDo value)
		{
			if (id != value.Id)
			{
				Response.Headers.Add("x-status-reason", $"ToDo with id '{value.Id}' does not match the id '{id}'.");
				return BadRequest();
			}
			var ToDo = _Mapper.Map<Core.Dal.ToDo>(value);

			var entity = await _unitOfWork.ToDos.Get(id);

			if (entity == null)
			{
				Response.Headers.Add("x-status-reason", $"ToDo with id '{id}' is not found.");
				return NotFound();
			}

			entity.Description = value.Description;
			entity.Name = value.Name;
			entity.Priority = (int)value.Priority;
			entity.Updated = DateTime.UtcNow;

			await _unitOfWork.CommitAsync();

			//var updated = await _ToDoRepository.Update(ToDo);
			//if (updated == null || updated.Id <= 0)
			//{
			//	Response.Headers.Add("x-status-reason", $"ToDo '{value}' is not saved.");
			//	this.BadRequest();
			//}

			//var result = _Mapper.Map<Models.ToDo>(updated);
			//return this.Ok(result);

			return NoContent();
		}

		// DELETE: api/ToDo/5
		/// <summary>
		/// Deletes a specific ToDo
		/// </summary>
		/// <param name="id">The id of ToDo that needs to be deleted</param>
		/// <returns>NoContent</returns>
		/// <response code="204">Returns no content</response>
		/// <response code="404">If the ToDo with the supplied id is not found</response>
		[HttpDelete("{id}")]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesDefaultResponseType]
		public async Task<ActionResult<bool>> Delete(int id)
		{
			var toDo = await _unitOfWork.ToDos.Get(id);

			if (toDo == null)
			{
				Response.Headers.Add("x-status-reason", $"ToDo with id '{id}' is not found.");
				return NotFound();
			}

			_unitOfWork.ToDos.Remove(toDo);

			await _unitOfWork.CommitAsync();

			//var result = await _ToDoRepository.Remove(ToDo);
			//if (!result)
			//{
			//	Response.Headers.Add("x-status-reason", $"ToDo with id '{id}' is not deleted.");
			//	this.BadRequest();
			//}
			//return this.Ok(result);

			return NoContent();
		}
	}
}