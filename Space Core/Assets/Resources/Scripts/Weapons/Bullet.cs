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
    public float Velocity { get; set; }
    public BulletSource Source { get; set; }
    private float startX;
    private float lowRange;
    private float highRange;

    /// <summary>
    /// Initialize start x to the base x position of the bullet
    /// </summary>
    public void Start()
    {
        startX = transform.position.x;
        highRange = startX + Range;
        lowRange = startX - Range;
    }

    /// <summary>
    /// Move the bullet forward until it is out of range
    /// </summary>
    public void Update()
    {
        //if we have travelled outside the range, destroy the bullet
        if (transform.position.x > highRange || transform.position.x < lowRange)
        {
            Destroy(gameObject);
        }

        transform.position += (transform.right * Velocity * Time.deltaTime);
    }
}
