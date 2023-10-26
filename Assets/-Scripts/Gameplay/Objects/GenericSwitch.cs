using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public class GenericSwitch : MonoBehaviour, ISwitch
{
    [field: SerializeField] public float InteractionRange { get; set; }
    public UnityEvent<bool> Event { get; set; }
    public ILight[] Lights { get => _lights; set => _lights = (GenericLight[])value; }
    [SerializeField] private GenericLight[] _lights;

    private void OnEnable()
    {
        HelperFunctions.WaitForTask(WaitForManagers(), () =>
        {
            InputManager.Instance.Player_Interact.AddListener(InteractWrapper);
        });
    }
    private async Task WaitForManagers()
    {
        while (InputManager.Instance == null)
            await Task.Yield();
    }

    private void OnDisable()
    {
        if(InputManager.Instance != null)
            InputManager.Instance.Player_Interact.RemoveListener(InteractWrapper);
    }

    private void Start()
    {
        foreach (ILight light in Lights)
        {
            light?.Controllers.Add(this);
        }
    }

    public virtual void Evaluate()
    {
        PlayerController player;
        if (!GameManager.Instance.TryGetPlayer(out player)) return;
        if (Vector3.Distance(player.transform.position, gameObject.transform.position) > InteractionRange) return;
        Interact();

    }

    private void InteractWrapper(bool var) => Evaluate();
    public virtual void Interact()
    {
        foreach (GenericLight light in _lights)
        {
            light?.Toggle();
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(gameObject.transform.position, InteractionRange);
    }
}
