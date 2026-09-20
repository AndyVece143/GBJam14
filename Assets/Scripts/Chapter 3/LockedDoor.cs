using UnityEngine;

public class LockedDoor : MonoBehaviour
{
    public void OpenTheDoor()
    {
        Destroy(gameObject);
    }
}
