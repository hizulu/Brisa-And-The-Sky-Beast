Shader "Skybox/BlendCubemap"
{
    Properties
    {
        _Cubemap1("Day Skybox", CUBE) = "" {}
        _Cubemap2("Night Skybox", CUBE) = "" {}
        _Blend("Blend Factor", Range(0,1)) = 0
        _Rotation("Rotation", Range(0,360)) = 0
    }

        SubShader
        {
           Tags { "Queue" = "Background" }
           Cull Off ZWrite Off
           Fog { Mode Off }

           Pass
           {
               CGPROGRAM
               #pragma vertex vert
               #pragma fragment frag
               #include "UnityCG.cginc"

               samplerCUBE _Cubemap1;
               samplerCUBE _Cubemap2;
               float _Blend;
               float _Rotation;

               struct v2f
               {
                   float4 pos : SV_POSITION;
                   float3 texcoord : TEXCOORD0;
               };

               v2f vert(float4 vertex : POSITION)
               {
                   v2f o;
                   o.pos = UnityObjectToClipPos(vertex);
                   o.texcoord = mul(unity_ObjectToWorld, vertex).xyz;
                   return o;
               }

               fixed4 frag(v2f i) : SV_Target
               {
                   // Apply rotation
                   float3 dir = i.texcoord;
                   float theta = radians(_Rotation);
                   float cosTheta = cos(theta);
                   float sinTheta = sin(theta);
                   float3 rotatedDir = float3(
                       cosTheta * dir.x - sinTheta * dir.z,
                       dir.y,
                       sinTheta * dir.x + cosTheta * dir.z
                   );

                   fixed4 col1 = texCUBE(_Cubemap1, rotatedDir);
                   fixed4 col2 = texCUBE(_Cubemap2, rotatedDir);
                   return lerp(col1, col2, _Blend);
               }
               ENDCG
           }
        }
}
