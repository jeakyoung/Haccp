using F1Soft.Starmap.Service.Controllers.Database.Models;
using F1Soft.Starmap.Service.Controllers.Groupware.Approval.Models;
using F1Soft.Starmap.Service.Helper;
using F1Soft.Starmap.Service.Services.EnvService;
using System.Data;

namespace F1Soft.Starmap.Service.Controllers.Groupware.Approval
{
    /// <summary>
    /// 결재 컨트롤에서 사용할 서비스
    /// </summary>
    public class ApprovalService : IApprovalService
    {
        private readonly IEnvService _envService;
        private readonly string _connectionString;
        private readonly ILogger<ApprovalService> _logger;

        /// <summary>
        /// Approval 서비스
        /// </summary>
        /// <param name="envService"></param>
        /// <param name="logger"></param>
        public ApprovalService(IEnvService envService, ILogger<ApprovalService> logger)
        {
            _envService = envService;
            _connectionString = _envService.GetConnectionString();
            _logger = logger;
        }

        /// <summary>
        /// 문서 열람시간 등록
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<object> CheckOpenTime(ApprovalDetailRequest request)
        {
            try
            {
                string sEaExeId = request.EaExeId?.ToString() ?? string.Empty;
                string sEabusNo = request.EabusNo?.ToString() ?? string.Empty;
                string sEmployeeNo = request.EmployeeNo?.ToString() ?? string.Empty;
                int? iGbnCode = request.GbnCode ?? 0;
                int? iExeSeq = request.ExeSeq ?? 0;

                using (var helper = new SQLServerHelper(_connectionString, _logger))
                {
                    DbProcedureRequest request1 = new DbProcedureRequest();
                    request1.ProcedureName = "SP_WEB_STORE_FUNCTION_ChkOpenTime";
                    request1.Division = "doc";
                    request1.Parameters = new Dictionary<string, object>()
                    {
                        { "EA_EXE_ID", sEaExeId },
                        { "GBN_CODE", iGbnCode },
                        { "EABUS_NO", sEabusNo },
                        { "EXE_SEQ",  iExeSeq},
                        { "USERID", sEmployeeNo },
                    };
                    var result = await helper.CallProcedureAsync(new List<DbProcedureRequest>() { request1 });
                    if (result.Result)
                    {
                        _logger.LogDebug("완료");
                        return JsonHelper.ConvertDbResultToJsonObject(result);
                    }
                    else
                    {
                        throw new Exception("실패");
                    }
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "실패");
                throw new Exception("실패");
            }
        }

