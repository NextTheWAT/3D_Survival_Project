using System;
using UnityEngine;

// 플레이어가 피해를 입을 수 있다는 것을 명시하는 인터페이스입니다
public interface IDamagable
{
    void TakePhysicalDamage(int damageAmount);
}


public class PlayerConditionUI : MonoBehaviour, IDamagable
{
    
    public ConditionUI uiCondition;
    // UI를 업데이트하기 위해 ConditionUI 스크립트의 참조를 저장합니다

    
    Condition health { get { return uiCondition.health; } }
    Condition hunger { get { return uiCondition.hunger; } }
    Condition stamina { get { return uiCondition.stamina; } }

    // UI에 연결된 각 상태의 Condition 객체를 가져오는 속성입니다
    // ConditionUI에 적은 것과 마찬가지로 Condition은 임의로 지정한 이름입니다 (변경 가능)





    // 허기가 0일 때 체력이 감소하는 속도를 조절합니다
    [Header("Decay Rates")]
    public float noHungerHealthDecay;



    // 매 프레임마다 호출되어 플레이어 상태의 변화를 감지하고 처리합니다
    private void Update()
    {
        hunger.Subtract(hunger.passiveValue * Time.deltaTime);
        stamina.Add(stamina.passiveValue * Time.deltaTime);

        if (hunger.curValue <= 0)
        {
            health.Subtract(noHungerHealthDecay * Time.deltaTime);
        }

        if (health.curValue <= 0)
        {
            Die();
        }
    }

    // 플레이어의 체력을 회복
    public void Heal(float amount)
    {
        health.Add(amount);
    }

    // 플레이어의 허기를 회복
    public void Eat(float amount)
    {
        hunger.Add(amount);
    }

    // 플레이어가 물리적인 피해를 입었을 때 체력을 감소시킵니다
    public void TakePhysicalDamage(int damageAmount)
    {
        health.Subtract(damageAmount);
    }

    // 플레이어의 사망을 처리하는 메서드입니다
    public void Die()
    {
        Debug.Log("플레이어 사망");
    }
}






/*


이 코드는 ConditionUI 에서 CharacterManager(가칭) 을 사용하지 않을 경우 사용하는 수정코드입니다
ConditionUI uiCondition 부분을 public static으로 변경하여 외부에서 직접 접근할 수 있게 합니다



using System;
using UnityEngine;

public interface IDamagable
{
    void TakePhysicalDamage(int damageAmount);
}

public class PlayerConditionUI : MonoBehaviour, IDamagable
{
    
    public static ConditionUI uiCondition;

    Condition health { get { return uiCondition.health; } }
    Condition hunger { get { return uiCondition.hunger; } }
    Condition stamina { get { return uiCondition.stamina; } }

    [Header("Decay Rates")]
    public float noHungerHealthDecay;

    private void Update()
    {
        
        hunger.Subtract(hunger.passiveValue * Time.deltaTime);
        stamina.Add(stamina.passiveValue * Time.deltaTime);

        if (hunger.curValue <= 0)
        {
            health.Subtract(noHungerHealthDecay * Time.deltaTime);
        }

        if (health.curValue <= 0)
        {
            Die();
        }
    }
    
    public void Heal(float amount)
    {
        health.Add(amount);
    }

    public void Eat(float amount)
    {
        hunger.Add(amount);
    }
    
    public void TakePhysicalDamage(int damageAmount)
    {
        health.Subtract(damageAmount);
    }

    public void Die()
    {
        Debug.Log("플레이어 사망");
    }
}




 */





