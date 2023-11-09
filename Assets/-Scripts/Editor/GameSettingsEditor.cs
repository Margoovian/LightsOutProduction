using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GameSettings))]
public class GameSettingsEditor : Editor
{
    public static bool open = false;
    public override void OnInspectorGUI()
    {
        GameSettings settings = (GameSettings)target;

        GUILayout.BeginVertical();

        EditorGUILayout.LabelField("Gameplay");
        GUILayout.BeginHorizontal();
        {
            GUILayout.BeginVertical("GroupBox");
            {
                HelperFunctions.GroupWrapper("Difficulty", () =>
                {
                    HelperFunctions.LabeledWrapper("Difficulty", () =>
                    {
                        settings.Difficulty = (Difficulty)EditorGUILayout.EnumPopup(settings.Difficulty);
                    });

                    HelperFunctions.LabeledWrapper("Mode Name", () =>
                    {
                        settings.DifficultyName = EditorGUILayout.TextField(settings.DifficultyName);
                    });
                });

                HelperFunctions.GroupWrapper("Player", () =>
                {
                    HelperFunctions.LabeledWrapper("Base Speed", () =>
                    {
                        settings.PlayerBaseSpeed = EditorGUILayout.FloatField(settings.PlayerBaseSpeed);
                    });

                    HelperFunctions.LabeledWrapper("Fear Speed Multiplyer", () =>
                    {
                        settings.FearSpeedMultiplyer = EditorGUILayout.CurveField(settings.FearSpeedMultiplyer);
                    });

                    HelperFunctions.LabeledWrapper("Max Fear", () =>
                    {
                        settings.MaxFear = EditorGUILayout.FloatField(settings.MaxFear);
                    });

                    HelperFunctions.LabeledWrapper("Fear Tick Rate", () =>
                    {
                        settings.FearTickRate = EditorGUILayout.FloatField(settings.FearTickRate);
                    });
                    
                    HelperFunctions.LabeledWrapper("Fear Tick Amout", () =>
                    {
                        settings.FearTickAmount = EditorGUILayout.FloatField(settings.FearTickAmount);
                    });
                });

                HelperFunctions.GroupWrapper("Glow Toy", () => 
                {
                    HelperFunctions.LabeledWrapper("Glow Toy Fade In", () =>
                    {
                        settings.GlowToyFadeIn = EditorGUILayout.FloatField(settings.GlowToyFadeIn);
                    });

                    HelperFunctions.LabeledWrapper("Glow Toy Fade Modifier", () =>
                    {
                        settings.GlowToyFadeModifier = EditorGUILayout.FloatField(settings.GlowToyFadeModifier);
                    });

                    HelperFunctions.LabeledWrapper("Glow Toy Debounce Modifier", () =>
                    {
                        settings.GlowToyDebounceModifier = EditorGUILayout.FloatField(settings.GlowToyDebounceModifier);
                    });

                    HelperFunctions.LabeledWrapper("Glow Toy Max Battery", () =>
                    {
                        settings.GlowToyMaxBattery = EditorGUILayout.FloatField(settings.GlowToyMaxBattery);
                    });                    
                    
                    HelperFunctions.LabeledWrapper("Glow Toy Battery Rate", () =>
                    {
                        settings.GlowToyBatteryTickRate = EditorGUILayout.FloatField(settings.GlowToyBatteryTickRate);
                    });

                    HelperFunctions.LabeledWrapper("Glow Toy Battery Amount", () =>
                    {
                        settings.GlowToyBatteryTickAmount = EditorGUILayout.FloatField(settings.GlowToyBatteryTickAmount);
                    });
                });

            }
            GUILayout.EndVertical();
        }
        GUILayout.EndHorizontal();

        EditorGUILayout.LabelField("Miscellanous");
        GUILayout.BeginHorizontal();
        {
            GUILayout.BeginVertical("GroupBox");
            {
                HelperFunctions.LabeledWrapper("Enable Timer", () =>
                {
                    settings.EnableTimer = EditorGUILayout.Toggle(settings.EnableTimer);
                });
                
                HelperFunctions.LabeledWrapper("Enable Random Rooms", () =>
                {
                    settings.EnableRandomRooms = EditorGUILayout.Toggle(settings.EnableRandomRooms);
                });
            }
            GUILayout.EndVertical();


        }
        GUILayout.EndHorizontal();

        EditorGUILayout.LabelField("Cheats");
        GUILayout.BeginHorizontal();
        {
            GUILayout.BeginVertical("GroupBox");
            {
                HelperFunctions.LabeledWrapper("God Mode", () =>
                {
                    settings.EnableGodMode = EditorGUILayout.Toggle(settings.EnableGodMode);
                });

                HelperFunctions.LabeledWrapper("Speed Modifier", () =>
                {
                    settings.EnableSpeedModifier = EditorGUILayout.Toggle(settings.EnableSpeedModifier);
                    if (settings.EnableSpeedModifier)
                    {
                        GUILayout.BeginVertical();
                        settings.SpeedModifier = EditorGUILayout.FloatField(settings.SpeedModifier);
                        GUILayout.EndVertical();
                    }
                });
                
            }
            GUILayout.EndVertical();


        }
        GUILayout.EndHorizontal();

        GUILayout.EndVertical();
    }

}
