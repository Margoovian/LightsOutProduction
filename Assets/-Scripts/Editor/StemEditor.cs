using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Stem))]
public class StemEditor : Editor
{
    public override void OnInspectorGUI()
    {
        GUI.enabled = true;
        DrawDefaultInspector();

        Stem self = (Stem)target;

        float minPitchValue = self.RandomPitchRange.x;
        float maxPitchValue = self.RandomPitchRange.y;

        self.EnableRandomPitch = EditorHelperFunctions.EnableWrapper(self.EnableRandomPitch, () =>{ self.RandomPitchRange = EditorHelperFunctions.MinMaxSlider(self.RandomPitchRange, -3, 3, "Random Pitch Range"); });

        float minPanValue = self.RandomPanRange.x;
        float maxPanValue = self.RandomPanRange.y;

        self.EnableRandomPan = EditorHelperFunctions.EnableWrapper(self.EnableRandomPan, () => { self.RandomPanRange = EditorHelperFunctions.MinMaxSlider(self.RandomPanRange, -1, 1, "Random Pan Range"); });

        float minVolumeValue = self.RandomVolumeRange.x;
        float maxVolumeValue = self.RandomVolumeRange.y;

        self.EnableRandomVolume = EditorHelperFunctions.EnableWrapper(self.EnableRandomVolume, () => { self.RandomVolumeRange = EditorHelperFunctions.MinMaxSlider(self.RandomVolumeRange, 0, 1, "Random Volume Range"); });

    }
}
