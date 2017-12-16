using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    #region Singleton
    static EquipmentManager manager;

    public static EquipmentManager instance
    {
        get
        {
            manager = FindObjectOfType<EquipmentManager>();
            if (manager == null)
            {
                Debug.LogError("There needs to be one active EquipmentManager script on a GameObject in your scene.");
            }
            else
            {
                // Init EquipmentManager if it exists
            }
            return manager;
        }
    }
    #endregion

    public delegate void OnEquipmentChanged(Equipment newItem, Equipment oldItem);
    public OnEquipmentChanged onEquipmentChanged;

    public Equipment[] defaultItems;
    public SkinnedMeshRenderer targetMesh;

    Equipment[] currentEquipment;
    SkinnedMeshRenderer[] currentMeshes;

    void Start()
    {
        var numSlots = System.Enum.GetNames(typeof(EquipmentSlot)).Length;
        currentEquipment = new Equipment[numSlots];
        currentMeshes = new SkinnedMeshRenderer[numSlots];

        EquipDefaultItems();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            UnequipAll();
        }
    }

    public void Equip(Equipment item)
    {
        var slotIndex = (int)item.equipSlot;
        var oldItem = Unequip((EquipmentSlot)slotIndex);

        if (onEquipmentChanged != null)
            onEquipmentChanged.Invoke(item, oldItem);

        SetEquipmentBlendShapes(item, 100);

        currentEquipment[slotIndex] = item;

        var newMesh = Instantiate<SkinnedMeshRenderer>(item.mesh);
        newMesh.transform.parent = targetMesh.transform;
        newMesh.bones = targetMesh.bones;
        newMesh.rootBone = targetMesh.rootBone;
        currentMeshes[slotIndex] = newMesh;
    }

    public Equipment Unequip(EquipmentSlot slot)
    {
        var slotIndex = (int)slot;
        var currentItem = currentEquipment[slotIndex];
        if (currentItem == null)
            return null;

        if (currentMeshes[slotIndex] != null)
        {
            Destroy(currentMeshes[slotIndex].gameObject);
        }

        SetEquipmentBlendShapes(currentItem, 0);
        Inventory.instance.Add(currentItem);

        currentEquipment[slotIndex] = null;

        if (onEquipmentChanged != null)
            onEquipmentChanged.Invoke(null, currentItem);

        EquipDefaultItem(slot);

        return currentItem;
    }

    public void UnequipAll()
    {
        for (var i = 0; i < currentEquipment.Length; i++)
        {
            Unequip((EquipmentSlot)i);
        }
    }

    void SetEquipmentBlendShapes(Equipment item, int weight)
    {
        foreach (var blendShape in item.coveredMeshRegions)
        {
            targetMesh.SetBlendShapeWeight((int)blendShape, weight);
        }
    }

    void EquipDefaultItems()
    {
        foreach (var item in defaultItems)
        {
            EquipDefaultItem(item.equipSlot);
        }
    }

    void EquipDefaultItem(EquipmentSlot slot)
    {
        foreach (var item in defaultItems)
        {
            if (item.equipSlot != slot)
                continue;
            Equip(item);
            return;
        }
    }
}