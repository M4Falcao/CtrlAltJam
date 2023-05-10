using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstGun : MonoBehaviour, Iinteractive
{
    public void Interact()
    {
        Debug.Log("Agora voce esta armado ate os dentes");
        Destroy(gameObject);
    }
}
