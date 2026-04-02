    Shader "Custom/UnlitAlphaThresholdNew"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _Threshold("Threshold", Range(0, 1)) = 0.5
        _DesiredColor("Desired Color", Color) = (0,0,0,1)
    }

    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100

        Pass
        {
            Name "UnlitTransparent"
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            Cull Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _Threshold;
            float4 _DesiredColor;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);

                // Discard black pixels
                if (col.r < 0.1 && col.g < 0.1 && col.b < 0.1)
                {
                    discard;
                }

                // Apply threshold
                float alpha = i.uv.y < _Threshold ? 1 : 0;
                col.a = alpha;
                return col;
            }
            ENDCG
        }
    }

    FallBack "Diffuse"
}