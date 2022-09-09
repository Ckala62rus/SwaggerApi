using Architecture.DAL;
using System;

namespace Architecture.Tests
{
    public class TestCommandBase : IDisposable
    {
        protected readonly ApplicationDbContext Context;

        public TestCommandBase()
        {
            Context = DbMemoryContext.Create();
        }

        public void Dispose()
        {
            DbMemoryContext.Destroy(Context);
        }
    }
}
