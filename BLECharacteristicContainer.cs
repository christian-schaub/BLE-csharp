using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MultipleServCharac
{
    public class BLECharacteristicContainer
    {
        private bool CCCSet;


        private int handle;

        public int Handle
        {
            get { return handle; }
            set { handle = value; }
        }
        private int handle_ccc;

        public int Handle_ccc
        {
            get { return handle_ccc; }
            set { 
                handle_ccc = value;
                CCCSet = true;
            }
        }
        private int value;

        public int Value
        {
            get { return this.value; }
            set { this.value = value; }
        }
        private string name;

        public BLECharacteristicContainer(string name)
        {
            this.name = name;
            this.CCCSet = false;
        }


        internal bool isCCCSet()
        {
            return CCCSet;
        }
    }
}
