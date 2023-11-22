using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public class GenericSwitch : MonoBehaviour, ISwitch, IInteractable
{
    [field: SerializeField] public float InteractionRange { get; set; }
    public UnityEvent<bool> Event { get; set; } = new();
    public ILight[] Lights { get => _lights; set => _lights = (GenericLight[])value; }
    [SerializeField] protected GenericLight[] _lights;

    private GameObject _uiHolder;
    private SpriteRenderer _spriteRenderer;

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
        _uiHolder = new();
        _uiHolder.name = "UI Holder";
        _uiHolder.transform.SetParent(transform);
        _uiHolder.transform.position = new Vector3(transform.position.x, transform.position.y + 0.4f, transform.position.z);
        _uiHolder.transform.localScale = new Vector2(0.05f, 0.05f);

        _spriteRenderer = (SpriteRenderer)_uiHolder.AddComponent(typeof(SpriteRenderer));
        _spriteRenderer.gameObject.transform.SetParent(_uiHolder.transform);
        _spriteRenderer.sprite = GameManager.Instance.InteractSprite;
        _spriteRenderer.flipX = true;
        _spriteRenderer.enabled = false;

        foreach (ILight light in Lights)
        {
            light?.Controllers.Add(this);
        }
    }

    public virtual void Evaluate()
    {
        PlayerController player;
        
        if (!GameManager.Instance.TryGetPlayer(out player)) 
            return;
        
        if (Vector3.Distance(player.transform.position, gameObject.transform.position) > InteractionRange) 
            return;

        Interact();
    }

    private void Update()
    {
        PlayerController player;
        
        if (!GameManager.Instance.TryGetPlayer(out player)) 
            return;

        bool isInSight = !(Vector3.Distance(player.transform.position, gameObject.transform.position) > InteractionRange);
        _spriteRenderer.enabled = isInSight;

        if (!isInSight)
            return;

        _spriteRenderer.transform.LookAt(GameManager.Instance.Camera.transform);
    }

    private void InteractWrapper(bool var) => Evaluate();
    public virtual void Interact()
    {
        foreach (GenericLight light in _lights)
            light?.Toggle();

        Event?.Invoke(true);
        AudioManager.Instance.PlaySFX("Switch");
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(gameObject.transform.position, InteractionRange);
    }
}
