using UnityEngine;
namespace Mirror.MachDeath
{
    abstract public class EnviromentalBase : NetworkBehaviour
    {
        abstract public void OnTriggerEnter(Collider other);
    }
}