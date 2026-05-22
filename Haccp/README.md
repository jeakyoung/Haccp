# f1soft-starmap-service

## Getting started
f1soft-starmap-service 프로젝트를 시작하려면 다음 단계를 따라 주세요.

**1. 개발 환경 설정**
  * **.NET 8.0 SDK 설치**:  [.NET 8.0 SDK 다운로드](https://www.google.com/url?sa=E&source=gmail&q=https://dotnet.microsoft.com/download/dotnet/8.0) 페이지에서 .NET 8.0 SDK를 다운로드하여 설치합니다.
  * **IDE 설치**: Visual Studio 2022 이상 또는 Visual Studio Code와 같은 IDE를 설치합니다.
  * **Git 설치**: Git을 사용하여 프로젝트를 클론하려면 Git을 설치해야 합니다.

**2. 프로젝트 다운로드 및 실행**
  * **프로젝트 클론**: `git clone` 명령어를 사용하여 프로젝트를 클론합니다.
    ```bash
    git clone http://192.168.80.30/f1soft/f1soft-starmap-service.git
    ```
  * **프로젝트 열기**: IDE에서 클론한 프로젝트를 엽니다.
  * **빌드 및 실행**: IDE 또는 .NET CLI를 사용하여 프로젝트를 빌드하고 실행합니다.
    ```bash
    dotnet build
    dotnet run
    ```

**3. 데이터베이스 설정**

  * **SQLite 데이터베이스 파일 생성**: 프로젝트 루트 디렉토리에 `startmap.db`라는 이름의 SQLite 데이터베이스 파일을 생성합니다.
  * **연결 문자열 설정**: `appsettings.json` 파일에서 `ConnectionStrings` 섹션의 `StarmapConnection` 값을 생성한 SQLite 데이터베이스 파일 경로로 변경합니다. 예를 들어, `Data Source=startmap.db`와 같이 설정합니다.

**4. API 테스트**

  * **Swagger UI 접속**: 웹 브라우저에서 `https://localhost:<port>/swagger` URL로 접속하여 Swagger UI를 확인합니다. `<port>`는 애플리케이션이 실행되는 포트 번호입니다.
  * **API 테스트**: Swagger UI를 사용하여 API를 테스트합니다.

**참고:**

  * `your-username`은 실제 GitHub 사용자 이름으로 바꿔야 합니다.
  * 자세한 사용 방법은 **Usage** 섹션을 참조하십시오.
  * 문제가 발생하면 **Support** 섹션을 참조하십시오.



## Name
F1Soft Backend Service by ASP.NET Core

## Description
.NET 8.0을 사용하여 개발된 ASP.NET Core Web API 예제 프로젝트입니다. 사용자 인증, 데이터베이스 연동, API 문서화 등의 기능을 포함하고 있으며, 학습 및 테스트 목적으로 활용될 수 있습니다.

### Features
* **사용자 인증**: JWT 토큰 기반 인증 시스템 구현
* **데이터베이스 연동**: SQLite 데이터베이스를 사용하여 데이터 저장 및 관리
* **API 문서화**: Swagger/OpenAPI를 사용하여 API 문서 자동 생성
* **예제 API**: 날씨 예보 데이터를 반환하는 API 제공 (WeatherForecast)
* **인증 API**: JWT 토큰 발급 및 사용자 인증 API 제공 (AuthController)



## Badges
[![.NET 8.0](https://img.shields.io/badge/.NET-8.0-blue.svg)](https://dotnet.microsoft.com/download/dotnet/8.0)
[![License: MIT](https://img.shields.io/badge/License-MIT-green.svg)](https://opensource.org/licenses/MIT)

## Visuals
* **Swagger UI 스크린샷**:

  ![Swagger UI](./images/swagger-ui-screenshot.png) 

* **API 요청/응답 예시**:

  ```json
  // AuthController - login API 요청
  {
    "userId": "testuser",
    "password": "password"
  }

  // AuthController - login API 응답
  {
    "username": "testuser",
    "email": "[email address removed]",
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..." 
  }
  ```

## Installation
.NET 8.0 SDK 설치: .NET 8.0 SDK 다운로드 페이지에서 .NET 8.0 SDK를 다운로드하여 설치합니다.
프로젝트 다운로드: git clone 명령어를 사용하여 프로젝트를 다운로드합니다.
```Bash
git clone [invalid URL removed]
```

데이터베이스 설정: appsettings.json 파일에서 StarmapConnection 연결 문자열을 확인하고 필요에 따라 수정합니다. SQLite 데이터베이스 파일 (startmap.db) 을 프로젝트의 Database\StartmapDb 폴더에 위치시킵니다.

## Usage
1. Visual Studio 2022 이상 또는 .NET 8.0 CLI를 사용하여 프로젝트를 엽니다.
2. 프로젝트를 빌드하고 실행합니다.
```Bash
dotnet build
dotnet run
```
3. Swagger UI를 통해 API 문서를 확인하고 테스트합니다. (예: https://localhost:<port>/swagger)
4. AuthController의 /login API를 호출하여 JWT 토큰을 발급받습니다.
5. 발급받은 JWT 토큰을 Authorization 헤더에 Bearer 토큰 형식으로 추가하여 인증이 필요한 API를 호출합니다.

## Support
이 프로젝트에 대한 지원이 필요하면 하기 담당자에게 문의 바랍니다.
- 김영한 (yhkim@f1soft.co.kr)


## Roadmap
- 추가 API 개발: 다양한 기능을 제공하는 API 추가
- 단위 테스트 작성: 코드 품질 향상 및 안정성 확보
- 보안 강화: 인증 및 권한 부여 기능 강화
- 성능 최적화: API 성능 개선

## Contributing
프로젝트에 기여하고 싶다면, 다음 단계를 따르세요.

1. 프로젝트를 포크합니다.
2. 새로운 브랜치를 생성합니다. (git checkout -b feature/my-new-feature)
3. 변경 사항을 커밋합니다. (git commit -am 'Add some feature')
4. 브랜치를 푸시합니다. (git push origin feature/my-new-feature)
5. Merge Request(PR)를 생성합니다.


## Authors and acknowledgment
- 작성자: 김영한 (yhkim@f1soft.co.kr)

## License
이 프로젝트는 MIT 라이선스를 따릅니다. 자세한 내용은 LICENSE 파일을 참조하세요.

## Project status
현재 개발 중입니다.

