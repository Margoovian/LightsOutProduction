Shader "Unlit/SH_U_CustomStencil"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
    }
        SubShader
    {
        Tags
        {
            "RenderType" = "Transparent"
            "Queue" = "Transparent"
            "RenderPipeline" = "UniversalPipeline"
        }

        LOD 100

        Pass
        {
            Ztest Greater
            Zwrite off
            Cull Off
            ColorMask 0

            Stencil
            {
                Comp Always
                Ref 1
                Pass Replace
            }
        }
    }
}