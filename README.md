pvc-sql
=========

PVC SQL Plugin to run SQL Scripts as a task

Examples:

```
pvc.Source("db/*.sql")
   .Pipe(new PvcSql(
      connectionString: "Data Source=.;Initial Catalog=PVCSandbox;Integrated Security=True",
			providerName: "MsSql"
   ));
```

####PvcSql Configuration Options
The following options are available as named parameters of the constructor:

**connectionString** (Required) The [connection string](http://www.connectionstrings.com/) to your database. 

**providerName** The database provider to use. Defaults to `MsSql`. Valid values are:
* `MsSql`
* `MySql`
* `PostgreSql`
