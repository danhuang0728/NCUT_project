Shader "Custom/ReflectiveShader"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {} // 主紋理
        _Glossiness ("Smoothness", Range(0,1)) = 0.5 // 光澤度
        _LightColor ("Reflection Color", Color) = (1, 1, 1, 1) // 反射顏色
        _Color ("Tint Color", Color) = (1, 1, 1, 1) // 紋理顏色調整，包含透明度
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha // 支持透明度混合
        Pass
        {
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
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _Glossiness;
            float4 _LightColor;
            float4 _Color; // Tint 顏色屬性，包含透明度

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 texColor = tex2D(_MainTex, i.uv); // 紋理顏色
                fixed4 reflection = _LightColor * _Glossiness; // 反射效果
                fixed4 finalColor = (texColor * _Color) + reflection; // 調整後的顏色
                finalColor.a = texColor.a * _Color.a; // 使用紋理和 Tint 的透明度計算最終透明度
                return finalColor;
            }
            ENDCG
        }
    }
}
