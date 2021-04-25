namespace Bytewizer.TinyCLR.IO.Compression
{
	internal enum BlockState
	{
		NeedMore,
		BlockDone,
		FinishStarted,
		FinishDone
	}
}
