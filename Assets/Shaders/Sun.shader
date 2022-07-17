Shader "Custom/sun"
{
    Properties
    {
        _MainTex("MainTex",2D) = "White"{}
        _FirColor("FirColor",Color) = (1,1,1,1)
        _SecColor("SecColor",Color) = (1,1,1,1)
    }
    SubShader
    {
        Cull off
        Tags
        {"RenderPipeline" = "UniversalRenderPipeline" "RenderType" = "Opaque"}

        HLSLINCLUDE
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

        CBUFFER_START(UnityPerMaterial)         
            float4 _MainTex_ST;
            half4 _FirColor;
            half4 _SecColor;
        CBUFFER_END
        TEXTURE2D(_MainTex);
        SAMPLER(sampler_MainTex);


        struct a2v
        {
            float4 vertex:POSITION;
        };
        struct v2f
        {
            float4 pos:SV_POSITION;
        };

        v2f vert(a2v v)
        {
            v2f o;
            o.pos = TransformObjectToHClip(v.vertex);
            return o;
        }

        ENDHLSL
        pass
        {
            Tags{ "LightMode" = "UniversalForward" }
            Stencil
            {
                Ref 2
                Comp NotEqual
            }
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            half4 frag(v2f i) :SV_TARGET
            {
                return _FirColor;
            }
            ENDHLSL
        }
        pass
        {
            Stencil
            {
                Ref 2
                Comp Equal
            }
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            half4 frag(v2f i) :SV_TARGET
            {
                return _SecColor;
            }
            ENDHLSL
        }
        UsePass "Universal Render Pipeline/Lit/ShadowCaster"
    }
}