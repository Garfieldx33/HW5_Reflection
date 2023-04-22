// See https://aka.ms/new-console-template for more information

using HW5_Reflection;
using Newtonsoft.Json;
using System.Reflection;
int IterationCount = 0;

Class_F testObject = new Class_F { i1 = 1, i2 = 2, i3 = 3, i4 = 4, i5 = 5 };


Console.WriteLine("Введите количество требуемых итераций");
int.TryParse(Console.ReadLine(), out IterationCount);
Console.WriteLine("Сериализуем каждый объект в CSV по отдельности");
DateTime dateTimeStart = DateTime.Now;
string res1 = $"{GetPropNamestoString(testObject.GetType().GetProperties())}\r\n";
for (int i = 0; i <= IterationCount; i++)
{
    res1 += $"{GetObjectValuesToString(testObject)}";
}
Console.WriteLine(res1);
DateTime dateTimeFinish = DateTime.Now;
double DateDiffVar1 = (dateTimeFinish - dateTimeStart).TotalMilliseconds;


Console.WriteLine("Вариант 2. Сериализуем каждый объект в JSON по отдельности");
dateTimeStart = DateTime.Now;
string jsonString = string.Empty;
for (int i = 0; i <= IterationCount; i++)
{
    jsonString += JsonConvert.SerializeObject(testObject);
}
Console.WriteLine(jsonString);
dateTimeFinish = DateTime.Now;
double DateDiffVar2 = (dateTimeFinish - dateTimeStart).TotalMilliseconds;




Console.WriteLine($"Операция по сериализации {IterationCount} объектов по 1 варианту завершена за {DateDiffVar1} милисекунд");
Console.WriteLine($"Операция по сериализации {IterationCount} объектов по 2 варианту завершена за {DateDiffVar2} милисекунд");
Console.ReadKey();
string SerializeSingleObjectToCsv(object InputObject)
{
    var t = InputObject.GetType();
    string result = GetPropNamestoString(t.GetProperties());
    result += GetObjectValuesToString(InputObject);
    return result;
}

string SerializeListOfObjectToCsv(object InputObject)
{
    string result = string.Empty;
    var t = InputObject.GetType();

    var Input = (List<object>)InputObject;
    var listType = Input.GetType().GetGenericArguments().Single();
    result = GetPropNamestoString(listType.GetProperties());

    foreach (var el in Input)
    {
        result += $"{GetObjectValuesToString(el)}\r\n";
    }

    return result;
}

string GetPropNamestoString(PropertyInfo[] propertyInfos)
{
    string result = string.Empty;
    foreach (PropertyInfo propertyInfo in propertyInfos)
    {
        result += $"{propertyInfo.Name};";
    }
    return result.TrimEnd(';');
}

string GetObjectValuesToString(object InputObject)
{
    string result = string.Empty;
    foreach (var property in InputObject.GetType().GetProperties())
    {
        string value = property.GetValue(InputObject) == null ? "" : property.GetValue(InputObject).ToString();
        result += $"{value};";
    }
    return $"{result.TrimEnd(';')}\r\n";
}