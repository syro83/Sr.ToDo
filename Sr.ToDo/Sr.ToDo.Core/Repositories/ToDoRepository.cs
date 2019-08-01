using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Sr.ToDo.Core.Contracts;
using Sr.ToDo.Core.Dal;

namespace Sr.ToDo.Core.Repositories
{
	public class ToDoRepository : RepositoryBase<Dal.ToDo>, IToDoRepository
	{
		public ToDoRepository(Dal.SrToDoContext context) : base(context)
		{
		}

	}
}
