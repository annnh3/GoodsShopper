using System;

namespace Live.Libs.Procedure
{
	/// <summary>
	/// 處理過程介面
	/// </summary>
	public interface IProcedure<out TResultCtx>
		where TResultCtx : IProcedureContext
	{
		/// <summary>
		/// 執行處理程序
		/// </summary>
		IProcedure<TNextResultCtx> Execute<TNextResultCtx>(IProcedureProcess<TResultCtx, TNextResultCtx> procedure)
			where TNextResultCtx : IProcedureContext;

		/// <summary>
		/// 執行處理程序
		/// </summary>
		IProcedure<TNextResultCtx> Execute<TNextResultCtx, TParam>(
			IProcedureProcess<TResultCtx, TNextResultCtx, TParam> procedure, TParam param)
			where TNextResultCtx : IProcedureContext;


		/// <summary>
		/// 取得執行結果
		/// </summary>
		TResultCtx GetResult();

		/// <summary>
		/// 設定參數
		/// </summary>
		IProcedure<TResultCtx> SetParam(Action<TResultCtx> action);
	}
}