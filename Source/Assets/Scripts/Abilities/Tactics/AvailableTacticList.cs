using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvailableTacticList : MonoBehaviour
{
	#region Singleton
    static AvailableTacticList list;

    public static AvailableTacticList instance
    {
        get
        {
            if (!list)
            {
                list = FindObjectOfType<AvailableTacticList>();
                if (!list)
                {
                    Debug.LogError("There needs to be one active AvailableTacticList script on a GameObject in your scene.");
                }
                else
                {
                    // Init if exists
                }
            }
            return list;
        }
    }
    #endregion

	public AvailableTacticItem itemPrefab;
	public Transform container;
    public Transform panel;
	[Space]
	public List<Tactical> availableTactics = new List<Tactical>();

	void Start(){
		Clear();
		panel.gameObject.SetActive(false);
	}

	void Update(){
		if(Input.GetButtonDown("Available Tactics")){
			TogglePanel();	
		}
	}

	public void AddItem(Tactical tactic)
    {
        // Instantiate an ability item
        var item = Instantiate<AvailableTacticItem>(itemPrefab, Vector2.zero, Quaternion.identity, container);
		item.tactic = tactic;
        item.HandleTitle();
    }

	public void Clear()
    {
        var items = container.GetComponentsInChildren<AvailableTacticItem>();
        foreach (var item in items)
        {
            DestroyImmediate(item.gameObject);
        }
    }

	public void OnClosePanelButtonClick()
    {
        TogglePanel();
    }

	public void TogglePanel(){
		Clear();
		panel.gameObject.SetActive(!panel.gameObject.activeSelf);
		foreach(var tactic in availableTactics){
			if(tactic.isDefault)
				continue;
			AddItem(tactic);
		}
	}
}
