using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Dapper.Contrib.Extensions;
using MySql.Data.MySqlClient;
using Stump.ORM.SubSonic.Linq.Translation;

namespace Stump.ORM
{
    /// <summary>
    /// 
    /// </summary>
    public interface  IDbObject
    {
        IDatabase GenericTable
        {
            get;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class DbObject<T> : IDbObject
        where T : DbObject<T>, new()
    {
        public IDatabase GenericTable => Table;
        public static BasicTable<T> Table => BasicTable<T>.Instance;
    } 

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class BasicTable<T> : Database<BasicTable<T>, T>
        where T : DbObject<T>, new()
    {
        public BasicTable()
            : base(ConfigurationManager.AppSettings["db.connectionString"], 
                  ConfigurationManager.AppSettings["db.providerName"])
        {
        }

        public IEnumerable<T> All => Where("1=1");
    }

    /// <summary>
    /// 
    /// </summary>
    public interface IDatabase
    {
        int Execute(string query, dynamic parameters = null);
        Task<int> ExecuteAsync(string query, dynamic parameters = null);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TDb"></typeparam>
    /// <typeparam name="T"></typeparam>
    public abstract class Database<TDb, T> : IDatabase
        where TDb : Database<TDb, T>, new()
        where T : DbObject<T>, new()
    {
        private readonly Func<IDbConnection> _connectionProvider; 
        private readonly string _providerName;
        private readonly string _connectionString;
        private readonly string _table;

        public static TDb Instance => new TDb();

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        protected Database(string connectionString, string providerName)
        {
            _connectionString = connectionString;
            _providerName = providerName;
            _table = ((TableAttribute)typeof(T).GetCustomAttribute(typeof(TableAttribute))).Name;

            switch(_providerName.ToLower())
            {
                case "mysql":
                    _connectionProvider = () => new MySqlConnection(_connectionString);
                    break;

                case "sql":
                    _connectionProvider = () => new SqlConnection(_connectionString);
                    break;

                default:
                    throw new ArgumentException(string.Format("Unknow providerName for IDbConnection, providerName: {0}", _providerName));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected IDbConnection CreateConnection()
        {
            return _connectionProvider();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        public TResult WrapExecution<TResult>(Func<IDbConnection, TResult> func)
        {
            // We instantiate connections on the fly, disposing them when the query ended.
            // Since the framework will care about pooling the connections when closed, we dont need that logic
            using (var con = CreateConnection())
                return func(con);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IEnumerable<T> Query(string query, dynamic parameters = null)
        {
            return WrapExecution(con => con.Query<T>(query, (object)parameters));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> QueryAsync(string query, dynamic parameters = null)
        {
            return await WrapExecution(async con => await con.QueryAsync<T>(query, (object)parameters));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public int Execute(string query, dynamic parameters = null)
        {
            return WrapExecution(con => con.Execute(query, (object)parameters));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<int> ExecuteAsync(string query, dynamic parameters = null)
        {
            return await WrapExecution(async con => await con.ExecuteAsync(query, (object)parameters));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public IEnumerable<T> Where(string where, dynamic parameters = null)
        {
            return Query($"select * from {_table} where {where}", parameters);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> WhereAsync(string where, dynamic parameters = null)
        {
            return await QueryAsync($"select * from {_table} where {where}", parameters);
        }

        /// <summary>
        /// 
        /// </summary>
        public int DeleteAll()
        {
            return Execute($"delete from {_table}");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public T GetById(long id)
        {
            return WrapExecution(con => con.Get<T>(id));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<T> GetByIdAsync(long id)
        {
            return await WrapExecution(async con => await con.GetAsync<T>(id));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool Insert(T obj)
        {
            return WrapExecution(con => con.Insert(obj)) > 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool Delete(T obj)
        {
            return WrapExecution(con => con.Delete(obj));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public async Task<bool> DeleteAsync(T obj)
        {
            return await WrapExecution(async con => await con.DeleteAsync(obj));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool Update(T obj)
        {
            return WrapExecution(con => con.Update(obj));
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(T obj)
        {
            return await WrapExecution(async con => await con.UpdateAsync(obj));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public async Task<bool> InsertAsync(T obj)
        {
            return await WrapExecution(async con => await con.InsertAsync(obj)) > 0;
        }
    }
}
