Secret sauce to clone this MVC_ASP project.

- $ git clone whatever-the-url
  $ cd mk-bank
  
Start Visual studio, open the bank.sln file. Start the Nuget package 
mgmt console, let it update the new padkages. In the console:

  PM> Update-package --reinstall EntityFramework

Remove any Migrations/\*.cs file existing. Then reset the dataase:

  PM> sqllocaldb info
  PM> sqllocaldb stop v11.0
  PM> sqllocaldb delete v11.0
  PM> update-database

