Shader "Custom/ToonShaderWithHardShadows"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Main Color", COLOR) = (1,1,1,1)
        _Brightness ("Brightness", Range(0,1)) = 0.3
        _Strength ("Shadow Strength", Range(0,1)) = 0.5
        _Cutoff ("Shadow Cutoff", Range(0.0, 1.0)) = 0.5
        _ShadowColor ("Shadow Color", COLOR) = (0,0,0,1)
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Geometry" }
        LOD 100

        CGPROGRAM
        #pragma surface surf ToonShading fullforwardshadows
        #pragma lighting ToonyShading 

        sampler2D _MainTex;
        float4 _Color;
        float _Brightness;
        float _Strength;
        float _Cutoff;
        float4 _ShadowColor;

        struct Input
        {
            float2 uv_MainTex;
        };

        void surf(Input IN, inout SurfaceOutput o)
        {
            fixed4 col = tex2D(_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = col.rgb;
            o.Alpha = col.a;
        }

        half4 LightingToonShading(SurfaceOutput s, half3 lightDir, half3 viewDir, half atten)
        {
            half NdotL = dot(s.Normal, lightDir);
            half shadow = step(_Cutoff, NdotL) * _Strength + _Brightness;
            half3 color = lerp(_ShadowColor.rgb, _LightColor0.rgb * s.Albedo, shadow);
            return half4(color, s.Alpha);
        }

        #pragma surface surf ToonShading fullforwardshadows
        #pragma shader_feature _SHADOWS_ON
        #pragma shader_feature _SHADOWS_CLIP
        #pragma shader_feature _SHADOWS_SHADOWMASK
        #pragma target 3.0
        ENDCG
    }
    Fallback "Diffuse"
}