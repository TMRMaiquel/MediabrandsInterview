using System;
using System.Collections.Generic;
using System.Text;

namespace INT.Domain
{
    public interface IDatabaseTransaction: IDisposable
    {
        void Commit();
        void Rollback();
    }
}
