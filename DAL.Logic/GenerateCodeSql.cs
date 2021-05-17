using DAL.ILogic;
using Dapper;
using Entities;
using Extension;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace DAL.Logic
{
    public class GenerateCodeSql : IGenerateCodeSql
    {
        private readonly SqlConnection _con;
        private static readonly object codeObject = new object();
        public GenerateCodeSql(SqlConnection con)
        {
            _con = con;
        }
        public string GetCode(int codeName,SqlTransaction transaction)
        {
            lock (codeObject) {
                var generaCodes = _con.QueryFirst<GeneraCodesEntity>(@"select 
                  *from GeneraCodes 
                  where CodeName=@codeName", new { codeName });
                string code = "";
                if (generaCodes != null)
                {
                    generaCodes.CurrentNum += 1;
                    code = $"{generaCodes.StartSign}{CodeString("0",generaCodes.MaxNum.ToString().Length-generaCodes.CurrentNum.ToString().Length-1)}{(generaCodes.CurrentNum)}";
                    _con.UpdateReord(generaCodes,transaction);
                }
                return code;
            }
        }
        private string CodeString(string code,int length)
        {
            string codeName = code;
            for(int i = 0; i < length; i++)
            {
                codeName += code;
            }
            return codeName;
        }
    }
}
