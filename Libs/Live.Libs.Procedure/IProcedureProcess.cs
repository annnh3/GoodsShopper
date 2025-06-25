namespace Live.Libs.Procedure
{
	public interface IProcedureProcess
	{
	}

	/// <summary>
	/// 存儲過程程序介面
	/// </summary>
	public interface IProcedureProcess<in TFromCtx, out TToCtx> : IProcedureProcess
		where TFromCtx : IProcedureContext
		where TToCtx : IProcedureContext
	{
		/// <summary>
		/// 處理程序
		/// </summary>
		TToCtx Process(TFromCtx ctx);
	}


	/// <summary>
	/// 但參數的process
	/// </summary>
	public interface IProcedureProcess<in TFromCtx, out TToCtx, in TParam> : IProcedureProcess
		where TFromCtx : IProcedureContext
		where TToCtx : IProcedureContext
	{
		/// <summary>
		/// 執行程序，並加入參數
		/// </summary>
		TToCtx Process(TFromCtx ctx, TParam param);
	}
}