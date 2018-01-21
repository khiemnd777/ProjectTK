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
        if(!character.IsNull() && character.isDeath){
            character.model.gameObject.SetActive(false);
            ClearSlot();
        }
        Flip();
    }

    void LateUpdate()
    {
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
            // renderer.flipX = true;
            // flip of model according to origin
            var originScale = character.transform.localScale;
            var flipDelta = originScale.x < 0 ? -1 : 1;
            var flip = -1f * flipDelta;
            renderer.transform.localScale = new Vector3(flip, 1f, 1f);
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
        // To be coaxial for character model
        character.model.rotation = Quaternion.Euler(eulerAngles);
    }
}
