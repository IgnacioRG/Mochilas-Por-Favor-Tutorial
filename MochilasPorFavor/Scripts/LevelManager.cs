using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MochilasPorFavor {
    public class LevelManager
    {

        private List<LevelInfo> levels = new List<LevelInfo>();

        public LevelManager(int level)
        {
            Level = level;
            levels.Add(new LevelInfo(1, 3, 1, 0, 1, 0, 0.5f, 0.5f, 0.0f));
            levels.Add(new LevelInfo(2, 3, 2, 0, 2, 0, 0.3f, 0.7f, 0.0f));
            levels.Add(new LevelInfo(3, 4, 2, 1, 2, 1, 0.3f, 0.3f, 0.3f));
            levels.Add(new LevelInfo(4, 4, 2, 2, 1, 2, 0.4f, 0.3f, 0.3f));
            levels.Add(new LevelInfo(5, 5, 3, 3, 1, 2, 0.5f, 0.1f, 0.4f));
            levels.Add(new LevelInfo(6, 7, 3, 5, 2, 2, 0.3f, 0.1f, 0.6f));
            levels.Add(new LevelInfo(7, 9, 5, 7, 2, 1, 0.3f, 0.3f, 0.3f));
            levels.Add(new LevelInfo(8, 11, 6, 9, 2, 1, 0.2f, 0.4f, 0.4f));
            levels.Add(new LevelInfo(9, 13, 7, 12, 1, 1, 0.4f, 0.3f, 0.3f));
            levels.Add(new LevelInfo(10, 14, 8, 15, 1, 1, 0.3f, 0.3f, 0.3f));
        }

        private int _level;
        public int Level
        {
            get
            {
                return _level;
            }

            set
            {
                if (value > 0 && value <= 10)
                {
                    _level = value;
                }
            }
        }

        public int GetNumObjsMochila()
        {
            return levels[Level - 1].NumObjsMochila;
        }

        public int GetNumObjsDebeLlevar()
        {
            return levels[Level - 1].NumObjDebeLlevar;
        }

        public int GetNumObjsNoLlevar()
        {
            return levels[Level - 1].NumObjsNoLlevar;
        }

        public (float probA, float probRF, float probRO) GetProbs()
        {
            return (levels[Level - 1].ProbAceptar, levels[Level - 1].ProbFaltan, levels[Level - 1].ProbNoLlevar);
        }

        /// <summary>
        /// Regresa una tupla que indica el número de elementos en Debe Llevar que lleva en la mochila y el
        /// el número de elementos que faltan
        /// </summary>
        public (int lleva, int faltan) GetDebeLlevarRF()
        {
            if (levels[Level - 1].MaxFaltan > 1)
            {
                int faltan = Random.Range(1, levels[Level - 1].MaxFaltan + 1);
                int llevar = levels[Level - 1].NumObjDebeLlevar - faltan;
                return (llevar, faltan);
            }
            else
            {
                int llevar = levels[Level - 1].NumObjDebeLlevar - levels[Level - 1].MaxFaltan;
                return (llevar, levels[Level - 1].MaxFaltan);
            }
        }

        /// <summary>
        /// Regresa una tupla que indica el número de elementos en No Debe Llevar que no lleva en la mochila y el
        /// el número de elementos que lleva
        /// </summary>
        public (int noLleva, int lleva) GetNoLlevarRO()
        {
            if (levels[Level - 1].MaxNoLlevar > 1)
            {
                int lleva = Random.Range(1, levels[Level - 1].MaxNoLlevar + 1);
                int noLleva = levels[Level - 1].NumObjsNoLlevar - lleva;
                return (noLleva, lleva);
            }
            else
            {
                int noLleva = levels[Level - 1].NumObjsNoLlevar - levels[Level - 1].MaxNoLlevar;
                return (noLleva, levels[Level - 1].MaxNoLlevar);
            }
        }
    }
}