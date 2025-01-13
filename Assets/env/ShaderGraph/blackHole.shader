Shader "Custom/BlackHole"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _DistortionStrength ("Distortion Strength", Range(0, 1)) = 0.5
        _FresnelColor ("Fresnel Color", Color) = (0.5, 0, 1, 1)
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 200
        
        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            
            struct appdata_t {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _DistortionStrength;
            float4 _FresnelColor;

            v2f vert (appdata_t v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target {
                float2 uv = i.uv - 0.5; // Center the UVs
                float dist = length(uv); // Calculate distance from center
                float distortion = smoothstep(0.4, 0.6, dist) * _DistortionStrength;
                uv += normalize(uv) * distortion;

                float fresnel = pow(1.0 - dist, 3.0); // Fresnel effect
                fixed4 texColor = tex2D(_MainTex, uv + 0.5);
                fixed4 finalColor = texColor + _FresnelColor * fresnel;

                finalColor.a = smoothstep(0.45, 0.5, 1.0 - dist); // Black hole fade-out
                return finalColor;
            }
            ENDCG
        }
    }
}
