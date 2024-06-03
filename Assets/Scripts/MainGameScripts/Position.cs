public class Position
{
    public int arrayIndex;
    public int cellIndex;

    public Position(int arrayIndex = 0, int cellIndex = 0) {
        this.arrayIndex = arrayIndex;
        this.cellIndex = cellIndex;
    }

    public static Position CreatePrePosition(Position position) {
        if (position == null) {
            return null;
        }

        return new Position(position.arrayIndex, position.cellIndex);
    }
}
