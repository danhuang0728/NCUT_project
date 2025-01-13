Shader "Custom/EdgeDistortion"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _DistortAmount ("Distortion Amount", Range(0, 1)) = 0.1
        _EdgeThreshold ("Edge Threshold", Range(0, 1)) = 0.5
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            sampler2D _MainTex;
            float _DistortAmount;
            float _EdgeThreshold;
            float4 _MainTex_ST;

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

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            float edgeDetection(float2 uv, sampler2D tex)
            {
                // Sample the current pixel and its neighbors
                float center = tex2D(tex, uv).a;
                float left = tex2D(tex, uv + float2(-0.01, 0)).a;
                float right = tex2D(tex, uv + float2(0.01, 0)).a;
                float up = tex2D(tex, uv + float2(0, 0.01)).a;
                float down = tex2D(tex, uv + float2(0, -0.01)).a;

                // Calculate edge strength
                return step(_EdgeThreshold, abs(center - left + center - right + center - up + center - down));
            }

            float2 distortUV(float2 uv, float edge, float amount)
            {
                // Apply a sine wave distortion based on the edge mask
                float distortion = sin(uv.y * 20.0) * amount;
                return uv + edge * distortion * float2(1.0, 0.0);
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float edge = edgeDetection(i.uv, _MainTex);
                float2 distortedUV = distortUV(i.uv, edge, _DistortAmount);
                return tex2D(_MainTex, distortedUV);
            }
            ENDCG
        }
    }
}