Shader "Custom/SquareRoundedFadeGray"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _FadeAmount ("Fade Amount", Range(0,1)) = 0.5
        _Roundness ("Roundness", Range(0,1)) = 0.1
        _GrayAmount ("Gray Amount", Range(0,1)) = 0.5
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" }
        LOD 100

        Pass
        {
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float2 texcoord : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _FadeAmount;
            float _Roundness;
            float _GrayAmount;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
                return o;
            }

            half4 frag (v2f i) : SV_Target
            {
                half4 color = tex2D(_MainTex, i.texcoord);

                float2 uv = i.texcoord - 0.5;
                float2 absUV = abs(uv);
                float squareDist = max(absUV.x, absUV.y);
                float circleDist = length(uv);
                float dist = lerp(squareDist, circleDist, _Roundness);
                float alpha = smoothstep(0.5, 0.5 - _FadeAmount, dist);

                // Create gray color and mix based on gray amount
                half3 grayColor = half3(0.5, 0.5, 0.5);
                color.rgb = lerp(color.rgb, grayColor, _GrayAmount);

                color.a *= alpha;

                return color;
            }
            ENDCG
        }
    }
}
