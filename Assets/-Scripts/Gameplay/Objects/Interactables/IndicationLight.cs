public class IndicationLight : GenericLight
{
    private new void Start() { isIndication = true; base.Start(); } 
    
    internal override void EditLightCount(){}
}
