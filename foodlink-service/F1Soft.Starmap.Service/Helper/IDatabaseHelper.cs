using F1Soft.Starmap.Service.Controllers.Database.Models;

namespace F1Soft.Starmap.Service.Helper
{
    /// <summary>
    /// Database helper interface
    /// </summary>
    public interface IDatabaseHelper : IDisposable
    {
        /// <summary>
        /// 프로시져 호출
        /// </summary>
        /// <param name="dbProcs"></param>
        /// <returns></returns>
        Task<DbResult> CallProcedureAsync(List<DbProcedureRequest> dbProcs);

        /// <summary>
        /// SQL 커맨드 실행
        /// </summary>
        /// <param name="dbSqls"></param>
        /// <returns></returns>
        Task<DbResult> ExecuteSqlCommandAsync(List<DbSqlRequest> dbSqls);
    }
}
