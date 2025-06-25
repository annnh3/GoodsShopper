using Live.Libs.Procedure.Implements;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Live.Libs.Procedure.Tests.Implements
{
	[TestClass]
	public class BaseProcedureTest
	{
		[TestMethod]
		public void CreateProcedure()
		{
			var pro = BaseProcedure.From(new CountCreateProcess());
			Assert.IsNotNull(pro);
			Assert.AreEqual(1, pro.GetResult().Count);
			pro.Execute(new CountAddProcess(), 2);
			Assert.AreEqual(3, pro.GetResult().Count);
		}
	}
}