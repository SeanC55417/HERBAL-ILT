Shader "Custom/TwoSidedCutoutShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Cutoff ("Alpha Cutoff", Range(0,1)) = 0.5
    }
    SubShader
    {
        Tags { "Queue" = "AlphaTest" }
        LOD 200

        Cull Off // Render both sides of the geometry

        CGPROGRAM
        #pragma surface surf Standard alpha:fade clip:1

        sampler2D _MainTex;
        float _Cutoff;

        struct Input
        {
            float2 uv_MainTex;
        };

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
            o.Albedo = c.rgb;
            o.Alpha = c.a;

            // Alpha clipping
            clip(c.a - _Cutoff);
        }
        ENDCG
    }
    FallBack "Diffuse"
}
