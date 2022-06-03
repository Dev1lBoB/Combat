using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine;

public class Combat : MonoBehaviour
{
    [SerializeField]
    private Squad[] squads = new Squad[2];

    [SerializeField]
    private Button attackButton;
    [SerializeField]
    private Button skipButton;

    [SerializeField]
    private AttackHandler attackHandler;

    [SerializeField]
    private DialogWindowManager dialogWindowManager;
    
    private List<Unit> readyToMove;

    private Unit    activeUnit;
    private Squad   targetSquad;

    private bool choosingTargetToAttack = false;

    private YieldCollection yieldManager = new YieldCollection();

    private UnityAction attackAction;
    private UnityAction skipAction;

    private void Awake()
    {
        readyToMove = new List<Unit>();

        attackAction += AttackButtonPressed;
        skipAction += SkipButtonPressed;
    }

    private void Start()
    {
        SetUpReadyToMove();
        StartCoroutine(Approach());
    }

    private void SetUpReadyToMove()
    {
        // Fill readyToMove list with all the alive units in the battlefield
        foreach (Squad s in squads)
        {
            List<Unit> unitsInSquad = s.GetAllUnits();
            foreach(Unit u in unitsInSquad)
            {
                readyToMove.Add(u);
            }
        }
    }

    private IEnumerator Approach()
    {
        foreach(Squad s in squads)
        {
            // Move both teams to the batllefield
            StartCoroutine(yieldManager.CountCoroutine(s.GetComponent<SquadApproach>().StartApproaching()));
        }
        yield return yieldManager;

        SetUpButtons();
        SelectRandomUnit();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (choosingTargetToAttack == true)
            {
                // User selects the target to attack by clicking on it
                Unit target = CastRayToSelectTarget();
                if (target != null && target.canBeChoosen == true)
                {
                    targetSquad.UnsetAllUnitsAsTargets();
                    choosingTargetToAttack = false;
                    StartCoroutine(Attack(activeUnit, target));
                }
            }
        }
    }

    private IEnumerator Attack(Unit active, Unit target)
    {
        UnsetButtons(); // Disable buttons until attack is finished
        yield return AttackChoice(active, target);
        ClearUpDeadUnits();

        string winTeamName = CheckForWinningTeam();
        if (winTeamName != null)
            dialogWindowManager.GameEnded(winTeamName); // Show the endgame window if every unit in one of the teams are dead
        else
        {
            SetUpButtons();
            SelectRandomUnit();
        }
    }

    private IEnumerator AttackChoice(Unit active, Unit target)
    {
        // Send info about what side team is attacking (right/left)
        if (squads[0] == target.squad)
            yield return attackHandler.StartAttack(active, target, false);
        else
            yield return attackHandler.StartAttack(active, target, true);
    }

    private string CheckForWinningTeam()
    {
        if (squads[0].GetAllUnits().Count == 0)
            return "right";
        else if (squads[1].GetAllUnits().Count == 0)
            return "left";
        return null;
    }

    private Unit CastRayToSelectTarget()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast (ray.origin, ray.direction, Mathf.Infinity);

        if (hit)
        {
            Unit target = hit.collider.gameObject.GetComponent<Unit>();
            return target; 
        }
        return null;
    }

    private void SelectRandomUnit()
    {
        if (activeUnit) // Deactivate current active unit
            activeUnit.SetActiveStatus(false);

        if (readyToMove.Count == 0) // If all units already moved in this round refills the list again
            SetUpReadyToMove();

        int randomIndex = Random.Range(0, readyToMove.Count); // Choose random unit to move
        activeUnit = readyToMove[randomIndex];
        activeUnit.SetActiveStatus(true);

        readyToMove.Remove(activeUnit); // Remove unit from ready list, so every unit will move only once per round
        if (activeUnit.squad == squads[1])
        {
            // If it's the enemy team moving let the Computer to choose the target to attack
            Unit target = squads[0].GetRandomUnit();
            StartCoroutine(Attack(activeUnit, target));
        }
        else
        {
            // Else remember from which team will be selected target
            targetSquad = GetTargetSquad(activeUnit);
        }
    }

    private void ClearUpDeadUnits()
    {
        // Remove all destroyed units from the game
        for (int i = readyToMove.Count - 1; i >= 0; i--)
        {
            if (readyToMove[i].isToBeDestroyed == true)
                readyToMove.Remove(readyToMove[i]);
        }
        foreach (Squad s in squads)
        {
            s.ClearUpDeadUnits();
        }
    }

    private void SetUpButtons()
    {
        attackButton.onClick.AddListener(attackAction);
        skipButton.onClick.AddListener(skipAction);
    }

    private void UnsetButtons()
    {
        attackButton.onClick.RemoveListener(attackAction);
        skipButton.onClick.RemoveListener(skipAction);
    }

    private void AttackButtonPressed()
    {
        choosingTargetToAttack = !choosingTargetToAttack;
        if (choosingTargetToAttack == true)
        {
            // Mark all units in targetSquad as potential target
            targetSquad.SetAllUnitsAsTargets();
        }
        else
        {
            // Unmark them
            targetSquad.UnsetAllUnitsAsTargets();
        }
    }

    private Squad GetTargetSquad(Unit unit)
    {
        if (squads[0] == unit.squad)
            return squads[1];
        return squads[0];
    }

    private void SkipButtonPressed()
    {
        if (choosingTargetToAttack == true)
        {
            // Unmark all units in targetSquad as potential target if attack button was pressed before
            choosingTargetToAttack = false;
            targetSquad.UnsetAllUnitsAsTargets();
        }
        SelectRandomUnit(); // Choose next unit
    }
}
