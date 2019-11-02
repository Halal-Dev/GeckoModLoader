class UintWrapper
    {
        public string ingameVal;
        public uint dataVal;
        public UintWrapper(string vname, uint wname)
        {
            ingameVal = vname;
            dataVal = wname;
        }

        public override string ToString()
        {
            return ingameVal;
        }
    }
