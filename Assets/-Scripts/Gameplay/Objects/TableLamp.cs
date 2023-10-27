using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public class TableLamp : MonoBehaviour, IController, ILight, IInteractable
{
    public UnityEvent<bool> Event { get; set; } = new();
    public ILight[] Lights { get; set; }
    public bool isOn { get; set; }
    [field: SerializeField] public Material OnMaterial { get; set; }
    [field: SerializeField] public Material OffMaterial { get; set; }
    [field: SerializeField] public bool DefaultState { get; set; } = true;
    public List<IController> Controllers { get; set; } = new();
    [field: SerializeField] public float InteractionRange { get; set; }
    private MeshRenderer _meshRenderer;
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
        if (InputManager.Instance != null)
            InputManager.Instance.Player_Interact.RemoveListener(InteractWrapper);
    }

    private void Start()
    {
        Controllers.Add(this);
        _meshRenderer = GetComponent<MeshRenderer>();
        isOn = DefaultState;
        ChangeMaterial();
    }
    public void ChangeMaterial()
    {
        if (OnMaterial == null) { Debug.LogWarning("No OnMaterial", this); return; }
        if (OffMaterial == null) { Debug.LogWarning("No OffMaterial", this); return; }
        if (isOn)
            _meshRenderer.material = OnMaterial;
        else
            _meshRenderer.material = OffMaterial;
    }

    public void InteractWrapper(bool var) => Evaluate();
    public virtual void Evaluate()
    {
        PlayerController player;
        if (!GameManager.Instance.TryGetPlayer(out player)) return;
        if (Vector3.Distance(player.transform.position, gameObject.transform.position) > InteractionRange) return;
        Interact();
    }

    public virtual void Interact() => Toggle();

    public void Toggle() { isOn = !isOn; ChangeMaterial(); Event.Invoke(isOn); }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(gameObject.transform.position, InteractionRange);
    }
}
