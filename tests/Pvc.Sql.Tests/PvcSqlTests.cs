using NUnit.Framework;
using PvcPlugins;
using System;
using System.Linq;

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
                    providerName: "MsSql"));
            });
        }
    }
}
