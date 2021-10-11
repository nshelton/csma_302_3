Shader "RPSTessellation Sample" {
    Properties{
        _Tess("Tessellation", Range(1,32)) = 4
        _MainTex("Base (RGB)", 2D) = "white" {}
        _DispTex("Disp Texture", 2D) = "gray" {}
        _NormalMap("Normalmap", 2D) = "bump" {}
        _Displacement("Displacement", Range(0, 1.0)) = 0.3
        _Color("Color", color) = (1,1,1,0)
        _SpecColor("Spec color", color) = (0.5,0.5,0.5,0.5)
    }
        SubShader{
            Tags { "RenderType" = "Opaque" }
            LOD 300

            CGPROGRAM
            #pragma surface surf BlinnPhong addshadow fullforwardshadows vertex:disp tessellate:tessFixed nolightmap
            #pragma target 4.6

            struct appdata {
                float4 vertex : POSITION;
                float4 tangent : TANGENT;
                float3 normal : NORMAL;
                float2 texcoord : TEXCOORD0;
            };

            sampler2D _DispTex;
            float _Displacement;

            float _Tess;
            float4 tessFixed()
            {
                return _Tess;
            }

            void disp(inout appdata v)
            {
                float3 col = tex2Dlod(_DispTex, float4(v.texcoord.xy, 0, 0));

                if (col.g > 0){
                    col.r = float4(col.g, 0, 0, 1);
                }
                if (col.b > 0){
                    col.r = float4(col.b, 0, 0, 1);
                }

                float d = col * _Displacement;
                v.vertex.xyz -= v.normal * d; //this will draw into the plane
            }

            struct Input {
                float2 uv_MainTex;
            };

            sampler2D _MainTex;
            sampler2D _NormalMap;
            fixed4 _Color;

            void surf(Input IN, inout SurfaceOutput o) 
            {
                half4 c = tex2D(_MainTex, IN.uv_MainTex);
                o.Albedo = c.rgb;
                o.Specular = 0.2;
                o.Gloss = 1.0;
                o.Normal = UnpackNormal(tex2D(_NormalMap, IN.uv_MainTex));
            }
            ENDCG
        }
            FallBack "Diffuse"
}