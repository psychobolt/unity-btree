using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Editor.BTreeFramework.Templates
{
    public partial class BTreeBehaviourCode
    {
        private string className;
        private string behaviourName;

        public BTreeBehaviourCode(string className, string behaviourName)
        {
            this.className = className;
            this.behaviourName = behaviourName;
        }
    }
}
