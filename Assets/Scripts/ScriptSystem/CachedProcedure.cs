using System;
using System.Diagnostics;

public class CachedProcedure<T> {
	
	public Func<T> Procedure {
		get; private set;
	}
	
	public T Cache {
		get; private set;
	}
	
	public long CacheDurationMS {
		get; set;
	}
	
	private Stopwatch Stopwatch;
	
	public CachedProcedure(Func<T> procedure, long cacheDurationMS) {
		this.Procedure = procedure;
		this.CacheDurationMS = cacheDurationMS;
		this.Stopwatch = Stopwatch.StartNew();
	}
	
	public T Execute() {
		if (this.Stopwatch.ElapsedMilliseconds >= this.CacheDurationMS) {
			this.Cache = this.Procedure();
			this.Stopwatch.Restart();
		}
		
		return this.Cache;
	}
	
}
