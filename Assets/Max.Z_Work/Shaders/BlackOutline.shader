Shader "Custom/BlackOutline"
{
    Properties
    {
        _OutlineSize        ("Outline Size", float) = 0.02
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        
        Pass //back
        {
            Cull Front //shaderlab code, for outline purposes to draw a blackoutline

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float3 normal       : NORMAL;
                float4 vertex       : POSITION;
            };

            struct v2f
            {
                float4 vertex       : SV_POSITION;
            };

            float       _OutlineSize;

            v2f vert (appdata v)
            {
                v2f o;
                float3 newPos = v.vertex.xyz + v.normal * _OutlineSize;
                o.vertex = UnityObjectToClipPos(newPos);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                
                return fixed4(0, 0, 0, 1);
            }
            ENDCG
        }
    }
}
