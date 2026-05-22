using F1Soft.Starmap.Service.Controllers.Setting.Models;

namespace F1Soft.Starmap.Service.Services.Groupware.Setting
{
    /// <summary>
    /// 설정 인터페이스
    /// </summary>
    public interface ISettingService
    {
        /// <summary>
        /// 설정사항 조회 인터페이스 호출
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<object> GetSettingList(SettingListRequest request);

        /// <summary>
        /// 설정사항 저장 인터페이스 호출
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<object> SetSetting(SetSettingRequest request);

    }
}
