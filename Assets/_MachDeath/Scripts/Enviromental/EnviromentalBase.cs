using UnityEngine;
namespace Mirror.MachDeath
{
    abstract public class EnviromentalBase : MonoBehaviour
    {
        abstract public void OnTriggerEnter(Collider other);
    }
}