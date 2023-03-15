using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ReflectionApplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public Assembly AssemblyByName { get; set; }
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string path = @"C:\Working\U500\Builds\UHMI\ProjectsISM";
            string assemblyName = "Alstom.ICC.UHMI.ISM.HMI";

            var parentName = System.IO.Path.GetDirectoryName(path);
            if (assemblyName.Contains("ISM"))
            {
                string ism = "ISM";
                string hmiPath = System.IO.Path.Combine(parentName, $"{ism}\\{assemblyName + ".dll"}");

                if (File.Exists(hmiPath))
                {
                    MessageBox.Show("its there");
                }
            }

            //AssemblyByName = GetAssemblyByName("ReflectionPoc.dll");

            Assembly assembly = Assembly.LoadFile(path);
            Type[] types = assembly.GetTypes();
            
            foreach (var item in types)
            {
                Type t = Type.GetType("Calculator");

                MessageBox.Show(item.Name);

                if (item.Name=="Calculator")
                {
                    var cal1 = item;
                }
                MethodInfo[] methods1 = item.GetMethods();
                foreach (var method in methods1)
                {
                    MessageBox.Show(method.Name);
                }
                
            }

            Type classobj = assembly.GetTypes().Where(t=>t.Name == "Calculator").FirstOrDefault();
            object classInstance = Activator.CreateInstance(classobj, null);


            List<MethodInfo> methods = classobj.GetMethods().ToList();

            MethodInfo meth = methods.Where(m=>m.Name == "AddNumbers").FirstOrDefault();

            ParameterInfo[] param = meth.GetParameters();

            object result = GetResult(classInstance, meth,param);

          
        }

        public object GetInstance(string strNamesapace)
        {
            Type t = Type.GetType(strNamesapace);
            return Activator.CreateInstance(t);
        }

        private static object GetResult(Object classInstance, MethodInfo methodInfo,
                                        ParameterInfo[] parameters)
        {
            object result = null;

            if (parameters.Length == 0)
            {
                result = methodInfo.Invoke(classInstance, null);
            }
            else
            {
                var paramValueArray = ReturnParameterValueInputAsObjectArray(parameters);
                result = methodInfo.Invoke(classInstance, paramValueArray);
            }
            return result;
        }
        private static object[] ReturnParameterValueInputAsObjectArray(ParameterInfo[] parameters)
        {
            object[] paramValues = new object[parameters.Length];
            int itemCount = 0;

            foreach (ParameterInfo parameterInfo in parameters)
            {

                

                if (parameterInfo.ParameterType == typeof(string))
                {
                    string inputString = Console.ReadLine();
                    paramValues[itemCount] = inputString;
                }
                else if (parameterInfo.ParameterType == typeof(int))
                {
                    int inputInt = 10;
                    paramValues[itemCount] = inputInt;
                }
                else if (parameterInfo.ParameterType == typeof(double))
                {
                    double inputDouble = Double.Parse(Console.ReadLine());
                    paramValues[itemCount] = inputDouble;
                }

                itemCount++;

            }
            return paramValues;
        }

        private Assembly GetAssemblyByName(string name)
        {
            string fm = AppDomain.CurrentDomain.ToString();
            return AppDomain.CurrentDomain.GetAssemblies().
                   SingleOrDefault(assembly => assembly.GetName().Name == name);
        }
    }
}
