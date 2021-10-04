Shader "Hidden/ReactionDiffusionView"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _Colormap ("colormap", 2D) = "white" {}
        _colorSelection("_colorSelection", Range(0,1)) = 0.5
        _scale("scale", Range(0,5)) = 1
        _channel ("channel", Int) = 1
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
            sampler2D _Colormap;
            float _colorSelection;
            float _scale;
            int _channel;

            fixed4 frag (v2f i) : SV_Target
            {
                float4 col = tex2D(_MainTex, i.uv);
                float x = 0;
                
                if (_channel == 0) {
                    x = col.x;
                }

                if (_channel == 1) {
                    x = col.y;
                }
                
                if (_channel == 2) {
                    x = col.z;
                }

                x = pow(x, _scale);
                
                float2 colormapUV = float2(x, _colorSelection);

                return tex2D(_Colormap, saturate(colormapUV));

            }
            ENDCG
        }
    }
}
