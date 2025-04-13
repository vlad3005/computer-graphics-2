﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test1
{
    class Bin
    {
        public static int X, Y, Z;
        public static short[] array;
        public Bin() { }

        public void readBin(string path)
        {
            BinaryReader reader = new BinaryReader(File.Open(path, FileMode.Open));
            X = reader.ReadInt32();
            Y = reader.ReadInt32();
            Z = reader.ReadInt32();

            int arraySize = X * Y * Z;
            array = new short[arraySize];
            for (int i = 0; i < arraySize; i++)
            {
                array[i] = reader.ReadInt16();
            }
        }
    }
}
