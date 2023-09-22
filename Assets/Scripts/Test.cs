using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public class Locator
    {
        public static Locator locator;

        private IA _a;

        public IA A
        {
            get
            {
                if (_a == null)
                    _a = new AClass2();
                return _a;
            }
        }
    }


    private void Start()
    {
        var b = new BClass(Locator.locator.A);

        b.Run();
    }

    public interface IA
    {
        void Run();
    }

    public class AClass1 : IA
    {
        public void Run()
        {
            Debug.Log("Run A");
        }
    }

    public class AClass2 : IA
    {
        public void Run()
        {
            Debug.Log("Run A");
        }
    }

    public class BClass
    {
        private IA A;

        public BClass(IA a)
        {
            A = a;
        }

        public void Run()
        {
            A.Run();
        }
    }
}
