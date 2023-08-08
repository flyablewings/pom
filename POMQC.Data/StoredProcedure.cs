namespace POMQC.Data
{
    using System;
    using System.Collections;
    using System.Data;
    using Fos.Data;

    public sealed class StoredProcedure : ObjectDb, IDisposable
    {
        public StoredProcedure()
            : base()
        {
        }
            
        public StoredProcedure(IDbSetting setting, int commandTimeOut = 120)
            : base(setting, commandTimeOut)
        {
        }

        public StoredProcedure(string key, int commandTimeOut = 120) : base(key, commandTimeOut) 
        { 
        }

        public new void Dispose()
        {
            base.Dispose();
        }
    }
}
