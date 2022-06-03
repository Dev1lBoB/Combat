using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(OutlineUnderTheMouse))]
[RequireComponent(typeof(OutlineController))]
[RequireComponent(typeof(AnimationController))]
[RequireComponent(typeof(Health))]
public class Unit : MonoBehaviour
{
    private OutlineUnderTheMouse    outlineUnderTheMouse;
    private OutlineController       outlineController;

    [SerializeField]
    private Vector2             dmgRange;
    private Health              health;
    private AnimationController animationController;

    private bool _canBeChoosen = false;

    public bool canBeChoosen
    {
        get
        {
            return _canBeChoosen;
        }
        set
        {
            _canBeChoosen = value;
            if (outlineUnderTheMouse == true)
                outlineUnderTheMouse.isActive = value;
            if (outlineController == true)
            {
                if (value == true)
                    outlineController.BecomeTarget();
                else
                    outlineController.TurnOffOutline();
            }
        }      
    }

    [HideInInspector]
    public bool isActive = false;

    [HideInInspector]
    public bool isToBeDestroyed = false;

    [HideInInspector]
    public Squad squad;

    void Awake()
    {
        outlineUnderTheMouse = GetComponent<OutlineUnderTheMouse>();
        outlineController = GetComponent<OutlineController>();
        squad = transform.parent.gameObject.GetComponent<Squad>();
        animationController = GetComponent<AnimationController>();
        health = GetComponent<Health>();
    }

    public void SetActiveStatus(bool status)
    {
        if (status == true)
        {
            outlineController.BecomeActive();
        }
        else
        {
            outlineController.TurnOffOutline();
        }
        isActive = status;
    }

    public IEnumerator Attack(Unit target)
    {
        yield return animationController.PlayAnimationOnce("attack", "idle");
        yield return target.TakeDmg(Random.Range(dmgRange.x, dmgRange.y));
    }

    public IEnumerator TakeDmg(float dmgAmount)
    {
        health.TakeDamage(dmgAmount);
        yield return animationController.PlayAnimationOnce("fall", "idle");
        if (health.curHealth == 0)
            SelfDestruct();
    }

    public void SelfDestruct()
    {
        isToBeDestroyed = true;
        Destroy(gameObject);
    }
}
