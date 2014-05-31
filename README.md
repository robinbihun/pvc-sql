pvc-sql
=========

**WARNING**
Considered "Alpha" Quality - at best. Only a very limited test run against MSSql Server.

PVC SQL Plugin to run SQL Scripts as a task

Examples:

```
pvc.Source("db/*.sql")
   .Pipe(new PvcSql(
      connectionString: "Data Source=.;Initial Catalog=PVCSandbox;Integrated Security=True",
			providerName: "System.Data.SqlClient"
   ));
```

####PvcSql Configuration Options
The following options are available as optional/named parameters of the constructor:

**connectionString** (Required) The [connection string](http://www.connectionstrings.com/) to your database. 

**providerName** The ADO.NET provider to use. Defaults to `System.Data.SqlClient`
