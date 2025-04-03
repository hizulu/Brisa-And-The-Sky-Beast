Shader "UI/BlurShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _BlurSize ("Blur Amount", Range(0.0, 10.0)) = 1.0
        _Color ("Tint Color", Color) = (1,1,1,1) // Nuevo: Color ajustable
        _Brightness ("Brightness", Range(0.0, 5.0)) = 1.0 // Nuevo: Control de brillo
    }

    SubShader
    {
        Tags { "RenderType"="Transparent" }
        Cull Off ZWrite Off ZTest Always Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata_t
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
            float4 _MainTex_TexelSize;
            float _BlurSize;
            float4 _Color; // Nuevo: Variable para el color
            float _Brightness; // Nuevo: Variable para el brillo

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv;
                float2 blurSize = _MainTex_TexelSize.xy * _BlurSize;

                fixed4 sum = fixed4(0,0,0,0);

                // Kernel de desenfoque gaussiano 3x3
                sum += tex2D(_MainTex, uv + blurSize * float2(-1, -1)) * 0.0625;
                sum += tex2D(_MainTex, uv + blurSize * float2( 0, -1)) * 0.125;
                sum += tex2D(_MainTex, uv + blurSize * float2( 1, -1)) * 0.0625;
                
                sum += tex2D(_MainTex, uv + blurSize * float2(-1,  0)) * 0.125;
                sum += tex2D(_MainTex, uv + blurSize * float2( 0,  0)) * 0.25;
                sum += tex2D(_MainTex, uv + blurSize * float2( 1,  0)) * 0.125;
                
                sum += tex2D(_MainTex, uv + blurSize * float2(-1,  1)) * 0.0625;
                sum += tex2D(_MainTex, uv + blurSize * float2( 0,  1)) * 0.125;
                sum += tex2D(_MainTex, uv + blurSize * float2( 1,  1)) * 0.0625;

                // Aplicar color y brillo (modificación final)
                sum.rgb *= _Color.rgb * _Brightness; // Multiplica por el color y brillo
                sum.a *= _Color.a; // Mantiene el alpha original multiplicado por el alpha del color

                return sum;
            }
            ENDHLSL
        }
    }
}