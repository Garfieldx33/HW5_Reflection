// See https://aka.ms/new-console-template for more information

using HW5_Reflection;
using Newtonsoft.Json;
using System.Reflection;
using System.Runtime.CompilerServices;

int IterationCnt = 0;

Class_F testObject = new Class_F { i1 = 1, i2 = 2, i3 = 3, i4 = 4, i5 = 5 };


Console.WriteLine("Введите количество требуемых итераций");
int.TryParse(Console.ReadLine(), out IterationCnt);

Console.WriteLine("Сериализуем в CSV");
string csvString =  SerialiseToSCV(IterationCnt);

Console.WriteLine("Десериализуем из CSV в List");
List<Class_F> fList = DeserialiseFromCsv(csvString);

Console.WriteLine("Сериализуем в JSON");
string json = SerialiseListToJson(fList);

Console.WriteLine("Десериализуем из JSON строки в List");
fList = DeserialiseFromJson(json);

Console.ReadKey();


string SerialiseToSCV(int IterationCount)
{
    DateTime dateTimeStart = DateTime.Now;
    string resultString = $"{GetPropNamesToString(testObject.GetType().GetProperties())}\r\n";
    for (int i = 0; i < IterationCount; i++)
    {
        resultString += $"{SerializeOneObject(testObject)}";
    }
    //Console.WriteLine(resultString);
    DateTime dateTimeFinish = DateTime.Now;
    Console.WriteLine($"Сериализация {IterationCount} объектов в CSV завершена за {(dateTimeFinish - dateTimeStart).TotalMilliseconds} милисекунд");
    
    return resultString;
}

List<Class_F> DeserialiseFromCsv(string InputString)
{
    List<Class_F> tstList = new List<Class_F>();
    int ind = InputString.IndexOf("\r\n") + 2;
    string cuttedString = InputString.Substring(ind, InputString.Length - ind);
    List<string> CSVstrings = cuttedString.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries).ToList();
    DateTime dateTimeStart = DateTime.Now;
    
    foreach (string csvStr in CSVstrings)
    {
        Class_F obj = DeserializeOneObject<Class_F>(csvStr);
        tstList.Add(obj);
    }
    DateTime dateTimeFinish = DateTime.Now;
    Console.WriteLine($"Десериализация {tstList.Count} объектов из CSV завершена за {(dateTimeFinish - dateTimeStart).TotalMilliseconds} милисекунд");
    return tstList;
}

string SerialiseListToJson(List<Class_F> F_ClassList)
{
    DateTime dateTimeStart = DateTime.Now;
    string resultString = JsonConvert.SerializeObject(F_ClassList);
   // Console.WriteLine(resultString);
    DateTime dateTimeFinish = DateTime.Now;
    Console.WriteLine($"Сериализация {F_ClassList.Count} объектов в JSON завершена за {(dateTimeFinish - dateTimeStart).TotalMilliseconds} милисекунд");
    return resultString;
}

List<Class_F> DeserialiseFromJson(string json)
{
    DateTime dateTimeStart = DateTime.Now;
    List<Class_F> fList = JsonConvert.DeserializeObject<List<Class_F>>(json);
    DateTime dateTimeFinish = DateTime.Now;
    Console.WriteLine($"Десериализация {fList.Count} объектов из JSON завершена за {(dateTimeFinish - dateTimeStart).TotalMilliseconds} милисекунд");
    return fList;
}



string SerializeSingleObjectToCsv(object InputObject)
{
    var t = InputObject.GetType();
    string result = GetPropNamesToString(t.GetProperties());
    result += SerializeOneObject(InputObject);
    return result;
}

string SerializeListOfObjectToCsv(object InputObject)
{
    string result = string.Empty;
    var t = InputObject.GetType();

    var Input = (List<object>)InputObject;
    var listType = Input.GetType().GetGenericArguments().Single();
    result = GetPropNamesToString(listType.GetProperties());

    foreach (var el in Input)
    {
        result += $"{SerializeOneObject(el)}\r\n";
    }

    return result;
}

string GetPropNamesToString(PropertyInfo[] propertyInfos)
{
    string result = string.Empty;
    foreach (PropertyInfo propertyInfo in propertyInfos)
    {
        result += $"{propertyInfo.Name};";
    }
    return result.TrimEnd(';');
}

string SerializeOneObject(object InputObject)
{
    string result = string.Empty;
    foreach (var property in InputObject.GetType().GetProperties())
    {
        string value = property.GetValue(InputObject) == null ? "" : property.GetValue(InputObject).ToString();
        result += $"{value};";
    }
    return $"{result.TrimEnd(';')}\r\n";
}

T DeserializeOneObject<T>(string InputObject)
{
    Type type = typeof(T);
    string[] propValues = InputObject.Split(';');
    T result = (T)Activator.CreateInstance(typeof(T));
    int index = 0;
    foreach (var prop in type.GetProperties())
    {
        var value = Convert.ChangeType(propValues[index], prop.PropertyType);
        prop.SetValue(result, value);
        index++;
    }

    return result;
}