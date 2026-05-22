using System.Data;

namespace F1Soft.Starmap.Service.Controllers.Database.Models
{
    /// <summary>
    /// Database Procedure 요청
    /// </summary>
    public class DbProcedureRequest
    {
        /// <summary>
        /// 프로시져 명
        /// </summary>
        public string? ProcedureName { get; set; }

        /// <summary>
        /// DataTable 구분
        /// </summary>
        public string? Division { get; set; }

        /// <summary>
        /// 변경 프로시져 여부
        /// </summary>
        public bool IsChangeProcedure { get; set; } = false;

        /// <summary>
        /// 파라미터 리스트
        /// </summary>
        public Dictionary<string, object>? Parameters { get; set; }
    }

    /// <summary>
    /// Database Command 요청
    /// </summary>
    public class DbSqlRequest
    {
        /// <summary>
        /// 프로시져 명
        /// </summary>
        public object? SqlCommand { get; set; }

        /// <summary>
        /// 변경 프로시져 여부
        /// </summary>
        public bool IsChangeProcedure { get; set; } = false;
    }

    /// <summary>
    /// Database 호출 결과
    /// </summary>
    public class DbResult
    {
        /// <summary>
        /// 쿼리 결과 (true 성공, false 실패)
        /// </summary>
        public bool Result { get; set; }
        /// <summary>
        /// 결과 리스트
        /// </summary>
        public List<DbResultItem>? ResultList { get; set; }
        /// <summary>
        /// 에러 메세지
        /// </summary>
        public string? ErrorMessage { get; set; }
    }

    /// <summary>
    /// Database 호출 결과 항목
    /// </summary>
    public class DbResultItem
    {
        /// <summary>
        /// 결과 이름 (테이블 명)
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// DataTable 구분
        /// </summary>
        public string? Division { get; set; }

        /// <summary>
        /// 결과 count (변경된 행 갯수 or SELECT 된 행 갯수)
        /// </summary>
        public int ReturnValue { get; set; }

        /// <summary>
        /// 결과 값 (테이블)
        /// </summary>
        public DataTable? DataTable { get; set; }

        /// <summary>
        /// 항목 에레 메세지
        /// </summary>
        public string? ErrorMessage { get; set; }
    }
}
