# C# ASP.NET crawler
A little ASP.NET service which is able to get data form 
any JSON-returning API and save it to the database.

# Deployment

install dotnet 6.0 using instructions here: https://learn.microsoft.com/en-gb/dotnet/core/install/

```bash
git clone https://github.com/ScriptHound/csharp_crawler.git
```

### Please write a new appsettings.json from appsettings_example.json template

Fill in PostgreSQL connection parameters so they fit your 
PostgreSQL database.

Then you can use two options to migrate database for this application

### Migrate using EF migration tool
```bash
dotnet ef database update
```

### Migrate using SQL script
For the manual migration using SQL script please use create_database.sql file

```bash
psql -U username -p 5432 -h hostname -d database_name -f create_database.sql
```

### Run application
```bash
dotnet run
```

# Manual testing
Please open http://hostname:hostport/swagger for testing
