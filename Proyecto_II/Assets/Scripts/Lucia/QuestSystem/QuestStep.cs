using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Es abstract porque está pensada para que herede de otra clase, no para que se use directamente
public abstract class QuestStep : MonoBehaviour
{
    private bool isFinished = false;

    //Protected porque se va a usar en las clases hijas
    protected void FinishQuestStep()
    {
        if(!isFinished)
        {
            isFinished = true;

            //TODO - Advance the quest fordward now that we've finished this step

            Destroy(this.gameObject);
        }
    }
}
