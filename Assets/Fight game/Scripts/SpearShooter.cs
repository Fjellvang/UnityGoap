using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearShooter : MonoBehaviour
{
    public GameObject spearPrefab;

    public Transform spawnPosition;

    public float shootSpeed = 20;

    public int spearCount = 5;
    // Spawn a spear and shoot it in the forward direction
    public void Shoot()
    {
        if (spearCount > 0)
        {
            spearCount--;
            GameObject spear = Instantiate(spearPrefab, spawnPosition.position, transform.rotation);
            spear.GetComponent<Rigidbody>().velocity = transform.forward * shootSpeed;
        }
    }

    //TODO: reconsider if this logic should be here
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Spear"))
        {
            spearCount++;
            StartCoroutine(DestroyAfter(collision.gameObject, 1));
        }
    }

    // Destroy after x seconds
    IEnumerator DestroyAfter(GameObject obj, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Destroy(obj);
    }
}
