using System.Collections;
using System.Collections.Generic;

public class Tuple<V1, V2> {

    public V1 Val1 { get; set;  }
    public V2 Val2 { get; set;  }

    public Tuple() { }

    public Tuple(V1 val1, V2 val2)
    {
        Val1 = val1;
        Val2 = val2;
    }
    
}
