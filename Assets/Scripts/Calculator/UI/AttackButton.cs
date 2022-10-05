using UnityEngine;

namespace Calculator.UI
{
    public class AttackButton : MonoBehaviour
    {
        static public Calculator.Units.Units.IUnit a { get; set; }
        static public Calculator.Units.Units.IUnit d { get; set; }
        public void Attack()
        {
            Calculator.Calculate.Calculate.Calculating(a, d);
        }
    }
}