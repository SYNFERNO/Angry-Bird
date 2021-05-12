using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackBird : Bird
{
    public float force = 50;
    public float radius = 5;
    public float upliftModifer = 5;

    public void doExplosion(Vector3 position)
    {
        transform.localPosition = position;
        StartCoroutine(waitAndExplode());
    }

    private IEnumerator waitAndExplode()
    {
        yield return new WaitForFixedUpdate();

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius);

        foreach (Collider2D coll in colliders)
        {
            if (coll.GetComponent<Rigidbody2D>() && coll.name != "Bird")
            {
                AddExplosionForce(coll.GetComponent<Rigidbody2D>(), force, transform.position, radius, upliftModifer);
            }
        }
    }

    private void AddExplosionForce(Rigidbody2D body, float explosionForce, Vector3 explosionPosition, float explosionRadius, float upliftModifier = 0)
    {
        var dir = (body.transform.position - explosionPosition);
        float wearoff = 1 - (dir.magnitude / explosionRadius);
        Vector3 baseForce = dir.normalized * explosionForce * wearoff;
        baseForce.z = 0;
        body.AddForce(baseForce);

        if (upliftModifer != 0)
        {
            float upliftWearoff = 1 - upliftModifier / explosionRadius;
            Vector3 upliftForce = Vector2.up * explosionForce * upliftWearoff;
            upliftForce.z = 0;
            body.AddForce(upliftForce);
        }

    }

    void OnCollisionEnter2D(Collision2D _collision)
    {
        doExplosion(transform.position);
    }
}
