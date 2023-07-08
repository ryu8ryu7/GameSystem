using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterLoader : MonoBehaviour
{
    public static CharacterBase LoadModel()
    {
        GameObject obj = Instantiate<GameObject>(ResourceManager.LoadOnView<GameObject>("3d/Character/Prefab/SM_Chr_Biker_Male_01"));

        CharacterBase character = obj.AddComponent<CharacterBase>();

        character.Initialize();

        return character;
    }

    public static CharacterController AddCharacterController(GameObject obj)
    {
        CharacterController characterController = obj.AddComponent<CharacterController>();
        characterController.center = new Vector3(0, 1.1f, 0);
        return characterController;
    }
}
