dotnet tool install --global dotnet-ef
dotnet tool update --global dotnet-ef

dotnet tool install --global dotnet-aspnet-codegenerator
dotnet tool update --global dotnet-aspnet-codegenerator

Run from solution folder 
~~~sh
dotnet ef migrations add InitialCreate --project DAL --startup-project ConsoleApp 
dotnet ef database update --project DAL --startup-project ConsoleApp 
dotnet ef database drop --project DAL --startup-project ConsoleApp  
~~~
