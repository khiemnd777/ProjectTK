using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityList : MonoBehaviour
{
    #region Singleton
    static AbilityList list;

    public static AbilityList instance
    {
        get
        {
            if (!list)
            {
                list = FindObjectOfType<AbilityList>();
                if (!list)
                {
                    Debug.LogError("There needs to be one active AbilityList script on a GameObject in your scene.");
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

    public AbilityItem itemPrefab;
    public Transform container;
    public Transform panel;

    float skillPanelDisplayingPercent;
    float skillPanelClosingPercent;
    Canvas canvas;

    void Start()
    {
        canvas = panel.GetComponentInParent<Canvas>();
        panel.gameObject.SetActive(false);
    }

    public void AddItem(Ability item)
    {
        item.displayOrder = GetNextDisplayOrder();
        // Instantiate an ability item
        var abilityItem = Instantiate<AbilityItem>(itemPrefab, Vector2.zero, Quaternion.identity, container);
        abilityItem.ability = item;
        abilityItem.ClearAllTacticItems();
        abilityItem.InstantiateTacticItems();
        abilityItem.ToggleTacticContainer();
        abilityItem.HandleTitle();
    }

    int GetNextDisplayOrder()
    {
        var abilityItems = container.GetComponentsInChildren<AbilityItem>();
        if(abilityItems.Length == 0){
            return 1;
        }
        var nextDisplayOrder = abilityItems.Max(x => x.ability.displayOrder) + 1;
        abilityItems = null;
        return nextDisplayOrder;
    }

    public void SetDisplayOrder()
    {
        var abilityItems = container.GetComponentsInChildren<AbilityItem>();
        for (var i = 0; i < abilityItems.Length; i++)
        {
            var item = abilityItems[i];
            item.ability.displayOrder = i + 1;
            item = null;
        }
        abilityItems = null;
    }

    public void Clear()
    {
        var items = container.GetComponentsInChildren<AbilityItem>();
        foreach (var item in items)
        {
            DestroyImmediate(item.gameObject);
        }
    }

    public void OpenPanel(Vector3 fromPosition)
    {
        StopCoroutine("PanelOpening");
        skillPanelDisplayingPercent = 0f;
        panel.gameObject.SetActive(false);
        panel.transform.localScale = Vector3.one;
        StartCoroutine(PanelOpening(fromPosition));
    }

    public IEnumerator PanelOpening(Vector3 fromPosition)
    {
        var cloneAbilityList = Instantiate(panel, fromPosition, Quaternion.identity, canvas.transform);
        cloneAbilityList.gameObject.SetActive(true);
        var originalClonePosition = cloneAbilityList.transform.position;
        cloneAbilityList.transform.localScale = Vector3.zero;
        while (skillPanelDisplayingPercent < 1f)
        {
            skillPanelDisplayingPercent += Time.deltaTime / .25f;
            cloneAbilityList.transform.position = Vector2.Lerp(originalClonePosition, panel.transform.position, skillPanelDisplayingPercent);
            cloneAbilityList.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, skillPanelDisplayingPercent);
            yield return null;
        }
        Destroy(cloneAbilityList.gameObject);
        panel.gameObject.SetActive(true);
        skillPanelDisplayingPercent = 0f;
    }

    public void OnClosePanelButtonClick()
    {
        StartCoroutine(SkillPanelClosing());
    }

    IEnumerator SkillPanelClosing()
    {
        while (skillPanelClosingPercent < 1f)
        {
            skillPanelClosingPercent += Time.deltaTime / .175f;
            panel.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, skillPanelClosingPercent);
            yield return null;
        }
        skillPanelClosingPercent = 0f;
        panel.gameObject.SetActive(false);
    }
}