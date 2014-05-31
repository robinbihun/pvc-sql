using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using PvcCore;
using PvcPlugins;

namespace Pvc.Sql.Tests
{
    [TestFixture]
    public class PvcSqlTests
    {
        [Test]
        public void CanExecuteSqlScript()
        {
            var pvc = new PvcCore.Pvc();
            var source = pvc.Source("*.sql");

            Assert.DoesNotThrow(() =>
            {
                source.Pipe((sources) =>
                {
                    foreach (var src in sources)
                    {
                        Console.WriteLine(src.StreamName);
                    }
                    
                    Assert.AreEqual(1, sources.Count());
                    
                    return sources;
                });
                source.Pipe(new PvcSql(
                    connectionString: "Data Source=.;Initial Catalog=PVCSandbox;Integrated Security=True",
                    providerName: "System.Data.SqlClient"));
            });
        }
    }
}
