using UnityEngine;
using System.Collections;

namespace RPG.Core
{
    public class ActionScheduler : MonoBehaviour
    {
        MonoBehaviour currentAction;

        public void StartAction(MonoBehaviour action)
        {
            if (currentAction != null && currentAction != action)
            {
                print($"Cancelling action {currentAction}");
            }

            currentAction = action;
        }
    }
}
