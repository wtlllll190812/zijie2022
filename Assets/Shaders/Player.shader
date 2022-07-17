Shader "Custom/Player"
{
    Properties
    {
        _MainTex("MainTex",2D) = "White"{}
        _lineWidth("lineWidth",Range(0,10)) = 1
        _lineColor("lineColor",Color)=(1,1,1,1)
    }
    SubShader
    {
        Cull off
        Tags
        {"RenderPipeline" = "UniversalRenderPipeline" "Queue"="AlphaTest" "RenderType"="TransparentCutout"}

        HLSLINCLUDE
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

        CBUFFER_START(UnityPerMaterial)         
            float4 _MainTex_ST;
            float _lineWidth;
            half4 _lineColor;
        CBUFFER_END
        TEXTURE2D(_MainTex);
        SAMPLER(sampler_MainTex);


        struct a2v
        {
            float4 vertex:POSITION;
            float3 normal:NORMAL;
            float2 uv:TEXCOORD;
        };
        struct v2f
        {
            float4 pos:SV_POSITION;
            float2 uv:TEXCOORD;
            float3 worldNormal:TEXCOORD1;
            float3 worldPos:TEXCOORD2;
        };

        v2f vert(a2v v)
        {
            v2f o;
            o.pos = TransformObjectToHClip(v.vertex);
            o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
            o.worldNormal=TransformObjectToWorldNormal(v.normal);
            o.uv = TRANSFORM_TEX(v.uv,_MainTex);
            return o;
        }

        ENDHLSL
        pass
        {
            Tags{ "LightMode" = "UniversalForward" }
            // Stencil
            // {
                //     Ref 0
                //     Comp Equal
            // }
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            float4 _MainTex_TexelSize;
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS //主光源阴影
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE //主光源层级阴影是否开启
            #pragma multi_compile _ _SHADOWS_SOFT //软阴影

            half4 frag(v2f i) :SV_TARGET
            {
                half4 _BaseColor=SAMPLE_TEXTURE2D(_MainTex,sampler_MainTex,i.uv);
                float2 up_uv = i.uv + float2(0,1) * _lineWidth * _MainTex_TexelSize.xy;
                float2 down_uv = i.uv + float2(0,-1) * _lineWidth * _MainTex_TexelSize.xy;
                float2 left_uv = i.uv + float2(-1,0) * _lineWidth * _MainTex_TexelSize.xy;
                float2 right_uv = i.uv + float2(1,0) * _lineWidth * _MainTex_TexelSize.xy;
                float w = SAMPLE_TEXTURE2D(_MainTex,sampler_MainTex,i.uv).a * SAMPLE_TEXTURE2D(_MainTex,sampler_MainTex,i.uv).a * SAMPLE_TEXTURE2D(_MainTex,sampler_MainTex,i.uv).a * SAMPLE_TEXTURE2D(_MainTex,sampler_MainTex,i.uv).a;
                
                
                clip(_BaseColor.a-0.1f);
                float4 SHADOW_COORDS = TransformWorldToShadowCoord(i.worldPos);
                Light mainLight = GetMainLight(SHADOW_COORDS);
                half shadow = mainLight.shadowAttenuation;
                

                half4 finalRGB=half4(_BaseColor.xxx*(shadow+0.1f),_BaseColor.a);
                return lerp(_lineColor,finalRGB,w);
            }
            ENDHLSL
        }
        pass {
            Tags{ "LightMode" = "ShadowCaster" }
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            
            float4 frag(v2f i) : SV_Target
            {
                half4 _BaseColor=SAMPLE_TEXTURE2D(_MainTex,sampler_MainTex,i.uv);
                clip(_BaseColor.a-0.000001f);
                return _BaseColor;
            }
            ENDHLSL
        }

        // pass
        // {
            //     Tags{ "LightMode" = "SRPDefaultUnlit" }
            //     Stencil
            //     {
                //         Ref 1
                //         Comp Equal
            //     }
            //     HLSLPROGRAM
            //     #pragma vertex vert
            //     #pragma fragment frag

            //     #pragma multi_compile _ _MAIN_LIGHT_SHADOWS //主光源阴影
            //     #pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE //主光源层级阴影是否开启
            //     #pragma multi_compile _ _SHADOWS_SOFT //软阴影


            //     half4 frag(v2f i) :SV_TARGET
            //     {
                //         half4 _BaseColor=SAMPLE_TEXTURE2D(_Tex2,sampler_Tex2,i.uv);
                //         clip(_BaseColor.a-0.1f);
                //         float4 SHADOW_COORDS = TransformWorldToShadowCoord(i.worldPos);
                //         Light mainLight = GetMainLight(SHADOW_COORDS);
                //         half shadow = mainLight.shadowAttenuation;
                
                //         half3 lightDir=normalize(_MainLightPosition.xyz);
                //         half3 worldNormal=normalize(i.worldNormal);
                //         half lambert=saturate(dot(worldNormal,lightDir));

                //         half4 finalRGB=_BaseColor*(lambert+0.3f);
                //         return _BaseColor;
            //     }
            //     ENDHLSL
        // }
        // UsePass "Universal Render Pipeline/Lit/ShadowCaster"
    }
}