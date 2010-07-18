sampler s;
sampler s2;


float foo;
float time;

float x;


float4 PixelShaderFunction(float2 uv : TEXCOORD0, float4 c: COLOR0) : COLOR0
{
    // TODO: add your pixel shader code here.
 
        
    //blur
    float4 color;
    color = tex2D(s, uv); //normal
    
    // Use for 1,2 beat pulse
    //    x= (1+sin(time/400) + cos(time/200));
    //    x = clamp(x, 0.75,2);

	x = (0.75+abs(cos(time/500)/1.5));
	
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
