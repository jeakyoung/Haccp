using F1Soft.Starmap.Service.Controllers.Database.Models;
using F1Soft.Starmap.Service.Controllers.Groupware.Approval.Models;
using F1Soft.Starmap.Service.Controllers.Groupware.Board.Models;
using F1Soft.Starmap.Service.Helper;
using F1Soft.Starmap.Service.Services.EnvService;
using Microsoft.AspNetCore.Identity;
using System.Data;

namespace F1Soft.Starmap.Service.Controllers.Groupware.Board
{
    /// <summary>
    /// 캘린더 서비스
    /// </summary>
    public class BoardService : IBoardService
    {
        private readonly IEnvService _envService;
        private readonly string _connectionString;
        private readonly ILogger<BoardService> _logger;

        /// <summary>
        /// 캘린더 서비스 생성자
        /// </summary>
        /// <param name="envService"></param>
        /// <param name="logger"></param>
        public BoardService(IEnvService envService, ILogger<BoardService> logger)
        {
            _envService = envService;
            _connectionString = _envService.GetConnectionString();
            _logger = logger;
        }

        /// <summary>
        /// 게시물 본문 조회
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<object> GetBoardDetail(BoardDetailRequest request)
        {
            string sIssueNo = request.IssueNo;
            string sCommentNo = request.CommentNo;
            string sEmployeeNo = request.EmployeeNo;
            string sConfirmFlag = "0";
            string sSmsFlag = "1";
            string sIud = "IU";

            string sTitle = string.Empty;
            string sBaseName = string.Empty;
            string sDate1 = string.Empty;
            string sDate2 = string.Empty;

            try
            {
                using (var helper = new SQLServerHelper(_connectionString, _logger))
                {
                    // 본문 내용
                    DbProcedureRequest boardDetail = new DbProcedureRequest
                    {
                        ProcedureName = "SP_WEB_FrmBP602_01_LIST",
                        Division = "detail",
                        Parameters = new Dictionary<string, object>
                        {
                            { "ISSUE_NO", sIssueNo },
                            { "EMPLOYEE_NO", sEmployeeNo },
                            { "TITLE", sTitle },
                            { "BASE_NAME", sBaseName },
                            { "DATE1", sDate1 },
                            { "DATE2", sDate2 },
                            { "COMMENT_NO", sCommentNo }
                        }
                    };

                    // 댓글 리스트
                    DbProcedureRequest reviewRequest = new DbProcedureRequest
                    {
                        ProcedureName = "SP_WEB_FrmBP602_01_02_LIST",
                        Division = "review",
                        Parameters = new Dictionary<string, object>
                        {
                            { "@ISSUE_NO", sIssueNo },
                            { "@COMMENT_NO", sCommentNo }
                        }
                    };

                    // 첨부파일
                    DbProcedureRequest fileRequest = new DbProcedureRequest
                    {
                        ProcedureName = "SP_WEB_FrmBP602_01_01_LIST",
                        Division = "file",
                        Parameters = new Dictionary<string, object>
                        {
                            { "@ISSUE_NO", sIssueNo },
                            { "@COMMENT_NO", sCommentNo }
                        }
                    };

                    var result = await helper.CallProcedureAsync(new List<DbProcedureRequest> { boardDetail, reviewRequest, fileRequest });

                    bool markAsReadResult = false;
                    if (result.Result)
                    {
                        markAsReadResult = await MarkAsReadBoard(helper, sIssueNo, sCommentNo, sEmployeeNo, sConfirmFlag, sSmsFlag, sIud);

                        if (markAsReadResult)
                        {
                            _logger.LogDebug("완료");
                            return JsonHelper.ConvertDbResultToJsonObject(result);
                        }
                        else
                        {
                            throw new Exception("조회 기록 저장 실패");
                        }
                    }
                    else
                    {
                        throw new Exception("게시물 조회 실패");
                    }

                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "게시판 상세 조회 중 오류 발생");
                throw new Exception("실패");
            }
        }



