Shader "Custom/SpriteGlow"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        [Toggle] _UseOutline ("Use Outline", Float) = 1
        _GlowColor ("Glow Color", Color) = (1,1,1,1)
        _GlowIntensity ("Glow Intensity", Range(0, 10)) = 1.0
        _OutlineWidth ("Outline Width", Range(0, 10)) = 1.0
        _Smoothness ("Base Smoothness", Range(0, 5)) = 1.0
        _PulseSpeed ("Pulse Speed", Range(0.1, 5)) = 1.0
        _ShrinkAmount ("Sprite Shrink", Range(0, 0.5)) = 0.1
        [MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
    }

    SubShader
    {
        Tags
        { 
            "Queue"="Transparent" 
            "IgnoreProjector"="True" 
            "RenderType"="Transparent" 
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        Blend One OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile _ PIXELSNAP_ON
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex   : POSITION;
                float4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex   : SV_POSITION;
                fixed4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
            };

            fixed4 _Color;
            fixed4 _GlowColor;
            float _GlowIntensity;
            float _OutlineWidth;
            float _Smoothness;
            float _PulseSpeed;
            float _ShrinkAmount;
            float _UseOutline;
            sampler2D _MainTex;
            float4 _MainTex_TexelSize;

            float HeartbeatWave(float t)
            {
                float wave1 = sin(t * 6.28318530718);
                float wave2 = sin((t + 0.1) * 6.28318530718);
                
                float sharpRise = pow(max(wave1, 0), 2);
                float slowDecay = pow(max(wave2, 0), 3) * 0.5;
                
                float heartbeat = sharpRise + slowDecay;
                heartbeat = heartbeat * 0.6 + 0.2;
                
                float microPulse = sin(t * 50) * 0.05;
                heartbeat += microPulse;
                
                return heartbeat;
            }

            float GetDynamicSmoothness()
            {
                float time = _Time.y * _PulseSpeed;
                float heartbeat = HeartbeatWave(time);
                return lerp(0.8, 1.2, heartbeat) * _Smoothness;
            }

            v2f vert(appdata_t IN)
            {
                v2f OUT;
                OUT.vertex = UnityObjectToClipPos(IN.vertex);
                OUT.texcoord = IN.texcoord;
                OUT.color = IN.color * _Color;
                #ifdef PIXELSNAP_ON
                OUT.vertex = UnityPixelSnap(OUT.vertex);
                #endif
                return OUT;
            }

            float IsInBounds(float2 uv)
            {
                float margin = 0.001;
                return uv.x >= margin && uv.x <= (1 - margin) && 
                       uv.y >= margin && uv.y <= (1 - margin);
            }

            float4 SampleWithBounds(sampler2D tex, float2 uv)
            {
                float2 center = float2(0.5, 0.5);
                float2 shrunkUV = (uv - center) * (1 + _ShrinkAmount) + center;
                
                if (!IsInBounds(shrunkUV))
                {
                    return float4(0, 0, 0, 0);
                }
                
                return tex2D(tex, shrunkUV);
            }

            float SampleAlpha(float2 uv)
            {
                return SampleWithBounds(_MainTex, uv).a;
            }

            float GetBlurredAlpha(float2 uv)
            {
                float total = 0;
                float totalWeight = 0;
                
                for(int i = -4; i <= 4; i++)
                {
                    for(int j = -4; j <= 4; j++)
                    {
                        float2 offset = float2(i, j) * _MainTex_TexelSize.xy * _OutlineWidth;
                        float2 sampleUV = uv + offset;
                        
                        float dist = length(float2(i, j));
                        float weight = 1.0 / (1.0 + dist * dist);
                        
                        total += SampleAlpha(sampleUV) * weight;
                        totalWeight += weight;
                    }
                }
                
                return total / totalWeight;
            }

            fixed4 frag(v2f IN) : SV_Target
            {
                fixed4 c = SampleWithBounds(_MainTex, IN.texcoord) * IN.color;
                
                if (_UseOutline > 0.5)
                {
                    float blurredAlpha = GetBlurredAlpha(IN.texcoord);
                    float dynamicSmoothness = GetDynamicSmoothness();
                    float outlineAlpha = smoothstep(0.0, dynamicSmoothness, blurredAlpha);
                    outlineAlpha = pow(outlineAlpha, 2.2);
                    outlineAlpha *= (1 - step(0.1, c.a));
                    
                    fixed4 outlineColor = _GlowColor * _GlowIntensity;
                    outlineColor.a = outlineAlpha;
                    
                    c = lerp(outlineColor, c, c.a);
                }
                
                c.rgb *= c.a;
                return c;
            }
            ENDCG
        }
    }
}