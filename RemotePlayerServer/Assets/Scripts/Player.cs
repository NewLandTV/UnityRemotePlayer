using UnityEngine;

public class Player : MonoBehaviour
{
    public void MoveByDirection(Vector3 direction)
    {
        transform.position += direction;
    }
}
