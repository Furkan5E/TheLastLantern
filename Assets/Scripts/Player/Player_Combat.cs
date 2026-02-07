using UnityEngine;

public class Player_Combat : Entity_Combat
{
    public bool CounterAttackPerformed()
    {
        bool hasCountered = false;

        foreach (var target in GetDetectedColliders())
        {
            ICounterable counterable = target.GetComponent<ICounterable>();
            if(counterable != null)
            {
                counterable.HandleCounter();
                hasCountered = true;
            }
        }
        return hasCountered;
    }
}