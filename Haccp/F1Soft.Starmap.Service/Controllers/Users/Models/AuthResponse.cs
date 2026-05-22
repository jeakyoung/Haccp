namespace F1Soft.Starmap.Service.Controllers.Users.Models
{
    /// <summary>
    /// 인증 결과 반환
    /// </summary>
    public class AuthResponse
    {
        /// <summary>
        /// JWT 토큰 (API 요청 시 Header에 포함시켜야함)
        /// </summary>
        public string? Token { get; set; }

        /// <summary>
        /// 사원 번호
        /// </summary>
        public string? EmployeeNo { get; set; }

        /// <summary>
        /// 담당자 이름
        /// </summary>
        public string? ChargeName { get; set; }

        /// <summary>
        /// 공장 코드
        /// </summary>
        public string? FactoryCode { get; set; }

        /// <summary>
        /// 등급 (빈 값으로 초기화됨)
        /// </summary>
        public string? Grade { get; set; }

        /// <summary>
        /// 사용자 ID
        /// </summary>
        public string? UserId { get; set; }

        /// <summary>
        /// 부서 코드
        /// </summary>
        public string? DepartmentCode { get; set; }

        /// <summary>
        /// 플랜트 코드
        /// </summary>
        public string? PlantCode { get; set; }

        /// <summary>
        /// 코드 이름 (전체 이름)
        /// </summary>
        public string? CodeNameFull { get; set; }

        /// <summary>
        /// 플랜트 이름
        /// </summary>
        public string? PlantName { get; set; }

        /// <summary>
        /// 부서 이름
        /// </summary>
        public string? DepartmentName { get; set; }

        /// <summary>
        /// 이미지 경로 (예: 사용자 이미지 또는 ID 사진)
        /// </summary>
        public string? Imabmp2 { get; set; }

        /// <summary>
        /// 서명 이미지 경로
        /// </summary>
        public string? SignImage { get; set; }

        /// <summary>
        /// 기본 서명 여부
        /// </summary>
        public string? DefaultSignFlag { get; set; }

        /// <summary>
        /// 휴대폰 번호
        /// </summary>
        public string? HandphoneNo { get; set; }

        /// <summary>
        /// 메일 주소
        /// </summary>
        public string? AddrMail { get; set; }

        /// <summary>
        /// BTMS 주소
        /// </summary>
        public string? AddrBtms { get; set; }

        /// <summary>
        /// 비밀번호 변경 필요 여부 (1: 필요, 0: 불필요)
        /// </summary>
        public string? ChagepwFlag { get; set; }

        /// <summary>
        /// 이메일
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// 이메일 비밀번호
        /// </summary>
        public string? EmailPw { get; set; }

        /// <summary>
        /// 공용 이메일 개수
        /// </summary>
        public int PublicEmailCnt { get; set; }

        /// <summary>
        /// 연차 정보 (총 연차, 사용 연차, 잔여 연차)
        /// </summary>
        public string? AnnualCntText { get; set; }


    }
}
