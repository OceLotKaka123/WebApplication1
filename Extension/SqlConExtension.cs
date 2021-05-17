using Dapper;
using Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Extension
{
    public static class SqlConExtension
    {
        public static int UpdateReord<T>(this SqlConnection con,T entity,SqlTransaction transaction=null) where T:BaseEntity
        {
            var type=entity.GetType();
            var sql = "update ";
            sql+= ((TableAttribute)type.GetCustomAttributes(typeof(TableAttribute),false)[0]).Name+" set ";
            var fields = type.GetProperties();
            string keyName = " where ";
            int i = 0;
            foreach(var field in fields)
            {
               var key= field.GetCustomAttributes(typeof(KeyAttribute),false);
                if (key.Length>0)
                {
                    keyName += $"{field.Name}=@{field.Name}";
                }
                else
                {
                    if (field.GetValue(entity, null) != null)
                    {
                        if (i == 0)
                        {
                            sql += $"{field.Name}=@{field.Name} ";
                        }
                        else
                        {
                            sql += $",{field.Name}=@{field.Name}";
                        }
                        i++;
                    }
                }
            }
            sql += keyName;
            return con.Execute(sql, entity);
        }
        public static int DeleteRecord<T>(this SqlConnection con,T entity,SqlTransaction transaction=null) where T : BaseEntity
        {
            var type = entity.GetType();
            string tableName=((TableAttribute)type.GetCustomAttributes(typeof(TableAttribute), false)[0]).Name;
            string sql = $"update {tableName} set valid=0 where ";
            var fields = type.GetProperties();
            foreach (var field in fields)
            {
                var key = field.GetCustomAttributes(typeof(KeyAttribute), false);
                if (key.Length > 0)
                {
                    sql += $"{field.Name}=@{field.Name}";
                    break;
                }
            }
            return con.Execute(sql,entity,transaction);
        }
        public static int DeleteRecords<T>(this SqlConnection con, IEnumerable<T> entities, SqlTransaction transaction = null) where T : BaseEntity
        {
            if (entities.Count() == 0)
            {
                return 0;
            }
            var type = entities.First().GetType();
            string tableName = ((TableAttribute)type.GetCustomAttributes(typeof(TableAttribute), false)[0]).Name;
            string sql = $"update {tableName} set valid=0 where ";
            var fields = type.GetProperties();
            foreach (var field in fields)
            {
                var key = field.GetCustomAttributes(typeof(KeyAttribute), false);
                if (key.Length > 0)
                {
                    sql += $"{field.Name} = @{field.Name}";
                    break;
                }
            }
            return con.Execute(sql, entities, transaction);
        }
        public static T AddRecord<T>(this SqlConnection con, T entity,SqlTransaction transaction=null) where T : BaseEntity
        {
            var type = entity.GetType();
            var sqlPara = "";
            var sqlValue = "";
            string tableName = ((TableAttribute)type.GetCustomAttributes(typeof(TableAttribute), false)[0]).Name;
            var fields = type.GetProperties();
            string keyName = "";
            int i = 0;
            foreach (var field in fields)
            {
                var key = field.GetCustomAttributes(typeof(KeyAttribute), false);
                if (key.Length > 0)
                {
                    keyName = field.Name;
                }
                else
                {
                    if (field.GetValue(entity,null)!= null)
                    {
                        if (i == 0)
                        {
                            sqlPara += field.Name;
                            sqlValue += $"@{field.Name}";
                        }
                        else
                        {
                            sqlPara += $",{field.Name}";
                            sqlValue += $",@{field.Name}";
                        }
                        i++;
                    }
                }
            }
            string sql = $"insert into {tableName}({sqlPara}) values({sqlValue});select *from {tableName} where {keyName}=SCOPE_IDENTITY()";
            if (entity == null)
            {
                return null; 
            }
            var entitys= con.QueryFirst<T>(sql, entity,transaction);
            return entitys;
        }
        public static int AddRecords<T>(this SqlConnection con, IEnumerable<T> entities, SqlTransaction transaction = null) where T : BaseEntity
        {
            var type = entities.First().GetType();
            var sqlPara = "";
            var sqlValue = "";
            string tableName = ((TableAttribute)type.GetCustomAttributes(typeof(TableAttribute), false)[0]).Name;
            var fields = type.GetProperties();
            string keyName = "";
            int i = 0;
            foreach (var field in fields)
            {
                var key = field.GetCustomAttributes(typeof(KeyAttribute), false);
                if (key.Length > 0)
                {
                    keyName = field.Name;
                }
                else
                {
                    if (field.GetValue(entities.First(), null) != null)
                    {
                        if (i == 0)
                        {
                            sqlPara += field.Name;
                            sqlValue += $"@{field.Name}";
                        }
                        else
                        {
                            sqlPara += $",{field.Name}";
                            sqlValue += $",@{field.Name}";
                        }
                        i++;
                    }
                }
            }
            int rows = 0;
            if (entities != null||entities.Count()>0)
            {
                string sql = $"insert into {tableName}({sqlPara}) values({sqlValue});";
                rows = con.Execute(sql, entities, transaction);
            }
            return rows;
        }
        public static DbSet<T> Where<T>(this DbSet<T> dbSet, Expression<Func<T, bool>> whereEx) where T : BaseEntity
        {
            string whereSql=whereEx.GetSqlFromExpression<T>();
            if (dbSet.sql.Contains(" where"))
            {
                dbSet.sql +=$" and {whereSql}";
            }
            else
            {
                dbSet.sql += $" where {whereSql}";
            }
            return dbSet;
        }
        public static DbSet<T> GetEntities<T>(this SqlConnection con) where T : BaseEntity
        {
            DbSet<T> dbSet = new DbSet<T>();
            dbSet.con = con;
            var type = typeof(T);
            string tableName = ((TableAttribute)type.GetCustomAttributes(typeof(TableAttribute), false)[0]).Name;
            dbSet.sql = $"select *from {tableName}";
            return dbSet;
        }
    }
}
