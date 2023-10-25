# <h1>About The Project</h1>
The application is a web project that focuses on meal planning and management, with features tailored to help users keep track of their dietary needs. It doesn't just serve as a nutritional calculator but as a versatile meal planner that adapts to your preferences.
# <h1> Technologies used </h1>
 - C#
 - .NET 6
 - jQuery
 - HTML
 - CSS
 - SQL Server
# <h1> Features </h1>
- **Meal Planning:** Customize meal plans, add, edit, and remove food items.
- **Nutritional Tracking:** Tracks calories, protein, carbohydrates, and fat intake.
- **Search and Add:** Quickly find food items in an autocomplete search bar and add them to your meal.
- **Authorization System:** Register and Log In to the application
# <h1> Instalation </h1>
 <h3>1. SQL Server and SQL Server Managment Studio</h3>
Download both SQL Server and SSMS <br />
SQL Server - https://www.microsoft.com/en-us/sql-server/sql-server-downloads<br />
SQL Server Managment Studio - https://learn.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms?view=sql-server-ver16 <br />
<h3>2. .NET 6 </h3>
Download .NET 6 or newer version from https://dotnet.microsoft.com/en-us/download
<h3>3. Clone project </h3>
git clone https://github.com/AG0277/GymApp.git
<h3>4. Connect to database in SSMS </h3>
Connect to database using localhost
<h3>5. Set up database depenencies </h3>
Open project and find file appsettings.json, in the ConnectionStrings change "Server=Your Server;"
<h3>6. Update database </h3>
In your editor go to the Packet Manager Console and run command Update-Database
# <h3>If you run into certificate errors run command "dotnet dev-certs https --trust" in the command line</h3>
