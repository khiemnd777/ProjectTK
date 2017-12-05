using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterField : MonoBehaviour
{
    public bool flip;
    public Transform spawner;
    public Character character;

    Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        Flip();
        BeCoaxial();
    }

    public void AddField(Character character)
    {
        this.character = character;
        SpawnModel();
        Flip();
    }

    public void ClearSlot()
    {
        this.character = null;
    }

    public bool CanAdd()
    {
        return character.IsNull();
    }

    void SpawnModel()
    {
        if (character.model.IsNull())
            return;
        var model = character.model;
        model.gameObject.SetActive(true);
        // model.position = spawner.position;
        model.transform.SetParent(spawner.transform);
        model.localPosition = Vector3.zero;
        model.localScale = Vector3.one;
        model = null;
    }

    void Flip()
    {
        if (!flip)
            return;
        if (character.IsNull())
            return;
        if (!character.model.IsNull())
        {
            var renderer = character.model.GetComponentInChildren<SpriteRenderer>();
            renderer.flipX = true;
            renderer = null;
        }
    }

    void BeCoaxial()
    {
        if (character.IsNull())
            return;
        if (character.model.IsNull())
            return;
        var eulerAngles = cam.transform.rotation.eulerAngles;
        character.model.rotation = Quaternion.Euler(eulerAngles);
    }
}
