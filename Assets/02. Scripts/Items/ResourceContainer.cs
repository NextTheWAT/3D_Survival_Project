using System.Collections;
using UnityEngine;

public class ResourceContainer : MonoBehaviour
{
    public void DeactivateResource(ResourceObject resource)
    {
        resource.gameObject.SetActive(false);
        StartCoroutine(RespawnCoroutine(resource));

    }
    private IEnumerator RespawnCoroutine(ResourceObject resource)
    {
        yield return new WaitForSeconds(resource.data.respawnTime);
        resource.Init();
        resource.gameObject.SetActive(true);
    }
}
