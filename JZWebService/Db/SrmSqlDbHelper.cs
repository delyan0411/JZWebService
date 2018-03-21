using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using MySql.Data.MySqlClient;
using System.Data.Common;
using JZWebService.Util;

namespace JZWebService.Db
{
    public class SrmSqlDbHelper
    {
        private static int intCommandTimeout = 1800;//单位 秒
        protected string  m_connectionstring;
    
        private IDbProvider m_provider;

        private DbProviderFactory m_factory;

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
                                this.m_provider = (IDbProvider)Activator.CreateInstance(Type.GetType("JZWebService.Util.SqlServerProvider", false, true));
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
        protected internal string ConnectionString
        {
            get
            {
                return this.m_connectionstring;
            }
            set
            {
                this.m_connectionstring = value;
            }
        }

        public DbProviderFactory Factory
        {
            get
            {
                if (this.m_factory == null)
                {
                    this.m_factory = this.Provider.Instance();
                }
                return this.m_factory;
            }
        }
        public SrmSqlDbHelper(string connString){
			this.BuildConnection(connString);
		}

        public SrmSqlDbHelper()
        {
            // TODO: Complete member initialization
        }

		public void BuildConnection(string connectionString)
		{
			if (string.IsNullOrEmpty(connectionString))
			{
				new Terminator().Throw("请检查数据库连接信息，当前数据库连接信息为空。");
			}
			this.m_connectionstring = connectionString;
		}
       
     
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
            if (this.ConnectionString == null || this.ConnectionString.Length == 0)
            {
                throw new ArgumentNullException("ConnectionString");
            }
            DataSet result;
            using (DbConnection dbConnection = this.Factory.CreateConnection())
            {
                dbConnection.ConnectionString = this.ConnectionString;
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
            DbCommand dbCommand = this.Factory.CreateCommand();
            dbCommand.CommandTimeout = intCommandTimeout;
            bool flag = false;
            this.PrepareCommand(dbCommand, connection, null, commandType, commandText, commandParameters, out flag);
            DataSet result;
            using (DbDataAdapter dbDataAdapter = this.Factory.CreateDataAdapter())
            {
                dbDataAdapter.SelectCommand = dbCommand;
                DataSet dataSet = new DataSet();
                DateTime now = DateTime.Now;
                dbDataAdapter.Fill(dataSet);
                DateTime now2 = DateTime.Now;
                dbCommand.Parameters.Clear();
                if (flag)
                {
                    connection.Close();
                }
                result = dataSet;
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
            if (this.ConnectionString == null || this.ConnectionString.Length == 0)
            {
                throw new ArgumentNullException("ConnectionString");
            }
            int result;
            using (DbConnection dbConnection = this.Factory.CreateConnection())
            {
                dbConnection.ConnectionString = this.ConnectionString;
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
            DbCommand dbCommand = this.Factory.CreateCommand();
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