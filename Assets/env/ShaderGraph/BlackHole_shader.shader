Shader "Custom/BlackHoleWithAdjustableEdge"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {} // 可选背景贴图
        _HoleRadius ("Hole Radius", Float) = 0.45
        _CoreColorInner ("Core Inner Color", Color) = (0, 0, 1, 1) // 核心内层颜色
        _CoreColorOuter ("Core Outer Color", Color) = (0, 1, 0, 1) // 核心外层颜色
        _GlowWidth ("Glow Width", Float) = 0.2
        _GlowColor ("Glow Color", Color) = (1, 0.5, 0, 1)
        _OuterGlowColor ("Outer Glow Color", Color) = (1, 1, 1, 1)
        _EdgeColor ("Edge Color", Color) = (1, 1, 1, 1) // 白边颜色
        _EdgeWidth ("Edge Width", Float) = 0.02 // 白边宽度
        _BrightnessFalloff ("Brightness Falloff", Float) = 2.0
        _LightRayIntensity ("Light Ray Intensity", Float) = 1.5
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            sampler2D _MainTex;
            float _HoleRadius;
            float _GlowWidth;
            float _EdgeWidth;
            float _BrightnessFalloff;
            float _LightRayIntensity;
            fixed4 _CoreColorInner;
            fixed4 _CoreColorOuter;
            fixed4 _GlowColor;
            fixed4 _OuterGlowColor;
            fixed4 _EdgeColor;

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

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // 黑洞中心
                float2 center = float2(0.5, 0.5);
                float dist = distance(i.uv, center);

                // 核心区域颜色渐层
                if (dist < _HoleRadius)
                {
                    float gradient = smoothstep(0.0, _HoleRadius, dist);
                    return lerp(_CoreColorInner, _CoreColorOuter, gradient); // 从内层颜色渐变到外层颜色
                }

                // 白边部分
                float edge = smoothstep(_HoleRadius, _HoleRadius + _EdgeWidth, dist);
                if (dist >= _HoleRadius && dist < _HoleRadius + _EdgeWidth)
                {
                    return _EdgeColor * edge;
                }

                // 光环（内光环）
                float innerGlow = 1.0 - smoothstep(_HoleRadius + _EdgeWidth, _HoleRadius + _GlowWidth * 0.6, dist);
                fixed4 innerGlowColor = _GlowColor * innerGlow;

                // 外光环（添加径向光线效果）
                float outerGlowBase = pow(1.0 - smoothstep(_HoleRadius + _GlowWidth * 0.6, _HoleRadius + _GlowWidth, dist), _BrightnessFalloff);
                
                // 添加光线效果（使用 sin/cos 调整亮度）
                float angle = atan2(i.uv.y - center.y, i.uv.x - center.x); // 计算角度
                float lightRays = abs(sin(angle * 10.0) * _LightRayIntensity); // 光线条纹强度
                float outerGlow = outerGlowBase + lightRays;

                // 限制光线效果平滑过渡
                outerGlow = smoothstep(0.0, 1.0, outerGlow);

                fixed4 outerGlowColor = _OuterGlowColor * outerGlow;

                // 叠加渐变
                fixed4 finalGlow = innerGlowColor + outerGlowColor;

                // 背景颜色（可选）
                fixed4 bgColor = tex2D(_MainTex, i.uv);

                return bgColor + finalGlow;
            }
            ENDCG
        }
    }
}
