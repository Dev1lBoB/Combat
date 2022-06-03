using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHandler : MonoBehaviour
{
    [SerializeField]
    private BlurInOut blur;

    [SerializeField]
    private Vector2 leftAttackPosition;
    [SerializeField]
    private Vector2 rightAttackPosition;

    [SerializeField]
    private float attackTime = 0.05f;
    [SerializeField]
    private float returnTime = 0.15f;

    private YieldCollection manager = new YieldCollection();

    public IEnumerator StartAttack(Unit activeUnit, Unit target, bool side)
    {
        Vector3 activeUnitPos = activeUnit.transform.position;
        Vector3 targetPos = target.transform.position;

        // Move units to the front plan, so they will be in front of the blurred background
        activeUnit.transform.position = new Vector3(activeUnitPos.x, activeUnitPos.y, -5);
        target.transform.position = new Vector3(targetPos.x, targetPos.y, -5);

        yield return PrepareAttack(activeUnit, target, side);

        yield return activeUnit.Attack(target);

        yield return FinishAttack(activeUnit, target, activeUnitPos, targetPos);

        // Return units to their original Z order
        if (activeUnit.isToBeDestroyed != true)
            activeUnit.transform.position = activeUnitPos;
        if (target.isToBeDestroyed != true)
            target.transform.position = targetPos;
    }

    private IEnumerator PrepareAttack(Unit activeUnit, Unit target, bool side)
    {
        // Everything that should be done before attack

        yield return blur.BlurIn(.05f);

        // Move units to the attack positions based on their side
        if (side == true)
            MoveToAttackPositions(activeUnit, target, leftAttackPosition, rightAttackPosition);
        else
            MoveToAttackPositions(activeUnit, target, rightAttackPosition, leftAttackPosition);
        
        StartCoroutine(manager.CountCoroutine(activeUnit.transform.RescaleTo(new Vector2(1.4f, 1.4f), attackTime)));
        StartCoroutine(manager.CountCoroutine(target.transform.RescaleTo(new Vector2(1.4f, 1.4f), attackTime)));

        yield return manager;
    }

    private IEnumerator FinishAttack(Unit activeUnit, Unit target, Vector3 activeUnitPos, Vector3 targetPos)
    {
        // Everything that should be done after attack

        if (activeUnit.isToBeDestroyed != true)
        {
            StartCoroutine(manager.CountCoroutine(activeUnit.transform.RescaleTo(new Vector2(.8f, .8f), returnTime)));
            StartCoroutine(manager.CountCoroutine(activeUnit.transform.MoveTo(activeUnitPos, returnTime)));
        }
        if (target.isToBeDestroyed != true)
        {
            StartCoroutine(manager.CountCoroutine(target.transform.RescaleTo(new Vector2(.8f, .8f), returnTime)));
            StartCoroutine(manager.CountCoroutine(target.transform.MoveTo(targetPos, returnTime)));
        }

        yield return manager;

        yield return blur.BlurOut(.5f);
    }

    private void MoveToAttackPositions(Unit firstUnit, Unit secondUnit, Vector2 firstPosition, Vector2 secondPosition)
    {
        StartCoroutine(manager.CountCoroutine(firstUnit.transform.MoveTo(firstPosition, attackTime)));
        StartCoroutine(manager.CountCoroutine(secondUnit.transform.MoveTo(secondPosition, attackTime)));
    }
}
