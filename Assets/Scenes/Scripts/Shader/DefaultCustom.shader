Shader "Sprites/Default_Custom"
{
    Properties
    {
        [PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
        _Color("Tint", Color) = (1, 1, 1, 1)   // 変更可能なプロパティとして公開
        [MaterialToggle] PixelSnap("Pixel snap", Float) = 0
        [HideInInspector] _RendererColor("RendererColor", Color) = (1, 1, 1, 1)
        [HideInInspector] _Flip("Flip", Vector) = (1, 1, 1, 1)
        [PerRendererData] _AlphaTex("External Alpha", 2D) = "white" {}
        [PerRendererData] _EnableExternalAlpha("Enable External Alpha", Float) = 0
    }

        SubShader
        {
            Tags
            {
                "Queue" = "Transparent"
                "IgnoreProjector" = "True"
                "RenderType" = "Transparent"
                "PreviewType" = "Plane"
                "CanUseSpriteAtlas" = "True"
            }

            Cull Off
            Lighting Off
            ZWrite Off
            Blend One OneMinusSrcAlpha

            Pass
            {
                CGPROGRAM
                #pragma vertex SpriteVert
                #pragma fragment SpriteFrag_Custom
                #pragma target 2.0
                #pragma multi_compile_instancing
                #pragma multi_compile_local _ PIXELSNAP_ON
                #pragma multi_compile _ ETC1_EXTERNAL_ALPHA
                #include "UnitySprites.cginc"

                sampler2D _MainTex_Custom;

        // 変更可能なプロパティを適用したフラグメントシェーダー
        fixed4 SpriteFrag_Custom(v2f IN) : SV_Target
        {
            fixed4 c = SampleSpriteTexture(IN.texcoord) * _Color;
            c.rgb = c.rgb * 2.0 + max(fixed3(0, 0, 0), _Color.rgb - 0.5) * 2.0;
            c.rgb *= c.a;
            return c;
        }
        ENDCG
    }
        }
}
