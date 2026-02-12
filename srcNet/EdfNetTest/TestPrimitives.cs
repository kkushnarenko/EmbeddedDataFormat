using NetEdf.src;

namespace NetEdfTest;

[TestClass]
public class TestPrimitives
{
    [TestMethod]
    public void TrySrcToBin_DstLenLessPoTypeSize_ErrDstBufOverflow()
    {
        var type = PoType.Int16;
        short obj = 1234;
        Span<byte> dst = new byte[1];
        var errExpected = EdfErr.DstBufOverflow;

        var actual = Primitives.TrySrcToBin(type, obj, dst, out int w);

        Assert.AreEqual(errExpected, actual);
    }

    //PoType t один тип а obj другой
    [TestMethod]
    public void TrySrcToBin_PoTypeAndObjDifferentType_ErrWrongType()
    {
        var type = PoType.Int16;
        int obj = 1234;
        Span<byte> dst = new byte[4];
        var errExpected = EdfErr.WrongType;

        var actual = Primitives.TrySrcToBin(type, obj, dst, out int w);

        Assert.AreEqual(errExpected, actual);
    }

    //Безнаковый UInt8
    [TestMethod]
    public void TrySrcToBin_ObjIsUInt8_ErrIsOk()
    {
        var type = PoType.UInt8;
        byte obj = 123;
        Span<byte> dst = new byte[1];
        var errExpected = EdfErr.IsOk;

        var actual = Primitives.TrySrcToBin(type, obj, dst, out int w);

        Assert.AreEqual(errExpected, actual);
    }

    //[TestMethod]
    //public void TrySrcToBin_DstIsCurrectInt8_ErrIsOk()
    //{
    //    var type = PoType.UInt8;
    //    byte obj = 123;
    //    Span<byte> dst = new byte[1] {0x7B};
    //    var errExpected = EdfErr.IsOk;

    //    var actual = Primitives.TrySrcToBin(type, obj, dst, out int w);

    //    Assert.AreEqual(dst, actual);
    //}

    public void TrySrcToBin_Int8(sbyte val)
    {
        Span<byte> dst = new byte[1];
        var actual = Primitives.TrySrcToBin(PoType.Int8, val, dst, out int w);
        Assert.AreEqual(EdfErr.IsOk, actual);
        Assert.AreEqual(dst.Length, w);
        Assert.AreEqual(val, unchecked((sbyte)dst[0]));
    }
    //Знаковый UInt8
    [TestMethod(DisplayName = "тест Int8")]
    public void TrySrcToBin_NegativeObjIsInt8_ErrIsOk()
    {
        TrySrcToBin_Int8(sbyte.MinValue/2);
        TrySrcToBin_Int8(sbyte.MaxValue/2);
        TrySrcToBin_Int8(sbyte.MinValue);
        TrySrcToBin_Int8(sbyte.MaxValue);
        TrySrcToBin_Int8(-1);
        TrySrcToBin_Int8(0);
    }

    [TestMethod]
    
    public void TrySrcToBin_PositiveObjIsInt8_ErrIsOk()
    {
        var type = PoType.Int8;
        sbyte obj = 127;
        Span<byte> dst = new byte[1];
        var errExpected = EdfErr.IsOk;

        var actual = Primitives.TrySrcToBin(type, obj, dst, out int w);

        Assert.AreEqual(errExpected, actual);
    }

    //Беззнаковый UInt16
    [TestMethod]
    public void TrySrcToBin_ObjIsUInt16_ErrIsOk()
    {
        var type = PoType.UInt16;
        ushort obj = 65535;
        Span<byte> dst = new byte[2];
        var errExpected = EdfErr.IsOk;

        var actual = Primitives.TrySrcToBin(type, obj, dst, out int w);

        Assert.AreEqual(errExpected, actual);
    }

    //Знаковый Int16
    [TestMethod]
    public void TrySrcToBin_PositiveObjIsInt16_ErrIsOk()
    {
        var type = PoType.Int16;
        short obj = 32767;
        Span<byte> dst = new byte[2];
        var errExpected = EdfErr.IsOk;

        var actual = Primitives.TrySrcToBin(type, obj, dst, out int w);

        Assert.AreEqual(errExpected, actual);
    }

    [TestMethod]
    public void TrySrcToBin_NegativeObjIsInt16_ErrIsOk()
    {
        var type = PoType.Int16;
        short obj = -32768;
        Span<byte> dst = new byte[2];
        var errExpected = EdfErr.IsOk;

        var actual = Primitives.TrySrcToBin(type, obj, dst, out int w);

        Assert.AreEqual(errExpected, actual);
    }

    //Беззнаковый UInt32
    [TestMethod]
    public void TrySrcToBin_ObjIsUInt32_ErrIsOk()
    {
        var type = PoType.UInt32;
        uint obj = 4294967295;
        Span<byte> dst = new byte[4];
        var errExpected = EdfErr.IsOk;

        var actual = Primitives.TrySrcToBin(type, obj, dst, out int w);

        Assert.AreEqual(errExpected, actual);
    }

    //Знаковый Int32
    [TestMethod]
    public void TrySrcToBin_PositiveObjIsInt32_ErrIsOk()
    {
        var type = PoType.Int32;
        int obj = 2147483647;
        Span<byte> dst = new byte[4];
        var errExpected = EdfErr.IsOk;

        var actual = Primitives.TrySrcToBin(type, obj, dst, out int w);

        Assert.AreEqual(errExpected, actual);
    }

    [TestMethod]
    public void TrySrcToBin_NegativeObjIsInt32_ErrIsOk()
    {
        var type = PoType.Int32;
        int obj = -2147483648;
        Span<byte> dst = new byte[4];
        var errExpected = EdfErr.IsOk;

        var actual = Primitives.TrySrcToBin(type, obj, dst, out int w);

        Assert.AreEqual(errExpected, actual);
    }

    //Беззнаковый UInt64
    [TestMethod]
    public void TrySrcToBin_ObjIsUInt64_ErrIsOk()
    {
        var type = PoType.UInt64;
        ulong obj = 18446744073709551615;
        Span<byte> dst = new byte[8];
        var errExpected = EdfErr.IsOk;

        var actual = Primitives.TrySrcToBin(type, obj, dst, out int w);

        Assert.AreEqual(errExpected, actual);

    }

    //Знаковый Int64
    [TestMethod]
    public void TrySrcToBin_PositiveObjIsInt64_ErrIsOk()
    {
        var type = PoType.Int64;
        long obj = 9223372036854775807;
        Span<byte> dst = new byte[8];
        var errExpected = EdfErr.IsOk;

        var actual = Primitives.TrySrcToBin(type, obj, dst, out int w);

        Assert.AreEqual(errExpected, actual);
    }
    [TestMethod]
    public void TrySrcToBin_NegativeObjIsInt64_ErrIsOk()
    {
        var type = PoType.Int64;
        long obj = -9223372036854775808;
        Span<byte> dst = new byte[8];
        var errExpected = EdfErr.IsOk;

        var actual = Primitives.TrySrcToBin(type, obj, dst, out int w);

        Assert.AreEqual(errExpected, actual);
    }


    //Struct Half Single Double String Char

    //Тест на NaN
//    [TestMethod]
    //public void TrySrcToBin_ObjIsHalfNaN_ErrObjIsNaN()
    //{
    //    var type = PoType.Half;
    //    Half obj = Half.NaN;
    //    Span<byte> dst = new byte[2];


    //}



}
