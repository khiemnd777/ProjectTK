using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
	public Image icon;
	public Button removeButton;

	Item item;

	public void AddItem(Item item){
		this.item = item;
		icon.sprite = item.icon;
		icon.enabled = true;
		removeButton.interactable = true;
	}

	public void ClearSlot(){
		this.item = null;
		icon.sprite = null;
		icon.enabled = false;
		removeButton.interactable = false;
	}

	public void OnRemoveButtonClick(){
		Inventory.instance.Remove(item);
	}

	public void OnUseButtonClick(){
		if(this.item == null)
			return;
		this.item.Use();
	}
}