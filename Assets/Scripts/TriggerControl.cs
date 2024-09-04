using UnityEngine;

public class TriggerControl : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name+" entro a: "+gameObject.name);
    }
        private void OnTriggerExit(Collider other)
    {
        Debug.Log(other.gameObject.name+" salio de: "+gameObject.name);
    }
}