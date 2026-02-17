using NetEdf;
using NetEdf.src;
using System.Security.Cryptography;
using System.Xml.Serialization;

namespace NetEdfTest;

[TestClass]
public class TestPrimitives
{

    public void TrySrcBinToSrc<T>(PoType type, T value, Span<byte> expected)
        where T : struct
    {
        Span<byte> dst = new byte[10];
   
        var actual = Primitives.TrySrcToBin(type, value, dst, out int w);

        Assert.AreEqual(EdfErr.IsOk, actual);
        Assert.AreEqual(expected.Length, w);
        Assert.IsTrue(dst.Slice(0, w).SequenceEqual(expected));

        actual = Primitives.TryBinToSrc(type, dst, out int r, out object? obj);

        Assert.AreEqual(EdfErr.IsOk, actual);
        Assert.AreEqual(expected.Length, r);
        Assert.AreEqual(value, obj);
    }
    public void TrySrcToBin_ErrDstBufOverflow(PoType type, object obj, Span<byte> dst)
    {
        var actual = Primitives.TrySrcToBin(type, obj, dst, out int w);

        Assert.AreEqual(EdfErr.DstBufOverflow, actual);
    }
    public void TrySrcToBin_ErrWrongType(PoType type, object obj, Span<byte> dst)
    {
        var actual = Primitives.TrySrcToBin(type, obj, dst, out int w);

        Assert.AreEqual(EdfErr.WrongType, actual);
    }
    public void TrySrcBin_ObjIsString(PoType type, object value, Span<byte> expected)
    {
        Span<byte> dst = new byte[20];
       
        var actual = Primitives.TrySrcToBin(type, value, dst, out int w);

        Assert.AreEqual(EdfErr.IsOk, actual);
        Assert.AreEqual(expected.Length, w);
        Assert.IsTrue(dst.Slice(0,w).SequenceEqual(expected));

        actual = Primitives.TryBinToSrc(type, dst, out int r, out object? obj);

        Assert.AreEqual(EdfErr.IsOk, actual);
        Assert.AreEqual(expected.Length, r);
        Assert.AreEqual(value, obj);
    }


    [TestMethod]
    public void TestSrcToBinToSrc_Err()
    {
        TrySrcToBin_ErrDstBufOverflow(PoType.Int16, 123, new byte[1]);
        TrySrcToBin_ErrDstBufOverflow(PoType.String, "1234", new byte[1]);
   
    }

    [TestMethod]
    public void TestSrcToBinToSrc_Struct()
    {
        TrySrcToBin_ErrWrongType(PoType.Struct, 1, new byte[1]);
    }

    [TestMethod]
    public void TestSrcToBinToSrc_Int8()
    {
        TrySrcBinToSrc(PoType.Int8, sbyte.MaxValue, [0x7F]);
        TrySrcBinToSrc(PoType.Int8, sbyte.MinValue, [0x80]);
        TrySrcBinToSrc(PoType.Int8, (sbyte)-123, [0x85]);
        TrySrcBinToSrc(PoType.Int8, (sbyte)123, [0x7B]);
    }

    [TestMethod]
    public void TestSrcToBinToSrc_Unt8()
    {
        TrySrcBinToSrc(PoType.UInt8, byte.MinValue, [0x00]);
        TrySrcBinToSrc(PoType.UInt8, byte.MaxValue, [0xFF]);
        TrySrcBinToSrc(PoType.UInt8, (byte)123, [0x7B]);
    }

    [TestMethod]
    public void TestSrcToBinToSrc_Int16()
    {
        TrySrcBinToSrc(PoType.Int16, short.MaxValue, [0xFF, 0x7F]);
        TrySrcBinToSrc(PoType.Int16, short.MinValue, [0x00, 0x80]);
        TrySrcBinToSrc(PoType.Int16, (short)20520, [0x28, 0x50]);
        TrySrcBinToSrc(PoType.Int16, (short)-20520, [0xD8, 0xAF]);
    }
    [TestMethod]
    public void TestSrcToBinToSrc_UInt16()
    {
        TrySrcBinToSrc(PoType.UInt16, ushort.MaxValue, [0xFF, 0xFF]);
        TrySrcBinToSrc(PoType.UInt16, ushort.MinValue, [0x00, 0x00]);
        TrySrcBinToSrc(PoType.UInt16, (ushort)45662, [0x5E, 0xB2]);
    }

