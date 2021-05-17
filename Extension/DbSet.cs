using Dapper;
using Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Extension
{
    public class DbSet<T> where T:BaseEntity
    {
        public SqlConnection con { get; set; }
        public string sql { get; set; }
        public IEnumerable<M> ToQuery<M>() where M:class
        {
            return con?.Query<M>(sql);
        }
        public M QueryFirst<M>() where M : class
        {
            return con?.QueryFirstOrDefault<M>(sql);
        }
    }
}
