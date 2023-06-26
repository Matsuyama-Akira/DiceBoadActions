using UnityEngine;

public class DB_ClearList : MonoBehaviour
{
    [System.Serializable]
    public class MultiArrayClass
    {
        [Header("Vertical status")]
        public bool[] multiArray = new bool[11];

        public MultiArrayClass(bool[] _multiArray)
        {
            multiArray = _multiArray;
        }
    }

    [SerializeField, Header("Horizonal status")]
    private MultiArrayClass[] multiArrayClasses = new MultiArrayClass[11];

    public void GetBools(int num1, int num2, bool status)
    {
        multiArrayClasses[num1].multiArray[num2] = status;
    }
}
