using UnityEngine;

/// <summary>
/// Class that holds all information the bullet may need
/// </summary>
public class Bullet : MonoBehaviour
{
    //enum source to let objects know who fired the bullet
    public enum BulletSource { Player, Enemy }

    public int Damage { get; set; }
    public float Range { get; set; }
    public float Movement = 0.4f;
    public BulletSource Source { get; set; }
    private float startX;

    /// <summary>
    /// Initialize start x to the base x position of the bullet
    /// </summary>
    public void Start()
    {
        startX = transform.position.x;
    }

    /// <summary>
    /// Move the bullet forward until it is out of range
    /// </summary>
    public void Update()
    {
        //if we have travelled outside the range, destroy the bullet
        if (transform.position.x + startX > startX + Range || transform.position.x + startX < startX - Range)
        {
            Destroy(gameObject);
        }

        transform.position += (transform.right * Movement);
    }
}
