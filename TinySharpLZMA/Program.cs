using SevenZip;
using SevenZip.Compression.LZMA;
using System;
using System.IO;

namespace TinySharpLZMA
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var now = DateTime.Now;
            // place test.7z in debug folder
            compressFileLZMA("in.exe", "test.7z");
            Console.WriteLine($"Compression took {DateTime.Now.Subtract(now).TotalMilliseconds:F3}ms");

            now = DateTime.Now;
            decompressFileLZMA("test.7z", "out.exe");
            Console.WriteLine($"Decompression took {DateTime.Now.Subtract(now).TotalMilliseconds:F3}ms");

            // Console.ReadLine();

        }
        static CoderPropID[] propIDs =
        {
            CoderPropID.DictionarySize,
            CoderPropID.PosStateBits,   // smaller better compression
            CoderPropID.LitContextBits,
            CoderPropID.LitPosBits,
            CoderPropID.Algorithm,
            CoderPropID.NumFastBytes,
            CoderPropID.MatchFinder,    // for my case bt2 is better
            CoderPropID.EndMarker
        };

        // these are the default properties, keeping it simple for now:
        static object[] properties =
        {
            1 << 23, // dictionary       [0  , 29 ], default: 23
            1,       // PosStateBits     [0  , 4  ], default: 2
            3,       // LitContextBits   [0  , 8  ], default: 3    || 0 for 32-bit data
            0,       // LitPosBits       [0  , 4  ], default: 2    || 2 for 32-bit data
            2,       // Algorithm
            128,     // NumFastBytes     [5  , 273], default: 128
            "bt2",   // MatchFinder      [bt2, bt4], default: bt4
            false    // EndMarker        write End Of Stream marker
        };

        private static void compressFileLZMA(string inFile, string outFile)
        {
            Encoder coder = new Encoder();
            FileStream compressedSize = new FileStream(inFile, FileMode.Open);
            FileStream outStream = new FileStream(outFile, FileMode.Create);

            coder.SetCoderProperties(propIDs, properties);
            // Write the encoder properties
            coder.WriteCoderProperties(outStream);

            // Write the decompressed file size.
            outStream.Write(BitConverter.GetBytes(compressedSize.Length), 0, 8);

            // Encode the file.
            coder.Code(compressedSize, outStream, compressedSize.Length, -1, null);
            // no need flush, by default will flush
            outStream.Close();
        }

        private static void decompressFileLZMA(string inFile, string outFile)
        {
            Decoder coder = new Decoder();
            FileStream compressedSize = new FileStream(inFile, FileMode.Open);
            FileStream output = new FileStream(outFile, FileMode.Create);

            // Read the decoder properties
            byte[] properties = new byte[5];
            compressedSize.Read(properties, 0, 5);

            // Read in the decompress file size.
            byte[] fileLengthBytes = new byte[8];
            compressedSize.Read(fileLengthBytes, 0, 8);
            long outSize = BitConverter.ToInt64(fileLengthBytes, 0);

            coder.SetDecoderProperties(properties);
            coder.Code(compressedSize, output, compressedSize.Length, outSize, null);
            output.Flush();
            output.Close();
        }
    }
}
