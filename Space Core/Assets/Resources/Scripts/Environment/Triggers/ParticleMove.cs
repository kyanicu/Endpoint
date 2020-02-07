using UnityEngine;

public class ParticleMove : OnParticleEnd
{
    private float speed = 3f;

    public override void OnEnable()
    {
        base.OnEnable();
    }

    private void Update()
    {
        transform.position +=  speed * transform.right * Time.deltaTime;
    }
}
