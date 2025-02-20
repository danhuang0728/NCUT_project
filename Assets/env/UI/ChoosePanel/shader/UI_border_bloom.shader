Shader "UI/EpicBloom"
{
    Properties
    {
        _Color ("Main Color", Color) = (1,1,1,1)
        _MainTex ("Main Texture", 2D) = "white" {}
        _BloomIntensity ("Bloom Intensity", Range(0, 5)) = 1
        [Header(Stencil Settings)]
        [HideInInspector] _StencilComp ("Stencil Comparison", Float) = 8
        [HideInInspector] _Stencil ("Stencil ID", Float) = 0
        [HideInInspector] _StencilOp ("Stencil Operation", Float) = 0
        [HideInInspector] _StencilWriteMask ("Stencil Write Mask", Float) = 255
        [HideInInspector] _StencilReadMask ("Stencil Read Mask", Float) = 255
        [HideInInspector] _ColorMask ("Color Mask", Float) = 15
        [Header(Bloom Settings)]
        _Threshold ("Threshold", Range(0, 1)) = 0.5
        _BloomColor ("Color", Color) = (.5,.5,.5,1)
        [Header(Flow Effect)]
        _MoveSpeed ("Speed", Range(0, 5)) = 0.8
        _WaveFrequency ("Wave Density", Range(0, 10)) = 3.0
    }

    SubShader
    {
        Tags 
        { 
            "Queue"="Transparent+100"
            "RenderType"="Transparent" 
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Stencil
        {
            Ref [_Stencil]
            Comp [_StencilComp]
            Pass [_StencilOp]
            ReadMask [_StencilReadMask]
            WriteMask [_StencilWriteMask]
        }

        ColorMask [_ColorMask]

        Cull Off
        Lighting Off
        ZWrite Off
        ZTest LEqual
        Blend SrcAlpha OneMinusSrcAlpha

        CGINCLUDE
        #include "UnityCG.cginc"
        
        struct appdata {
            float4 vertex : POSITION;
            float2 uv : TEXCOORD0;
            fixed4 color : COLOR;
        };

        struct v2f {
            float2 uv : TEXCOORD0;
            float4 vertex : SV_POSITION;
            fixed4 color : COLOR;
        };

        sampler2D _MainTex;
        float4 _MainTex_ST;
        fixed4 _Color;
        float _BloomIntensity;
        float _Threshold;
        float4 _BloomColor;
        float _MoveSpeed;
        float _WaveFrequency;

        v2f vert (appdata v) {
            v2f o;
            o.vertex = UnityObjectToClipPos(v.vertex);
            o.uv = TRANSFORM_TEX(v.uv, _MainTex);
            o.color = v.color * _Color;
            return o;
        }
        ENDCG

        Pass
        {
            Name "BLOOM_MAIN"
            Blend SrcAlpha OneMinusSrcAlpha  // 主通道使用標準UI混合
            BlendOp Add
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            half4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv) * i.color;
                
                // 流動效果計算
                float scroll = _Time.y * _MoveSpeed;
                float positionFactor = frac(i.uv.x + scroll);
                float wave = sin(positionFactor * _WaveFrequency * UNITY_PI * 2) * 0.5 + 0.5;
                
                // Bloom計算
                float luminance = Luminance(col.rgb);
                float dynamicThreshold = lerp(_Threshold, _Threshold * 0.3, wave);
                float bloomFactor = smoothstep(dynamicThreshold, dynamicThreshold + 0.2, luminance);
                
                // 最終混合 (使用加法混合處理Bloom效果)
                col.rgb += _BloomColor.rgb * bloomFactor * _BloomIntensity;
                col.rgb *= _BloomIntensity;  // 整體強度控制
                
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
    FallBack "UI/Default"
}