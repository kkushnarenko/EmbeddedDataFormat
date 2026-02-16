using NetEdf;
using NetEdf.src;
using System.CodeDom.Compiler;

namespace NetEdfTest;

[TestClass]
public class TestPrimitives
{
   
    public void TrySrcBin_ObjIsStruct_ErrIsOk<T>(PoType type, T obj, Span<byte> dst) where T : struct
    {
        Span<byte> expectedDst = new byte[dst.Length];
        MemoryMarshal.Write(expectedDst, obj);

        var actual = Primitives.TrySrcToBin(type, obj, dst, out int w);

        Assert.AreEqual(EdfErr.IsOk, actual);
        Assert.AreEqual(dst.Length, w);
        Assert.IsTrue(expectedDst.SequenceEqual(dst));
    }
    public void TrySrcToBin_DstLenLessPoTypeSize_ErrDstBufOverflow(PoType type, object obj, Span<byte> dst)
    {
        var actual = Primitives.TrySrcToBin(type, obj, dst, out int w);

        Assert.AreEqual(EdfErr.DstBufOverflow, actual);
    }
    public void TrySrcToBin_ObjIsStruct_ErrWrongType(PoType type, object obj, Span<byte> dst)
    {
        var actual = Primitives.TrySrcToBin(type, obj, dst, out int w);

        Assert.AreEqual(EdfErr.WrongType, actual);
    }
    public void TrySrcBin_ObjIsSting_ErrIsOk(PoType type, object obj, Span<byte> dst)
    {
        Span<byte> expectedDst = new byte[dst.Length];
        int len = EdfBinString.WriteBin((string)obj, expectedDst);

        var actual = Primitives.TrySrcToBin(type, obj, dst, out int w);

        Assert.AreEqual(EdfErr.IsOk, actual);
        Assert.AreEqual(len, w);
        Assert.IsTrue(expectedDst.SequenceEqual(dst));
    }

