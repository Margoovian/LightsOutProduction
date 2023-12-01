using System.Threading.Tasks;

using UnityEngine;
using UnityEngine.Events;

public class LeverController : MonoBehaviour, IController
{
    [field: SerializeField] public LeverSwitch[] Switches {get;set;}
    [field: SerializeField] public int TimeInMS { get; set; }
    public UnityEvent<bool> Event { get; set; }
    public ILight[] Lights { get => _lights; set => _lights = (GenericLight[])value; }
    [SerializeField] private GenericLight[] _lights;

    [field: SerializeField] public string CompleteSound { get; set; } = "LeverComplete";
    [field: SerializeField] public string TimerSound { get; set; } = "LeverTimer";

    private Task _timer = null;
    private bool _complete = false;

    // For audio
    private bool isPlayingAudio = false;
    private float timerAudio = 0;

    private void Start()
    {
        timerAudio = 0;

        foreach (ILight light in Lights)
        {
            light?.Controllers.Add(this);
        }
        foreach (LeverSwitch lever in Switches)
        {
            lever.Event?.AddListener(EvaluateWrapper);
        }
    }
    private void OnDestroy()
    {
        foreach (LeverSwitch lever in Switches)
        {
            lever.Event?.RemoveListener(EvaluateWrapper);
        }
    }

    private void StartTimer()
    {
        bool timerIsRunning;

        if (_timer != null)
            timerIsRunning = !_timer.IsCompleted;
        else
        {
            if (!isPlayingAudio)
                isPlayingAudio = true;

            _timer = HelperFunctions.Timer(TimeInMS);
            AudioManager.Instance.Play(TimerSound);

            return;
        }

        _complete = !ValidateSwitches();

        if (!timerIsRunning && !_complete)
        {
            ResetSwitches();
            return;
        }

        if (timerIsRunning && _complete)
        {
            AudioManager.Instance.Stop(TimerSound);
            AudioManager.Instance.Play(CompleteSound);
        }

        return;
    }

    private bool ValidateSwitches()
    {
        bool eval = true;

        foreach (LeverSwitch lever in Switches)
        {
            eval = lever.isOn;
            if (eval)
                return eval;
        }

        return eval;
    }
    
    private void Update()
    {
        if (_timer == null) return;
        if(_timer.IsCompleted && !_complete) ResetSwitches();

        if (isPlayingAudio)
        {
            timerAudio += Time.unscaledDeltaTime;

            if (timerAudio >= 5)
            {
                isPlayingAudio = false;
                timerAudio = 0;
            }
        }
    }

    private void ResetSwitches()
    {
        foreach(LeverSwitch lever in Switches)
            lever?.ResetSwitch();

        AudioManager.Instance.PlaySFX("FailedSound");
        _timer = null;
    }

    private void EvaluateWrapper(bool var) => Evaluate();
    public void Evaluate()
    {
        if (_complete) 
            return;

        StartTimer();
        bool eval = ValidateSwitches();

        _complete = !eval;
        FlipLights(!eval);
    }

    private void FlipLights(bool state)
    {
        foreach(GenericLight light in _lights)
        {
            light.Toggle();
        }
    }
}
