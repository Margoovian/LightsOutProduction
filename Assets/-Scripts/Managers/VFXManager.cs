using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXManager : Manager<VFXManager>
{
    public Dictionary<string, VFX> VFXs;
    [field: SerializeField] public string[] VFXPaths { get; set; }
    protected override void Initialize()
    {
        VFXs = new();
        List<VFX> vfxs = new List<VFX>();

        foreach (string path in VFXPaths)
        {
            vfxs.AddRange(Resources.LoadAll<VFX>(path));
        }

        foreach (VFX vfx in vfxs)
        {
            if (vfx.FriendlyName == "" || VFXs.ContainsKey(vfx.FriendlyName))
                VFXs.Add(vfx.ParticalPrefab.name, vfx);
            else
                VFXs.Add(vfx.FriendlyName, vfx);
        }
    }

    internal void PlayAttached(string vfxName, GameObject attachment)
    {
        VFX vfx;
        bool exists = VFXs.TryGetValue(vfxName, out vfx);
        if (exists) vfx.Play(attachment);
        else return;
    }
    internal void Play(string vfxName, Transform position) => Play(vfxName, position.position);
    internal void Play(string vfxName, Vector3 position)
    {
        VFX vfx;
        bool exists = VFXs.TryGetValue(vfxName, out vfx);
        if (exists) vfx.Play(position);
        else return;
    }
}
