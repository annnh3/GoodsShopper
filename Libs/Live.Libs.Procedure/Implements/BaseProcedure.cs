using System;

namespace Live.Libs.Procedure.Implements
{
	public class BaseProcedure<TResultCtx> : IProcedure<TResultCtx>
		where TResultCtx : IProcedureContext
	{
		protected TResultCtx Ctx { get; set; }

		public IProcedure<TNextResultCtx> Execute<TNextResultCtx>(IProcedureProcess<TResultCtx, TNextResultCtx> process)
			where TNextResultCtx : IProcedureContext
		{
			var result = process.Process(Ctx);

			return new BaseProcedure<TNextResultCtx>()
			{
				Ctx = result,
			};
		}

		public IProcedure<TNextResultCtx> Execute<TNextResultCtx, TParam>(
			IProcedureProcess<TResultCtx, TNextResultCtx, TParam> process, TParam param)
			where TNextResultCtx : IProcedureContext
		{
			var result = process.Process(Ctx, param);

			return new BaseProcedure<TNextResultCtx>()
			{
				Ctx = result,
			};
		}


		public IProcedure<TResultCtx> SetParam(Action<TResultCtx> action)
		{
			action(this.Ctx);
			return this;
		}


		public TResultCtx GetResult()
		{
			return Ctx;
		}
	}

	public class BaseProcedure : BaseProcedure<BaseProcedureContext>
	{
		public static IProcedure<TResultCtx> From<TResultCtx>(
			IProcedureProcess<BaseProcedureContext, TResultCtx> process) where TResultCtx : IProcedureContext
		{
			var procedure = new BaseProcedure
			{
				Ctx = new BaseProcedureContext()
			};
			var rsProcedure = procedure.Execute(process);

			return rsProcedure;
		}

		public static IProcedure<TResultCtx> From<TResultCtx, TParam>(
			IProcedureProcess<BaseProcedureContext, TResultCtx, TParam> process,
			TParam param
		) where TResultCtx : IProcedureContext
		{
			var procedure = new BaseProcedure
			{
				Ctx = new BaseProcedureContext(),
			};

			var rsProcedure = procedure.Execute(process, param);

			return rsProcedure;
		}
	}
}