.class public abstract sealed auto ansi beforefieldinit
  Hw2.Parser extends System.Object
{
    .method public static void ParseCalcArguments(string[] args,
        [out] float64& value1,
        [out] valuetype Hw2.CalculatorOperation& operation,
        [out] float64& value2
        ) cil managed
    {
        ldarg.0
        call bool Hw2.Parser::IsArgLengthSupported(string[])
        brfalse ThrowArg 
        
        ldarg.0
        ldc.i4.0
        ldelem.ref 
        ldarg.1 
        call bool [System.Runtime]System.Double::TryParse(string, float64&)
        brfalse ThrowArg

        ldarg.0
        ldc.i4.2
        ldelem.ref 
        ldarg.3 
        call bool [System.Runtime]System.Double::TryParse(string, float64&)
        brfalse ThrowArg

        ldarg.2
        ldarg.0
        ldc.i4.1
        ldelem.ref
        call valuetype Hw2.CalculatorOperation Hw2.Parser::ParseOperation(string)
        stind.i4

        ldarg.2
        ldind.i4
        ldc.i4.4
        ceq
        brtrue ThrowInvOp

        ret

        ThrowArg:
        newobj instance void [System.Runtime]System.ArgumentException::.ctor()
        throw

        ThrowInvOp:
        newobj instance void [System.Runtime]System.InvalidOperationException::.ctor()
        throw
    }

    .method private static 
    valuetype Hw2.CalculatorOperation ParseOperation(string arg) cil managed
    {
        ldarg.0
        ldstr "+"
        call bool [System.Runtime]System.String::op_Equality(string, string)
        brtrue Addition

        ldarg.0
        ldstr "-"
        call bool [System.Runtime]System.String::op_Equality(string, string)
        brtrue Substraction

        ldarg.0
        ldstr "*"
        call bool [System.Runtime]System.String::op_Equality(string, string)
        brtrue Multiplication

        ldarg.0
        ldstr "/"
        call bool [System.Runtime]System.String::op_Equality(string, string)
        brtrue Division

        br Undefined

        Addition: 
        ldc.i4.0
        ret

        Substraction: 
        ldc.i4.1
        ret

        Multiplication: 
        ldc.i4.2
        ret

        Division: 
        ldc.i4.3
        ret

        Undefined:
        ldc.i4.4
        ret
    }

    .method public static bool IsArgLengthSupported(string[] args) cil managed
    {
        ldarg.0
        ldlen
        ldc.i4.3
        ceq
        ret
    }
}