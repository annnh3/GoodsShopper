namespace Live.Libs.Procedure.Tests.Implements
{
	public class CountCreateProcess : IProcedureProcess<BaseProcedureContext, CountContext>
	{
		public CountContext Process(BaseProcedureContext ctx)
		{
			return new CountContext()
			{
				Count = 1
			};
		}
	}
}