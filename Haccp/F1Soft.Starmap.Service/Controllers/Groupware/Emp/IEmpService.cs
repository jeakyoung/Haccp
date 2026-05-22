using F1Soft.Starmap.Service.Controllers.Groupware.Emp.Models;

namespace F1Soft.Starmap.Service.Controllers.Groupware.Board
{
    /// <summary>
    /// 캘린더 서비스 인터페이스
    /// </summary>
    public interface IEmpService
    {

        /// <summary>
        /// 사원 정보 리스트
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<object> GetEmpList(EmpListRequest request);

    }
}
