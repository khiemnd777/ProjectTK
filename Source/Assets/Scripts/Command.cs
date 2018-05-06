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
	public CharacterList characterList;

	void Awake()
	{
		if(!_instance)
		{
			DontDestroyOnLoad(gameObject);
		}
	}

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
		if(Input.GetKeyUp(KeyCode.E)){
			SceneManager.LoadScene("SimBattleMode", LoadSceneMode.Single);
			// var sence = SceneManager.GetSceneByBuildIndex(1);
			// SceneManager.LoadScene(sence.name, LoadSceneMode.Single);
			// SceneManager.MoveGameObjectToScene(gameObject, sence);
			characterList.MoveToSence("SimBattleMode");
		}
    }
}