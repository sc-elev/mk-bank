Secret sauce to clone this MVC_ASP project.

- $ git clone whatever-the-url
  $ cd mk-bank
  
Start Visual studio, open the bank.sln file
Start the Nuget package mgmt console, let it update the
new padkages

In the console:

  > Uninstall-package -force EntityFramework -force
  > Install-package EntityFramework

Remove any Migrations/\*.cs file existing.
Update the DefauiltConnection database string in Web.config
(two places) to reset the db file.

  > Update-Database

