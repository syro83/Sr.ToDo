using Sr.ToDo.Core.Contracts;

namespace Sr.ToDo.Core.Repositories
{
	public class ToDoRepository : RepositoryBase<Dal.ToDo>, IToDoRepository
	{
		public ToDoRepository(Dal.SrToDoContext context) : base(context)
		{
		}
	}
}