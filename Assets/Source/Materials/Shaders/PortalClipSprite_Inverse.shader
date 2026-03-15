Shader "Custom/PortalClipSprite_Inverse"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _PortalPos ("Portal Position", Vector) = (0,0,0,0)
        _PortalNormal ("Portal Normal", Vector) = (1,0,0,0)
        _ClipEnabled ("Clip Enabled", Float) = 0
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "RenderType"="Transparent"
            "IgnoreProjector"="True"
        }

        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        ZWrite Off

        Pass
        {
            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_ST;

            float4 _PortalPos;
            float4 _PortalNormal;
            float _ClipEnabled;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                fixed4 color : COLOR;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float3 worldPos : TEXCOORD1;
                fixed4 color : COLOR;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                float4 world = mul(unity_ObjectToWorld, v.vertex);
                o.worldPos = world.xyz;
                o.color = v.color;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 worldXY = i.worldPos.xy;
                float2 portalPos = _PortalPos.xy;
                float2 portalNormal = normalize(_PortalNormal.xy);
                float2 portalToPixel = worldXY - portalPos;
                float side = dot(portalToPixel, portalNormal);

                // инвертированный клиппинг: показываем только пиксели, которые обычно скрыты
                if (_ClipEnabled > 0.5 && side >= 0)
                    discard;

                // цвет берём напрямую из SpriteRenderer
                fixed4 col = tex2D(_MainTex, i.uv) * i.color;

                return col;
            }

            ENDCG
        }
    }
}