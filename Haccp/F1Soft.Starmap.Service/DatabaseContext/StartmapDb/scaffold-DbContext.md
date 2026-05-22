

## 갱신

```
dotnet ef dbcontext scaffold "Data Source=Database\StarmapDb\Starmap.db" Microsoft.EntityFrameworkCore.Sqlite -c StarmapTempDbContext -o Database/StarmapDb/Models -v -f
```

```
dotnet ef dbcontext scaffold "Data Source=Database\StarmapDb\Starmap.db" Microsoft.EntityFrameworkCore.Sqlite -c StarmapTempDbContext -o Database/StarmapDb/Models -v -f -t
```

##명령어 옵션

- Scaffold-DbContext 명령어는 다양한 옵션을 제공합니다
```
-t 또는 --table : 특정 테이블만 스캐폴딩합니다. 여러 테이블을 지정하려면 쉼표로 구분합니다.
-f 또는 --force : 기존 파일을 덮어씁니다.
-v 또는 --verbose : 자세한 로그를 출력합니다.
-c 또는 --context : DbContext 클래스의 이름을 지정합니다.
-n 또는 --namespace : 생성된 클래스의 네임스페이스를 지정합니다.
```