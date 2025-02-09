
public class Player
{

    private readonly string id;
    private readonly string name;

    public Player (string id, string name) {
        this.id = id;
        this.name = name;
    }

    public string GetId() {
        return id;
    }

    public string GetName() {
        return name;
    }

    public bool ComparePlayer(Player playerToCompare) {
        if (id == playerToCompare.id && name == playerToCompare.name) {
            return true;
        }

        return false;
    }

}
