using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sr.ToDo.Core.Contracts;
using Sr.ToDo.Core.Repositories;

namespace Sr.ToDo.WebApi.Controllers
{
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
		[HttpGet("{id}", Name = "GetToDo")]
		[ProducesResponseType(200)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesDefaultResponseType]
		public async Task<ActionResult<Models.ToDo>> Get(int id)
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


		/// <summary>
		/// Creates a ToDo.
		/// </summary>
		/// <remarks>
		/// Sample request:
		///
		///     POST /ToDo
		///     {
		///        "id": 1,
		///        "name": "Item1",
		///        "isComplete": true
		///     }
		///
		/// </remarks>
		/// <param name="value"></param>
		/// <returns>A newly created ToDo</returns>
		/// <response code="201">Returns the newly created ToDo</response>
		/// <response code="400">If the ToDo is null</response>   
		// POST: api/ToDo
		[HttpPost]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesDefaultResponseType]
		public async Task<ActionResult<Models.ToDo>> Post([FromBody] Models.ToDo value)
		{
			if (value.Id > 0)
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
		[HttpPut("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesDefaultResponseType]
		public async Task<ActionResult<Models.ToDo>> Put(int id, [FromBody] Models.ToDo value)
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

		/// <summary>
		/// Deletes a specific ToDo.
		/// </summary>
		/// <param name="id"></param> 
		// DELETE: api/ToDo/5
		[HttpDelete("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
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
