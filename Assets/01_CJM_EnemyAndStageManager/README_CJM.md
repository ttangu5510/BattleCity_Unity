# 테스트 메뉴얼

플레이어
```
사용 방법 : 
1. Assets/01_CJM_EnemyAndStageManager/Prefabs/Characters/Player 프리펩을 하이에라키 뷰에 추가
2. Assets/01_CJM_EnemyAndStageManager/Prefabs/Entities/RespawnPoint 프리펩을 원하는 위치에 추가 (리스폰 위치 설정)
3. 씬에 있는 Player의 인스펙터상 필드 RespawnPoint에 씬에 있는 스폰 포인트 오브젝트를 드래그로 넣어줍니다

테스트 입력 키)
이동 : 방향키 + wasd
발사 : x
업글 : 1번 (키보드 상단 숫자)
피격 : 2번 (키보드 상단 숫자)
```

적 스포너 (EnemySpawner)
```
사용 방법 :	
1. Assets/01_CJM_EnemyAndStageManager/Prefabs/Entities/EnemySpawner 프리펩을 하이에라키 뷰에 추가
2. 소환하고 싶은 적 순서대로 하이에라키 뷰 상의 EnemySpawner/StandByEnemys 의 자식으로 적 프리펩 추가 후 사용
3. 적 프리펩 위치 : Assets/01_CJM_EnemyAndStageManager/Prefabs/Characters에 등급별 존재 

테스트 입력 키)
소환 : SpaceBar
```

에러 발생 시 씬 상에 인스턴스 매니저가 없어서 그럴 경우가 많을 테니- 
테스트 시에는 Assets/01_CJM_EnemyAndStageManager/Prefabs/Managers 폴더에 존재하는 매니저 프리펩들을 추가하고 해주세요

#### EnemyManager에서는 적 등급별 설정을 해줄 수 있습니다. 씬 시작 전 세팅 기준이니 참고해주세요.

#### StageManager는 현재 스테이지의 정보를 총괄합니다. 
- 현재 스테이지에 적 스포너 개수
- 활성화된 적 수
- 플레이어가 죽인 적 수
- 현재 스테이지에서 획득한 점수

#### StageData는 현재 스테이지씬의 설정을 저장해두었다가, 해당 스테이지씬이 시작될 때 StageManager에게 정보를 전달해주는 역할입니다.
- 스테이지를 시작하려면 StageData 프리펩을 씬상에 넣어야 StageManager가 동기화 됩니다.

- 