        /// <summary>
        /// 승인, 확인, 반송 [결재 기능] + 합의 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<object> ConfirmApproval(SubmitApprovalRequest request)
        {
            try
            {
                string sMuldecFlag = request.MuldecFlag ?? string.Empty;
                string sLastCnfrmerFlag = request.LastCnfrmerFlag ?? string.Empty;
                string sLastOwnerFlag = request.LastOwnerFlag ?? string.Empty;
                string sEaExeId = request.EaExeId ?? string.Empty;
                int iGbnCode = request.GbnCode ?? 0;
                int iExeSeq = request.ExeSeq ?? 0;
                int iOrderSeq = request.OrderSeq ?? 0;
                string sOptionName = request.OptionName ?? string.Empty;
                string sAppFlag = request.AppFlag ?? string.Empty;

                string sEabusNo = request.EabusNo?.ToString() ?? string.Empty;
                string sEmployeeNo = request.EmployeeNo?.ToString() ?? string.Empty;

                string sAnswerGbnName = string.Empty;
                string sAnswerCnt = string.Empty;

                using (var helper = new SQLServerHelper(_connectionString, _logger))
                {

                    DbProcedureRequest procedureRequest;
                    // 결재 구분에 따른 답변 구분 이름 및 내용 설정
                    if (sAppFlag == "1" && sMuldecFlag == "4")
                    {
                        sOptionName = "합의";
                        sAnswerGbnName = "합의";
                        sAnswerCnt = "ⓜ[합의 하였습니다.]";
                    }
                    else if (sAppFlag == "1")
                    {
                        sAnswerGbnName = "승인";
                        sAnswerCnt = "ⓜ[승인 하였습니다.]";
                    }
                    else // sAppFlag == "2"
                    {
                        sAnswerGbnName = "확인";
                        sAnswerCnt = "ⓜ[확인 하였습니다.]";
                    }

                    if (sAppFlag == "1" || sAppFlag == "2")
                    {
                        procedureRequest = new DbProcedureRequest
                        {
                            ProcedureName = "SP_WEB_STORE_FUNCTION_ChkAnswer_IUD",
                            Division = "agree",
                            Parameters = new Dictionary<string, object>
                            {
                                { "MULDEC_FLAG", sMuldecFlag },             // 결재구분
                                { "LAST_CNFRMER_FLAG", sLastCnfrmerFlag },  // 최종결재자 flag
                                { "LAST_OWNER_FLAG", sLastOwnerFlag },      // 최종소유자 flag
                                { "EA_EXE_ID", sEaExeId },                  // 기안번호
                                { "GBN_CODE", iGbnCode },                   // 기안, 조치 구분코드
                                { "EXE_SEQ", iExeSeq },                     // 결재순번
                                { "ORDER_SEQ", iOrderSeq },                 // 결재순번
                                { "OPTION_NAME", sOptionName },             // 결재 명칭 (승인, 확인, 반송 등)
                                { "APP_FLAG", sAppFlag },                   // 결재 flag(1: 승인, 2: 확인, 3: 반송 등)
                            }
                        };


                    }
                    else if (sAppFlag == "3")
                    {
                        procedureRequest = new DbProcedureRequest
                        {
                            ProcedureName = "SP_WEB_STORE_FUNCTION_ChkAnswer_IUD2",
                            Division = "agree",
                            Parameters = new Dictionary<string, object>
                            {
                                { "MULDEC_FLAG", sMuldecFlag },             // 결재구분
                                { "LAST_CNFRMER_FLAG", sLastCnfrmerFlag },  // 최종결재자 flag
                                { "LAST_OWNER_FLAG", sLastOwnerFlag },      // 최종소유자 flag
                                { "EA_EXE_ID", sEaExeId },                  // 기안번호
                                { "GBN_CODE", iGbnCode },                   // 기안, 조치 구분코드
                                { "EXE_SEQ", iExeSeq },                     // 결재순번
                                { "ORDER_SEQ", iOrderSeq },                 // 결재순번
                                { "OPTION_NAME", sOptionName },             // 결재 명칭 (승인, 확인, 반송 등)
                                { "EMPLOYEE_NO", sEmployeeNo },
                            }
                        };
                        sAnswerGbnName = "반송";
                        sAnswerCnt = "ⓜ[반송 하였습니다.]";
                    }
                    else
                    {
                        throw new Exception("appFlag를 정확히 입력해 주세요(1: 승인, 2: 확인, 3: 반송, 4: 합의)");
                    }




                    // 프로시저 실행
                    var iudResult = await helper.CallProcedureAsync(new List<DbProcedureRequest>() { procedureRequest });

                    if (!iudResult.Result)
                    {
                        _logger.LogError("결재 승인 프로시저 실패: {0}", iudResult.ErrorMessage);
                        throw new Exception("결재 승인 처리 중 오류 발생");
                    }

                    int answerSeq = await GetAnswerSeq(helper, sEaExeId, sEabusNo, iGbnCode, iExeSeq);

                    ApprovalAnswerRequest answerRequest = new ApprovalAnswerRequest
                    {
                        EabusNo = sEabusNo,
                        EaExeId = sEaExeId,
                        GbnCode = iGbnCode,
                        ExeSeq = iExeSeq,
                        AnswerSeq = answerSeq,
                        AnswerGbnName = sAnswerGbnName,
                        AnswerTime = "", // 사용 안함
                        AnswerCnt = sAnswerCnt,
                        OpmanCode = sEmployeeNo,
                        Optime = "", // 사용 안함
                        MainViewFlag = "0",
                        Iud = "I"
                    };

                    var answerResult = await PostAnswer(answerRequest);
                    if (answerResult == null)
                    {
                        _logger.LogError("댓글 등록 실패");
                        throw new Exception("댓글 등록 실패");
                    }

                    var result = await GetApprovalDetailList(new ApprovalDetailRequest
                    {
                        EaExeId = request.EaExeId,
                        EabusNo = request.EabusNo,
                        EmployeeNo = request.EmployeeNo,
                        GbnCode = request.GbnCode,
                        ExeSeq = request.ExeSeq
                    });

                    if (result == null)
                    {
                        _logger.LogError("결재 문서 조회 실패");
                        throw new Exception("결재 문서 정보를 찾을 수 없습니다.");
                    }

                    _logger.LogDebug("결재 승인 및 재조회 성공");
                    return result;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"오류 발생: {ex.Message}");
                throw new Exception("실패");
            }
        }

