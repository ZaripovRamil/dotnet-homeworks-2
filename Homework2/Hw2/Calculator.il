.class public abstract sealed auto ansi beforefieldinit
  Hw2.Calculator
{
    .method public static float64 Calculate(float64 value1, valuetype Hw2.CalculatorOperation operation, float64 value2)
    {
        ldarg.0 
        ldarg.2
        ldarg.1
        ldc.i4.0
        beq Addition
        ldarg.1
        ldc.i4.1
        beq Substraction
        ldarg.1
        ldc.i4.2
        beq Multiplication
        ldarg.1
        ldc.i4.3
        beq Division
        newobj instance void [System.Runtime]System.ArgumentOutOfRangeException::.ctor()
        throw
        
        Addition:
        add
        ret
        Substraction:
        sub
        ret
        Multiplication:
        mul
        ret
        Division:
        div
        ret
    }
}



