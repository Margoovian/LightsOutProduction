using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "VFX",menuName = "Visual/VFX")]
public class VFX : ScriptableObject
{
    [field: SerializeField] public string FriendlyName { get; set; }
    [field: SerializeField] public ParticleSystem ParticalPrefab { get; set; }
    [field: SerializeField] public bool DestroyAfter { get; set; }
    [field: SerializeField] public float AliveTime { get; set; }


    internal GameObject Play(Transform position) => Play(position.position);
    internal GameObject Play(Vector3 position) {
        GameObject obj = Instantiate(ParticalPrefab.gameObject);
        obj.transform.position = position;
        if (DestroyAfter)
            Destroy(obj, AliveTime);
        obj.GetComponent<ParticleSystem>().Play();
        return obj;
    }
    internal GameObject Play(GameObject parent)
    {
        GameObject obj = Play(parent.transform);
        obj.transform.SetParent(parent.transform);
        return obj;
    }

}
