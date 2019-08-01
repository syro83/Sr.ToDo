using System.Threading.Tasks;

namespace Sr.ToDo.Core.Contracts
{
	public interface IUnitOfWork
	{
		IToDoRepository ToDos { get; }

		int Commit();

		Task<int> CommitAsync();

		void Rollback();
	}
}