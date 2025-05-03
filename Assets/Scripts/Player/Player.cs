using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] GameObject ball;
    [SerializeField] bool canShoot = false;

    void Update()
    {
        if (ball != null && canShoot)
        {
            
        }
    }

}
