
public class BasicAPI : IScriptSystemAPI {
	
	private static BasicAPI instance;
	public static BasicAPI GetInstance() {
		if (instance == null) {
			instance = new BasicAPI();
		}
		
		return instance;
	}
		
	private BasicAPI() {
		
	}
	
	public void Register(ScriptSystem scriptSystem) {
		
	}
}
