sampler s;
sampler s2;


float foo;
float fade_percentage; // 100% percent means completely faded

static float x;


float4 PixelShaderFunction(float2 uv : TEXCOORD0, float4 c: COLOR0) : COLOR0
{
    // TODO: add your pixel shader code here.
 
        
    //blur
    float4 color;
    color = tex2D(s, uv); //normal
    
    x = 100 - fade_percentage;
    x = x/100;			
    			
	color = color * x ;

	
    return color ;

}

technique Technique1
{
    pass Pass1
    {
        // TODO: set renderstates here.


        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}
