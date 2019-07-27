using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sr.Reminder.Core.Repositories;

namespace Sr.Reminder.WebApi.Controllers
{
	[Produces("application/json")]
	[ApiConventionType(typeof(DefaultApiConventions))]
	[Route("api/[controller]")]
	[ApiController]
	public class ReminderController : ControllerBase
	{
		private readonly IUnitOfWork _unitOfWork;
		protected IMapper _Mapper = null;


		public ReminderController(IUnitOfWork uow)
		{
			_unitOfWork = uow;

			// ToDo: SR:SR Maybe use DI to inject the mapper
			var config = new MapperConfiguration(cfg =>
			{
				cfg.CreateMap<Core.Dal.Reminder, Models.Reminder>();
				cfg.CreateMap<Models.Reminder, Core.Dal.Reminder>();
				cfg.CreateMap<Core.Dal.Category, Models.Category>();
				cfg.CreateMap<Models.Category, Core.Dal.Category>();
				//cfg.CreateMap<Core.Dal.ReminderCategory, Models.ReminderCategory>();
				//cfg.CreateMap<Models.ReminderCategory, Core.Dal.ReminderCategory>();
				cfg.CreateMap<Core.Dal.Task, Models.Task>();
				cfg.CreateMap<Models.Task, Core.Dal.Task>();
			});
			_Mapper = config.CreateMapper();
		}


		// GET: api/Reminder
		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesDefaultResponseType]
		public async Task<ActionResult<IEnumerable<Models.Reminder>>> Get()
		{
			var reminders = await _unitOfWork.Reminders.GetAll();

			var result = _Mapper.Map<List<Models.Reminder>>(reminders);
			return this.Ok(result);
		}

		// GET: api/Reminder/5
		[HttpGet("{id}", Name = "Get")]
		[ProducesResponseType(200)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesDefaultResponseType]
		public async Task<ActionResult<Models.Reminder>> Get(int id)
		{
			var reminder = await _unitOfWork.Reminders.Get(id);

			if (reminder == null)
			{
				Response.Headers.Add("x-status-reason", $"Reminder with id '{id}' is not found.");
				return NotFound();
			}

			var result = _Mapper.Map<Models.Reminder>(reminder);
			return this.Ok(result);
		}


		/// <summary>
		/// Creates a Reminder.
		/// </summary>
		/// <remarks>
		/// Sample request:
		///
		///     POST /Reminder
		///     {
		///        "id": 1,
		///        "name": "Item1",
		///        "isComplete": true
		///     }
		///
		/// </remarks>
		/// <param name="value"></param>
		/// <returns>A newly created Reminder</returns>
		/// <response code="201">Returns the newly created reminder</response>
		/// <response code="400">If the reminder is null</response>   
		// POST: api/Reminder
		[HttpPost]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesDefaultResponseType]
		public async Task<ActionResult<Models.Reminder>> Post([FromBody] Models.Reminder value)
		{
			if (value.Id > 0)
			{
				Response.Headers.Add("x-status-reason", $"Reminder should not have an non positive id '{value.Id}' ");
				return BadRequest();
			}
			var reminder = _Mapper.Map<Core.Dal.Reminder>(value);

			_unitOfWork.Reminders.Add(reminder);

			await _unitOfWork.CommitAsync();

			//var inserted = await _reminderRepository.Add(reminder);
			//if (inserted == null || inserted.Id <= 0)
			//{
			//	Response.Headers.Add("x-status-reason", $"Reminder '{value}' is not saved.");
			//	this.BadRequest();
			//}

			//var result = _Mapper.Map<Models.Reminder>(inserted);
			//return this.Ok(result);

			return CreatedAtAction(nameof(Get), new { id = reminder.Id }, reminder);
		}

		// PUT: api/Reminder/5
		[HttpPut("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesDefaultResponseType]
		public async Task<ActionResult<Models.Reminder>> Put(int id, [FromBody] Models.Reminder value)
		{
			if (id != value.Id)
			{
				Response.Headers.Add("x-status-reason", $"Reminder with id '{value.Id}' does not match the id '{id}'.");
				return BadRequest();
			}
			var reminder = _Mapper.Map<Core.Dal.Reminder>(value);

			var entity = await _unitOfWork.Reminders.Get(id);

			if (entity == null)
			{
				Response.Headers.Add("x-status-reason", $"Reminder with id '{id}' is not found.");
				return NotFound();
			}

			entity.Description = value.Description;
			entity.DueDate = value.DueDate;
			entity.Name = value.Name;
			entity.Priority = (int)value.Priority;
			entity.Updated = DateTime.UtcNow;

			await _unitOfWork.CommitAsync();

			//var updated = await _reminderRepository.Update(reminder);
			//if (updated == null || updated.Id <= 0)
			//{
			//	Response.Headers.Add("x-status-reason", $"Reminder '{value}' is not saved.");
			//	this.BadRequest();
			//}

			//var result = _Mapper.Map<Models.Reminder>(updated);
			//return this.Ok(result);

			return NoContent();
		}

		/// <summary>
		/// Deletes a specific Reminder.
		/// </summary>
		/// <param name="id"></param> 
		// DELETE: api/Reminder/5
		[HttpDelete("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesDefaultResponseType]
		public async Task<ActionResult<bool>> Delete(int id)
		{
			var reminder = await _unitOfWork.Reminders.Get(id);

			if (reminder == null)
			{
				Response.Headers.Add("x-status-reason", $"Reminder with id '{id}' is not found.");
				return NotFound();
			}

			_unitOfWork.Reminders.Remove(reminder);

			await _unitOfWork.CommitAsync();

			//var result = await _reminderRepository.Remove(reminder);
			//if (!result)
			//{
			//	Response.Headers.Add("x-status-reason", $"Reminder with id '{id}' is not deleted.");
			//	this.BadRequest();
			//}
			//return this.Ok(result);


			return NoContent();
		}
	}
}
