Shader "Custom/StencilLight Color"
{
    Properties
    {
        [HDR] _Color("Color",Color) = (1,1,1,1)
        _Emission("Emission", float) = 0
    }
        HLSLINCLUDE

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"        
        struct appdata
    {
        float4 vertex : POSITION;
    };

    struct v2f
    {
        float4 vertex : SV_POSITION;
    };



    v2f vert(appdata v)
    {
        v2f o;
        o.vertex = TransformObjectToHClip(v.vertex.xyz);
        return o;
    }

    CBUFFER_START(UnityPerMaterial)
        float4 _Color;
        float _Emission;
    CBUFFER_END

        float4 frag(v2f i) : SV_Target
    {
        return _Color * _Color.a* _Emission;
    }
        ENDHLSL

        SubShader
    {
        Tags{ "Queue" = "Transparent" "RenderType" = "Transparent" }
            Pass
        {
            Tags
            {
                "RenderType" = "Transparent"
                "RenderPipeline" = "UniversalPipeline"
            }
            Zwrite off
            Ztest Lequal
            Cull Back
            Blend DstColor One

            Stencil
            {
                comp equal
                ref 1
                pass zero
                fail zero
                zfail zero
            }
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag         
            ENDHLSL
        }
            Pass
        {
            Tags
            {
                "RenderPipeline" = "UniversalPipeline"
                "LightMode" = "UniversalForward"
            }
            ZTest always
            ZWrite on
            Cull Front
            Blend DstColor One

            Stencil
            {
                Ref 1
                Comp equal
                Pass zero
            }
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            ENDHLSL
        }
    }
}
