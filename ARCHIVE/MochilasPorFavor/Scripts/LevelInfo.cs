using System.Collections;
using System.Collections.Generic;

public readonly struct LevelInfo
{

    public LevelInfo(int level,  int numObjsMochila, int numObjsDebeLlevar, int numObjsNoLlevar, int maxFaltan, int maxNoLlevar, float probAceptar, float probFaltan, float probNoLlevar)
    {
        Level = level;
        NumObjsMochila = numObjsMochila;
        NumObjDebeLlevar = numObjsDebeLlevar;
        NumObjsNoLlevar = numObjsNoLlevar;
        MaxFaltan = maxFaltan;
        MaxNoLlevar = maxNoLlevar;
        ProbAceptar = probAceptar;
        ProbFaltan = probFaltan;
        ProbNoLlevar = probNoLlevar;
    }

    public int Level { get; }
    public int NumObjsMochila { get; }
    public int NumObjDebeLlevar { get; }
    public int NumObjsNoLlevar { get; }
    public int MaxFaltan { get; }
    public int MaxNoLlevar { get; }
    public float ProbAceptar { get; }
    public float ProbFaltan { get; }
    public float ProbNoLlevar { get; }


}
