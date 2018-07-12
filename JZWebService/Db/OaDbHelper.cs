using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using MySql.Data.MySqlClient;
using System.Data.Common;
using JZWebService.Util;

namespace JZWebService.Db
{
    public class OaDbHelper
    {
        private static int intCommandTimeout = 300;//单位 秒
        public static string oaConn = System.Configuration.ConfigurationManager.AppSettings["oaconnString"].ToString();

        private IDbProvider m_provider;

        private object lockHelper = new object();
        public IDbProvider Provider
        {
            get
            {
                if (this.m_provider == null)
                {
                    lock (this.lockHelper)
                    {
                        if (this.m_provider == null)
                        {
                            try
                            {
                                this.m_provider = (IDbProvider)Activator.CreateInstance(Type.GetType("JZWebService.Util.MySqlServerProvider", false, true));
                            }
                            catch
                            {
                                new Terminator().Throw("SqlServerProvider 数据库访问器创建失败！");
                            }
                        }
                    }
                }
                return this.m_provider;
            }
        }

        public  int mysqlcon()
        {
            //MySqlConnection con =new MySql.Data.MySqlClient.MySqlConnection("Database='td_oa_outer';Data Source='192.168.1.89';Port=3336;UserId='oadb';Password='123456';charset='gbk'");
            MySqlConnection con = new MySql.Data.MySqlClient.MySqlConnection(oaConn);
            con.Open();
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = con;
            cmd.CommandTimeout = 1800;
            
            return 1;
        }
      
        #region  建立MySql数据库连接
        /// <summary>
        /// 建立数据库连接.
        /// </summary>
        /// <returns>返回MySqlConnection对象</returns>
        public MySqlConnection GetMysqlCon()
        {
            //MySqlConnection myCon = new MySql.Data.MySqlClient.MySqlConnection("Database='td_oa_outer';Data Source='192.168.1.89';Port=3336;UserId='oadb';Password='123456';charset='gbk'");
            // MySqlConnection myCon = new MySql.Data.MySqlClient.MySqlConnection("Database='td_oa_test';Data Source='192.168.1.89';Port=3336;UserId='oadb';Password='123456';charset='gbk'");
            MySqlConnection myCon = new MySql.Data.MySqlClient.MySqlConnection(oaConn);
            return myCon;
        }
        #endregion

        #region  执行MySqlCommand命令
        /// <summary>
        /// /// 执行MySqlCommand
        /// </summary>
        /// <param name="sqlStr">SQL语句</param>
        public void GetMysqlCom(string sqlStr)
        {
            MySqlConnection mysqlcon = this.GetMysqlCon();
            mysqlcon.Open();
            MySqlCommand mysqlcom = new MySqlCommand(sqlStr,mysqlcon);
                
            mysqlcom.ExecuteNonQuery();
            mysqlcom.Dispose();
            mysqlcon.Close();
            mysqlcon.Dispose();
        }
        #endregion

        #region  创建MySqlDataReader对象
        /// <summary>
        /// 创建一个MySqlDataReader对象
        /// </summary>
        /// <param name="M_str_sqlstr">SQL语句</param>
        /// <returns>返回MySqlDataReader对象</returns>
        public MySqlDataReader GetMysqlRead(string sqlStr)
        {
            MySqlConnection mysqlcon = this.GetMysqlCon();
            MySqlCommand mysqlcom = new MySqlCommand(sqlStr, mysqlcon);
            mysqlcon.Open();
            MySqlDataReader mysqlread = mysqlcom.ExecuteReader(CommandBehavior.CloseConnection);
            return mysqlread;
        }
        #endregion


        public T ExecuteObject<T>(string commandText)
        {
            DataSet dataSet = this.ExecuteDataset(commandText);
            if (CheckedDataSet(dataSet))
            {
                return DataHelper.ConvertRowToObject<T>(dataSet.Tables[0].Rows[0]);
            }
            return default(T);
        }

        public T ExecuteObject<T>(string commandText, List<DbParameter> prams)
        {
            DataSet dataSet = this.ExecuteDataset(CommandType.Text, commandText, prams.ToArray());
            if (CheckedDataSet(dataSet))
            {
                return DataHelper.ConvertRowToObject<T>(dataSet.Tables[0].Rows[0]);
            }
            return default(T);
        }

        public IList<T> ExecuteObjectList<T>(string commandText)
        {
            DataSet dataSet = this.ExecuteDataset(commandText);
            if (CheckedDataSet(dataSet))
            {
                return DataHelper.ConvertDataTableToObjects<T>(dataSet.Tables[0]);
            }
            return null;
        }

        public IList<T> ExecuteObjectList<T>(string commandText, List<DbParameter> prams)
        {
            DataSet dataSet = this.ExecuteDataset(CommandType.Text, commandText, prams.ToArray());
            if (CheckedDataSet(dataSet))
            {
                return DataHelper.ConvertDataTableToObjects<T>(dataSet.Tables[0]);
            }
            return null;
        }


        public DataSet ExecuteDataset(string commandText)
        {
            return this.ExecuteDataset(CommandType.Text, commandText, null);
        }

        public DataSet ExecuteDataset(CommandType commandType, string commandText)
        {
            return this.ExecuteDataset(commandType, commandText, null);
        }

