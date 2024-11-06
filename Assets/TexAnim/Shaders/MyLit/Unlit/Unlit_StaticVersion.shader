Shader "Unlit/Unlit_StaticVersion"
{
    Properties
	{
        [MainColor] _BaseColor("Color", Color) = (1, 1, 1, 1)
		_MainTex("Texture", 2D) = "white" {}
	}
	
    SubShader
    {
        Tags { "RenderType" = "Opaque" }
        Cull Off
		LOD 100

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_instancing

            #include "UnityCG.cginc"

            struct appdata
            {
                float2 uv : TEXCOORD0;
                float4 pos : POSITION;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            fixed4 _BaseColor;
            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert(appdata v)
            {
                UNITY_SETUP_INSTANCE_ID(v);

                v2f o;
                o.vertex = UnityObjectToClipPos(v.pos * 0.01);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);

                return o;
            }
            
            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv) * _BaseColor;
                return col;
            }
            ENDHLSL
        }
	}
}