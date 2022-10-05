using System;
using UnityEngine;


namespace Calculator.Calculate
{


    public class Calculate : MonoBehaviour
    {
        private static int _hus;
        private static int _hts;
        private static int _hu;
        private static int _ht;
        private static int _aus;
        private static int _ats;
        
         static public void Calculating(Calculator.Units.Units.IUnit a, Calculator.Units.Units.IUnit d)
        {
           // _aus = a.Attack * a.Count;
           // _ats = d.Attack * d.Count;
           // _hus = a.Health * a.Count;
         //   _hts = d.Health * d.Count;
            _hu = a.Health;
            _ht = d.Health;
            if (a.GetType() == typeof(Calculator.Units.Units.Warrior))
            {
                if (d.GetType() == typeof(Calculator.Units.Units.Warrior)||d.GetType() == typeof(Calculator.Units.Units.Mage))
                {
                    //a.Count = (_hus - _ats) / _hu;
                  //  d.Count = (_hts - _aus) / _ht;
                }

                if (d.GetType() == typeof(Calculator.Units.Units.Assassin))
                {
                   // d.Count = (_hts - _aus) / _ht;
                }
            }

            if (a.GetType() == typeof(Calculator.Units.Units.Assassin))
            {
                if (d.GetType() == typeof(Calculator.Units.Units.Warrior))
                {
                   // d.Count = (int)Math.Floor((_hts - _aus*a.WeakCoeficient) / _ht); 
                }

                if (d.GetType() == typeof(Calculator.Units.Units.Assassin))
                {
                 //   d.Count = (_hts - _aus) / _ht;
                }

                if (d.GetType() == typeof(Calculator.Units.Units.Mage))
                {
                    //d.Count = (int)Math.Floor((_hts - _aus*a.StrongCoefitient) / _ht);
                }
            }

            if (a.GetType() == typeof(Calculator.Units.Units.Mage))
            {
                if (d.GetType() == typeof(Calculator.Units.Units.Warrior))
                {
                    //d.Count = (int)Math.Floor((_hts - _aus*a.StrongCoefitient) / _ht);
                }

                if (d.GetType() == typeof(Calculator.Units.Units.Assassin))
                {
                   // d.Count = (int)Math.Floor((_hts - _aus*a.WeakCoeficient) / _ht);
                }

                if (d.GetType() == typeof(Calculator.Units.Units.Mage))
                {
                //    d.Count = (_hts - _aus) / _ht;
                }
            }

        }
    }
}