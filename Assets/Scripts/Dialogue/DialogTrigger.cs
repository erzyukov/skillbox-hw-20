using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Запускает выбранный диалог, по срабатыванию триггера
/// </summary>
public class DialogTrigger : MonoBehaviour
{
    [SerializeField] private DialogueObject dialogue = null;
    [SerializeField] private DialogueManager manager = null;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerRacks"))
        {
            manager.StartDialogue(dialogue);
            Destroy(this);
        }
    }

}
