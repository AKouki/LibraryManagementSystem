# LibraryManagementSystem
A Library Management System in .NET 7 using Generic Repository Pattern and UnitOfWork

## Getting Started
* Download the project
* Open project with Visual Studio and wait for dependencies to be resolved
* Set `LMS.Web.Admin` as StartUp Project
* Open Package Manager Console and make sure that `Default Project` is `LMS.Web.Admin`
* Run the following commands: <br />
`update-database -Context ApplicationDbContext` <br />
`update-database -Context LibraryContext`
* That's all, you are ready!
