# 유니티짱의 3D Survival

## 📖 소개
이 프로젝트는 Unity 기반 3D 서바이벌 게임입니다.  
플레이어는 다양한 환경 속에서 체력, 허기, 스태미나를 관리하며, 아이템을 채집 → 제작(Crafting) → 장비/소비하여 생존 확률을 높여야 합니다.  
  
또한, 적 몬스터의 위협과 자원 관리, NPC와의 상호작용을 통해 게임 내 목표를 달성하는 것이 핵심입니다.  
  
- **문제 의식**: 단순히 몬스터를 잡는 것이 아니라, 생존을 위해 상태 관리, 제작, 건축, 대화 등 종합적인 시스템이 필요함.  
- **주요 기능 요약**:   
  - 플레이어 이동, 전투, 건축, 상호작용  
  - 인벤토리 & 제작 시스템  
  - ScriptableObject 기반 아이템/레시피 관리  
  - FSM 기반 몬스터 AI   
  - NPC 대화 시스템

## 조작법

- **WASD**: 캐릭터 이동
- **F1**: 커서 모드 변경
- **F2**: 1인칭/3인칭 변경
- **Tab**: UI 켜기/끄기
- **Enter**: 건축물 설치
- **Mouse Scroll**: 건축물 회전
- **Q**: 건축물 축 변경
- **Mouse Right Click**: 건축 취소 명령

