using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.InputSystem;
using UnityEngine;

interface Iinteractive {
    
    public void Interact();
}

public class Interact : MonoBehaviour
{
    public float InteractRange;
    public LayerMask objectsLayer;

    private StarterAssetsInputs _inputs;

    private void Awake()
    {
        _inputs = GetComponent<StarterAssetsInputs>();
    }

    
    void Update()
    {
        if (_inputs.interact)
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, InteractRange, objectsLayer);
            foreach (Collider collider in hitColliders)
            {            
                if(collider.gameObject.TryGetComponent<Iinteractive>(out Iinteractive interactiveObj))
                {
                    interactiveObj.Interact();
                }
               
            }
        }
        _inputs.interact = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, InteractRange);
        
    }
}