        public DataSet ExecuteDataset(CommandType commandType, string commandText, params DbParameter[] commandParameters)
        {
            if (oaConn == null || oaConn.Length == 0)
            {
                throw new ArgumentNullException("ConnectionString");
            }
            DataSet result;
            using (MySqlConnection dbConnection = this.GetMysqlCon())
            {
                dbConnection.Open();
                result = this.ExecuteDataset(dbConnection, commandType, commandText, commandParameters);
            }
            return result;
        }

        public DataSet ExecuteDataset(DbConnection connection, CommandType commandType, string commandText)
        {
            return this.ExecuteDataset(connection, commandType, commandText, null);
        }

        public DataSet ExecuteDataset(DbConnection connection, CommandType commandType, string commandText, params DbParameter[] commandParameters)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }

            DbCommand dbCommand = new MySqlCommand();
            bool flag = false;
            this.PrepareCommand(dbCommand, connection, null, commandType, commandText, commandParameters, out flag);
            DataSet result=new DataSet();
            using (DbDataAdapter dbDataAdapter = new MySqlDataAdapter(dbCommand as MySqlCommand))
            {
                try
                {
                    dbDataAdapter.SelectCommand = dbCommand;                    
                    DateTime now = DateTime.Now;
                    dbDataAdapter.Fill(result);
                    DateTime now2 = DateTime.Now;
                    dbCommand.Parameters.Clear();
                    if (flag)
                    {
                        connection.Close();
                    }
                    dbDataAdapter.Dispose();
                    //result = dataSet;
                }
                catch(Exception ex)
                {
                    //string aaa = ex.Message;
                }
            }
            return result;
        }


        private void PrepareCommand(DbCommand command, DbConnection connection, DbTransaction transaction, CommandType commandType, string commandText, DbParameter[] commandParameters, out bool mustCloseConnection)
        {
            if (command == null)
            {
                throw new ArgumentNullException("command");
            }
            if (commandText == null || commandText.Length == 0)
            {
                throw new ArgumentNullException("commandText");
            }
            if (connection.State != ConnectionState.Open)
            {
                mustCloseConnection = true;
                connection.Open();
            }
            else
            {
                mustCloseConnection = false;
            }
            command.Connection = connection;
            command.CommandText = commandText;
            if (transaction != null)
            {
                if (transaction.Connection == null)
                {
                    throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
                }
                command.Transaction = transaction;
            }
            command.CommandType = commandType;
            if (commandParameters != null)
            {
                this.AttachParameters(command, commandParameters);
            }
        }

        private void AttachParameters(DbCommand command, DbParameter[] commandParameters)
        {
            if (command == null)
            {
                throw new ArgumentNullException("command");
            }
            if (commandParameters != null)
            {
                for (int i = 0; i < commandParameters.Length; i++)
                {
                    DbParameter dbParameter = commandParameters[i];
                    if (dbParameter != null)
                    {
                        if ((dbParameter.Direction == ParameterDirection.InputOutput || dbParameter.Direction == ParameterDirection.Input) && dbParameter.Value == null)
                        {
                            dbParameter.Value = DBNull.Value;
                        }
                        command.Parameters.Add(dbParameter);
                    }
                }
            }
        }


        public static bool CheckedDataSet(DataSet ds){
            return ds != null && ds.Tables != null && (ds.Tables == null || (ds.Tables.Count != 0 && ds.Tables[0] != null && ds.Tables[0].Rows.Count != 0));
        }


        public DbParameter MakeInParam(string paraName, object paraValue)
        {
            return this.MakeParam(paraName, paraValue, ParameterDirection.Input);
        }

        public DbParameter MakeParam(string paraName, object paraValue, ParameterDirection direction)
        {
            return this.Provider.MakeParam(paraName, paraValue, direction);
        }

        public int ExecuteNonQuery(string commandText)
        {
            return this.ExecuteNonQuery(CommandType.Text, commandText, null);
        }

        public int ExecuteNonQuery(CommandType commandType, string commandText)
        {
            return this.ExecuteNonQuery(commandType, commandText, null);
        }

       

        public int ExecuteNonQuery(DbConnection connection, CommandType commandType, string commandText)
        {
            return this.ExecuteNonQuery(connection, commandType, commandText, null);
        }

        public int ExecuteNonQuery(CommandType commandType, string commandText, params DbParameter[] commandParameters)
        {
            if (oaConn == null || oaConn.Length == 0){
                throw new ArgumentNullException("ConnectionString");
            }
            int result;
            using (MySqlConnection dbConnection = this.GetMysqlCon())
            {
                dbConnection.Open();
                result = this.ExecuteNonQuery(dbConnection, commandType, commandText, commandParameters);
            }
            return result;
        }

        public int ExecuteNonQuery(DbConnection connection, CommandType commandType, string commandText, params DbParameter[] commandParameters)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }
            DbCommand dbCommand = new MySqlCommand();
            bool flag = false;
            this.PrepareCommand(dbCommand, connection, null, commandType, commandText, commandParameters, out flag);
            DateTime now = DateTime.Now;
            int result = dbCommand.ExecuteNonQuery();
            DateTime now2 = DateTime.Now;
            
            dbCommand.Parameters.Clear();
            if (flag){
                connection.Close();
            }
            return result;
        }
    }
   
}