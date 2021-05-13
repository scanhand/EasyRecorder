﻿using AUT.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsInput.Native;

namespace AUT.UI
{
    public class KeyItem
    {
        public VirtualKeyCode VKKeyCode { get; set; }
        public KeyItem(VirtualKeyCode code)
        {
            this.VKKeyCode = code;
        }

        public override string ToString()
        {
            return AUtil.ToVKeyToString(VKKeyCode);
        }

        public override bool Equals(object obj) => obj is KeyItem other && this.Equals(other);

        public bool Equals(KeyItem p) => this.VKKeyCode == p.VKKeyCode;

        public override int GetHashCode() => (this.VKKeyCode).GetHashCode();

        public static bool operator ==(KeyItem lhs, KeyItem rhs) => lhs.Equals(rhs);

        public static bool operator !=(KeyItem lhs, KeyItem rhs) => !(lhs == rhs);
    }
}