    [TestMethod]
    public void TestSrcToBinToSrc_UInt32()
    {
        TrySrcBinToSrc(PoType.UInt32, uint.MaxValue, [0xff, 0xff, 0xff, 0xff]);
        TrySrcBinToSrc(PoType.UInt32, uint.MinValue, [0x00, 0x00, 0x00, 0x00]);
        TrySrcBinToSrc(PoType.UInt32, (uint)123456, [0x40, 0xE2, 0x01, 0x00]);
    }

    [TestMethod]
    public void TestSrcToBinToSrc_Int32()
    {
        TrySrcBinToSrc(PoType.Int32, int.MaxValue, [0xff, 0xff, 0xff, 0x7f]);
        TrySrcBinToSrc(PoType.Int32, int.MinValue, [0x00, 0x00, 0x00, 0x80]);
        TrySrcBinToSrc(PoType.Int32, (int)123456, [0x40, 0xE2, 0x01, 0x00]);
        TrySrcBinToSrc(PoType.Int32, (int)-123456, [0xC0, 0x1D, 0xFE, 0xFF]);
    }

    [TestMethod]
    public void TestSrcToBinToSrc_UInt64()
    {
        TrySrcBinToSrc(PoType.UInt64, ulong.MaxValue, [0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff]);
        TrySrcBinToSrc(PoType.UInt64, ulong.MinValue, [0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00]);
        TrySrcBinToSrc(PoType.UInt64, (ulong)652212, [0xB4, 0xF3, 0x09, 0x00, 0x00, 0x00, 0x00, 0x00]);
    }

    [TestMethod]
    public void TestSrcToBinToSrc_Int64()
    {
        TrySrcBinToSrc(PoType.Int64, long.MaxValue, [0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0x7f]);
        TrySrcBinToSrc(PoType.Int64, long.MinValue, [0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x80]);
        TrySrcBinToSrc(PoType.Int64, (long)652212, [0xB4, 0xF3, 0x09, 0x00, 0x00, 0x00, 0x00, 0x00]);
        TrySrcBinToSrc(PoType.Int64, (long)-652212, [0x4C, 0x0C, 0xF6, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF]);
    }


    [TestMethod]
    public void TestSrcToBinToSrc_Half()
    {
        TrySrcBinToSrc(PoType.Half, Half.MaxValue, [0xFF, 0x7B]);
        TrySrcBinToSrc(PoType.Half, Half.MinValue, [0xFF, 0xFB]);
        TrySrcBinToSrc(PoType.Half, (Half)6.75, [0xC0, 0x46]);
        TrySrcBinToSrc(PoType.Half, (Half)(-6.75), [0xC0, 0xC6]);
    }

    [TestMethod]
    public void TestSrcToBinToSrc_Single()
    {
        TrySrcBinToSrc(PoType.Single, float.MaxValue, [0xFF, 0xFF, 0x7F, 0x7F]);
        TrySrcBinToSrc(PoType.Single, float.MinValue, [0xFF, 0xFF, 0x7F, 0xFF]);
        TrySrcBinToSrc(PoType.Single, (float)6.75, [0x00, 0x00, 0xD8, 0x40]);
        TrySrcBinToSrc(PoType.Single, (float)-6.75, [0x00, 0x00, 0xD8, 0xC0]);
    }

    [TestMethod]
    public void TestSrcToBinToSrc_Double()
    {
        TrySrcBinToSrc(PoType.Double, double.MaxValue, [0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xef, 0x7f]);
        TrySrcBinToSrc(PoType.Double, double.MinValue, [0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xef, 0xff]);
        TrySrcBinToSrc(PoType.Double, (double)6.75, [0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x1B, 0x40]);
        TrySrcBinToSrc(PoType.Double, (double)-6.75, [0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x1B, 0xC0]);
    }

    [TestMethod]
    public void TestSrcToBinToSrc_String()
    {
        TrySrcBin_ObjIsString(PoType.String, "aaaaa S", [0x07, 0x61, 0x61, 0x61, 0x61, 0x61, 0x20, 0x53]);
    }


}