## 📸 게임 플레이 영상 / Gif
![Animation](https://github.com/user-attachments/assets/5760978c-d968-4368-a36c-8080adf4601f)
- https://youtu.be/pPr7Np8qwy8
  
## 🛠️ 기술 스택

```
Engine: Unity 2022.3.17f
Languege: C#
Version Control: Git
```

## 🚀 주요 기능

### 🎮 플레이어 시스템  

- 이동 속도에 따라 Idle / Walk / Run 애니메이션 자동 전환  
- 스태미나, 허기, 체력 등 플레이어 상태 관리  
  
### 🏗️ 빌드 시스템 

- 자원(나무, 돌 등)을 채집하여 건축 재료 확보  
- 건축 모드 전환 → 오브젝트 배치 및 설치 가능  
- 설치된 구조물은 파괴 및 수정 가능  
   
### 👾 몬스터 & 전투 시스템 

- 다양한 몬스터 스폰 및 AI 이동  
- 근접 / 원거리 공격 패턴 구현  
- 플레이어와 몬스터 간의 전투 (데미지 처리, HP UI 반영)  
- 전투 결과에 따른 아이템 드랍 및 보상 시스템   
   
### 📦 인벤토리 & 아이템    

- 획득한 자원을 인벤토리에 저장  
- 아이템 사용 / 버리기 기능  
- 장비 착용 시 능력치 반영  

### 🗨️ NPC 시스템   

- DialogueSO 기반의 대사 데이터 관리  
- DialogueRunner를 통한 대화 진행 제어   
- DialogueViewUI, NPCNameTag를 통한 UI 연동  
- NPC와 상호작용 시 대화, 퀘스트 안내, 정보 제공 등 다양한 기능 지원  
  
## 📂 프로젝트 구조  

```
02. Scripts   
├─ Condition/
│  └─ BaseCondition.cs  
├─ Enemy/
│  ├─ Enemy.cs  
│  ├─ Enemy.State.cs   
│  ├─ EnemyAnimParam.cs  
│  ├─ EnemyDetect.cs   
│  ├─ EnemySpawnArea.cs  
│  ├─ EnemySpawnTrigger.cs   
│  ├─ FieldOfViewDraw.cs  
│  └─ HitFlash.cs   
├─ Interface/
│  ├─ IDamagable.cs  
│  ├─ IState.cs  
│  └─ IValueChangable.cs    
├─ Inventory/
│  ├─ EquipmentController.cs   
│  ├─ EquipmentModel.cs  
│  ├─ InventoryManager.cs  
│  └─ InventoryModel.cs  
├─ Items/
│  ├─ CraftSystem.cs 
│  ├─ enums.cs  
│  ├─ GameManager.cs  
│  ├─ IInteractable.cs    
│  ├─ ItemObject.cs   
│  ├─ ResourceContainer.cs  
│  └─ ResourceObject.cs 
├─ Object/
│  ├─ Character/
│  │  └─ Player/
│  │     ├─ Weapon/                        
│  │     ├─ PlayerAttackController.cs   
│  │     ├─ PlayerBuildingController.cs  
│  │     ├─ PlayerCondition.cs   
│  │     ├─ PlayerInteractionController.cs  
│  │     ├─ PlayerMovementController.cs   
│  │     └─ PlayerPerspectiveController.cs  
│  ├─ BuildingSimulationRenderer.cs     
│  └─ CollisionDetector.cs  
├─ ScriptableObject/
│  ├─ Items/
│  │  ├─ ConsumeItemData.cs  
│  │  ├─ EquipItemData.cs   
│  │  ├─ ItemData.cs    
│  │  ├─ ItemDataBase.cs  
│  │  ├─ RecipeData.cs  
│  │  └─ ResourceData.cs  
│  └─ NPC/
│     └─ DialogueSO.cs  
├─ State/
│  ├─ BaseState.cs   
│  └─ FiniteStateMachine.cs   
├─ UI/
│  ├─ Craft/
│  │  └─ CraftUI.cs   
│  ├─ Inventory/
│  │  ├─ InventorySlotUI.cs  
│  │  └─ InventoryUI.cs  
│  ├─ Mediator/
│  │  └─ InventoryMediator.cs  
│  ├─ NPC/
│  │  ├─ DialogueViewUI.cs  
│  │  ├─ NPCDialogue.cs  
│  │  └─ NPCNameTag.cs  
│  ├─ Player/
│  │  └─ PlayerConditionUI.cs   
│  └─ Runner/
│     ├─ DialogueRunner.cs 
│     └─ BaseUI.cs   
└─ Utils/
   ├─ Attribute/
   │  └─ AliasAttribute.cs   
   ├─ Editor/
   │  └─ AliasDrawer.cs    
   ├─ Extension/
   │  └─ GameObject.Extension.cs  
   ├─ Input/
   │  └─ CharacterControls.cs  
   ├─ Management/
   │  ├─ Pooling/
   │  │  ├─ Clone.cs  
   │  │  ├─ Container.cs  
   │  │  └─ ObjectPooling.cs  
   │  ├─ ApplicationManager.cs  
   │  ├─ DataManager.cs  
   │  ├─ ObjectPoolingManager.cs  
   │  ├─ Singleton.cs  
   │  ├─ SingletonBehavior.cs   
   │  ├─ SingletonGameObject.cs   
   │  ├─ SoundManager.cs   
   └─ Layer.cs
```
   
### 📂폴더 한 줄 설명  

- **Condition**: 플레이어/엔티티 공통 상태 기반.  
- **Enemy**: 적 AI, 감지·스폰, 피격 이펙트.  
- **Interface**: 데미지/상태/FSM 공통 인터페이스.  
- **Inventory**: 인벤토리·장비 데이터/로직.  
- **Items**: 제작, 상호작용, 자원·아이템 오브젝트.  
- **Object**: 플레이어 컨트롤러와 빌딩 시뮬.  
- **ScriptableObject**: 아이템/레시피/자원/NPC 대화 데이터.    
- **State**: 공통 FSM 베이스.  
- **UI**: Craft/Inventory/NPC/Player UI와 러너.  
- **Utils**: Attribute/Extension/Input/Managers/Pooling/Layer 유틸.  

## 👥 팀원 소개

- **팀장** : 이재은 - NPC & 대화 시스템, UI(Anim)
- **팀원** : 이형권 - 전투 & 적 AI 파트
- **팀원** : 오경민 - 건축 & 기지 구축 파트 및 프로젝트 구조
- **팀원** : 유형준 - 생존 관리 시스템 파트
- **팀원** : 진영아 - 자원 수집,시스템 & 가공 시스템 파트
  
## 📌 프로젝트 수행 경과  

### 기술적 의사결정 

- **FSM 구조**: IState/BaseState/FiniteStateMachine으로 AI와 UI 흐름을 모듈화  
- **데이터 중심 설계**: 아이템/레시피/자원/대화를 전부 ScriptableObject로 관리  
- **UI 구조**: InventoryModel ↔ InventoryMediator ↔ InventoryUI로 역할 분리(Mediator 패턴)   
- **인터페이스 일원화**: IDamagable(피해), IInteractable(상호작용), IValueChangable(수치 변화)로 공통 행위 통합 
- **인프라(설계만)**: ObjectPoolingManager, DataManager, SoundManager, 3종 Singleton 베이스를 준비  
  
### 구현 과정  

- **플레이어**: 이동/시점/공격/상호작용/건축/상태(HP·허기·스태미나) 일체 구현  
- **적 AI**: 감지→이동→공격→사망의 상태 전환과 스폰(영역/트리거) 구현, 피격 피드백(HitFlash) 연동  
- **인벤토리·장비**: 획득/소비/장착 동작, 슬롯/스택 관리, UI 갱신  
- **제작(Craft)**: RecipeData 기반 제작 가능 여부 검증→결과 아이템 지급  
- **NPC 대화**: DialogueSO 데이터 기반 출력, Runner로 진행 제어, 이름표/대화창 UI  
  
### 트러블슈팅  
<img width="1000" height="522" alt="image" src="https://github.com/user-attachments/assets/811ded3e-bb2a-41e6-b7ee-3d68adced88f" />
<img width="1000" height="495" alt="image" src="https://github.com/user-attachments/assets/12a07596-3381-4148-adc8-f2535206b52d" />
<img width="1000" height="555" alt="image" src="https://github.com/user-attachments/assets/1080a3b8-2573-4b15-981b-aa9133282c55" />
<img width="1000" height="548" alt="image" src="https://github.com/user-attachments/assets/222cd14b-e478-484b-84dc-8fe5aec59699" />
<img width="1000" height="530" alt="image" src="https://github.com/user-attachments/assets/28428c86-443e-4bcc-8a26-319fb6534016" />
<img width="1000" height="428" alt="image" src="https://github.com/user-attachments/assets/10a1f614-2998-4a4d-be1c-66617d0d699b" />



  
### 성과 및 개선점

- **성과**: FSM·Mediator·SO 도입으로 확장성과 협업 효율 확보, 핵심 게임루프(탐색→채집/전투→제작/건축) 완성  
- **개선 예정**:  
  - ObjectPooling 실제 적용(스폰/이펙트/드랍)  
  - DataManager로 인벤토리/설정 저장·로드 구현  
  - SoundManager 이벤트 연동(BGM/효과음)  
  - Singleton 방식 통일(불필요 베이스 정리)  
