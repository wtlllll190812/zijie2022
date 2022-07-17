Shader "Unlit/Outline"
{
    Properties
    {
        _OutlineColor("Outline Color",Color) = (0,0,0,1)
        _Outline ("Outline",Range(0,1)) = 0.1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "RenderPipeline"="UniversalRenderPipeline"}
        LOD 100
        Cull Front

        pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            
            
            CBUFFER_START(UnityPerMaterial)         
                float4 _OutlineColor;
                float _Outline;
            CBUFFER_END
            
            struct Atributes
            {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
            };
            Varyings vert (Atributes v)
            {
                Varyings o = (Varyings)0;
                o.positionCS = TransformObjectToHClip(v.positionOS);
                //法线转到屏幕坐标
                float3 vNormal = normalize(mul((float3x3)UNITY_MATRIX_IT_MV, v.normalOS));
                //再转到裁切坐标
                float2 projPos = normalize(mul((float2x2)UNITY_MATRIX_P,vNormal.xy));
                
                o.positionCS.xy += projPos * _Outline * 0.1;
                return o;
            }

            real4 frag (Varyings i) : SV_Target
            {
                return _OutlineColor;
            }
            ENDHLSL
        }
    }
}
