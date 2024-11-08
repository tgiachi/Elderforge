namespace Elderforge.Core.Server.Utils;

public static class CSharpLuaConverterUtils
{
    public static string ConvertCSharpTypeToLuaDef(string csharpType)
    {
        return csharpType switch
        {
            "Int32"   => "number",
            "Int64"   => "number",
            "float"   => "number",
            "double"  => "number",
            "string"  => "string",
            "String"  => "string",
            "bool"    => "boolean",
            "Boolean" => "boolean",
            "void"    => "void",
            "Void"    => "void",
            "Action"  => "() => void",
            "action"  => "() => void",
            "task"    => "Promise<void>",
            "object"  => "any",
            _         => "any" // Default per i tipi non mappati
        };
    }
}
