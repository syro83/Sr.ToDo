https://docs.microsoft.com/en-us/ef/core/get-started/aspnetcore/existing-db

Install-Package Microsoft.EntityFrameworkCore
Install-Package Microsoft.EntityFrameworkCore.SqlServer


Tools –> NuGet Package Manager –> Package Manager Console

Run the following command to create a model from the existing database:
	Scaffold-DbContext "Server=.;Database=SrToDo;Trusted_Connection=True;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Dal -Context "SrToDoContext" -DataAnnotations -f

Change the Default Project sometimes do not work, so create it in the Api and then move the files :S
