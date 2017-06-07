

public interface IScriptSystemAPI {
	
	void Register(ScriptSystem scriptSystem);
	void PreRun(ScriptSystem scriptSystem);
	void PostRun(ScriptSystem scriptSystem);

}
