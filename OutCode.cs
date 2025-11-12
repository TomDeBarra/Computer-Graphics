using Unity.VisualScripting;
using UnityEngine;

public class OutCode
{
    // UDLR
    internal bool up;
    internal bool down;
    internal bool left;
    internal bool right;

    public OutCode(Vector2 v)
    {
        up = v.y > 1;
        down = v.y < -1;
        left = v.x < -1;
        right = v.x > 1;
    }
    public OutCode() // Produces the 0000 ie in viewport OutCode
    {
        up = false;
        down = false;
        left = false;
        right = false;
    }
    public OutCode(OutCode oc) // constructs a copy of a given OutCode 
    {
        this.up = oc.up;
        this.down = oc.down;
        this.left = oc.left;
        this.right = oc.right;
    }
    public OutCode(bool up, bool down, bool left, bool right)
    {
        this.up = up;
        this.down = down;
        this.left = left;
        this.right = right;
    }
    static public OutCode operator +(OutCode o1, OutCode o2)
    {
        return new OutCode(o1.up || o2.up, o1.down || o2.down, o1.left || o2.left, o1.right || o2.right);
    }
    static public OutCode operator *(OutCode o1, OutCode o2)
    {
        return new OutCode(o1.up && o2.up, o1.down && o2.down, o1.left && o2.left, o1.right && o2.right);
    }
    static public bool operator ==(OutCode o1, OutCode o2)
    {
        return (o1.up == o2.up) && (o1.down == o2.down) && (o1.left == o2.left) && (o1.right == o2.right);
    }
    // both inside (both 0000) → accept line
    // both outside in same region (1000 * 1000 = 1000) → reject line
    // don't share same region (1000 * 01000 = 0000) -> may not trivially reject.
    // * is a AND statement
    static public bool operator !=(OutCode o1, OutCode o2)
    {
        return !(o1 == o2);
    }
    
    /*
    public void printFunction()
    {
        Debug.Log((up ? "0" : "1") + (down ? "0" : "1") + (left ? "0" : "1") + (right ? "0" : "1"));
    }
    */
    public void printFunction()
    {
        Debug.Log((up ? "1" : "0") + (down ? "1" : "0") + (left ? "1" : "0") + (right ? "1" : "0"));
    }
}
