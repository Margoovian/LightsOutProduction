void ToonShading_float(in float3 Normal, in float ToonRampSmoothness, in float3 ClipSpacePos, in float3 WorldPos, in float4 ToonRampTinting, in float ToonRampOffset, in float ToonRampOffsetPoint, in float Ambient, out float3 ToonRampOutput, out float3 Direction)
{
    // set the shader graph node previews
#ifdef SHADERGRAPH_PREVIEW
        ToonRampOutput = float3(0.5,0.5,0);
        Direction = float3(0.5,0.5,0);
#else
 
        // grab the shadow coordinates
#if SHADOWS_SCREEN
            half4 shadowCoord = ComputeScreenPos(ClipSpacePos);
#else
    half4 shadowCoord = TransformWorldToShadowCoord(WorldPos);
#endif 
 
        // grab the main light
#if _MAIN_LIGHT_SHADOWS_CASCADE || _MAIN_LIGHT_SHADOWS
    Light light = GetMainLight(shadowCoord);
#else
    Light light = GetMainLight();
#endif
 
        // dot product for toonramp
    half d = dot(Normal, light.direction) * 0.5 + 0.5;
        
        // toonramp in a smoothstep
    half toonRamp = smoothstep(ToonRampOffset, ToonRampOffset + ToonRampSmoothness, d);
    
    float3 extraLights;
    // Get the amount of extra lights
    int pixelLightCount = GetAdditionalLightsCount();
    //Loop for every extra light in scene
    for (int j = 0; j < pixelLightCount; ++j)
    {
        // grab point light
        //remover half4 if error occurs
        Light alight = GetAdditionalLight(j, WorldPos, half4(1, 1, 1, 1));
        // grab light, shadow and color info
        float3 attenuatedLightColor = alight.color * (alight.distanceAttenuation * alight.shadowAttenuation);
        //dot product
        half d = dot(Normal, alight.direction) * 0.5 + 0.5;
        //toonramp smoothsetp
        half toonRampExtra = smoothstep(ToonRampOffsetPoint, ToonRampOffsetPoint + ToonRampSmoothness, d);
        //addition
        extraLights += (attenuatedLightColor * toonRampExtra);

    }
        // multiply with shadows;
    toonRamp *= light.shadowAttenuation;
        // add in lights and extra tinting
    ToonRampOutput = light.color * (toonRamp + ToonRampTinting) + Ambient;
    // add extra lights
    ToonRampOutput += extraLights;
        // output direction for rimlight
    Direction = light.direction;
    
    #if MAIN_LIGHT
        Direction = normalize(light.direction);
    #else
        Direction = float3(0.5, 0.5, 0);
    #endif
    
#endif
 
}