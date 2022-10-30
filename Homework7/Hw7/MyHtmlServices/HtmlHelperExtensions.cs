using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using static System.String;

namespace Hw7.MyHtmlServices;

public static class HtmlHelperExtensions
{
    public static IHtmlContent MyEditorForModel(this IHtmlHelper helper)
    {
        var data = helper.ViewData;
        var model = data.Model;
        var properties = data.ModelMetadata.ModelType.GetProperties();
        return CreateHtml(model, properties);
    }

    private static IHtmlContent CreateHtml(object? model, PropertyInfo[] properties)
    {
        var layoutBuilder = new HtmlContentBuilder();
        Array.ForEach(properties, property => layoutBuilder.AppendHtmlLine(ConstructDivFromProperty(model, property)));
        return layoutBuilder;
    }

    private static string ConstructDivFromProperty(object? model, PropertyInfo property)
    {
        StringBuilder sb = new();
        sb.AppendLine("<div>");
        ConstructPropertyAttribute(sb, property);
        ConstructPropertyInputString(sb, model, property);
        sb.AppendLine("</div>");
        return sb.ToString();
    }

    private static void ConstructPropertyInputString(StringBuilder sb, object? model, PropertyInfo property)
    {
        var modelValue = model!=null ? property.GetValue(model):null;
        var modelValueTag = model == null ? Empty : $"value=\"{modelValue}\"";
        if (property.PropertyType.IsEnum)
            ConstructSelect(sb, modelValueTag, property);
        else
            ConstructInput(sb, modelValueTag, property);
        if(modelValue!=null)
            ConstructErrorField(sb, property, modelValue);
    }

    private static void ConstructErrorField(StringBuilder sb, PropertyInfo property, object? value)
    {
        var validationFails = property.GetCustomAttributes<ValidationAttribute>()
            .Where(attribute => !attribute.IsValid(value)).ToList();
        foreach(var fail in validationFails)
            sb.AppendLine($"<span>{fail.ErrorMessage!}</span>");
    }

    private static void ConstructInput(StringBuilder sb, string modelValue, PropertyInfo property)
    {
        var inputType = property.PropertyType == typeof(int) ? "number" : "text";
        sb.AppendLine($"<input name=\"{property.Name}\" type=\"{inputType}\"{modelValue}/>");
    }

    private static void ConstructSelect(StringBuilder sb, string modelValue, PropertyInfo property)
    {
        sb.AppendLine($"<select {modelValue}>");
        foreach (var enumValue in Enum.GetValues(property.PropertyType))
            sb.AppendLine($"<option>{enumValue}</option>");
        sb.AppendLine("</select>");
    }

    private static void ConstructPropertyAttribute(StringBuilder sb, PropertyInfo property)
    {
        var attribute = property.GetCustomAttribute<DisplayAttribute>();
        sb.AppendLine($"<label for=\"{property.Name}\">");
        sb.AppendLine(attribute == null ? ConvertStringToUpperCase(property.Name) : attribute.Name);
        sb.AppendLine("</label>");
    }

    private static string ConvertStringToUpperCase(string s) => Join(" ", Regex.Split(s, @"(?<!^)(?=[A-Z0-9])"));
}