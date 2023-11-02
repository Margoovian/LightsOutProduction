using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Candle : TableLamp
{
    public override void Evaluate() { if (isOn) base.Evaluate(); }
}
