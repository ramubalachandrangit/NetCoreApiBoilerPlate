using System;
using System.Collections.Generic;
using System.Text;

namespace Utility.Model
{
    public class EncryptResModel
    {
        public byte[] Password { get; set; }
        public byte[] Salt { get; set; }
        public byte[] Key { get; set; }

    }
}
