Shader "Custom/StencilTest"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _testId ("_testId", int) = 1
    }
    SubShader
    {
        Pass
        {
            Cull off
            ZWrite off
            ZTest off
            ColorMask 0
            Tags {"Queue"="AlphaTest" "IgnoreProjector"="True" "RenderType"="TransparentCutout"}
            Stencil
            {
                Ref [_testId]
                Comp always
                pass replace
            }
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                return fixed4(0,0,0,0);
            }
            ENDCG
        }
    }
}
