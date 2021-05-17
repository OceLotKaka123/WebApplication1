using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace DAL.ILogic
{
    public interface IGenerateCodeSql
    {
        string GetCode(int codeName, SqlTransaction transaction);
    }
}
