# API 응답 명세


모든 백엔드내 JSON 응답은 아래 `DbResult` 구조를 따릅니다.

```json
{
  "Result": true,
  "ResultList": [
    {
      "Name": "서비스명",
      "Division": "구분",
      "ReturnValue": 2,
      "DataTable": [ 
        { 
          "COL1": "data1",
          "COL2": "data2" 
        },
        {
          "OTHER1": "dummy",
          "OTHER2": "data"
        }, 
        ],
      "ErrorMessage": null
    }
  ],
  "ErrorMessage": null
}
```

| 필드 | 타입 | 설명 |
|---|---|---|
| `Result` | `bool` | 성공 여부 (`true` / `false`) |
| `ResultList` | `array \| null` | 결과 항목 목록. 오류 시 `null` |
| `ResultList[].Name` | `string` | 작업명 또는 프로시저명 |
| `ResultList[].Division` | `string` | 결과 구분 |
| `ResultList[].ReturnValue` | `int` | 처리된 행 수 또는 결과 건수 |
| `ResultList[].DataTable` | `array` | 실제 데이터 (행 목록) |
| `ResultList[].ErrorMessage` | `string \| null` | 항목 단위 오류 메시지 |
| `ErrorMessage` | `string \| null` | 전체 오류 메시지. 성공 시 `null` |

---

## 상태 코드

| 상태 코드 | 상황 |
|---|---|
| `200 OK` | 정상 처리 |
| `400 Bad Request` | 요청 파라미터 누락 또는 유효하지 않은 요청 |
| `401 Unauthorized` | JWT 토큰 없음 또는 유효하지 않은 토큰 |
| `500 Internal Server Error` | 서버 내부 예외 발생 |

### 200 OK
```json
{
  "Result": true,
  "ResultList": [ { ... } ],
  "ErrorMessage": null
}
```

### 400 Bad Request
```json
{
  "Result": false,
  "ResultList": null,
  "ErrorMessage": "유효하지 않은 요청입니다."
}
```

### 401 Unauthorized
```json
{
  "message": "Invalid token"
}
```

### 500 Internal Server Error
```json
{
  "Result": false,
  "ResultList": null,
  "ErrorMessage": "System.Exception: ..."
}
```

---