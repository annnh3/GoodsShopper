namespace Live.Libs.Procedure.Tests.Implements
{
	public class CountAddProcess : IProcedureProcess<CountContext, CountContext, int>
	{
		public CountContext Process(CountContext ctx, int param)
		{
			ctx.Count += param;
			return ctx;
		}
	}
}