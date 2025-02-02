Shader "Custom/FlashEffect" {
    Properties {
        [PerRendererData] _MainTex ("Texture", 2D) = "white" {}
        _FlashColor ("Flash Color", Color) = (1, 1, 1, 1) // 闪光颜色
        _FlashAmount ("Flash Amount", Range(0, 1)) = 0    // 闪光强度
        _FlashPower ("Flash Power", Range(0.1, 8)) = 3     // 闪光范围
    }
    
    SubShader {
        Tags { 
            "RenderType"="Transparent"
            "Queue"="Transparent"
        }
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off

        CGPROGRAM
        #pragma surface surf Lambert alpha:fade

        struct Input {
            float2 uv_MainTex;
            fixed4 color : COLOR;
        };

        sampler2D _MainTex;
        float4 _FlashColor;
        float _FlashAmount;
        float _FlashPower;

        void surf (Input IN, inout SurfaceOutput o) {
            fixed4 tex = tex2D(_MainTex, IN.uv_MainTex) * IN.color;
            o.Albedo = tex.rgb;
            o.Alpha = tex.a;

            float flash = pow(saturate(_FlashAmount), _FlashPower);
            o.Emission = _FlashColor.rgb * flash;
            o.Alpha = saturate(o.Alpha + flash);
        }
        ENDCG
    }
    FallBack "Sprites/Default"
} 