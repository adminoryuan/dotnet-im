﻿using SqlSugar;
using DbType = SqlSugar.DbType;

namespace IM_Router.untils
{
    public class CodeGenerate
    {
        public void Start()
        {
            string constr = "Server=120.48.61.75;Port=3306;Database=im;Uid=root;Pwd=123hh456;charset=utf8;Allow User Variables=True";

            var db = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = constr,
                DbType = DbType.MySql,
                IsAutoCloseConnection = true
            }, db =>
            {
                db.Aop.OnLogExecuting = (sql, pars) =>
                {
                    Console.WriteLine(sql);//输出sql,查看执行sql 性能无影响

                };

            });
            string prefix = "im_";
            foreach (var item in db.DbMaintenance.GetTableInfoList())
            {
                string entityName = item.Name.Replace(prefix, "");//去除前缀

                entityName = FirstUpper(entityName);


                db.MappingTables.Add(entityName, item.Name);
                foreach (var col in db.DbMaintenance.GetColumnInfosByTableName(item.Name))
                {
                    string newName = FirstUpper(col.DbColumnName);
                    db.MappingColumns.Add(newName, col.DbColumnName, entityName);
                }
            }


            db.DbFirst.IsCreateAttribute().CreateClassFile("D:\\daima\\im\\Solution1\\IM-Router\\Models", "Models");

        }
        /// <summary>
        /// 首字母大写
        /// </summary>
        /// <param name="world"></param>
        /// <returns></returns>
        public string FirstUpper(string world)
        {
            return world.Replace(world[0].ToString(), world[0].ToString().ToUpper());

        }
    }
}
