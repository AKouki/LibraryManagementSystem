# LibraryManagementSystem
A Library Management System in ASP.NET Core using Generic Repository Pattern and UnitOfWork

# Getting Started
* Download the project
* Open project with Visual Studio and wait for dependencies to be resolved
* Set `LMS.Web.Admin` as StartUp Project
* Open Package Manager Console and make sure that `Default Project` is `LMS.Web.Admin`
* Run the following commands <br />
`update-database -c ApplicationDbContext` <br />
`add-migration Initial -c LibraryContext` <br />
`update-database -c LibraryContext`
* That's all, you are ready!