        /// <summary>
        /// 미결함, 보관함, 기안함, 결재함
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<object> GetApprovalAllList(ApprovalRequest request)
        {
            try
            {
                using (var helper = new SQLServerHelper(_connectionString, _logger))
                {

                    string sUserId = request.UserId ?? string.Empty;
                    string sFromDate = request.FromDate ?? string.Empty;
                    string sToDate = request.ToDate ?? string.Empty;
                    string sRegmanName = "";
                    string sEaTitle = "";
                    string sEaExeId = "";
                    string sEabusNo = "";

                    // 추후에 사용하게 되면 model도 주석 풀고 실행해야 함
                    //string sRegmanName = request.RegmanName ?? string.Empty;
                    //string sEaTitle = request.EaTitle ?? string.Empty;
                    //string sEaExeId = request.EaExeId ?? string.Empty;
                    //string sEabusNo = request.EabusNo ?? string.Empty;

                    int iKeepCode = 1;

                    // 미결함 - fromdate, todate 반대로 넣어야 함
                    DbProcedureRequest request1 = new DbProcedureRequest();
                    request1.ProcedureName = "SP_WEB_FrmEA121_01_LIST";
                    request1.Division = "dependence";
                    request1.Parameters = new Dictionary<string, object>()
                    {
                        { "USER_ID", sUserId },
                        { "REGMANNAME", sRegmanName },
                        { "EA_TITLE", sEaTitle },
                        { "EA_EXE_ID", sEaExeId },
                        { "FROMDATE", sToDate },
                        { "TODATE",  sFromDate},
                        { "EABUS_NO", sEabusNo }
                    };

                    // 보관함
                    DbProcedureRequest request2 = new DbProcedureRequest();
                    request2.ProcedureName = "SP_WEB_DASHBOARD_DASH_11";
                    request2.Division = "keep";
                    request2.Parameters = new Dictionary<string, object>()
                    {
                        { "USER_ID", sUserId },
                        { "KEEP_CODE", iKeepCode },
                    };

                    // 기안함
                    DbProcedureRequest request3 = new DbProcedureRequest();
                    request3.ProcedureName = "SP_WEB_FrmEA123_01_LIST";
                    request3.Division = "drafting";
                    request3.Parameters = new Dictionary<string, object>()
                    {
                        { "USER_ID", sUserId },
                        { "EA_EXE_ID", "" },
                        { "TODATE1", sFromDate },
                        { "FROMDATE1", sToDate },
                        { "REG_NAME", "" },
                        { "CUSTOMER_NAME", "" },
                        { "MATERIAL_CODE", "" },
                        { "MATERIAL_NAME", "" },
                        { "EA_TITLE", "" },
                        { "EABUS_NO", "" },
                    };

                    // 결재함
                    DbProcedureRequest request4 = new DbProcedureRequest();
                    request4.ProcedureName = "SP_WEB_FrmEA122_01_LIST";
                    request4.Division = "approval";
                    request4.Parameters = new Dictionary<string, object>()
                    {
                        { "USER_ID", sUserId },
                        { "EA_EXE_ID", sEaExeId },
                        { "TODATE1", sFromDate },
                        { "FROMDATE1", sToDate },
                        { "REG_NAME", "" },
                        { "TODATE2", "" },
                        { "FROMDATE2", "" },
                        { "CUSTOMER_NAME", "" },
                        { "MATERIAL_CODE", "" },
                        { "MATERIAL_NAME", "" },
                        { "EA_TITLE", sEaTitle },
                        { "EABUS_NO", sEabusNo },
                        { "RADIO", "2" },
                        { "CHECK", "0" }
                    };


                    var result = await helper.CallProcedureAsync(new List<DbProcedureRequest>() { request1, request2, request3, request4 });

                    if (result.Result)
                    {
                        _logger.LogDebug("완료");
                        return JsonHelper.ConvertDbResultToJsonObject(result);
                    }
                    else
                    {
                        throw new Exception("실패");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "미결함, 보관함, 기안함, 결재함 조회 중 오류 발생");
                throw new Exception("실패");
            }
        }

        /// <summary>
        /// 결재 문서 상세 정보 (본문 내용)
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<object> GetApprovalDetailList(ApprovalDetailRequest request)
        {

            string sEaExeId = request.EaExeId?.ToString() ?? string.Empty;
            string sEabusNo = request.EabusNo?.ToString() ?? string.Empty;
            string sEmployeeNo = request.EmployeeNo?.ToString() ?? string.Empty;
            int? iGbnCode = request.GbnCode ?? 0;
            int? iExeSeq = request.ExeSeq ?? 0;

            try
            {
                using (var helper = new SQLServerHelper(_connectionString, _logger))
                {
                    // 결재 내용
                    DbProcedureRequest request1 = new DbProcedureRequest();
                    request1.ProcedureName = "SP_WEB_FrmEA101_Window_01_LIST2";
                    request1.Division = "main";
                    request1.Parameters = new Dictionary<string, object>()
                    {
                        { "EA_EXE_ID", sEaExeId },
                        { "EABUS_NO", sEabusNo },
                        { "EMPLOYEE_NO", sEmployeeNo },
                    };

                    // 첨부파일
                    DbProcedureRequest request2 = new DbProcedureRequest();
                    request2.ProcedureName = "SP_WEB_FrmEA101_07_LIST";
                    request2.Division = "file";
                    request2.Parameters = new Dictionary<string, object>()
                    {
                        { "EA_EXE_ID", sEaExeId },
                        { "EABUS_NO", sEabusNo },
                        { "EMP_CODE", sEmployeeNo },
                    };

                    // 결재선
                    DbProcedureRequest request3 = new DbProcedureRequest();
                    request3.ProcedureName = "SP_WEB_FrmEA101_Window_02_LIST";
                    request3.Division = "sign";
                    request3.Parameters = new Dictionary<string, object>()
                    {
                        { "EA_EXE_ID", sEaExeId },
                    };

                    // 댓글 리스트
                    DbProcedureRequest request4 = new DbProcedureRequest();
                    request4.ProcedureName = "SP_WEB_FrmEA101_Window_04_LIST";
                    request4.Division = "review";
                    request4.Parameters = new Dictionary<string, object>()
                    {
                        { "EA_EXE_ID", sEaExeId },
                        { "EABUS_NO", sEabusNo },
                        { "GBN_CODE", iGbnCode },
                        { "EMPLOYEE_NO", sEmployeeNo },

                    };

                    // 문서에 대한 나의 역할 및 상태
                    DbProcedureRequest request5 = new DbProcedureRequest();
                    request5.ProcedureName = "SP_WEB_STORE_FUNCTION_ChkAnswer_List";
                    request5.Division = "level";
                    request5.Parameters = new Dictionary<string, object>()
                    {
                        { "EA_EXE_ID", sEaExeId },
                        { "EABUS_NO", sEabusNo },
                        { "GBN_CODE", iGbnCode },
                        { "EXE_SEQ",  iExeSeq },
                        { "EMPLOYEE_NO", sEmployeeNo },
                    };

                    // 합의자 리스트
                    DbProcedureRequest request6 = new DbProcedureRequest();
                    request6.ProcedureName = "SP_WEB_FUNCTION_GET_DISCUSSION_USER_LIST";
                    request6.Division = "discussion";
                    request6.Parameters = new Dictionary<string, object>()
                    {
                        { "EA_EXE_ID", sEaExeId }
                    };

                    //합의자 리스트 프로시저 미존재 -> 주석처리
                    //var result = await helper.CallProcedureAsync(new List<DbProcedureRequest> { request1, request2, request3, request4, request5, request6 });
                    var result = await helper.CallProcedureAsync(new List<DbProcedureRequest> { request1, request2, request3, request4, request5 });


                    if (result.Result)
                    {

                        var markAsReadList = new List<(int exeSeq, int answerSeq)>();

                        var commentList = result?.ResultList?.FirstOrDefault(r => r.Division == "review")?.DataTable;
                        if (commentList != null)
                        {
                            foreach (DataRow row in commentList.Rows)
                            {
                                string answerEmp = row["EMPLOYEE_NO"].ToString() ?? string.Empty;

                                if (answerEmp != sEmployeeNo)
                                {
                                    int answerSeq = Convert.ToInt32(row["ANSWER_SEQ"]);
                                    int exeSeq = Convert.ToInt32(row["EXE_SEQ"]);
                                    markAsReadList.Add((exeSeq, answerSeq));
                                    //await MarkAsReadAsync(helper, sEaExeId, sEabusNo, iGbnCode, exeSeq, answerSeq, sEmployeeNo);
                                }
                                else if (answerEmp == sEmployeeNo)
                                {
                                    row["ME_VIEW_FLAG"] = "1";
                                }
                            }
                        }

                        var jsonResponse = JsonHelper.ConvertDbResultToJsonObject(result!);

                        foreach (var (exeSeq, answerSeq) in markAsReadList)
                        {
                            await MarkAsReadAsync(helper, sEaExeId, sEabusNo, iGbnCode, exeSeq, answerSeq, sEmployeeNo);
                        }
                        _logger.LogDebug("완료");
                        return jsonResponse;
                    }
                    else
                    {
                        throw new Exception("결재 문서 상세 정보 조회 중 오류 발생");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "결재 문서 상세 정보 조회 중 오류 발생");
                throw new Exception("실패");
            }
        }

        /// <summary>
        /// 댓글 작성 (댓글 등록)
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<object> PostAnswer(ApprovalAnswerRequest request)
        {
            try
            {
                string sEabusNo = request.EabusNo ?? string.Empty;
                string sEaExeId = request.EaExeId ?? string.Empty;
                int iGbnCode = request.GbnCode ?? 0;
                int iExeSeq = request.ExeSeq ?? 0;

                string sAnswerGbnName = request.AnswerGbnName ?? "작성"; // 승인, 작성, 승인 반송 

                string sAnswerTime = request.AnswerTime ?? string.Empty;
                string sAnswerCnt = request.AnswerCnt ?? string.Empty;
                string sOpmanCode = request.OpmanCode ?? string.Empty;
                string sOptime = string.Empty;
                string sMainViewFlag = request.MainViewFlag ?? "0";
                string sIud = "I";

                using (var helper = new SQLServerHelper(_connectionString, _logger))
                {
                    int iAnswerSeq = await GetAnswerSeq(helper, sEaExeId, sEabusNo, iGbnCode, iExeSeq);
                    DbProcedureRequest procedureRequest = new DbProcedureRequest
                    {

                        ProcedureName = "SP_WEB_FrmEA101_Window_04_IUD",
                        Division = "answer",
                        Parameters = new Dictionary<string, object>
                        {
                            { "EABUS_NO", sEabusNo },                // 업무 번호
                            { "EA_EXE_ID", sEaExeId },               // 전자문서 실행 ID
                            { "GBN_CODE", iGbnCode },                // 구분 코드
                            { "EXE_SEQ", iExeSeq },                  // 실행 순번
                            { "ANSWER_SEQ", iAnswerSeq },            // 답변 순번
                            { "ANSWER_GBN_NAME", sAnswerGbnName },   // 답변 구분 이름
                            { "ANSWER_TIME", sAnswerTime },          // 답변 시간
                            { "ANSWER_CNT", sAnswerCnt },            // 답변 내용
                            { "OPMAN_CODE", sOpmanCode },            // 작업자 코드
                            { "OPTIME", sOptime },                   // 작업 시간
                            { "MAIN_VIEW_FLAG", sMainViewFlag },     // 메인 뷰 플래그
                            { "IUD", sIud }                          // 작업 구분 (I/U/D)
                        }
                    };

                    var result = await helper.CallProcedureAsync(new List<DbProcedureRequest> { procedureRequest });

                    if (!result.Result)
                    {
                        _logger.LogError($"프로시저 호출 실패: {result.ErrorMessage}");
                        throw new Exception("댓글 등록 실패");
                    }

                    // 댓글 작성 성공 후 본문 내용 최신화 (자동 호출)
                    _logger.LogDebug("댓글 작성 완료. 최신 본문 내용 조회 시작.");
                    var detailResult = await GetApprovalDetailList(new ApprovalDetailRequest
                    {
                        EaExeId = request.EaExeId,
                        EabusNo = request.EabusNo,
                        EmployeeNo = request.OpmanCode,
                        GbnCode = request.GbnCode,
                        ExeSeq = request.ExeSeq
                    });

                    return detailResult;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"오류 발생: {ex.Message}");
                throw new Exception("실패");
            }
        }

        /// <summary>
        /// 댓글 순번 반환
        /// </summary>
        private async Task<int> GetAnswerSeq(SQLServerHelper helper, string EaExeId, string EabusNo, int? GbnCode, int? ExeSeq)
        {
            string sEaExeId = EaExeId ?? string.Empty;
            string sEabusNo = EabusNo ?? string.Empty;
            int iGbnCode = GbnCode ?? 0;
            int iExeSeq = ExeSeq ?? 0;

            DbProcedureRequest request = new DbProcedureRequest
            {
                ProcedureName = "SP_WEB_STORE_MAX_01",
                Division = "",
                Parameters = new Dictionary<string, object>
                {
                    { "EA_EXE_ID", sEaExeId },
                    { "EABUS_NO", sEabusNo },
                    { "GBN_CODE", iGbnCode },
                    { "EXE_SEQ",  iExeSeq }
                }
            };

            var result = await helper.CallProcedureAsync(new List<DbProcedureRequest> { request });

            if (result.Result && result.ResultList != null && result.ResultList.Count > 0)
            {
                var dataTable = result.ResultList[0].DataTable;
                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    return Convert.ToInt32(dataTable.Rows[0][0]);
                }
            }

            return 1; // 기본값 반환
        }

        /// <summary>
        /// 페이지 진입 시 읽음 처리
        /// </summary>
        private async Task<bool> MarkAsReadAsync(SQLServerHelper helper, string EaExeId, string EabusNo, int? GbnCode, int? ExeSeq, int? AnswerSeq, string UserId)
        {
            string sEaExeId = EaExeId ?? string.Empty;
            string sEabusNo = EabusNo ?? string.Empty;
            int iGbnCode = GbnCode ?? 0;
            int iExeSeq = ExeSeq ?? 0;
            int iAnswerSeq = AnswerSeq ?? 0;
            string sUserId = UserId ?? string.Empty;

            DbProcedureRequest request = new DbProcedureRequest
            {
                ProcedureName = "SP_WEB_FrmEA101_Window_08_LIST",
                Division = "",
                Parameters = new Dictionary<string, object>
                {
                    { "EA_EXE_ID", sEaExeId },
                    { "GBN_CODE", iGbnCode },
                    { "EABUS_NO", sEabusNo },
                    { "EXE_SEQ", iExeSeq },
                    { "ANSWER_SEQ", iAnswerSeq },
                    { "USERID", sUserId }
                }
            };

            var result = await helper.CallProcedureAsync(new List<DbProcedureRequest> { request });

            return result.Result; // 프로시저 실행 결과 반환
        }


    }

}
