Shader "Custom/BlackOutline_V2"
{
    Properties
    {
        _OutlineSize        ("Outline Size", float) = 0.02

        _OpacityTex         ("Opacity Texture", 2D) = "white" {}
        _Opacity            ("Opacity", float) = 1
    }

    SubShader
    {
        Tags
        { 
            "queue" = "Transparent" 
            "RenderType" = "Transparent" 
        }

        LOD 100

        
        Pass //back
        {
            Blend srcAlpha OneMinusSrcAlpha
            ZWrite On
            Cull Front //shaderlab code, for outline purposes to draw a blackoutline

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float2 uv           : TEXCOORD0;
                float3 normal       : NORMAL;
                float4 vertex       : POSITION;
            };

            struct v2f
            {
                float2 uv           : TEXCOORD0;
                float4 vertex       : SV_POSITION;
            };

            float       _OutlineSize;
            sampler2D   _OpacityTex;
            float       _Opacity;

            v2f vert (appdata v)
            {
                v2f o;
                o.uv = v.uv;
                float3 newPos = v.vertex.xyz + v.normal * _OutlineSize;
                o.vertex = UnityObjectToClipPos(newPos);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 OpacityCol = tex2D(_OpacityTex, i.uv);
                //col.a = OpacityCol * clamp(_Opacity, 0, 1);

                float OpacityMask = OpacityCol.r;

                fixed4 col = float4(0, 0, 0, 1);

                col.a = lerp(1.0, clamp(_Opacity, 0, 1), OpacityMask); //makes sure 

                return col;
                //return fixed4(0, 0, 0, 1);
            }
            ENDCG
        }
    }
}
