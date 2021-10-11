using System;

[Serializable]
public class Camera
{
    public float width;
    public float height;

    public override bool Equals(object obj)
    {
        return (obj as Camera).height == height && (obj as Camera).width == width;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}
