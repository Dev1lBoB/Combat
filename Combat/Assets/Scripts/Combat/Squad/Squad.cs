using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Squad : MonoBehaviour
{
    private List<Unit> units;

    private int numOfUnits;

    private void Awake()
    {
        units = (from o in gameObject.GetChildren() where o.GetComponent<Unit>()
            select o.GetComponent<Unit>()).ToList();
        numOfUnits = units.Count;
    }

    public Unit GetRandomUnit()
    {
        int randomIndex = Random.Range(0, numOfUnits);
        return units[randomIndex];
    }

    public List<Unit> GetAllUnits()
    {
        return units;
    }

    public void SetAllUnitsAsTargets()
    {
        foreach (Unit u in units)
        {
            u.canBeChoosen = true;
        }
    }

    public void UnsetAllUnitsAsTargets()
    {
        foreach (Unit u in units)
        {
            u.canBeChoosen = false;
        }
    }

    public void ClearUpDeadUnits()
    {
        // Remove all destroyed units from the list
        for (int i = units.Count - 1; i >= 0; i--)
        {
            if (units[i].isToBeDestroyed == true)
                units.Remove(units[i]);
        }
        numOfUnits = units.Count;
    }
}
