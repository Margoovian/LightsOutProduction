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
                EditorHelperFunctions.GroupWrapper("Difficulty", () =>
                {
                    EditorHelperFunctions.LabeledWrapper("Difficulty", () =>
                    {
                        settings.Difficulty = (Difficulty)EditorGUILayout.EnumPopup(settings.Difficulty);
                    });

                    EditorHelperFunctions.LabeledWrapper("Mode Name", () =>
                    {
                        settings.DifficultyName = EditorGUILayout.TextField(settings.DifficultyName);
                    });
                });

                EditorHelperFunctions.GroupWrapper("Player", () =>
                {
                    EditorHelperFunctions.LabeledWrapper("Base Speed", () =>
                    {
                        settings.PlayerBaseSpeed = EditorGUILayout.FloatField(settings.PlayerBaseSpeed);
                    });


                    EditorHelperFunctions.LabeledWrapper("Fear Speed Multiplyer", () =>
                    {
                        settings.FearSpeedMultiplyer = EditorGUILayout.CurveField(settings.FearSpeedMultiplyer);
                    });

                    EditorHelperFunctions.LabeledWrapper("Max Fear", () =>
                    {
                        settings.MaxFear = EditorGUILayout.FloatField(settings.MaxFear);
                    });

                    EditorHelperFunctions.LabeledWrapper("Fear Tick Rate", () =>
                    {
                        settings.FearTickRate = EditorGUILayout.FloatField(settings.FearTickRate);
                    });
                    
                    EditorHelperFunctions.LabeledWrapper("Fear Tick Amout", () =>
                    {
                        settings.FearTickAmount = EditorGUILayout.FloatField(settings.FearTickAmount);
                    });
                });

                EditorHelperFunctions.GroupWrapper("End Sequence", () =>
                {
                    EditorHelperFunctions.LabeledWrapper("Fear Wall Speed", () =>
                    {
                        settings.FearWallSpeed = EditorGUILayout.FloatField(settings.FearWallSpeed);
                    });

                    EditorHelperFunctions.LabeledWrapper("Fear Wall Tick", () =>
                    {
                        settings.FearWallTick = EditorGUILayout.FloatField(settings.FearWallTick);
                    });
                });

                EditorHelperFunctions.GroupWrapper("Glow Toy", () => 
                {
                    EditorHelperFunctions.LabeledWrapper("Glow Toy Fade In", () =>
                    {
                        settings.GlowToyFadeIn = EditorGUILayout.FloatField(settings.GlowToyFadeIn);
                    });

                    EditorHelperFunctions.LabeledWrapper("Glow Toy Fade Modifier", () =>
                    {
                        settings.GlowToyFadeModifier = EditorGUILayout.FloatField(settings.GlowToyFadeModifier);
                    });

                    EditorHelperFunctions.LabeledWrapper("Glow Toy Debounce Modifier", () =>
                    {
                        settings.GlowToyDebounceModifier = EditorGUILayout.FloatField(settings.GlowToyDebounceModifier);
                    });

                    EditorHelperFunctions.LabeledWrapper("Glow Toy Max Battery", () =>
                    {
                        settings.GlowToyMaxBattery = EditorGUILayout.FloatField(settings.GlowToyMaxBattery);
                    });                    
                    
                    EditorHelperFunctions.LabeledWrapper("Glow Toy Battery Rate", () =>
                    {
                        settings.GlowToyBatteryTickRate = EditorGUILayout.FloatField(settings.GlowToyBatteryTickRate);
                    });

                    EditorHelperFunctions.LabeledWrapper("Glow Toy Battery Amount", () =>
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
                EditorHelperFunctions.LabeledWrapper("Enable Timer", () =>
                {
                    settings.EnableTimer = EditorGUILayout.Toggle(settings.EnableTimer);
                });
                
                EditorHelperFunctions.LabeledWrapper("Enable Random Rooms", () =>
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
                EditorHelperFunctions.LabeledWrapper("God Mode", () =>
                {
                    settings.EnableGodMode = EditorGUILayout.Toggle(settings.EnableGodMode);
                });

                EditorHelperFunctions.LabeledWrapper("Speed Modifier", () =>
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
