using F1Soft.Starmap.Service.Controllers.Database.Models;
using System.Data;
using System.Text;
using Npgsql;

namespace F1Soft.Starmap.Service.Helper;

/// <summary>
/// 데이터베이스 작업을 위한 헬퍼 클래스
/// </summary>
public class PostgreSQLHelper : IDatabaseHelper
{
    private readonly NpgsqlConnection _connection;
    private readonly ILogger _logger;

    /// <summary>
    /// DatabaseHelper 생성자
    /// </summary>
    /// <param name="connectionString"></param>
    /// <param name="logger"></param>
    public PostgreSQLHelper(string connectionString, ILogger logger)
    {
        _connection = new NpgsqlConnection(connectionString);
        _logger = logger;
        _connection.Open();
    }

    /// <summary>
    /// Stored Procedure 비동기 호출
    /// </summary>
    /// <param name="dbProcs"></param>
    /// <returns></returns>
    public async Task<DbResult> CallProcedureAsync(List<DbProcedureRequest> dbProcs)
    {
        using (var transaction = _connection.BeginTransaction())
        {
            DbResult result = new DbResult();
            result.Result = false;
            result.ResultList = new List<DbResultItem>();
            StringBuilder errorMessages = new StringBuilder();

            foreach (var procInfo in dbProcs)
            {
                var procedureName = procInfo.ProcedureName;
                var parameters = procInfo.Parameters;
                DbResultItem sqlDataItem = new DbResultItem();
                sqlDataItem.DataTable = new DataTable();
                sqlDataItem.Name = procedureName;

                var command = new NpgsqlCommand(procedureName, _connection)
                {
                    CommandType = CommandType.StoredProcedure,
                    Transaction = transaction
                };

                if (procInfo.Parameters != null)
                {
                    //숫자, 날짜 파라미터 테스트 필요
                    foreach (var param in procInfo.Parameters)
                    {
                        command.Parameters.AddWithValue(param.Key, param.Value);
                    }
                }

                if (procInfo.IsChangeProcedure)
                {
                    try
                    {
                        sqlDataItem.ReturnValue = await command.ExecuteNonQueryAsync();
                    }
                    catch (NpgsqlException ex)
                    {
                        errorMessages.AppendLine($"Error in {procInfo.ProcedureName}: {ex.Message}");
                        transaction.Rollback();
                    }
                }
                else
                {
                    try
                    {
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            DataTable dt = new DataTable();

                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                int suffix = 1;
                                string newColumnName = reader.GetName(i);

                                while (dt.Columns.Contains(newColumnName))
                                {
                                    newColumnName = $"{reader.GetName(i)}_{suffix}";
                                    suffix++;
                                }

                                dt.Columns.Add(new DataColumn(newColumnName, reader.GetFieldType(i)));
                            }

                            while (await reader.ReadAsync())
                            {
                                object[] values = new object[reader.FieldCount];
                                reader.GetValues(values);
                                dt.Rows.Add(values);
                            }
                            sqlDataItem.DataTable = dt;
                        }
                    }
                    catch (NpgsqlException ex)
                    {
                        errorMessages.AppendLine($"Error in {procInfo.ProcedureName}: {ex.Message}");
                        transaction.Rollback();
                    }
                }
                result.ResultList.Add(sqlDataItem);
            }

            if (errorMessages.Length > 0)
            {
                result.ErrorMessage = errorMessages.ToString();
                return result;
            }

            result.Result = true;
            transaction.Commit();
            return result;
        }
    }

    /// <summary>
    /// Sql Command Procedure 비동기 호출
    /// </summary>
    /// <param name="dbSqls"></param>
    /// <returns></returns>
    public async Task<DbResult> ExecuteSqlCommandAsync(List<DbSqlRequest> dbSqls)
    {
        using (var transaction = _connection.BeginTransaction())
        {
            DbResult result = new DbResult();
            result.Result = false;
            result.ResultList = new List<DbResultItem>();
            StringBuilder errorMessages = new StringBuilder();

            foreach (var sqlInfo in dbSqls)
            {
                DbResultItem sqlDataItem = new DbResultItem();
                sqlDataItem.DataTable = new DataTable();
                sqlDataItem.Name = dbSqls.IndexOf(sqlInfo).ToString();


                var command = sqlInfo.SqlCommand as NpgsqlCommand;
                command!.Connection = _connection;
                command!.CommandType = CommandType.Text;
                command!.Transaction = transaction;

                if (sqlInfo.IsChangeProcedure)
                {
                    try
                    {
                        sqlDataItem.ReturnValue = await command.ExecuteNonQueryAsync();
                    }
                    catch (NpgsqlException ex)
                    {
                        errorMessages.AppendLine($"Error in {sqlInfo.SqlCommand}: {ex.Message}");
                        transaction.Rollback();
                    }
                }
                else
                {
                    try
                    {
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            DataTable dt = new DataTable();

                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                int suffix = 1;
                                string newColumnName = reader.GetName(i);

                                while (dt.Columns.Contains(newColumnName))
                                {
                                    newColumnName = $"{reader.GetName(i)}_{suffix}";
                                    suffix++;
                                }

                                dt.Columns.Add(new DataColumn(newColumnName, reader.GetFieldType(i)));
                            }

                            while (await reader.ReadAsync())
                            {
                                object[] values = new object[reader.FieldCount];
                                reader.GetValues(values);
                                dt.Rows.Add(values);
                            }
                            sqlDataItem.DataTable = dt;
                        }
                    }
                    catch (NpgsqlException ex)
                    {
                        errorMessages.AppendLine($"Error in: {ex.Message}");
                        transaction.Rollback();
                    }
                }
                result.ResultList.Add(sqlDataItem);
            }

            if (errorMessages.Length > 0)
            {
                result.ErrorMessage = errorMessages.ToString();
                return result;
            }

            result.Result = true;
            transaction.Commit();
            return result;
        }
    }


    /// <summary>
    /// 리소스 해제
    /// </summary>
    public void Dispose()
    {
        _connection.Dispose();
    }
}
