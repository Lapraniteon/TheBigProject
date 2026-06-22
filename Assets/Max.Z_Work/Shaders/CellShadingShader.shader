Shader "Custom/CellShadingShader"
{
    Properties
    {
        _MainTex            ("Texture", 2D) = "white" {}
        _Ambient            ("Ambient", float) = 1
        _Diffuse            ("Diffuse", float) = 1
        _Specular           ("Specular", float) = 1
        _SpecSharpness      ("Spec sharpness", float) = 1
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

            #include "UnityCG.cginc"
            #include "unityLightingCommon.cginc"

            struct appdata
            {
                float2 uv           : TEXCOORD0;
                float3 worldPos     : TEXCOORD2;
                float3 normal       : NORMAL;
                float4 vertex       : POSITION;
            };

            struct v2f
            {
                float2 uv           : TEXCOORD0;
                float3 normal       : TEXCOORD1;
                float3 worldPos     : TEXCOORD2;
                float3 cameraNormal : TEXCOORD3;
                float4 vertex       : SV_POSITION;
            };

            sampler2D   _MainTex;
            float4      _MainTex_ST;

            float       _Ambient;
            float       _Diffuse;
            float       _Specular;
            float       _SpecSharpness;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.normal = mul(UNITY_MATRIX_M, v.normal);
                o.worldPos = mul(UNITY_MATRIX_M, v.vertex);
                o.cameraNormal = mul(UNITY_MATRIX_IT_MV, float4(v.normal, 0));
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                //ADS properties -------------------------------------------------------------------------------

                //Ambient------------------------------------------------------
                float ambient = clamp(_Ambient, 0, 1);
                float3 ambientColor = float3(0.5, 1, 1);
                //Ambient------------------------------------------------------

                //Diffuse------------------------------------------------------
                float3 normal = normalize(i.normal);
                float3 lichtDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 diffuse = saturate(normalize(dot(lichtDirection, normal)));
                float3 newDiffuse = diffuse * _Diffuse;
                //Diffuse------------------------------------------------------

                //Specular-----------------------------------------------------
                float3 Dir = normalize(_WorldSpaceCameraPos - i.worldPos);
                //float3 CameraDir = smoothstep(_value, _value + 1, Dir);
                float3 lightReflection = reflect(-lichtDirection, normal);
                float3 specularDot = saturate(dot(lightReflection, Dir));
                float specular = step(_SpecSharpness, specularDot);
                float sharpness = clamp(_Specular, 0, 100);
                //Specular-----------------------------------------------------

                //ADS properties -------------------------------------------------------------------------------

                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                col.rgb *= ambient * ambientColor + (_LightColor0.rgb * newDiffuse);
                col.rgb += _LightColor0.rgb * (specular * (sharpness * 10));

                return col;
            }
            ENDCG
        }
    }
}
