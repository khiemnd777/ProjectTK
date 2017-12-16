using UnityEngine;
using UnityEngine.UI;

public class EnemyFieldSlot : MonoBehaviour
{
    public Image icon;
    public Enemy enemy;

    DragDropHandler dragDropHandler;

    void Start()
    {
        dragDropHandler = GetComponent<DragDropHandler>();
        dragDropHandler.onDragged += OnUpdateSlot;
    }

    public void AddField(Enemy enemy)
    {
        this.enemy = enemy;
        icon.sprite = enemy.icon;
        icon.enabled = true;
    }

    public void ClearSlot()
    {
        this.enemy = null;
        icon.enabled = false;
    }

    void OnUpdateSlot(GameObject item, int index, bool isAlternative)
    {
        var oldFieldSlot = item.GetComponent<EnemyFieldSlot>();
        if (isAlternative)
        {
            var currentEnemy = enemy;
            AddField(oldFieldSlot.enemy);
            oldFieldSlot.AddField(currentEnemy);
        }
        else
        {
            AddField(oldFieldSlot.enemy);
            oldFieldSlot.ClearSlot();
        }
    }
}