    [TestMethod]
    public void TestTrySrcToBin()
    {
        //EdfErr.DstBufOverflow
        TrySrcToBin_DstLenLessPoTypeSize_ErrDstBufOverflow(PoType.Int16, 123, new byte[1]);
        TrySrcToBin_DstLenLessPoTypeSize_ErrDstBufOverflow(PoType.String, "1234", new byte[1]);
        //Struct
        TrySrcToBin_ObjIsStruct_ErrWrongType(PoType.Struct, 1, new byte[1]);
        //UInt8
        TrySrcBin_ObjIsStruct_ErrIsOk(PoType.UInt8, byte.MinValue, new byte[1]);
        TrySrcBin_ObjIsStruct_ErrIsOk(PoType.UInt8, byte.MaxValue, new byte[1]);
        TrySrcBin_ObjIsStruct_ErrIsOk(PoType.UInt8, (byte)123, new byte[1]);
        //Int8
        TrySrcBin_ObjIsStruct_ErrIsOk(PoType.Int8, sbyte.MaxValue, new byte[1]);
        TrySrcBin_ObjIsStruct_ErrIsOk(PoType.Int8, sbyte.MinValue, new byte[1]);
        TrySrcBin_ObjIsStruct_ErrIsOk(PoType.Int8, (sbyte)-123, new byte[1]);
        TrySrcBin_ObjIsStruct_ErrIsOk(PoType.Int8, (sbyte)123, new byte[1]);
        //UInt16
        TrySrcBin_ObjIsStruct_ErrIsOk(PoType.UInt16, ushort.MaxValue, new byte[2]);
        TrySrcBin_ObjIsStruct_ErrIsOk(PoType.UInt16, ushort.MinValue, new byte[2]);
        TrySrcBin_ObjIsStruct_ErrIsOk(PoType.UInt16, (ushort)123, new byte[2]);
        //Int16
        TrySrcBin_ObjIsStruct_ErrIsOk(PoType.Int16, short.MaxValue, new byte[2]);
        TrySrcBin_ObjIsStruct_ErrIsOk(PoType.Int16, short.MinValue, new byte[2]);
        TrySrcBin_ObjIsStruct_ErrIsOk(PoType.Int16, (short)123, new byte[2]);
        TrySrcBin_ObjIsStruct_ErrIsOk(PoType.Int16, (short)-123, new byte[2]);
        //UInt32
        TrySrcBin_ObjIsStruct_ErrIsOk(PoType.UInt32, uint.MaxValue, new byte[4]);
        TrySrcBin_ObjIsStruct_ErrIsOk(PoType.UInt32, uint.MinValue, new byte[4]);
        TrySrcBin_ObjIsStruct_ErrIsOk(PoType.UInt32, (uint)123, new byte[4]);
        //Int32
        TrySrcBin_ObjIsStruct_ErrIsOk(PoType.Int32, int.MaxValue, new byte[4]);
        TrySrcBin_ObjIsStruct_ErrIsOk(PoType.Int32, int.MinValue, new byte[4]);
        TrySrcBin_ObjIsStruct_ErrIsOk(PoType.Int32, (int)123, new byte[4]);
        TrySrcBin_ObjIsStruct_ErrIsOk(PoType.Int32, (int)-123, new byte[4]);
        //UInt64
        TrySrcBin_ObjIsStruct_ErrIsOk(PoType.UInt64, ulong.MaxValue, new byte[8]);
        TrySrcBin_ObjIsStruct_ErrIsOk(PoType.UInt64, ulong.MinValue, new byte[8]);
        TrySrcBin_ObjIsStruct_ErrIsOk(PoType.UInt64, (ulong)123, new byte[8]);
        //Int64
        TrySrcBin_ObjIsStruct_ErrIsOk(PoType.Int64, long.MaxValue, new byte[8]);
        TrySrcBin_ObjIsStruct_ErrIsOk(PoType.Int64, long.MinValue, new byte[8]);
        TrySrcBin_ObjIsStruct_ErrIsOk(PoType.Int64, (long)123, new byte[8]);
        TrySrcBin_ObjIsStruct_ErrIsOk(PoType.Int64, (long)-123, new byte[8]);
        //Half
        TrySrcBin_ObjIsStruct_ErrIsOk(PoType.Half, Half.MaxValue, new byte[2]);
        TrySrcBin_ObjIsStruct_ErrIsOk(PoType.Half, Half.MinValue, new byte[2]);
        TrySrcBin_ObjIsStruct_ErrIsOk(PoType.Half, (Half)6.75, new byte[2]);
        TrySrcBin_ObjIsStruct_ErrIsOk(PoType.Half, (Half)(-6.75), new byte[2]);
        TrySrcBin_ObjIsStruct_ErrIsOk(PoType.Half, Half.NaN, new byte[2]);
        //Single
        TrySrcBin_ObjIsStruct_ErrIsOk(PoType.Single, float.MaxValue, new byte[4]);
        TrySrcBin_ObjIsStruct_ErrIsOk(PoType.Single, float.MinValue, new byte[4]);
        TrySrcBin_ObjIsStruct_ErrIsOk(PoType.Single, (float)6.75, new byte[4]);
        TrySrcBin_ObjIsStruct_ErrIsOk(PoType.Single, (float)-6.75, new byte[4]);
        TrySrcBin_ObjIsStruct_ErrIsOk(PoType.Single, float.NaN, new byte[4]);
        //Double
        TrySrcBin_ObjIsStruct_ErrIsOk(PoType.Double, double.MaxValue, new byte[8]);
        TrySrcBin_ObjIsStruct_ErrIsOk(PoType.Double, double.MinValue, new byte[8]);
        TrySrcBin_ObjIsStruct_ErrIsOk(PoType.Double, (double)6.75, new byte[8]);
        TrySrcBin_ObjIsStruct_ErrIsOk(PoType.Double, (double)-6.75, new byte[8]);
        TrySrcBin_ObjIsStruct_ErrIsOk(PoType.Double, double.NaN, new byte[8]);
        //Char
        //       TrySrcBin_ObjIsStruct_ErrIsOk(PoType.Char, 'a', new byte[1]);
        //String
        TrySrcBin_ObjIsSting_ErrIsOk(PoType.String, "aaaaa A aaaaa", new byte[20]);
    }
}
