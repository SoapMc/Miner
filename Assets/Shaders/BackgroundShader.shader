Shader "Unlit/BackgroundShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)
		_OffsetX("OffsetX", float) = 0
		_OffsetY("OffsetY", float) = 0
    }
    SubShader
    {
        Tags
		{
			"Queue" = "Transparent"
			"RenderType" = "Transparent"
		}
        
		Cull Off ZWrite Off ZTest Always
		Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
				float4 color : COLOR;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
				fixed4 color : COLOR;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
			float _OffsetX;
			float _OffsetY;
			fixed4 _Color;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.color = v.color * _Color;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv + float2(_OffsetX, _OffsetY)) * i.color;
				col.rgb *= col.a;
                return col;
            }
            ENDCG
        }
    }
}