        /// <summary>
        /// 게시판 이슈(타이틀) 리스트
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<object> GetBoardIssueList(BoardIssueRequest request)
        {
            try
            {
                using (var helper = new SQLServerHelper(_connectionString, _logger))
                {
                    string sTitle = string.Empty;
                    string sStatusGbn = string.Empty;
                    string sBaseName = string.Empty;
                    string sEmployeeNo = request.EmployeeNo ?? string.Empty;

                    // 개인
                    DbProcedureRequest issueRequest = new DbProcedureRequest();
                    issueRequest.ProcedureName = "SP_WEB_FrmBP601_01_LIST";
                    issueRequest.Division = "issue";
                    issueRequest.Parameters = new Dictionary<string, object>()
                    {
                        { "@TITLE", sTitle },
                        { "@STATUS_GBN", sStatusGbn },
                        { "@BASE_NAME", sBaseName },
                        { "@EMPLOYEE_NO", sEmployeeNo }
                    };

                    var result = await helper.CallProcedureAsync(new List<DbProcedureRequest>() { issueRequest });

                    if (result.Result)
                    {
                        _logger.LogDebug("완료");
                        DataTable? dt = result.ResultList?.FirstOrDefault()?.DataTable;
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            dt.Columns.Remove("OPTIME");
                            dt.Columns.Remove("REG_MAN_NAME");
                            dt.Columns.Remove("STATUS_FLAG");
                            dt.Columns.Remove("STATUS_NAME");
                            dt.Columns.Remove("MANAGER_CODE1");
                            dt.Columns.Remove("MANAGER_CODE2");
                            dt.Columns.Remove("MANAGER_CODE3");
                            dt.Columns.Remove("MANAGER_NAME2");
                            dt.Columns.Remove("MANAGER_NAME3");
                            dt.Columns.Remove("BIGO");
                            dt.Columns.Remove("EMPLOYEE_NO");
                            dt.Columns.Remove("LAST_OPTIME");
                            //dt.Columns.Remove("BOARD_TYPE");
                            //dt.Columns.Remove("BOARD_TYPE_NAME");   -> 오류발생지점 두 칼럼이 송연 프로시저내 존재하지 않음.

                            return JsonHelper.ConvertDbResultToJsonObject(result);
                        }
                        else
                        {
                            return new { Message = "데이터가 없습니다." };
                        }
                    }
                    else
                    {
                        throw new Exception("실패");
                    }

                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "게시판 타이틀(이슈) 데이터 조회 중 오류 발생");
                throw new Exception("실패");
            }

        }

