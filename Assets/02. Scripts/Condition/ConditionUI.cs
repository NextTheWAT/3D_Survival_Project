using UnityEngine;




/*  유니티 가동 시 오류 방지를 위한 주석 처리


public class ConditionUI : MonoBehaviour
{
    // 각 상태에 해당하는 Condition 컴포넌트(임의로 Condition 이라 명칭)를 연결하는 변수입니다
    // 컴포넌트의 이름에 따라 변경 가능합니다

    public Condition health;
    public Condition hunger;
    public Condition stamina;

    // 게임 시작 시 PlayerConditionUI 스크립트에게 자신의 참조를 전달합니다
    // 임의로 지정한 CharacterManager(싱글톤) 를 통해 전달하게 설정하였습니다 (CharacterManager는 추후 변경 가능)
    private void Start()
    {
        CharacterManager.Instance.Player.condition.uiCondition = this;
    }
}


*/















/*

이 코드들은 CharacterManager 싱글톤을 사용하지 않을 경우
PlayerConditionUI의 정적 변수에 직접 할당하는 수정 코드입니다 (PlayerConditionUI도 변경해야 함)


using UnityEngine;

public class ConditionUI : MonoBehaviour
{
    public Condition health;
    public Condition hunger;
    public Condition stamina;

    private void Start()
    {
        
        PlayerConditionUI.uiCondition = this;
    }
}

*/




