using UnityEngine;
using System.Collections;

public class TrainerController : MonoBehaviour
{
    [SerializeField] GameObject exclamation;
    [SerializeField] Dialog dialog;

    bool battleTriggered;

    public IEnumerator TriggerTrainerBattle(PlayerMovement player)
    {
        if (battleTriggered)
            yield break;

        battleTriggered = true;

        exclamation.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        exclamation.SetActive(false);

        void OnDialogClosed()
        {
            DialogManager.Instance.OnCloseDialog -= OnDialogClosed;
            
            player.TriggerEncounter();
        }

        DialogManager.Instance.OnCloseDialog += OnDialogClosed;

        yield return StartCoroutine(DialogManager.Instance.ShowDialog(dialog));
    }
}
