using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Command : MonoBehaviour
{
	#region Singleton
    static Command _instance;

    public static Command instance
    {
        get
        {
            if (!_instance)
            {
                _instance = FindObjectOfType<Command>();
                if (!_instance)
                {
                    Debug.LogError("There needs to be one active Command script on a GameObject in your scene.");
                }
                else
                {

                }
            }
            return _instance;
        }
    }
    #endregion

	public Canvas generatedCharacterUI;
	public Canvas selectionUI;

    void Update()
    {
		if(Input.GetKeyUp(KeyCode.Q)){
			if(SceneManager.GetActiveScene().name != "Generation")
			{
				SceneManager.LoadScene("Generation", LoadSceneMode.Single);
				return;
			}
			generatedCharacterUI.gameObject.SetActive(true);
			selectionUI.gameObject.SetActive(false);
		}
		if(Input.GetKeyUp(KeyCode.W)){
			if(SceneManager.GetActiveScene().name != "Generation")
			{
				SceneManager.LoadScene("Generation", LoadSceneMode.Single);
				return;
			}
			generatedCharacterUI.gameObject.SetActive(false);
			selectionUI.gameObject.SetActive(true);
		}
		if(Input.GetKeyUp(KeyCode.E)){
			if(SceneManager.GetActiveScene().name == "SimBattleMode")
				return;
			SceneManager.LoadScene("SimBattleMode", LoadSceneMode.Single);
		}
    }
}