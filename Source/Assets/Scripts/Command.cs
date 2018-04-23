using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Command : MonoBehaviour
{
	public Canvas generatedCharacterUI;
	public Canvas selectionUI;

    void Update()
    {
		if(Input.GetKeyUp(KeyCode.Q)){
			generatedCharacterUI.gameObject.SetActive(true);
			selectionUI.gameObject.SetActive(false);
		}
		if(Input.GetKeyUp(KeyCode.W)){
			generatedCharacterUI.gameObject.SetActive(false);
			selectionUI.gameObject.SetActive(true);
			SelectionCharacterUI.instance.AssignSquad();
			SelectionCharacterUI.instance.AssignPreparatorySquad();
		}
    }
}