
public class Weapon {

	private string name;
	private int id;
	private float timer;

	public Weapon(int id, string name,float timer){
		this.id = id;
		this.timer = timer;
		this.name = name;
	}

	public string getName(){
		return name;
	}

	public int getId(){
		return id;
	}

	public float getTime(){
		return timer;
	}

	public void UpdateTimer(float time){
		timer += time;
	}
}
