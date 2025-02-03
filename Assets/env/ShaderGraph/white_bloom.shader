Shader "Custom/white_Bloom"
{
    Properties
    {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        [Header(Bloom Settings)]
        _BloomIntensity ("Intensity", Range(0, 5)) = 1
        _Threshold ("Threshold", Range(0, 1)) = 0.5
        _BloomColor ("Color", Color) = (.5,.5,.5,1)
        [Header(Flow Effect)]
        _MoveSpeed ("Speed", Range(0, 5)) = 0.8
        _WaveFrequency ("Wave Density", Range(0, 10)) = 3.0
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Transparent" }
        AlphaToMask On

        CGINCLUDE
        #include "UnityCG.cginc"
        
        struct appdata {
            float4 vertex : POSITION;
            float2 uv : TEXCOORD0;
        };

        struct v2f {
            float2 uv : TEXCOORD0;
            float4 vertex : SV_POSITION;
        };

        sampler2D _MainTex;
        float _BloomIntensity;
        float _Threshold;
        float4 _BloomColor;
        float _MoveSpeed;
        float _WaveFrequency;

        v2f vert (appdata v) {
            v2f o;
            o.vertex = UnityObjectToClipPos(v.vertex);
            o.uv = v.uv;
            return o;
        }
        ENDCG

        Pass
        {
            Name "BLOOM_MAIN"
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            half4 frag (v2f i) : SV_Target
            {
                // 基础纹理采样
                half4 col = tex2D(_MainTex, i.uv);
                
                // 流动效果计算
                float scroll = _Time.y * _MoveSpeed;
                float positionFactor = frac(i.uv.x + scroll);
                float wave = sin(positionFactor * _WaveFrequency * UNITY_PI * 2) * 0.5 + 0.5;
                
                // Bloom计算
                float luminance = Luminance(col.rgb);
                float dynamicThreshold = lerp(_Threshold, _Threshold * 0.3, wave);
                float bloomFactor = smoothstep(dynamicThreshold, dynamicThreshold + 0.2, luminance);
                
                // 最终混合
                col.rgb += _BloomColor.rgb * bloomFactor * _BloomIntensity;
                col.a = saturate(col.a); // 确保alpha在合理范围
                return col;
            }
            ENDCG
        }

        Pass
        {
            Name "BLUR"
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile _ BLUR_HORIZONTAL BLUR_VERTICAL

            float4 _MainTex_TexelSize;

            half4 frag (v2f i) : SV_Target
            {
                const float weights[5] = {0.227027, 0.1945946, 0.1216216, 0.054054, 0.016216};
                const float offsets[5] = {0.0, 1.3846153846, 3.2307692308, 5.0769230769, 7.0};

                half4 col = tex2D(_MainTex, i.uv) * weights[0];
                
                #ifdef BLUR_HORIZONTAL
                    for(int j = 1; j < 5; j++) {
                        float offset = _MainTex_TexelSize.x * offsets[j];
                        col += tex2D(_MainTex, i.uv + float2(offset, 0)) * weights[j];
                        col += tex2D(_MainTex, i.uv - float2(offset, 0)) * weights[j];
                    }
                #else
                    for(int j = 1; j < 5; j++) {
                        float offset = _MainTex_TexelSize.y * offsets[j];
                        col += tex2D(_MainTex, i.uv + float2(0, offset)) * weights[j];
                        col += tex2D(_MainTex, i.uv - float2(0, offset)) * weights[j];
                    }
                #endif

                return col;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}