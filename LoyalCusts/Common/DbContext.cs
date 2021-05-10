using LiteDB;
using Microsoft.Extensions.Options;
using System;

namespace LoyalCusts.Common
{
    public class DbContext
    {
        public readonly LiteDatabase Context;
        public DbContext(IOptions<DbConfig> configs)
        {
            try
            {
                var db = new LiteDatabase(configs.Value.DatabasePath);
                if (db != null)
                {
                    Context = db;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("LiteDb database could not be opened or created!", ex);
            }
        }
    }

    public class DbConfig
    {
        public string DatabasePath { get; set; }
    }
}