        /// <summary>
        /// 게시물 리스트 조회
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<object> GetBoardList(BoardListRequest request)
        {
            try
            {
                using (var helper = new SQLServerHelper(_connectionString, _logger))
                {
                    string sIssueNo = request.IssueNo ?? string.Empty;
                    string sEmployeeNo = request.EmployeeNo ?? string.Empty;
                    string sTitle = string.Empty;
                    string sBaseName = string.Empty;
                    string sDate1 = request.SDate ?? string.Empty;
                    string sDate2 = request.EDate ?? string.Empty;
                    string sCommentNo = request.CommentNo ?? string.Empty;

                    DbProcedureRequest boardListRequest = new DbProcedureRequest();
                    boardListRequest.ProcedureName = "SP_WEB_FrmBP602_01_LIST";
                    boardListRequest.Division = "boardList";
                    boardListRequest.Parameters = new Dictionary<string, object>()
                    {
                        { "ISSUE_NO", sIssueNo },
                        { "EMPLOYEE_NO", sEmployeeNo },
                        { "TITLE", sTitle },
                        { "BASE_NAME", sBaseName },
                        { "DATE1", sDate1 },
                        { "DATE2", sDate2 },
                        { "COMMENT_NO", sCommentNo }
                    };

                    var result = await helper.CallProcedureAsync(new List<DbProcedureRequest>() { boardListRequest });

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
                _logger.LogError(ex, "게시물 리스트 조회 중 오류 발생");
                throw new Exception("실패");
            }
        }

        /// <summary>
        /// 댓글 작성, 댓글 등록
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<object> PostAnswer(BoardlAnswerRequest request)
        {
            try
            {
                using (var helper = new SQLServerHelper(_connectionString, _logger))
                {
                    string sIssueNo = request.IssueNo ?? string.Empty;
                    string sCommentNo = request.CommentNo ?? string.Empty;
                    string sReplyNo = string.Empty;
                    string sReplyComment = request.ReplyComment ?? string.Empty;
                    string sOpmanCode = request.EmployeeNo ?? string.Empty;
                    string sIUD = request.IUD.ToUpper() ?? "I";

                    DbProcedureRequest getReplyNo = new DbProcedureRequest();
                    getReplyNo.ProcedureName = "SP_WEB_STORE_MAX_10";
                    getReplyNo.Parameters = new Dictionary<string, object>();

                    var replyNoResult = await helper.CallProcedureAsync(new List<DbProcedureRequest>() { getReplyNo });

                    if (replyNoResult.Result)
                    {
                        sReplyNo = replyNoResult.ResultList?.FirstOrDefault()?.DataTable?.Rows[0]["REPLY_NO"].ToString() ?? string.Empty;
                    }
                    else
                    {
                        throw new Exception("댓글 번호 조회 실패");
                    }

                    DbProcedureRequest commentRequest = new DbProcedureRequest();
                    commentRequest.ProcedureName = "SP_WEB_FrmBP602_01_02_IUD";
                    commentRequest.Division = "processBoardComment";
                    commentRequest.Parameters = new Dictionary<string, object>()
                    {
                        { "ISSUE_NO", sIssueNo },
                        { "COMMENT_NO", sCommentNo },
                        { "REPLY_NO", sReplyNo },
                        { "REPLY_COMMENT", sReplyComment },
                        { "OPMAN_CODE", sOpmanCode },
                        { "IUD", sIUD }
                    };

                    var result = await helper.CallProcedureAsync(new List<DbProcedureRequest>() { commentRequest });

                    if (result.Result)
                    {
                        _logger.LogDebug("완료");
                        var detailResult = await GetBoardDetail(new BoardDetailRequest
                        {
                            IssueNo = sIssueNo,
                            CommentNo = sCommentNo,
                            EmployeeNo = sOpmanCode
                        });

                        return detailResult;
                    }
                    else
                    {
                        throw new Exception("실패");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "댓글 처리 중 오류 발생");
                throw new Exception("실패");
            }
        }

        private async Task<bool> MarkAsReadBoard(SQLServerHelper helper, string issueNo, string commentNo, string employeeNo, string ConfirmFlag, string SmsFlag, string Iud)
        {
            string sIssueNo = issueNo ?? string.Empty;
            string sCommentNo = commentNo ?? string.Empty;
            string sOpManCode = employeeNo ?? string.Empty;
            string sConfirmFlag = ConfirmFlag ?? "0";
            string sSmsFlag = SmsFlag ?? "1";
            string sIud = Iud ?? "IU";

            DbProcedureRequest request = new DbProcedureRequest
            {
                ProcedureName = "SP_WEB_FrmBP602_01_03_IUD",
                Division = "readBoard",
                Parameters = new Dictionary<string, object>
                {
                    { "ISSUE_NO", sIssueNo },
                    { "COMMENT_NO", sCommentNo },
                    { "OPMAN_CODE", sOpManCode },
                    { "CONFIRM_FLAG", sConfirmFlag },
                    { "SMS_FLAG", sSmsFlag },
                    { "IUD", sIud }
                }
            };

            var result = await helper.CallProcedureAsync(new List<DbProcedureRequest> { request });

            return result.Result;
        }





    }
}
