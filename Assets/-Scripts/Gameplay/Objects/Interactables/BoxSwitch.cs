public class BoxSwitch : GenericSwitch
{
    // What was this used for exactly?
    // I reckon it could be removed
    // -- Corey A.
    public new bool isOn { get; set; } = true;

    public override void Interact()
    {
        isOn = !isOn;
        base.Interact();
    }
}
