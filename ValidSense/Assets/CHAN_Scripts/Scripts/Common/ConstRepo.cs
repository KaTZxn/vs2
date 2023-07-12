
public static class ConstRepo 
{
    
    /// <summary>
    /// poor自動判定用の変数
    /// </summary>
    public const long poorTiming = -125;


    /// <summary>
    /// ノーツのタイプ（引数用に0スタート）
    /// </summary>
    public enum NotesType
    { 
        Normal = 0,
        Slide,
        Hold,
        Link,
    }


    /// <summary>
    /// ノーツのタイプ（引数用に0スタート）
    /// </summary>
    public enum Chara
    {
        Sight = 0,
        Tactile,
        Smell_Taste,
        Hear,
    }


    /// <summary>
    /// プレイヤー（引数用に0スタート）
    /// </summary>
    public enum Player
    {
        P1 = 0,
        P2,
    }
}
