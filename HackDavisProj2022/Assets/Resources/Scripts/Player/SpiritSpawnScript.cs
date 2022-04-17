using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiritSpawnScript : MonoBehaviour
{
    public PlayerController controller;
    private void Start()
    {
        controller.SetCharacter(false);
    }
}
